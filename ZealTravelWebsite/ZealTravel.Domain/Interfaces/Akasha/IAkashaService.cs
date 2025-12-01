using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.Akasha
{
    public interface IAkashaService
    {
        Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector);
        Task<string> GetAirFareRulesAsync(AirAvaibilityModel model);
        Task<string> GetAirFareAsync(AirAvaibilityModel model);
        Task<bool> GetAirCommitAsync(AirAvaibilityModel model, int BookingRef, string PassengerRS, string CompanyRS, string GstRS);
    }
}
