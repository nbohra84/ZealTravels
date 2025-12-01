using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Booking;

namespace ZealTravel.Domain.AgencyManagement
{
    public interface IReportingService
    {

        Task<List<DailyBookingReport>> GetDailyBookingReportsAsync(string companyId, DateTime? startDate, DateTime? endDate);
        Task<List<FlightRefrenceData>> GetCancelRefundReport(DateTime startDate, DateTime endDate);
        Task<List<LedgerReport>> LedgerDescriptiveReportAsync(string companyId, DateTime fromDate, DateTime toDate, string searchBy, string ticketSearchType, string searchByValue, string eventId);
        Task<List<AirlineBookings>> FilghtBookingReportAsync(string companyId, DateTime fromDate, DateTime toDate, string ticketSearchType, string searchByValue);
        Task<List<CancelFlightBookingReport>> GetCancelFlightBookingReportsAsync(string? supplierId, DateTime startDate, DateTime endDate);


    }

}
