using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Common.Helpers.Flight;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.GSTManagement;
using ZealTravel.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZealTravel.Domain.Services.AirelineManagement.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IBookingManagementService _bookingManagementService;
        private readonly IAgencyService _agencyService;
        private readonly IGSTService _gstService;
        private readonly IDBLoggerService _dbLoggerService;

        public BookingService(IBookingManagementService bookingManagementService, IAgencyService agencyService, IGSTService gstService, IDBLoggerService dbLoggerService)
        {
            _bookingManagementService = bookingManagementService;
            _agencyService = agencyService;
            _gstService = gstService;
            _dbLoggerService = dbLoggerService;
        }   

        public async Task<Int32> SetBooking(string SearchID, string CompanyID, string UpdatedBy, bool IsCombi, bool IsRTfare, bool IsQueue, bool IsOffline, string PaymentType, string PaymentID, string PassengerResponse, string AvailabilityResponse, string RefID_O, string RefID_I, bool IsUserGateway, string GstInfo)
        {
            Int32 BookingRef = 0;
            var Error = string.Empty;
            string SearchCriteria = string.Empty;

            try
            {
                AvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);
                SearchCriteria = SearchQueryHelper.GetSearchQuery(AvailabilityResponse);
                DataSet dsAvailability = CommonFunction.StringToDataSet(AvailabilityResponse);
                DataSet dsPassenger = CommonFunction.StringToDataSet(PassengerResponse);
                DataSet dsGstInfo = CommonFunction.StringToDataSet(GstInfo);
                if (dsAvailability != null && dsPassenger != null && dsAvailability.Tables.Count > 0 && dsPassenger.Tables.Count > 0)
                {
                    bool bBAL = false;
                    bool bFLT = false;
                    bool bFARE = false;
                    bool bPAX = false;
                    bool bTRAN = false;
                    bool bFltrule = false;

                    Decimal Debit = 0;
                    Decimal Debit_SA = 0;
                    Decimal Credit_SA = 0;
                    Decimal Credit = 0;
                    Decimal Totalcfee = 0;
                    Int32 TotalAGmarkup = 0;

                    Debit = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B");
                    if (Debit > 0)
                    {
                        bool Iswallet_Cfee = await _bookingManagementService.IsWalletCfee(CompanyID, "AIRLINE");

                        
                        var objFLT = GETFlightDetailAirline(SearchID, CompanyID, 0, dsAvailability, dsPassenger.Tables[0], IsCombi, IsRTfare);
                        BookingRef = await _bookingManagementService.GetBookingRefFlightDetailAirline(SearchID, SearchCriteria, CompanyID, UpdatedBy, IsQueue, IsOffline, objFLT, Iswallet_Cfee);
                        Credit = objFLT.TotalCommission;
                        Credit_SA = objFLT.TotalCommission_SA;
                        Totalcfee = objFLT.Totalcfee;

                        TotalAGmarkup = objFLT.AG_Markup_D;
                        TotalAGmarkup += objFLT.AG_Markup_A;

                        if (BookingRef > 0)
                        {
                            if (PaymentType.ToUpper().Equals("PREPAID"))
                            {
                                if (CompanyID.IndexOf("-SA-") != -1)
                                {
                                    Debit_SA = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B2B");
                                    //Debit_SA += TotalAGmarkup;
                                    Credit += TotalAGmarkup;

                                    if (Iswallet_Cfee)
                                    {
                                        Debit_SA = Debit_SA + Totalcfee;
                                        Credit = Credit + Totalcfee;
                                    }

                                   
                                    bool bal1 = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), Debit, "D", BookingRef, UpdatedBy, "Book-AIR");
                                    bool bal2 = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CompanyID, Debit_SA, "D", BookingRef, UpdatedBy, "Book-AIR");

                                    if (bal1.Equals(true) && bal2.Equals(true))
                                    {
                                        bBAL = true;
                                    }
                                }
                                else if (CompanyID.IndexOf("A-") != -1)
                                {
                                    bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CompanyID, Debit, "D", BookingRef, UpdatedBy, "Book-AIR");
                                }
                            }
                            else
                            {
                                if (IsUserGateway.Equals(false))
                                {
                                    if (CompanyID.IndexOf("C-") != -1 || CompanyID.IndexOf("-C-") != -1)
                                    {
                                        Debit_SA = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B2C");
                                        Debit_SA = Debit_SA + Totalcfee + TotalAGmarkup;
                                        Credit = Credit + Totalcfee + TotalAGmarkup;

                                        Int32 Markup = Decimal.ToInt32(objFLT.TotalMarkup_SA);
                                        Credit = (Credit + Markup) - Credit_SA;
                                        Credit = Credit - objFLT.TotalTds;

                                       await _bookingManagementService.SetCustomerFareDetailAirline(CompanyID, BookingRef, (Debit + objFLT.TotalTds), Debit_SA, Credit, Credit_SA, Markup, objFLT.TotalTds);

                                        if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("A-") == -1)
                                        {
                                            bBAL = true;
                                        }
                                        else
                                        {
                                            if (Credit > 0)
                                            {
                                                bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), Credit, "C", BookingRef, UpdatedBy, "Book-AIR-Deal");
                                            }
                                            else if (Credit < 0)
                                            {
                                                bBAL = await _agencyService.SETDebitCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), Credit, "D", BookingRef, UpdatedBy, "Book-AIR-Deal");
                                            }
                                            else
                                            {
                                                bBAL = true;
                                            }
                                        }
                                    }
                                    else if (CompanyID.IndexOf("-SA-") != -1)
                                    {
                                        Debit_SA = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B2B");
                                        Debit_SA = Debit_SA + Totalcfee; //+ TotalAGmarkup;
                                        Credit = Credit + Totalcfee + TotalAGmarkup;

                                        if (Credit > 0)
                                        {
                                            Credit = Credit - Credit_SA;
                                            Decimal diffTds = objFLT.TotalTds - objFLT.TotalTds_SA;
                                            Credit = Credit - diffTds;

                                            bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), Credit, "C", BookingRef, UpdatedBy, "Book-AIR");
                                        }
                                        else
                                        {
                                            bBAL = true;
                                        }
                                    }
                                    else if (CompanyID.IndexOf("A-") != -1)
                                    {
                                        bBAL = true;
                                    }
                                }
                                else
                                {
                                    // client gateway
                                    if (CompanyID.IndexOf("C-") != -1 || CompanyID.IndexOf("-C-") != -1)
                                    {
                                        Debit_SA = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B2C");
                                        Debit_SA = Debit_SA + Totalcfee + TotalAGmarkup;
                                        Credit = Credit + Totalcfee + TotalAGmarkup;

                                        Int32 Markup = Decimal.ToInt32(objFLT.TotalMarkup_SA);
                                        Credit = (Credit + Markup) - Credit_SA;
                                        Credit = Credit - objFLT.TotalTds;

                                       await _bookingManagementService.SetCustomerFareDetailAirline(CompanyID, BookingRef, (Debit + objFLT.TotalTds), Debit_SA, Credit, Credit_SA, Markup, objFLT.TotalTds);

                                        if (Credit > 0)
                                        {
                                            bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), (Debit - Credit), "D", BookingRef, UpdatedBy, "Book-AIR");
                                        }
                                        else if (Credit < 0)
                                        {
                                            bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), (Debit + Credit), "D", BookingRef, UpdatedBy, "Book-AIR");
                                        }
                                        else
                                        {
                                            bBAL = true;
                                        }
                                    }
                                    else if (CompanyID.IndexOf("-SA-") != -1)
                                    {
                                        Debit_SA = SaveBookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, dsAvailability, dsPassenger, IsCombi, IsRTfare, "B2B2B");
                                        Debit_SA = Debit_SA + Totalcfee; //+ TotalAGmarkup;
                                        Credit = Credit + Totalcfee + TotalAGmarkup;

                                        Credit = Credit - Credit_SA;
                                        Decimal diffTds = objFLT.TotalTds - objFLT.TotalTds_SA;
                                        Credit = Credit - diffTds;

                                        if (Credit > 0)
                                        {
                                            bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), (Debit - Credit), "D", BookingRef, UpdatedBy, "Book-AIR");
                                        }
                                        else if (Credit < 0 || Credit.Equals(0))
                                        {
                                            bBAL = await _agencyService.SetGetCompanyAmountTransaction(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), (Debit + Credit), "D", BookingRef, UpdatedBy, "Book-AIR");
                                        }
                                    }
                                    else if (CompanyID.IndexOf("A-") != -1)
                                    {
                                        bBAL = false; //agent cant make a booking with own gateway
                                    }
                                }
                            }

                            if (bBAL.Equals(true))
                            {
                                bFltrule = await _bookingManagementService.SetFlightSegmentRuleDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, dsAvailability.Tables[0]);
                                bFLT = await _bookingManagementService.SetFlightSegmentDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, dsAvailability.Tables[0], IsCombi, IsRTfare);
                                bFARE = await SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, dsAvailability, dsPassenger, IsCombi, IsRTfare, _bookingManagementService);
                                bPAX = await _bookingManagementService.SetPaxDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, dsPassenger.Tables[0]);

                                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("-C-") != -1)
                                {

                                    bool bal1 = false;
                                    if (IsUserGateway.Equals(true))
                                    {
                                        bal1 = await _agencyService.SetTransactionDetail(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), BookingRef, Debit, Credit, "Prepaid", "", UpdatedBy, "Book-AIR", true, false);
                                    }
                                    else
                                    {
                                        bal1 = await _agencyService.SetTransactionDetail(SearchID, SearchCriteria, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), BookingRef, Debit, Credit, PaymentType, PaymentID, UpdatedBy, "Book-AIR", true, false);
                                    }

                                    bool bal2 = await _agencyService.SetTransactionDetail(SearchID, SearchCriteria, CompanyID, BookingRef, Debit_SA, Credit_SA, PaymentType, PaymentID, UpdatedBy, "Book-AIR", true, false);

                                    if (bal1.Equals(true) && bal2.Equals(true))
                                    {
                                        bTRAN = true;
                                    }
                                }
                                else
                                {
                                    if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
                                    {
                                        if (Credit.ToString().IndexOf("-") != -1)
                                        {
                                            Credit = Credit * -1;
                                        }

                                        bTRAN = await _agencyService.SetTransactionDetail(SearchID, SearchCriteria, CompanyID, BookingRef, Debit, Credit, PaymentType, PaymentID, UpdatedBy, "Book-AIR", true, false);
                                    }
                                    else
                                    {
                                        bTRAN = await _agencyService.SetTransactionDetail(SearchID, SearchCriteria, CompanyID, BookingRef, Debit, Credit, PaymentType, PaymentID, UpdatedBy, "Book-AIR", true, false);
                                    }
                                }

                                if (dsGstInfo != null && dsGstInfo.Tables.Count.Equals(1))
                                {
                                   await _gstService.SetGSTAirline(CompanyID, BookingRef, dsGstInfo.Tables[0]);
                                }
                            }
                            else
                            {
                                Error = "Booking Amount Transaction Not Found";
                            }
                        }
                        else
                        {
                            Error = "BookingRef Not Found";
                        }

                        if (bBAL.Equals(false) || bFLT.Equals(false) || bFARE.Equals(false) || bPAX.Equals(false) || bTRAN.Equals(false))
                        {
                            BookingRef = 0;
                            Error = "Error Found";
                        }
                        else
                        {
                            bool BookingStatus =  await _bookingManagementService.BookingStatus(BookingRef, true);
                        }
                    }
                    else
                    {
                        Error = "Transaction Amount Not Found";
                    }
                }
                else
                {
                    Error = "Availability or Passenger Data Not Found";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                await _dbLoggerService.dbLogg(CompanyID, BookingRef, "SetBooking", "LibraryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }

            // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "SetBooking", "STORE", AvailabilityResponse + Environment.NewLine + PassengerResponse + Environment.NewLine + RefID_O + Environment.NewLine + RefID_I + Environment.NewLine + PaymentID + Environment.NewLine + PaymentType + Environment.NewLine + UpdatedBy, SearchCriteria, SearchID);
            return BookingRef;
        }


        private static CompanyFlightFareDetailAirline GETFlightDetailAirline(string SearchID, string CompanyID, Int32 BookingRef, DataSet dsAvailability, DataTable dtPassenger, bool IsCombi, bool IsRTfare)
        {

          var ssrDetails =  SSRTotalHelper.GETCalculateSSR(SearchID, CompanyID, BookingRef, dtPassenger);

            CompanyFlightFareDetailAirline obj = new CompanyFlightFareDetailAirline();
            if (dsAvailability.Tables["AvailabilityInfo"] != null && IsCombi.Equals(false))
            {
                DataRow[] drOutbound = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                DataRow[] drInbound = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "I" + "'");
                if (drOutbound.Length > 0)
                {
                    DataRow drO = drOutbound.CopyToDataTable().Rows[0];

                    int pax = int.Parse(drO["Adt"].ToString().Trim());
                    pax += int.Parse(drO["Chd"].ToString().Trim());
                    int AG_Markup_D = 0;
                    int AG_Markup_A = 0;

                    if (dsAvailability.Tables["AvailabilityInfo"].Columns.Contains("TotalCfee"))
                    {
                        obj.Totalcfee = Convert.ToDecimal(drO["TotalCfee"].ToString());
                    }

                    if (dsAvailability.Tables["AvailabilityInfo"].Columns.Contains("AG_Markup"))
                    {
                        AG_Markup_D = pax * Convert.ToInt32(drO["AG_Markup"].ToString());
                        obj.AG_Markup_D = AG_Markup_D;
                    }

                    obj.SupplierID = (drO["AirlineID"].ToString().Trim());
                    obj.Adt = int.Parse(drO["Adt"].ToString().Trim());
                    obj.Chd = int.Parse(drO["Chd"].ToString().Trim());
                    obj.Inf = int.Parse(drO["Inf"].ToString().Trim());
                    obj.Sector = drO["Sector"].ToString();
                    obj.Trip = drO["Trip"].ToString();
                    obj.Origin = drO["Origin"].ToString();
                    obj.Destination = drO["Destination"].ToString();
                    obj.PriceType_D = drO["PriceType"].ToString();

                    if (drO["Trip"].ToString().Equals("M"))
                    {
                        obj.Destination = dsAvailability.Tables["AvailabilityInfo"].Rows[dsAvailability.Tables["AvailabilityInfo"].Rows.Count - 1]["Destination"].ToString().Trim();
                    }

                    obj.CarrierCode_D = drO["CarrierCode"].ToString();
                    obj.DepartureDate_D = drO["DepartureDate"].ToString();

                    obj.TotalTax = Convert.ToDecimal(drO["TotalTax"].ToString());
                    obj.TotalBasic = Convert.ToDecimal(drO["TotalBasic"].ToString());

                    Decimal dYq = int.Parse(drO["Adt"].ToString().Trim()) * Convert.ToDecimal(drO["Adt_YQ"].ToString());
                    if (int.Parse(drO["Chd"].ToString().Trim()) > 0)
                    {
                        dYq += int.Parse(drO["Chd"].ToString().Trim()) * Convert.ToDecimal(drO["Chd_YQ"].ToString());
                    }

                    obj.TotalYq = dYq;
                    obj.TotalFare = Convert.ToDecimal(drO["TotalFare"].ToString());
                    obj.TotalServiceTax = Convert.ToDecimal(drO["TotalServiceTax"].ToString());

                    if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                    {
                        obj.TotalMarkup = 0;
                        obj.TotalFare = Convert.ToDecimal(drO["TotalFare"].ToString()) - Convert.ToDecimal(drO["TotalMarkup"].ToString());
                        obj.TotalFare -= AG_Markup_D;
                    }
                    else
                    {
                        obj.TotalMarkup = Convert.ToDecimal(drO["TotalMarkup"].ToString());
                    }

                    obj.TotalCommission = Convert.ToDecimal(drO["TotalCommission"].ToString());
                    obj.TotalServiceFee = Convert.ToDecimal(drO["TotalServiceFee"].ToString());
                    obj.TotalTds = Convert.ToDecimal(drO["TotalTds"].ToString());

                    obj.TotalMeal = ssrDetails.dTotalMeal;
                    obj.TotalBaggage = ssrDetails.dTotalBaggage;

                    obj.TotalQueue = Convert.ToDecimal(drO["TotalImport"].ToString());
                    obj.TotalBasic_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_BAS"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_BAS"].ToString());
                    obj.TotalYQ_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_Y"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_Y"].ToString());
                    obj.TotalCB_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_CB"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_CB"].ToString());

                    if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
                    {
                        obj.TotalPromo_deal = 0;
                        obj.TotalPromo_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_PR"].ToString());
                    }
                    else
                    {
                        obj.TotalPromo_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_PR"].ToString());
                    }

                    if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("-C-") != -1 || CompanyID.IndexOf("C-") != -1)
                    {
                        obj.TotalFare_SA = (Convert.ToDecimal(drO["TotalFare"].ToString()) + Convert.ToDecimal(drO["TotalTds_SA"].ToString())) - Convert.ToDecimal(drO["TotalTds"].ToString());
                        obj.TotalMarkup_SA = Convert.ToDecimal(drO["TotalMarkup"].ToString());
                        obj.TotalCommission_SA = Convert.ToDecimal(drO["TotalCommission_SA"].ToString());
                        obj.TotalTds_SA = Convert.ToDecimal(drO["TotalTds_SA"].ToString());
                        Hashtable HTdeal = GetSAdeal(drO["SA_deal"].ToString());
                        if (HTdeal.Count > 9)
                        {
                            obj.TotalBasic_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_BAS"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_BAS"].ToString());
                            obj.TotalYQ_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_Y"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_Y"].ToString());
                            obj.TotalCB_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_CB"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_CB"].ToString());
                            obj.TotalPromo_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_PR"].ToString());
                        }
                    }

                    if (drInbound.Length > 0)
                    {
                        DataRow drI = drInbound.CopyToDataTable().Rows[0];
                        obj.CarrierCode_A = drI["CarrierCode"].ToString();
                        obj.DepartureDate_A = drI["DepartureDate"].ToString();
                        obj.TotalQueue += Convert.ToDecimal(drI["TotalImport"].ToString());
                        obj.PriceType_A = drI["PriceType"].ToString();

                        if (IsRTfare.Equals(false))
                        {
                            dYq = int.Parse(drO["Adt"].ToString().Trim()) * Convert.ToDecimal(drI["Adt_YQ"].ToString());
                            if (int.Parse(drO["Chd"].ToString().Trim()) > 0)
                            {
                                dYq += int.Parse(drO["Chd"].ToString().Trim()) * Convert.ToDecimal(drI["Chd_YQ"].ToString());
                            }

                            obj.TotalYq += dYq;

                            if (dsAvailability.Tables["AvailabilityInfo"].Columns.Contains("AG_Markup"))
                            {
                                AG_Markup_A = pax * Convert.ToInt32(drI["AG_Markup"].ToString());
                                obj.AG_Markup_A = AG_Markup_A;
                            }

                            obj.TotalTax += Convert.ToDecimal(drI["TotalTax"].ToString());
                            obj.TotalBasic += Convert.ToDecimal(drI["TotalBasic"].ToString());

                            obj.TotalServiceTax += Convert.ToDecimal(drI["TotalServiceTax"].ToString());
                            obj.TotalCommission += Convert.ToDecimal(drI["TotalCommission"].ToString());
                            obj.TotalServiceFee += Convert.ToDecimal(drI["TotalServiceFee"].ToString());
                            obj.TotalTds += Convert.ToDecimal(drI["TotalTds"].ToString());
                            obj.TotalBasic_deal += Convert.ToInt32(drI["Adt"].ToString()) * Convert.ToDecimal(drI["Adt_BAS"].ToString()) + Convert.ToInt32(drI["Chd"].ToString()) * Convert.ToDecimal(drI["Chd_BAS"].ToString());
                            obj.TotalYQ_deal += Convert.ToInt32(drI["Adt"].ToString()) * Convert.ToDecimal(drI["Adt_Y"].ToString()) + Convert.ToInt32(drI["Chd"].ToString()) * Convert.ToDecimal(drI["Chd_Y"].ToString());
                            obj.TotalCB_deal += Convert.ToInt32(drI["Adt"].ToString()) * Convert.ToDecimal(drI["Adt_CB"].ToString()) + Convert.ToInt32(drI["Chd"].ToString()) * Convert.ToDecimal(drI["Chd_CB"].ToString());

                            if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
                            {
                                obj.TotalPromo_deal = 0;
                                obj.TotalPromo_deal_SA += Convert.ToInt32(drI["Adt"].ToString()) * Convert.ToDecimal(drI["Adt_PR"].ToString()) + Convert.ToInt32(drI["Chd"].ToString()) * Convert.ToDecimal(drI["Chd_PR"].ToString());
                            }
                            else
                            {
                                obj.TotalPromo_deal += Convert.ToInt32(drI["Adt"].ToString()) * Convert.ToDecimal(drI["Adt_PR"].ToString()) + Convert.ToInt32(drI["Chd"].ToString()) * Convert.ToDecimal(drI["Chd_PR"].ToString());
                            }

                            if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("-C-") != -1 || CompanyID.IndexOf("C-") != -1)
                            {
                                obj.TotalFare_SA += (Convert.ToDecimal(drI["TotalFare"].ToString()) + Convert.ToDecimal(drI["TotalTds_SA"].ToString())) - Convert.ToDecimal(drI["TotalTds"].ToString());
                                obj.TotalMarkup_SA += Convert.ToDecimal(drI["TotalMarkup"].ToString());
                                obj.TotalCommission_SA += Convert.ToDecimal(drI["TotalCommission_SA"].ToString());
                                obj.TotalTds_SA += Convert.ToDecimal(drI["TotalTds_SA"].ToString());
                                Hashtable HTdeal = GetSAdeal(drI["SA_deal"].ToString());
                                if (HTdeal.Count > 9)
                                {
                                    obj.TotalBasic_deal_SA += Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_BAS"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_BAS"].ToString());
                                    obj.TotalYQ_deal_SA += Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_Y"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_Y"].ToString());
                                    obj.TotalCB_deal_SA += Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_CB"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_CB"].ToString());
                                    obj.TotalPromo_deal_SA += Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_PR"].ToString());
                                }
                            }

                            if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                            {
                                obj.TotalMarkup += 0;
                                obj.TotalFare += (Convert.ToDecimal(drI["TotalFare"].ToString()) - Convert.ToDecimal(drI["TotalMarkup"].ToString()));
                                obj.TotalFare -= AG_Markup_A;
                            }
                            else
                            {
                                obj.TotalFare += Convert.ToDecimal(drI["TotalFare"].ToString());
                                obj.TotalMarkup += Convert.ToDecimal(drI["TotalMarkup"].ToString());
                            }
                        }
                    }
                }
            }
            else if (dsAvailability.Tables["AvailabilityInfo"] != null && IsCombi.Equals(true))
            {
                DataRow[] drOutbound = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                DataRow[] drInbound = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "I" + "'");
                if (drOutbound.Length > 0)
                {
                    DataRow drO = drOutbound.CopyToDataTable().Rows[0];

                    int pax = int.Parse(drO["Adt"].ToString().Trim());
                    pax += int.Parse(drO["Chd"].ToString().Trim());
                    int AG_Markup_D = 0;

                    if (dsAvailability.Tables["AvailabilityInfo"].Columns.Contains("TotalCfee"))
                    {
                        obj.Totalcfee = Convert.ToDecimal(drO["TotalCfee"].ToString());
                    }

                    if (dsAvailability.Tables["AvailabilityInfo"].Columns.Contains("AG_Markup"))
                    {
                        AG_Markup_D = pax * Convert.ToInt32(drO["AG_Markup"].ToString());
                        obj.AG_Markup_D = AG_Markup_D;
                    }

                    obj.SupplierID = (drO["AirlineID"].ToString().Trim());
                    obj.Adt = int.Parse(drO["Adt"].ToString().Trim());
                    obj.Chd = int.Parse(drO["Chd"].ToString().Trim());
                    obj.Inf = int.Parse(drO["Inf"].ToString().Trim());
                    obj.Sector = drO["Sector"].ToString();
                    obj.Trip = drO["Trip"].ToString();
                    obj.Origin = drO["Origin"].ToString();
                    obj.Destination = drO["Destination"].ToString();
                    obj.CarrierCode_D = drO["CarrierCode"].ToString();
                    obj.DepartureDate_D = drO["DepartureDate"].ToString();

                    obj.TotalTax = Convert.ToDecimal(drO["TotalTax"].ToString());
                    obj.TotalBasic = Convert.ToDecimal(drO["TotalBasic"].ToString());

                    Decimal dYq = int.Parse(drO["Adt"].ToString().Trim()) * Convert.ToDecimal(drO["Adt_YQ"].ToString());
                    if (int.Parse(drO["Chd"].ToString().Trim()) > 0)
                    {
                        dYq += int.Parse(drO["Chd"].ToString().Trim()) * Convert.ToDecimal(drO["Chd_YQ"].ToString());
                    }

                    obj.TotalYq = dYq;
                    obj.TotalFare = Convert.ToDecimal(drO["TotalFare"].ToString());

                    if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                    {
                        obj.TotalMarkup = 0;
                        obj.TotalFare = Convert.ToDecimal(drO["TotalFare"].ToString()) - Convert.ToDecimal(drO["TotalMarkup"].ToString());
                        obj.TotalFare -= AG_Markup_D;
                    }
                    else
                    {
                        obj.TotalMarkup = Convert.ToDecimal(drO["TotalMarkup"].ToString());
                    }

                    obj.TotalServiceTax = Convert.ToDecimal(drO["TotalServiceTax"].ToString());
                    obj.TotalCommission = Convert.ToDecimal(drO["TotalCommission"].ToString());
                    obj.TotalServiceFee = Convert.ToDecimal(drO["TotalServiceFee"].ToString());
                    obj.TotalTds = Convert.ToDecimal(drO["TotalTds"].ToString());
                    obj.TotalQueue = Convert.ToDecimal(drO["TotalImport"].ToString());

                    obj.TotalMeal = ssrDetails.dTotalMeal;
                    obj.TotalBaggage = ssrDetails.dTotalBaggage;

                    obj.TotalBasic_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_BAS"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_BAS"].ToString());
                    obj.TotalYQ_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_Y"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_Y"].ToString());
                    obj.TotalCB_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_CB"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_CB"].ToString());

                    if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
                    {
                        obj.TotalPromo_deal = 0;
                        obj.TotalPromo_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_PR"].ToString());
                    }
                    else
                    {
                        obj.TotalPromo_deal = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(drO["Adt_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(drO["Chd_PR"].ToString());
                    }

                    if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("-C-") != -1 || CompanyID.IndexOf("C-") != -1)
                    {
                        obj.TotalFare_SA = (Convert.ToDecimal(drO["TotalFare"].ToString()) + Convert.ToDecimal(drO["TotalTds_SA"].ToString())) - Convert.ToDecimal(drO["TotalTds"].ToString());
                        obj.TotalMarkup_SA = Convert.ToDecimal(drO["TotalMarkup"].ToString());
                        obj.TotalCommission_SA = Convert.ToDecimal(drO["TotalCommission_SA"].ToString());
                        obj.TotalTds_SA = Convert.ToDecimal(drO["TotalTds_SA"].ToString());
                        Hashtable HTdeal = GetSAdeal(drO["SA_deal"].ToString());
                        if (HTdeal.Count > 9)
                        {
                            obj.TotalBasic_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_BAS"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_BAS"].ToString());
                            obj.TotalYQ_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_Y"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_Y"].ToString());
                            obj.TotalCB_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_CB"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_CB"].ToString());
                            obj.TotalPromo_deal_SA = Convert.ToInt32(drO["Adt"].ToString()) * Convert.ToDecimal(HTdeal["ADT_PR"].ToString()) + Convert.ToInt32(drO["Chd"].ToString()) * Convert.ToDecimal(HTdeal["CHD_PR"].ToString());
                        }
                    }
                    if (drInbound.Length > 0)
                    {
                        DataRow drI = drInbound.CopyToDataTable().Rows[0];
                        obj.CarrierCode_A = drI["CarrierCode"].ToString();
                        obj.DepartureDate_A = drI["DepartureDate"].ToString();
                        obj.TotalQueue += Convert.ToDecimal(drI["TotalImport"].ToString());
                        obj.TotalMarkup += 0;
                    }
                }
            }

            return obj;
        }
        private static CompanyFlightFareDetailAirline GetFareDetailAirline(DataTable dtBound, bool IsCombi, string CompanyID)
        {
            CompanyFlightFareDetailAirline obj = new CompanyFlightFareDetailAirline();

            DataRow drSelect = dtBound.Rows[0];
            obj.SupplierID = (drSelect["AirlineID"].ToString().Trim());
            obj.Adt = int.Parse(drSelect["Adt"].ToString().Trim());
            obj.Chd = int.Parse(drSelect["Chd"].ToString().Trim());
            obj.Inf = int.Parse(drSelect["Inf"].ToString().Trim());

            obj.Sector = drSelect["Sector"].ToString();
            obj.Trip = drSelect["Trip"].ToString();
            obj.Origin = drSelect["Origin"].ToString();
            obj.Destination = drSelect["Destination"].ToString();

            obj.CarrierCode_D = drSelect["CarrierCode"].ToString();
            obj.DepartureDate_D = drSelect["DepartureDate"].ToString();

            obj.TotalTax = Convert.ToDecimal(drSelect["TotalTax"].ToString());
            obj.TotalBasic = Convert.ToDecimal(drSelect["TotalBasic"].ToString());

            Decimal dYq = int.Parse(drSelect["Adt"].ToString().Trim()) * Convert.ToDecimal(drSelect["Adt_YQ"].ToString());
            if (int.Parse(drSelect["Chd"].ToString().Trim()) > 0)
            {
                dYq += int.Parse(drSelect["Chd"].ToString().Trim()) * Convert.ToDecimal(drSelect["Chd_YQ"].ToString());
            }
            obj.TotalYq = dYq;

            obj.TotalFare = Convert.ToDecimal(drSelect["TotalFare"].ToString());

            obj.TotalServiceTax = Convert.ToDecimal(drSelect["TotalServiceTax"].ToString());

            if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
            {
                obj.TotalMarkup = 0;
                obj.TotalFare = Convert.ToDecimal(drSelect["TotalFare"].ToString()) - Convert.ToDecimal(drSelect["TotalMarkup"].ToString());
            }
            else
            {
                obj.TotalMarkup = Convert.ToDecimal(drSelect["TotalMarkup"].ToString());
            }

            obj.TotalCommission = Convert.ToDecimal(drSelect["TotalCommission"].ToString());
            obj.TotalServiceFee = Convert.ToDecimal(drSelect["TotalServiceFee"].ToString());
            obj.TotalTds = Convert.ToDecimal(drSelect["TotalTds"].ToString());

            //obj.TotalSeat = Convert.ToDecimal(drSelect["TotalSeat"].ToString());
            //obj.TotalMeal = Convert.ToDecimal(drSelect["TotalMeal"].ToString());
            //obj.TotalBaggage = Convert.ToDecimal(drSelect["TotalBaggage"].ToString());

            obj.TotalQueue = Convert.ToDecimal(drSelect["TotalImport"].ToString());
            obj.TotalBasic_deal = Convert.ToInt32(drSelect["Adt"].ToString()) * Convert.ToDecimal(drSelect["Adt_BAS"].ToString()) + Convert.ToInt32(drSelect["Chd"].ToString()) * Convert.ToDecimal(drSelect["Chd_BAS"].ToString());
            obj.TotalYQ_deal = Convert.ToInt32(drSelect["Adt"].ToString()) * Convert.ToDecimal(drSelect["Adt_Y"].ToString()) + Convert.ToInt32(drSelect["Chd"].ToString()) * Convert.ToDecimal(drSelect["Chd_Y"].ToString());
            obj.TotalCB_deal = Convert.ToInt32(drSelect["Adt"].ToString()) * Convert.ToDecimal(drSelect["Adt_CB"].ToString()) + Convert.ToInt32(drSelect["Chd"].ToString()) * Convert.ToDecimal(drSelect["Chd_CB"].ToString());

            if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
            {
                obj.TotalPromo_deal = 0;
            }
            else
            {
                obj.TotalPromo_deal = Convert.ToInt32(drSelect["Adt"].ToString()) * Convert.ToDecimal(drSelect["Adt_PR"].ToString()) + Convert.ToInt32(drSelect["Chd"].ToString()) * Convert.ToDecimal(drSelect["Chd_PR"].ToString());
            }

            return obj;
        }

        private static Hashtable GetSAdeal(string SAdeal)
        {
            //6,127,0,0,0*0,0,0,0,0
            Hashtable HTdeal = new Hashtable();
            if (SAdeal.Length > 0 && SAdeal.IndexOf("*") != -1)
            {
                string[] split = SAdeal.Split('*');
                if (split.Length.Equals(2))
                {
                    string[] split1 = split[0].ToString().Split(',');
                    if (split1.Length.Equals(5))
                    {
                        HTdeal.Add("ADT_TDS", split1[0].ToString());
                        HTdeal.Add("ADT_BAS", split1[1].ToString());
                        HTdeal.Add("ADT_Y", split1[2].ToString());
                        HTdeal.Add("ADT_CB", split1[3].ToString());
                        HTdeal.Add("ADT_PR", split1[4].ToString());
                    }

                    string[] split2 = split[1].ToString().Split(',');
                    if (split2.Length.Equals(5))
                    {
                        HTdeal.Add("CHD_TDS", split2[0].ToString());
                        HTdeal.Add("CHD_BAS", split2[1].ToString());
                        HTdeal.Add("CHD_Y", split2[2].ToString());
                        HTdeal.Add("CHD_CB", split2[3].ToString());
                        HTdeal.Add("CHD_PR", split2[4].ToString());
                    }
                }
            }
            return HTdeal;
        }

        public static async Task<bool> SetFareDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataSet dsAvailability, DataSet dsPassenger, bool IsCombi, bool IsRTfare, IBookingManagementService bookingManagementService)
        {
            bool Status = false;

            try
            {
                Decimal dTotalMeal_O = 0;
                Decimal dTotalMeal_I = 0;
                Decimal dTotalBaggage_O = 0;
                Decimal dTotalBaggage_I = 0;

                var objSSR = SSRTotalHelper.GETCalculateSSR(SearchID, CompanyID, BookingRef, dsPassenger.Tables[0]);

                dTotalMeal_O = objSSR.dTotalMealAdt_O + objSSR.dTotalMealChd_O;
                dTotalMeal_I = objSSR.dTotalMealAdt_I + objSSR.dTotalMealChd_I;
                dTotalBaggage_O = objSSR.dTotalBaggageAdt_O + objSSR.dTotalBaggageChd_O;
                dTotalBaggage_I = objSSR.dTotalBaggageAdt_I + objSSR.dTotalBaggageChd_I;

                if (dsAvailability.Tables[0].Rows[0]["Trip"].ToString().Trim().Equals("R"))
                {
                    if (IsCombi.Equals(true))
                    {
                        DataRow[] drO = dsAvailability.Tables[0].Select("FltType='" + "O" + "'");
                        DataRow[] drI = dsAvailability.Tables[0].Select("FltType='" + "I" + "'");

                        bool IsSingleFare = true;
                        //bool IsLCC = dbCommon.CommonFunction.IsLCC(drO.CopyToDataTable().Rows[0]["CarrierCode"].ToString()).Equals(true);
                        //if (IsLCC.Equals(true))
                        //{
                        //    IsSingleFare = false;
                        //}

                        
                        var objFare1 = GetFareDetailAirline(drO.CopyToDataTable(), IsCombi, CompanyID);
                        var objFare2 = GetFareDetailAirline(drI.CopyToDataTable(), IsCombi, CompanyID);

                        Status = await bookingManagementService.SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", dTotalMeal_O, dTotalBaggage_O, 0, objFare1, false);
                        if (Status.Equals(true))
                        {
                            Status = await bookingManagementService.SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", dTotalMeal_I, dTotalBaggage_I, 0, objFare2, IsSingleFare);
                        }
                    }
                    else
                    {
                        DataRow[] drO = dsAvailability.Tables[0].Select("FltType='" + "O" + "'");
                        DataRow[] drI = dsAvailability.Tables[0].Select("FltType='" + "I" + "'");

                        
                        var objFare1 = GetFareDetailAirline(drO.CopyToDataTable(), IsCombi, CompanyID);
                        var objFare2 = GetFareDetailAirline(drI.CopyToDataTable(), IsCombi, CompanyID);

                        Status = await bookingManagementService.SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", dTotalMeal_O, dTotalBaggage_O, 0, objFare1, false);
                        if (Status.Equals(true))
                        {
                            bool IsSingleFare = false;
                            if (IsRTfare.Equals(true))
                            {
                                IsSingleFare = true;
                            }

                            Status = await bookingManagementService.SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", dTotalMeal_I, dTotalBaggage_I, 0, objFare2, IsSingleFare);
                        }
                    }
                }
                else
                {
                    
                    var objFare = GetFareDetailAirline(dsAvailability.Tables[0], IsCombi, CompanyID);
                    Status = await bookingManagementService.SetFareDetailAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", dTotalMeal_O, dTotalBaggage_O, 0, objFare, false);
                }

                if (Status.Equals(true))
                {
                    Status = await SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, dsAvailability, dsPassenger.Tables[0], IsCombi, IsRTfare, bookingManagementService);
                }
            }
            catch (Exception ex)
            {
               // await  _dbLoggerService.dbLogg(CompanyID, BookingRef, "SET_Fare_Detail_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }
            return Status;
        }

        private static async Task<bool> SetFareDetailSegmentAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataSet dsAvailability, DataTable dtPassenger, bool IsCombi, bool IsRTfare, IBookingManagementService bookingManagementService)
        {
            bool Status = false;
            var objSSR  = SSRTotalHelper.GETCalculateSSR(SearchID, CompanyID, BookingRef, dtPassenger);
            if (dsAvailability.Tables["AvailabilityInfo"] != null)
            {
                if (IsCombi.Equals(true))
                {
                    DataRow[] dr1 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                    DataRow[] dr2 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "I" + "'");
                    DataRow drOutbound = dr1.CopyToDataTable().Rows[0];
                    DataRow drInbound = dr2.CopyToDataTable().Rows[0];

                    bool IsSingleFare = true;
                   

                    Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "ADT", objSSR.dTotalMealAdt_O, 0, objSSR.dTotalBaggageAdt_O, drOutbound, false);
                    if (Convert.ToInt32(drOutbound["Chd"].ToString().Trim()) > 0)
                    {
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "CHD", objSSR.dTotalMealChd_O, 0, objSSR.dTotalBaggageChd_O, drOutbound, false);
                    }
                    if (Convert.ToInt32(drOutbound["Inf"].ToString().Trim()) > 0)
                    {
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "INF", 0, 0, 0, drOutbound, false);
                    }
                    //-------------------------------------------------------------------------------------------------------------------------
                    Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "ADT", objSSR.dTotalMealAdt_I, 0, objSSR.dTotalBaggageAdt_I, drInbound, IsSingleFare);
                    if (Convert.ToInt32(drInbound["Chd"].ToString().Trim()) > 0)
                    {
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "CHD", objSSR.dTotalMealChd_I, 0, objSSR.dTotalBaggageChd_I, drInbound, IsSingleFare);
                    }

                    if (Convert.ToInt32(drInbound["Inf"].ToString().Trim()) > 0)
                    {
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "INF", 0, 0, 0, drInbound, IsSingleFare);
                    }
                }
                else
                {
                    bool IsSingleFare = false;
                    if (IsRTfare.Equals(true))
                    {
                        IsSingleFare = true;
                    }

                    DataRow[] dr1 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                    DataRow[] dr2 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "I" + "'");
                    if (dr1.Length > 0)
                    {
                        DataRow drOutbound = dr1.CopyToDataTable().Rows[0];
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "ADT", objSSR.dTotalMealAdt_O, 0, objSSR.dTotalBaggageAdt_O, drOutbound, false);
                        if (Convert.ToInt32(drOutbound["Chd"].ToString().Trim()) > 0)
                        {
                            Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "CHD", objSSR.dTotalMealChd_O, 0, objSSR.dTotalBaggageChd_O, drOutbound, false);
                        }
                        if (Convert.ToInt32(drOutbound["Inf"].ToString().Trim()) > 0)
                        {
                            Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "O", "INF", 0, 0, 0, drOutbound, false);
                        }
                    }

                    if (dr2.Length > 0)
                    {
                        DataRow drInbound = dr2.CopyToDataTable().Rows[0];
                        Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "ADT", objSSR.dTotalMealAdt_I, 0, objSSR.dTotalBaggageAdt_I, drInbound, IsSingleFare);
                        if (Convert.ToInt32(drInbound["Chd"].ToString().Trim()) > 0)
                        {
                            Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "CHD", objSSR.dTotalMealChd_I, 0, objSSR.dTotalBaggageChd_I, drInbound, IsSingleFare);
                        }
                        if (Convert.ToInt32(drInbound["Inf"].ToString().Trim()) > 0)
                        {
                            Status = await bookingManagementService.SetFareDetailSegmentAirline(SearchID, SearchCriteria, CompanyID, BookingRef, "I", "INF", 0, 0, 0, drInbound, IsSingleFare);
                        }
                    }
                }
            }

            return Status;
        }


    }
}
