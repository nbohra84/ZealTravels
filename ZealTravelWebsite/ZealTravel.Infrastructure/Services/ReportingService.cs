using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.AgencyManagement;
using ZealTravelWebsite.Infrastructure.Context;
using log4net;
using Microsoft.Data.SqlClient;
using ZealTravel.Domain.Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models.Booking;

namespace ZealTravel.Infrastructure.Services
{
    public class ReportingService : IReportingService
    { 
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ReportingService));

        public ReportingService(ZealdbNContext context)
        {
            _context = context;
        }


        public async Task<List<FlightRefrenceData>> GetCancelRefundReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<FlightRefrenceData>(
                    $"EXECUTE Company_Pax_Segment_Cancellation_Detail_Airline_Refund_Proc @ProcNo = 10, @Dt1 = {startDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {endDate.ToString("dd-MMM-yy 12:00:00 tt")}"
                ).ToListAsync();

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving daily booking reports: {ex.Message}", ex);
            }
        }

        public async Task<List<DailyBookingReport>> GetDailyBookingReportsAsync(string companyId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<DailyBookingReport>(
                    $"EXECUTE DailyBookingList_Proc @ProcNo = 1, @CompanyID = {companyId}, @Dt1 = {startDate?.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {endDate?.ToString("dd-MMM-yy 12:00:00 tt")}"
                ).ToListAsync();

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving daily booking reports: {ex.Message}", ex);
            }
        }

        public async Task<List<DailyBookingReport>> GetAirSheetReportsAsync(string companyId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<DailyBookingReport>(
                    $"EXECUTE DailyBookingList_Proc @ProcNo = 1, @CompanyID = {companyId}, @Dt1 = {startDate?.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {endDate?.ToString("dd-MMM-yy 12:00:00 tt")}"
                ).ToListAsync();

                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving daily booking reports: {ex.Message}", ex);
            }
        }


        public async Task<List<LedgerReport>> LedgerDescriptiveReportAsync(string companyId, DateTime fromDate, DateTime toDate, string searchBy, string ticketSearchType, string searchByValue, string eventId)
        {
            try
            {
                toDate = toDate.AddDays(1);
                var reports = await _context.Database.SqlQuery<LedgerReport>(
                    $"EXECUTE short_ledger_statement @ProcNo = 1, @Dt1 = {fromDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {toDate.ToString("dd-MMM-yy 12:00:00 tt")}, @CompanyID = {companyId}, @SearchBy = {searchBy}, @TicketSearchType = {ticketSearchType}, @SearchByValue = {searchByValue}, @EventID = {eventId}"
                ).ToListAsync();

               
                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving ledger descriptive report: {ex.Message}");
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }
        public async Task<List<AirlineBookings>> FilghtBookingReportAsync(string companyId, DateTime fromDate, DateTime toDate, string ticketSearchType, string? searchByValue)
        {
            try
            {

                toDate = toDate.AddDays(1);
                var reports = await _context.Database.SqlQuery<AirlineBookings>(
                    $"EXECUTE short_air_ticket_report @ProcNo = 1, @Dt1 = {fromDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {toDate.ToString("dd-MMM-yy 12:00:00 tt")}, @CompanyID = {companyId}, @TicketSearchType = {ticketSearchType}, @SearchByValue = {searchByValue}"
                ).ToListAsync();
                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving ledger descriptive report: {ex.Message}");
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }

        public async Task<List<CancelFlightBookingReport>> GetCancelFlightBookingReportsAsync(string supplierId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                supplierId = supplierId == null? string.Empty : supplierId ;
                var reports = await _context.Database.SqlQuery<CancelFlightBookingReport>(
                    $"EXECUTE Company_Pax_Segment_Cancellation_Detail_Airline_Proc @ProcNo = 10, @Dt1 = {fromDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {toDate.ToString("dd-MMM-yy 12:00:00 tt")}, @SupplierID = {supplierId}"
                ).ToListAsync();
                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving ledger descriptive report: {ex.Message}");
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }
    }
}
