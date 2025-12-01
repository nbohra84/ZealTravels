using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.Spicejet
{
    public interface ISpicejetService
    {
        Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, string priceType);
        Task<string> GetAirFareRulesAsync(AirAvaibilityModel model);
        Task<string> GetAirFareAsync(AirAvaibilityModel model);
        Task<string> GetAirSSRAsync(AirAvaibilityModel model);
        Task<bool> GetAirCommitAsync(AirAvaibilityModel model, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType);
        Task<bool> GetAirCommitForHoldBookingAsync(string Supplierid, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, Decimal TotalApiFare, string RecordLocator, int Paxcount, string FltType);
    }
}
