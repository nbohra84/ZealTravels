using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using ZealTravel.Domain.Interfaces.TBO;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetServices :IGetServices
    {
        //AVAILABILITY============================================================================================================================================================================
        public string GetFlights(string JourneyType, string Supplierid, string Password, string Searchid, string Companyid, string AirRQ, string EndUserIp)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Supplierid, Password, Searchid, Companyid, AirRQ, EndUserIp);
            return objthrd.Thread(JourneyType);
        }
        public async Task<string> GetFlightsAsync(string JourneyType, string Supplierid, string Password, string Searchid, string Companyid, string AirRQ, string EndUserIp)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Supplierid, Password, Searchid, Companyid, AirRQ, EndUserIp);
            return await objthrd.ThreadAsync(JourneyType);
        }


        //FARE RULE===============================================================================================================================================================================
        public string GetFareRule(string JourneyType, string Searchid, string Companyid, string AirRS, string EndUserIp)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetApiFlightFareRule objFlightFareRule = new GetApiFlightFareRule(JourneyType, Searchid, Companyid, EndUserIp);
                    return objFlightFareRule.GetFareRule(dtBound);
                }
            }
            return string.Empty;
        }

        //FARE UPDATE=============================================================================================================================================================================
        public string GetFare(string JourneyType, string Searchid, string Companyid, string AirRS, string EndUserIp)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetApiFareQuote objFare = new GetApiFareQuote(JourneyType, Searchid, Companyid, EndUserIp);
                    return objFare.GetFare(dtBound);
                }
            }
            return string.Empty;
        }

        //SSR ====================================================================================================================================================================================
        public string GetSSR(string JourneyType, string Searchid, string Companyid, string AirRS, string EndUserIp)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetApiSsrAvailability objSsrAvailability = new GetApiSsrAvailability(JourneyType, Searchid, Companyid, EndUserIp);
                    return objSsrAvailability.GetSSR(dtBound);
                }
            }
            return string.Empty;
        }

        //COMMIT =================================================================================================================================================================================
        public string GetCommit(string JourneyType, string Searchid, string Companyid, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS, string EndUserIp)
        {
            if (AirRS != null && PassengerRS != null && CompanyRS != null && AirRS.IndexOf("RefID") != -1
               && PassengerRS.IndexOf("PassengerInfo") != -1 && CompanyRS.IndexOf("CompanyInfo") != -1)
            {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                DataTable dtPassengerInfo = GetCommonFunctions.StringToDataSet(PassengerRS).Tables["PassengerInfo"];
                DataTable dtComppanyInfo = GetCommonFunctions.StringToDataSet(CompanyRS).Tables["CompanyInfo"];
                DataTable dtGstInfo = new DataTable();
                if (GstRS != null && GstRS.IndexOf("GstInfo") != -1)
                {
                    dtGstInfo = GetCommonFunctions.StringToDataSet(GstRS).Tables["CompanyInfo"];
                }

                if (dtBound != null && dtBound.Rows.Count > 0 && dtPassengerInfo != null && dtPassengerInfo.Rows.Count > 0 && dtComppanyInfo != null && dtComppanyInfo.Rows.Count > 0)
                {
                    return "";
                }
            }
            return string.Empty;
        }
        //Calender ==================================================================================================================================================================================
        public string GetFareCalender(string Supplierid, string Password, string Searchid, string Companyid, string Origin, string Destination, string BeginDate, string Cabin, string EndUserIp)
        {
            GetApiFareCalender objApiBookings = new GetApiFareCalender(Searchid, Companyid);
            return objApiBookings.GetCalenderData(Supplierid, Password, Origin, Destination, BeginDate, Cabin, EndUserIp);
        }
        //============================================================================================================================================================================================
    }
}

