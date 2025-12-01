using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.FlightManagement.Akasaa
{
    public interface IGetServices
    {
        public string GetFlights(string JourneyType, string SupplierID, string SearchID, string CompanyID, string AirRQ);
        public Task<string> GetFlightsAsync(string JourneyType, string SupplierID, string SearchID, string CompanyID, string AirRQ);
        public Task<string> GetFareRuleAsync(string JourneyType, string Searchid, string Companyid, string AirRS);
        public Task<string> GetFareAsync(string JourneyType, string Searchid, string Companyid, string AirRS);
       public Task<bool> GetCommitAsync(string JourneyType, string SearchID, string CompanyID, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS);
    }
}
