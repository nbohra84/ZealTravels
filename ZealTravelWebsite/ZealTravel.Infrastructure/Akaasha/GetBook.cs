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

using System.Threading.Tasks;
using ZealTravel.Domain.BookingManagement;

/// <summary>
/// Summary description for GetBook
/// </summary>
/// 
namespace ZealTravel.Infrastructure.Akaasha
{
    class GetBook
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string SupplierID;
        private string CompanyID;
        private Int32 BookingRef;
        private string _Token;
        private IBookingManagementService _bookingService;
        //public GetBook(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string SearchID, string CompanyID, int BookingRef)
        public GetBook(string SearchID, string SupplierID, string CompanyID, int BookingRef, string Token, IBookingManagementService bookingService)
        {
            //this.NetworkUserName = NetworkUserName;
            //this.NetworkPassword = NetworkPassword;
            //this.TargetBranch = TargetBranch;
            //this.UserName = UserName;
            //this.Password = Password;

            this._Token = Token;

            this.SearchID = SearchID;
            this.SupplierID = SupplierID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
            this._bookingService = bookingService;

        }
        public async Task<bool> GetBookResponse(string TResponse, string TPassenger, string RS_Company, string RS_GstDetail, bool IsOW)
        {
            bool PnrStatus = false;
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
                        PnrStatus = await GetBookResponse(CompanyID, dtBound.Copy(), dtPassenger.Copy(), dtCompanyInfo, dtGstInfo, true);
                    }
                    else
                    {
                        PnrStatus = await GetBookResponse(CompanyID, dtBound.Copy(), dtPassenger.Copy(), dtCompanyInfo, dtGstInfo, false);
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookResponse1", "air_Qp", "", SearchID, ex.Message);
            }
            return PnrStatus;
        }
        private async Task<bool> GetBookResponse(string CompanyID, DataTable dtBound, DataTable dtPassenger, DataTable dtCompanyInfo, DataTable dtGstInfo, bool IsOW)
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


                //GetAirCreatePnrRequest objRQ = new GetAirCreatePnrRequest(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef);
                //BookRequest = objRQ.GetBookRequest(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo);


                //DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetCommit-BFR-RQ", "air_Qp", BookRequest, SearchID, BookResponse);

                //CommonUapi cs = new CommonUapi();
                //BookResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, BookRequest, "AirService", "Book");

                GetAirCreatePnrRequest objbook = new GetAirCreatePnrRequest(SearchID,SupplierID, CompanyID, BookingRef);

                //===== TripSell
                string JsonTripSellRQ = objbook.GetTripSellRequest(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo);
                Task<string> JsonTripSellRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonTripSellRQ, "TripSell", _Token));
                string JsonFlightTripSellRS = JsonTripSellRSAw.GetAwaiter().GetResult();
                //==== end TripSell

                //===== Quote
                string JsonQuoteRQ = objbook.GetQuoteRequest(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo);
                Task<string> JsonQuoteRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonQuoteRQ, "Quote", _Token));
                string JsonFlightQuoteRS = JsonQuoteRSAw.GetAwaiter().GetResult();


                string xmlString = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(JsonFlightQuoteRS, "passengers").OuterXml;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);



                Dictionary<string, string> _PassengerKeyList = new Dictionary<string, string>();
                XmlNode root = xmlDoc.DocumentElement;
                XmlNodeList passengers = root.SelectNodes("/passengers/data/passengers/*");
                foreach (XmlNode passenger in passengers)
                {
                    XmlNode passengerKey = passenger.SelectSingleNode("passengerKey");
                    string key = passengerKey.InnerText;

                    XmlNode passengerType = passenger.SelectSingleNode("passengerTypeCode");
                    string type = passengerType.InnerText;

                    _PassengerKeyList.Add(key, type);
                    if (type == "ADT")
                    {
                        XmlNode node__ = passenger.SelectSingleNode("infant/fees");
                        //XmlNodeList __infantEle = root.SelectNodes("/passengers/data/passengers/infant/fees/*");
                        //XmlNode node__ = passenger.SelectSingleNode("infant"); // root.SelectSingleNode("/passengers/data/passengers/infant/fees");
                        
                        if (node__ != null && node__.HasChildNodes==true) 
                        {
                            _PassengerKeyList.Add(key + "/infant", "INF");
                        }
                    }


                }
                decimal _TotalJournyAmt = 0;
                XmlNodeList journeyTotals = root.SelectNodes("/passengers/data/breakdown/journeyTotals/*");
                foreach(XmlNode _node in journeyTotals)
                {
                    if (_node.Name.Equals("totalAmount",StringComparison.OrdinalIgnoreCase))
                    {
                        _TotalJournyAmt += Convert.ToDecimal(_node.InnerText);
                    }
                    else if (_node.Name.Equals("totalTax", StringComparison.OrdinalIgnoreCase))
                    {
                        _TotalJournyAmt += Convert.ToDecimal(_node.InnerText);
                    }

                }
                //var sss = journeyTotals[0].SelectSingleNode("totalAmount").InnerText;
                //var sss1 = journeyTotals[0].SelectSingleNode("totalTax").InnerText;
                //_TotalJournyAmt += Convert.ToDecimal(journeyTotals[0]["totalAmount"] == null ? "0" : journeyTotals[0]["totalAmount"].FirstChild.InnerText);
                //_TotalJournyAmt += Convert.ToDecimal(journeyTotals[0]["totalTax"] == null ? "0" : journeyTotals[0]["totalTax"].FirstChild.InnerText);




                //==== end Quote


                //===== Contracts
                string JsonContactsRQ = objbook.GetContactsRequest(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo);
                Task<string> JsonContactsRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonContactsRQ, "Contacts", _Token));
                string JsonFlightContactsRS = JsonContactsRSAw.GetAwaiter().GetResult();
                //==== end Contact

                //===== Contracts4GST  Created 24Jan2024
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetContactsRequest4GST", "air_Qp", "", SearchID, "Before - GST Call");
                string JsonContactsRQ4GST = objbook.GetContactsRequest4GST(CompanyID, dtBound, dtPassenger, dtCompanyInfo, dtGstInfo);
                if (JsonContactsRQ4GST != "")
                {
                    DBCommon.Logger.dbLogg(CompanyID, 0, "GetContactsRequest4GST", "air_Qp", "", SearchID, JsonContactsRQ4GST);
                    Task<string> JsonContactsRSAw4GST = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonContactsRQ4GST, "Contacts", _Token));
                    string JsonFlightContacts4GSTRS = JsonContactsRSAw4GST.GetAwaiter().GetResult();
                }
                //==== end Contact4GST



                //====== passenger updaate start
                Dictionary<string, string> _PassengerRQList = objbook.GetPassengersRequest(CompanyID, dtBound, dtPassenger, dtGstInfo);
                string _JasonPassengerRQ = "";
                foreach (var passengerKey in _PassengerKeyList)
                {
                    var _RQ = _PassengerRQList.Where(x => x.Key.StartsWith(passengerKey.Value,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    _JasonPassengerRQ = _RQ.Value;
                    Task<string> JsonPassengerRSAw;
                    string JsonFlightPassengerRS = "";
                    if (_RQ.Key.StartsWith("INF", StringComparison.OrdinalIgnoreCase))
                    {
                       JsonPassengerRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, _JasonPassengerRQ, "Passengers", _Token, passengerKey.Key, "POST"));
                        JsonFlightPassengerRS = JsonPassengerRSAw.GetAwaiter().GetResult();
                    }
                    else
                    {
                        JsonPassengerRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, _JasonPassengerRQ, "Passengers", _Token, passengerKey.Key, "PUT"));
                        JsonFlightPassengerRS = JsonPassengerRSAw.GetAwaiter().GetResult();
                    }
                    _PassengerRQList.Remove(_RQ.Key);
                }
                //========= passenger update end
                //===== start payment
                string JsonPaymentsRQ = objbook.GetPaymentsRequest(CompanyID, dtBound, dtPassenger,  dtGstInfo);
                Task<string> JsonPaymentsRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonPaymentsRQ, "Payments", _Token));
                string JsonFlightPaymentsRS = JsonPaymentsRSAw.GetAwaiter().GetResult();
                //======= end payment 

                //===== start Bookings
                string JsonBookingsRQ = objbook.GetBookingsRequest(CompanyID);
                Task<string> JsonBookingsRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonBookingsRQ, "Bookings", _Token));
                string JsonFlightBookingsRS = JsonBookingsRSAw.GetAwaiter().GetResult();
                //======= end Bookings 

                //===== start GetBookingState
                string JsonGetBookingStateRQ = objbook.GetBookingStateRequest(CompanyID);
                Task<string> JsonGetBookingStateRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonGetBookingStateRQ, "GetBookingState", _Token,"GET"));
                string JsonFlightGetBookingStateRS = JsonGetBookingStateRSAw.GetAwaiter().GetResult();

                //Task<string> JsonGetBookingStateRSAw = Task.Run(() => CommonQP.GetResponseBookingStateAsync(SearchID,  _Token));
                //string JsonFlightGetBookingStateRS = JsonGetBookingStateRSAw.GetAwaiter().GetResult();



                //======= end GetBookingState

                BookResponse = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(JsonFlightGetBookingStateRS, "GetBookingState").OuterXml;



                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetCommit-AFT-RS", "air_Qp", BookRequest, SearchID, BookResponse);


                //if (BookResponse != null && BookResponse.IndexOf("recordLocator") != -1 && BookResponse.IndexOf("bookingKey") != 1)
                if (BookResponse != null)
                {
                    string AirlinePNR = "";
                    string AirReservationLocatorCode = "";

                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(BookResponse);
                    XmlNode rootPnr = xmlDoc.DocumentElement;


                    XmlNodeList _PnrNodeList = rootPnr.SelectNodes("/GetBookingState/data/*");
                    foreach (XmlNode _node in _PnrNodeList)
                    {
                        if (_node.Name.Equals("recordLocator", StringComparison.OrdinalIgnoreCase))
                        {
                            AirlinePNR= _node.InnerText;
                            //break;
                        }
                        else if (_node.Name.Equals("locators", StringComparison.OrdinalIgnoreCase))
                        {
                            if (_node.SelectSingleNode("numericRecordLocator") != null)
                            {
                                AirReservationLocatorCode = _node.SelectSingleNode("numericRecordLocator").InnerText;
                            }
                        }

                    }


                    if (IsOW && (AirlinePNR.Length > 0))    
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", FltType, AirlinePNR, AirlinePNR, AirlineID);

                    }
                    else if (Trip.Equals("M") && AirlinePNR.Length > 0)
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "O", AirlinePNR, AirlinePNR, AirlineID);
                    }
                    else if (!IsOW && Trip.Equals("R") && AirlinePNR.Length > 0)
                    {
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "O", AirlinePNR, AirlinePNR, AirlineID);
                        await _bookingService.UpdatePNR(BookingRef, "AD-101", "I", AirlinePNR, AirlinePNR, AirlineID);
                    }
                    //DBCommon.dbCallCenter.UpdateTicketNumber(BookingRef, new ArrayList { AirReservationLocatorCode }, false);   // ==== this is added on 21Dec2023

                    if (AirlinePNR != null && AirlinePNR.Length > 0)
                    {
                        /*if (!dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                        {
                            if (AirlinePNR != null && AirlinePNR.Length > 0 && cs.IsTicketAutoMode(CompanyID, dtBound.Rows[0]["CarrierCode"].ToString(), dtBound.Rows[0]["Sector"].ToString(), BookingRef))
                            {
                                GetAirCreateTicketing objCreateTkt = new GetAirCreateTicketing(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, SearchID, CompanyID, BookingRef);
                                objCreateTkt.CreateTicket(GalileoPNR, AirlinePNR, AirReservationLocatorCode, dtBound.Rows[0]["CarrierCode"].ToString());

                                status = true;
                            }
                            else
                            {
                                DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, FltType, SearchCriteria, "TKT-False-Status", "", "", "");
                            }
                        }
                        else
                        {
                            status = true;
                        }*/
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookResponse", "air_Qp", SearchCriteria, SearchID, ex.Message);
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


