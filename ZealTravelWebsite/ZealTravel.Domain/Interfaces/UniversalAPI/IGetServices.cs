using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.UniversalAPI
{
    public interface IGetServices
    {
        string GetFlights(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRQ, string Sector);
        Task<string> GetFlightsAsync(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRQ, string Sector, bool uapiSME=false);
        Task<string> GetFare(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS);
        Task<string> GetFareRule(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS);
        Task<string> GetSSR(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRS);
        Task<string> GetCommit(string JourneyType, string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string Searchid, string Companyid, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS, string PaymentType);
        Task<string> Get6eBookingData(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID);
        Task<string> GetBookingData(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID);
        Task<bool> GetAirCommitForHoldBookingAsync(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, string fltType);
        Task<bool> GetAirCommit6EForHoldBookingAsync(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string Username, string Password, string SearchID, int BookingRef, string CompanyID, string fltType);
    }
}
