using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using ZealTravel.Domain.BookingManagement;
using TIRequestResponse;
using Microsoft.Extensions.Configuration;
using ZealTravel.Infrastructure.Akaasa;

namespace ZealTravel.Infrastructure.UAPI
{
    public class GetBook
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string CompanyID;
        private Int32 BookingRef;
        private IBookingManagementService _bookingService;

        public GetBook(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string SearchID, string CompanyID, int BookingRef, IBookingManagementService bookingService)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.UserName = UserName;
            this.Password = Password;

            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
            this._bookingService = bookingService;            
        }

        public async Task<bool> GetBookResponse(string TResponse, string TPassenger, string RS_Company, string RS_GstDetail, bool IsOW,string PaymentType)
        {
            bool PnrStatus = false;
            var bookResponse = string.Empty;
            try
            {
                DataTable dtBound = DBCommon.CommonFunction.StringToDataSet(TResponse).Tables["AvailabilityInfo"];
                DataTable dtPassenger = DBCommon.CommonFunction.StringToDataSet(TPassenger).Tables["PassengerInfo"];
                DataTable dtCompanyInfo = DBCommon.CommonFunction.StringToDataSet(RS_Company).Tables["CompanyInfo"];
                DataTable dtGstInfo = new DataTable();
                if (RS_GstDetail != null && RS_GstDetail.IndexOf("GstInfo") != -1)
                {
                    dtGstInfo = DBCommon.CommonFunction.StringToDataSet(RS_GstDetail).Tables["GstInfo"];
                }


                if (dtBound != null && dtPassenger != null)
                {
                    if (IsOW)
                    {
                        PnrStatus = await GetBookResponse(CompanyID, dtBound.Copy(), dtPassenger.Copy(), dtCompanyInfo, dtGstInfo, true, PaymentType);
                    }
                    else
                    {
                        PnrStatus = await GetBookResponse(CompanyID, dtBound.Copy(), dtPassenger.Copy(), dtCompanyInfo, dtGstInfo, false, PaymentType);
                    }

                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookResponse1", "air_uapi", "", SearchID, ex.Message);
            }
            return PnrStatus;
        }
        private async Task<bool> GetBookResponse(string CompanyID, DataTable dtBound, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo, bool IsOW,string PaymentType)
        {
            bool status = false;
            string SearchCriteria = "";
            string BookRequest = "";
            string BookResponse = "";
            string PassengerRequest = "";
            string Trip = "";
            string FltType = "";

            try
            {
                PassengerRequest = DBCommon.CommonFunction.DataTableToString(dtPassenger);
                string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
                string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();
                string BeginDate = dtBound.Rows[0]["DepDate"].ToString().Trim();
                string EndDate = dtBound.Rows[dtBound.Rows.Count - 1]["DepDate"].ToString().Trim().ToUpper();

                short iAdt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
                short iChd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
                short iInf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());

                Trip = dtBound.Rows[0]["Trip"].ToString();
                FltType = dtBound.Rows[0]["FltType"].ToString();
                string AirlineID = dtBound.Rows[0]["AirlineID"].ToString();

                if (IsOW)
                {
                    SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, iAdt, iChd, iInf);
                }
                else
                {
                    SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, EndDate, iAdt, iChd, iInf);
                }
                
                if (dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    string PriceType = dtBound.Rows[0]["PriceType"].ToString();
                    if (HaveGst(dtGstInfo) || PriceType.ToUpper().IndexOf("SME") != -1) //call air price with gst
                    {
                        //GetairPriceWithGST objAPGST = new GetairPriceWithGST(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef, dtBound, dtPassenger);
                        //bool returnGst = objAPGST.GetBookResponse();
                    }

                    GetAirCreatePnrRequest objRQ = new GetAirCreatePnrRequest(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef, _bookingService);
                    BookRequest = objRQ.GetBookRequest_LCC(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo,PaymentType);
                }
                else
                {
                    GetAirCreatePnrRequest objRQ = new GetAirCreatePnrRequest(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef, _bookingService);
                    BookRequest = await objRQ.GetBookRequest(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo,PaymentType);
                }


               // DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetCommit-BFR-RQ", "air_uapi", BookRequest, SearchID, BookResponse);

                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, BookRequest, "AirService", "Book");

                // DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetCommit-AFT-RS", "air_uapi", BookRequest, SearchID, BookResponse);

                if (BookResponse != null && BookResponse.IndexOf("SupplierLocator") != -1 && BookResponse.IndexOf("AirReservation") != 1 && BookResponse.IndexOf("UniversalRecord") != -1)
                {
                    string AirlinePNR = "";
                    string GalileoPNR = "";
                    string AirReservationLocatorCode = "";
                    string UniversalRecordLocatorCode = "";
                    AirlinePNR = CommonUapi.RetrievePNR(SearchID, CompanyID, BookingRef, BookResponse, out GalileoPNR, out AirReservationLocatorCode,out UniversalRecordLocatorCode);
                    if (AirlinePNR.Length.Equals(0) && GalileoPNR.Length > 0)
                    {
                        AirlinePNR = GalileoPNR;
                    }
                    if (GalileoPNR.Length.Equals(0) && AirlinePNR.Length > 0)
                    {
                        GalileoPNR = AirlinePNR;
                    }

                    if (IsOW && (AirlinePNR.Length > 0 || GalileoPNR.Length > 0))
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", FltType, AirlinePNR, GalileoPNR, AirlineID,UniversalRecordLocatorCode);
                    }
                    else if (Trip.Equals("M") && ( AirlinePNR.Length > 0 || GalileoPNR.Length  > 0))
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "O", AirlinePNR, GalileoPNR, AirlineID,UniversalRecordLocatorCode);
                    }
                    else if (!IsOW && Trip.Equals("R") && ( AirlinePNR.Length > 0 || GalileoPNR.Length > 0))
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "O", AirlinePNR, GalileoPNR, AirlineID,UniversalRecordLocatorCode);
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "I", AirlinePNR, GalileoPNR, AirlineID,UniversalRecordLocatorCode);
                    }


                    if (AirlinePNR != null && AirlinePNR.Length > 0 )
                    {
                        if (!PaymentType.ToUpper().Equals("PAYMENTHOLD"))
                        {
                            if (!dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                            {
                                if (AirlinePNR != null && AirlinePNR.Length > 0 && await _bookingService.IsTicketAutoMode(CompanyID, dtBound.Rows[0]["CarrierCode"].ToString(), dtBound.Rows[0]["Sector"].ToString(), BookingRef))
                                {
                                    GetAirCreateTicketing objCreateTkt = new GetAirCreateTicketing(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef, _bookingService);
                                    status = await objCreateTkt.CreateTicket(GalileoPNR, AirlinePNR, AirReservationLocatorCode, dtBound.Rows[0]["CarrierCode"].ToString());
                                }
                                else
                                {
                                    DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, FltType, SearchCriteria, "TKT-False-Status", "", "", "");
                                }
                            }
                            else
                            {
                                status = true;
                            }
                        }
                        else
                        {
                            status = true;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
              //  DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookResponse2", "air_uapi", SearchCriteria, SearchID, ex.Message);
            }

           DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, FltType, SearchCriteria, "PNR-" + Trip, BookRequest, BookResponse, PassengerRequest);
           DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "PNR", "PNR", BookRequest + Environment.NewLine + BookResponse, SearchCriteria, SearchID);
            return status;
        }
        //=====================================================================================================================================
        private bool HaveGst(DataTable dtGstInfo)
        {
            bool IsGSTApplicable = false;
            string GSTCompanyAddress = string.Empty;
            string GSTCompanyContactNumber = string.Empty;
            string GSTCompanyName = string.Empty;
            string GSTNumber = string.Empty;
            string GSTCompanyEmail = string.Empty;

            if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
            {
                GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();

                if (GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0 && GSTCompanyName.Length > 0)
                {
                    IsGSTApplicable = true;
                }
            }
            return IsGSTApplicable;
        }
    }
}
