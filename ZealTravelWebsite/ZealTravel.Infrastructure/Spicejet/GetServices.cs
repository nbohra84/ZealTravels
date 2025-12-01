using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZealTravel.Infrastructure.DBCommon;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.FlightManagement.Spicejet;
using svc_booking;
using CommonComponents;

namespace ZealTravel.Infrastructure.Spicejet
{ 
    public class GetServices : IGetServices
    {

    IBookingManagementService _bookingService;

    public GetServices(IBookingManagementService bookingService)
    {
        _bookingService = bookingService;
    }


    //AVAILABILITY============================================================================================================================================================================
    public string GetFlights(string JourneyType, string Supplierid, string Password, string PriceType, string Searchid, string Companyid, string AirRQ)
        {
            GetRequestData objRequestData = new GetRequestData();
            string Sector = "";
            if (JourneyType != "MC")
            {

               // objRequestData.GetSector(AirRQ);
                objRequestData.getAvailabilityRequest(AirRQ);
            }
            string Origin = objRequestData.DepartureStation;
            string Destination = objRequestData.ArrivalStation;
            string BeginDate = objRequestData.BeginDate;
            string EndDate = objRequestData.EndDate;
            int Adt = int.Parse(objRequestData.Adt==null?"0":objRequestData.Adt);
            int Chd = int.Parse(objRequestData.Chd==null?"0": objRequestData.Chd);
            int Inf = int.Parse(objRequestData.Inf==null?"0": objRequestData.Inf);


            if (JourneyType.Equals("MC"))
            {
               // Sector = objRequestData.GetSector(AirRQ);
                //objRequestData.getAvailabilityRequest(AirRQ, true, Sector);
                //objRequestData.getAvailabilityRequest(AirRQ, Sector);
                objRequestData.getAvailabilityRequest(AirRQ);

                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                //return objAvailability.GetOneWayMC(objRequestData.availabilityRequestVOList_);
                return objAvailability.GetOneWay(Origin, Destination, BeginDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("OW"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return objAvailability.GetOneWay(Origin, Destination, BeginDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RW"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return objAvailability.GetDomesticRoundWay(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RTLCC") && Sector.Equals("D"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return objAvailability.GetDomesticRT(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RT") && Sector.Equals("I"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return objAvailability.GetInternationalRT(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            return string.Empty;
        }
     public async Task<string> GetFlightsAsync(string JourneyType, string Supplierid, string Password, string PriceType, string Searchid, string Companyid, string AirRQ, string sector)
        {
            GetRequestData objRequestData = new GetRequestData();
            string Sector = sector;
            if (JourneyType != "MC")
            {

               // objRequestData.GetSector(AirRQ);
                objRequestData.getAvailabilityRequest(AirRQ);
            }
            string Origin = objRequestData.DepartureStation;
            string Destination = objRequestData.ArrivalStation;
            string BeginDate = objRequestData.BeginDate;
            string EndDate = objRequestData.EndDate;
            int Adt = int.Parse(objRequestData.Adt == null ? "0" : objRequestData.Adt);
            int Chd = int.Parse(objRequestData.Chd == null ? "0" : objRequestData.Chd);
            int Inf = int.Parse(objRequestData.Inf == null ? "0" : objRequestData.Inf);


            if (JourneyType.Equals("MC"))
            {
                //Sector = objRequestData.GetSector(AirRQ);
                //objRequestData.getAvailabilityRequest(AirRQ, true, Sector);
                //objRequestData.getAvailabilityRequest(AirRQ, Sector);

                objRequestData.getAvailabilityRequest(AirRQ);

                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                //return objAvailability.GetOneWayMC(objRequestData.availabilityRequestVOList_);
                return await objAvailability.GetOneWayAsync(Origin, Destination, BeginDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("OW"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return await objAvailability.GetOneWayAsync(Origin, Destination, BeginDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RW"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return await objAvailability.GetDomesticRoundWayAsync(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RTLCC") && Sector.Equals("D"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return await objAvailability.GetDomesticRTAsync(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            else if (JourneyType.Equals("RT") && Sector.Equals("I"))
            {
                GetAvailability objAvailability = new GetAvailability(Supplierid, Password, PriceType, Searchid, Companyid);
                return await objAvailability.GetInternationalRTAsync(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            }
            return string.Empty;
        }

        //FARE RULE=======================================================================================================================================================================
        public async Task<string> GetFareRuleAsync(string JourneyType, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetApiFlightFareRule objFlightFareRule = new GetApiFlightFareRule(Searchid, Companyid, 0);
                    return objFlightFareRule.GetFareRule(dtBound);
                }
            }
            return string.Empty;
        }

        //FARE UPDATE=====================================================================================================================================================================
        public async Task<string> GetFareAsync(string JourneyType, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
                {
                    GetFare objFare = new GetFare();
                    return objFare.GetFareRT(Searchid, Companyid, AirRS);
                }
                else
                {
                    GetFare objFare = new GetFare();
                    return objFare.GetFareOneWay(Searchid, Companyid, AirRS);
                }
            }
            return string.Empty;
        }

        ////GST UPDATE======================================================================================================================================================================
        //public bool GetGstUpdate(string Searchid, string Companyid, string AirRS, string PassengerRS, string CompanyRS, string GstRS)
        //{
        //    if (AirRS != null && PassengerRS != null && CompanyRS != null && AirRS.IndexOf("RefID") != -1
        //      && PassengerRS.IndexOf("PassengerInfo") != -1 && CompanyRS.IndexOf("CompanyInfo") != -1)
        //    {
        //        DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
        //        DataTable dtPassengerInfo = GetCommonFunctions.StringToDataSet(PassengerRS).Tables["PassengerInfo"];
        //        DataTable dtComppanyInfo = GetCommonFunctions.StringToDataSet(CompanyRS).Tables["CompanyInfo"];

        //        DataTable dtGstInfo = new DataTable();
        //        if (GstRS != null && GstRS.IndexOf("GstInfo") != -1)
        //        {
        //            dtGstInfo = GetCommonFunctions.StringToDataSet(GstRS).Tables["GstInfo"];
        //        }

        //        if (dtBound != null && dtBound.Rows.Count > 0 && dtPassengerInfo != null && dtPassengerInfo.Rows.Count > 0 && dtComppanyInfo != null && dtComppanyInfo.Rows.Count > 0)
        //        {
        //            GetApiUpdateContacts objBooking = new GetApiUpdateContacts(Searchid, Companyid, 0);
        //            return objBooking.GetUpdateContacts(dtBound, dtPassengerInfo, dtComppanyInfo, dtGstInfo);
        //        }
        //    }
        //    return false;
        //}

        ////SSR ======================================================================================================================================================================
        public async Task<string> GetSSRAsync(string JourneyType, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
                {
                    GetAddOn objSell = new GetAddOn();
                    return objSell.GetSSRRoundWay(Searchid, Companyid, AirRS);
                }
                else
                {
                    GetAddOn objSell = new GetAddOn();
                    return objSell.GetSSROneWay(Searchid, Companyid, AirRS);
                }
            }
            return string.Empty;
        }

        ////SELL UPDATE======================================================================================================================================================================
        //public Decimal GetSell(string JourneyType, string Searchid, string Companyid, string AirRS)
        //{
        //    if (AirRS != null && AirRS.IndexOf("RefID") != -1)
        //    {
        //        if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
        //        {
        //            GetSell objSell = new GetSell();
        //            return objSell.GetSellRoundWay(Searchid, Companyid, AirRS);
        //        }
        //        else
        //        {
        //            GetSell objSell = new GetSell();
        //            return objSell.GetSellOneWay(Searchid, Companyid, AirRS);
        //        }
        //    }
        //    return 0;
        //}
        ////SELL REMOVE======================================================================================================================================================================
        //public bool GetClearSell(string Searchid, string Companyid, string AirRS)
        //{
        //    if (AirRS != null && AirRS.IndexOf("RefID") != -1)
        //    {
        //        DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
        //        if (dtBound != null && dtBound.Rows.Count > 0)
        //        {
        //            GetApiSell objSell = new GetApiSell(Searchid);
        //            objSell.ClearSellFromApi(420, dtBound.Rows[0]["Api_SessionID"].ToString());
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ////COMMIT ==========================================================================================================================================================================
        public async Task<bool> GetCommitAsync(string JourneyType, string Searchid, string Companyid, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS,string PaymentType)
        {
            
            if (AirRS != null && PassengerRS != null && CompanyRS != null && AirRS.IndexOf("RefID") != -1
                && PassengerRS.IndexOf("PassengerInfo") != -1 && CompanyRS.IndexOf("CompanyInfo") != -1)
            {
                // dbCommon.Logger.dbLogg(Companyid, BookingRef, "GetCommit", "air_spicejet", AirRS, Searchid, PassengerRS);

                if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
                {
                    GetCommit objBooking = new GetCommit();
                    return await objBooking.GetCommitRT(Searchid, Companyid, BookingRef, AirRS, PassengerRS, CompanyRS, GstRS, _bookingService,PaymentType);
                }
                else
                {
                    GetCommit objBooking = new GetCommit();
                    return await objBooking.GetCommitOneWay(Searchid, Companyid, BookingRef, AirRS, PassengerRS, CompanyRS, GstRS, _bookingService,PaymentType);
                }
            }
            return false;
        }
        //BOOKINGS ==========================================================================================================================================================================
        public async Task<string> GetBookingByRecordLocatorAsync(string Searchid, string Supplierid, string Password, string RecordLocator)
        {
            GetApiBookings objApiBookings = new GetApiBookings(Searchid, "", 0);
            return  objApiBookings.GetBookingData(Supplierid, Password, RecordLocator);
        }
        public async Task<string> GetBookingByRecordLocatorAsync(string Searchid, string Signature, string RecordLocator)
        {
            GetApiBookings objApiBookings = new GetApiBookings(Searchid, "", 0);
            return objApiBookings.GetBookingData(Signature, RecordLocator);
        }

        //Hold Bookings =====================================================================================================================================================================
        public Task<bool> GetCommitForHoldBookingAsync(string Supplierid, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, Decimal TotalApiFare, string RecordLocator, int Paxcount,string FltType)
        {
            GetConfirmForHoldBooking confirmForHoldBooking = new GetConfirmForHoldBooking(_bookingService);
            return confirmForHoldBooking.GetConfirmForHoldBookingResponse(Supplierid, NetworkPassword, SearchID,  BookingRef, CompanyID, TotalApiFare, RecordLocator, Paxcount,FltType);

        }
    }
}

