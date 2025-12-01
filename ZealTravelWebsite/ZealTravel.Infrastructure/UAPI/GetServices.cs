using CommonComponents;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.UniversalAPI;

namespace ZealTravel.Infrastructure.UAPI
{
    public class GetServices: IGetServices
    {
        IBookingManagementService _bookingService;
        IConfiguration _config;


        public GetServices(IBookingManagementService bookingService, IConfiguration config)
        {
            _bookingService = bookingService;
            _config = config;
        }
        //AVAILABILITY============================================================================================================================================================================
        public string GetFlights(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRQ, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid, AirRQ, Sector);
            return objthrd.Thread(JourneyType);
        }
        public async Task<string> GetFlightsAsync(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRQ, string Sector, bool uapiSME)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid, AirRQ, Sector, uapiSME);
            return await objthrd.ThreadAsync(JourneyType);
        }

        //FARE RULE===============================================================================================================================================================================
        public async Task<string> GetFareRule(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = CommonFunction.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetApiFlightFareRule objfs = new GetApiFlightFareRule(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid);
                    return objfs.GetRuleResponse(dtBound);
                }
            }
            return string.Empty;
        }
        //FARE UPDATE=============================================================================================================================================================================
        public async Task<string> GetFare(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = CommonFunction.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetPriceItinerary objfs = new GetPriceItinerary(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid, 0);
                    dtBound = objfs.GetPriceItineraryResponse(dtBound);
                    dtBound.TableName = "AvailabilityInfo";
                    CommonUapi.Add_Journey_Duration_TimeDetail(dtBound);

                    DataTable dtAddOnItinerary = objfs.dtSSRItinerary;

                    DataSet dsBound = new DataSet();
                    dsBound.Tables.Add(dtBound.Copy());
                    if (dtAddOnItinerary != null && dtAddOnItinerary.Rows.Count > 0)
                    {
                        dtAddOnItinerary.TableName = "SSRInfo";
                        dsBound.Tables.Add(dtAddOnItinerary.Copy());
                    }
                    dsBound.DataSetName = "root";
                    return DBCommon.CommonFunction.DataSetToString(dsBound);
                }
            }
            return string.Empty;
        }
        //FARE UPDATE=============================================================================================================================================================================
        public async Task<string> GetSSR(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS)
        {
            if (AirRS != null && AirRS.IndexOf("RefID") != -1)
            {
                DataTable dtBound = DBCommon.CommonFunction.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetSSR objSSR = new GetSSR();
                    DataTable dtAddOnItinerary = objSSR.GetSSRData(JourneyType, Searchid, Companyid, dtBound);

                    if (dtAddOnItinerary != null && dtAddOnItinerary.Rows.Count > 0)
                    {
                        dtAddOnItinerary.TableName = "SSRInfo";
                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtAddOnItinerary);

                        dsBound.DataSetName = "root";
                        return DBCommon.CommonFunction.DataSetToString(dsBound);
                    }
                }
            }
            return string.Empty;
        }
        //COMMIT =================================================================================================================================================================================
        public async Task<string> GetCommit(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string Searchid, string Companyid, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS, string PaymentType)
        {
            if (AirRS != null && PassengerRS != null && CompanyRS != null && AirRS.IndexOf("RefID") != -1
               && PassengerRS.IndexOf("PassengerInfo") != -1 && CompanyRS.IndexOf("CompanyInfo") != -1)
            {

               // DBCommon.Logger.dbLogg(Companyid, BookingRef, "GetCommit", "air_uapi", AirRS, Searchid, PassengerRS);

                bool IsOW = false;
                if (JourneyType.Equals("OW"))
                {
                    IsOW = true;
                }

                GetBook objbp = new GetBook(NetworkUserName, NetworkPassword, TargetBranch, UserName, Password, Searchid, Companyid, BookingRef, _bookingService);
                return (await objbp.GetBookResponse(AirRS, PassengerRS, CompanyRS, GstRS, IsOW,PaymentType)).ToString();
            }
            return string.Empty;
        }

        //BOOKINGS ==============================================================================================================================================================================
        public async Task<string> Get6eBookingData(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID)
        {
            GetApiBookings objApiBookings = new GetApiBookings();
            return objApiBookings.Get6eBookingDataByRecordLocator(universalRecordLocatorCode, targetBranch, NetworkUserName, NetworkPassword, SearchID);
        }
        public async Task<string> GetBookingData(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID)
        {
            GetApiBookings objApiBookings = new GetApiBookings();
            return objApiBookings.GetBookingDataByRecordLocator(universalRecordLocatorCode, targetBranch, NetworkUserName, NetworkPassword, SearchID);
        }


        //Hold Booking ===========================================================================================================================================================================
        
        public Task<bool> GetAirCommitForHoldBookingAsync(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, string fltType)
        {
            GetConfirmForHoldBooking confirmForHoldBooking = new GetConfirmForHoldBooking(_bookingService,_config);
            return confirmForHoldBooking.GetConfirmForHoldBookingResponse(universalRecordLocatorCode, targetBranch, NetworkUserName, NetworkPassword, SearchID, BookingRef, CompanyID, fltType);
        }

        public Task<bool> GetAirCommit6EForHoldBookingAsync(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string UserName, string Password, string SearchID, int BookingRef, string CompanyID, string fltType)
        {
            GetConfirmForHoldBooking confirmForHoldBooking = new GetConfirmForHoldBooking(_bookingService, _config);
            return confirmForHoldBooking.Get6EConfirmForHoldBookingResponse(universalRecordLocatorCode, targetBranch, NetworkUserName, NetworkPassword, UserName, Password, SearchID, BookingRef, CompanyID, fltType);
        }
    }
}
