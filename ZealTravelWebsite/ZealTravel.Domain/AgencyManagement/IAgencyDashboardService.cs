using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Agency;

namespace ZealTravel.Domain.AgencyManagement
{
    public interface IAgencyDashboardService
    {
        Task<List<DashboardLedger>> LedgerDescriptiveDashboardReportAsync(string companyId, DateTime fromDate, DateTime toDate, string searchBy, string ticketSearchType, string searchByValue);
        Task<List<DashboardNotification>> GetDashboardNotification(string companyId);
        Task<List<DashBoardChart>> GetDashboardChart(string companyId);
        Task<List<DashboardCorporate>> DashboardCorporateData(string companyId);
        Task<AgencyCompanyDetails> GetCompanyDetailAfterLogin(int accountId);
        
    }
}
