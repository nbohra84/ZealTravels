using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.FlightManagement.Spicejet
{
    public interface IGetServices
    {
        public string GetFlights(string JourneyType, string SupplierID, string Password, string PriceType, string SearchID, string CompanyID, string AirRQ);
        public Task<string> GetFlightsAsync(string JourneyType, string SupplierID, string Password, string PriceType, string SearchID, string CompanyID, string AirRQ, string sector);
        public Task<string> GetFareRuleAsync(string JourneyType, string Searchid, string Companyid, string AirRS);
        public Task<string> GetFareAsync(string JourneyType, string Searchid, string Companyid, string AirRS);
        public Task<string> GetSSRAsync(string JourneyType, string Searchid, string Companyid, string AirRS);
        public Task<bool> GetCommitAsync(string JourneyType, string SearchID, string CompanyID, int BookingRef, string AirRS, string PassengerRS, string CompanyRS, string GstRS,string PaymentType);
        public Task<string> GetBookingByRecordLocatorAsync(string Searchid, string Supplierid, string Password, string RecordLocator);
        //public Task<string> GetBookingByRecordLocatorAsync(string Searchid, string Signature, string RecordLocator);
        public Task<bool> GetCommitForHoldBookingAsync(string Supplierid, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, Decimal TotalApiFare, string RecordLocator, int Paxcount,string FltType);
    }
}
