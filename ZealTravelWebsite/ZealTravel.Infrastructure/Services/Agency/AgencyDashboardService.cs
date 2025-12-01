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
using Dapper;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Data.Models.Agency;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Infrastructure.Services.Agency
{
    public class AgencyDashboardService : IAgencyDashboardService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AgencyDashboardService));

        public AgencyDashboardService(ZealdbNContext context)
        {
            _context = context;
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


        public async Task<List<DashboardLedger>> LedgerDescriptiveDashboardReportAsync(string companyId, DateTime fromDate, DateTime toDate, string searchBy, string ticketSearchType, string searchByValue)
        {
            try
            {
                toDate = toDate.AddDays(1);
                searchBy = companyId;
                var reports = await _context.Database.SqlQuery<DashboardLedger>(
                $"Company_Transaction_Detail_Proc @ProcNo = 9, @Dt1 = {fromDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {toDate.ToString("dd-MMM-yy 12:00:00 tt")}, @CompanyID = {companyId}, @SearchBy = {searchBy}, @TicketSearchType = {ticketSearchType}, @SearchByValue = {searchByValue}"
            ).ToListAsync();

                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                throw new Exception($"{ex.Message}", ex);
            }
        }
        public async Task<List<DashboardCorporate>> DashboardCorporateData(string companyId)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<DashboardCorporate>(
                    $"Dashboard_Corprate_data @ProcNo = 6,@CompanyID = {companyId}"
                ).ToListAsync();


                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<List<DashboardNotification>> GetDashboardNotification(string companyId)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<DashboardNotification>(
                    $"Notice_Company_Proc @ProcNo = 3,@CompanyID = {companyId}"
                ).ToListAsync();


                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async Task<List<DashBoardChart>> GetDashboardChart(string companyId)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<DashBoardChart>(
                    $"DailyBookingList_Proc @ProcNo = 2,@CompanyID = {companyId}"
                ).ToListAsync();
                return reports;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}");
                throw new Exception($"{ex.Message}", ex);
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

        public async Task<AgencyCompanyDetails> GetCompanyDetailAfterLogin(Int32 accountID)
        {
            var agencyCompanyDetails = new AgencyCompanyDetails();
            try
            {
                var connection = _context.Database.GetDbConnection();
             
                using (var multi = connection.QueryMultiple("Dashboard_FrontOffice_Proc", new { ProcNo = 1, AccountID = accountID }, commandType: CommandType.StoredProcedure))
                {

                    agencyCompanyDetails.CompanyDetails = multi.Read<CompanyDetails>().ToList();
                    agencyCompanyDetails.CompanyRegisterProduct = multi.Read<CompanyRegisterProduct>().ToList();
                }
               
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return agencyCompanyDetails;
        }
    }
}
