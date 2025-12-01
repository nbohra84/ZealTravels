using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.UniversalAPI
{
    public interface IUAPIServices
    {
        Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, bool uapiSME = false);
        Task<string> GetAirAvailability6EAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector);
        Task<string> GetAirFareRulesAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<string> GetAirFareAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<string> GetAirFareRules6EAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<string> GetAirFare6EAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<string> GetAirSSR6EAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<string> GetAirSSRAsync(AirAvaibilityModel model, string supplierType, string supplier);
        Task<bool> GetAirCommit6EAsync(AirAvaibilityModel model, string supplierType, string supplier, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType);
        Task<bool> GetAirCommitAsync(AirAvaibilityModel model, string supplierType, string supplier, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType);
        Task<string> GetAirLineBookingStatusData(string universalRecordLocatorCode, string supplier,string supplierType, string searchID);
        Task<bool> GetAirCommitForHoldBookingAsync(string universalRecordLocatorCode, string supplier, string supplierType, string searchID,int bookingRef,string companyID,string fltType);
        Task<bool> GetAirCommit6EForHoldBookingAsync(string universalRecordLocatorCode, string supplier, string supplierType, string searchID,int bookingRef,string companyID, string fltType);
    }
}
