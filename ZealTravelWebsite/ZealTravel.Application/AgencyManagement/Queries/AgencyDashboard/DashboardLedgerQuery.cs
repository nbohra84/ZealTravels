using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard
{
    public class DashboardLedgerQuery
    {
        public string CompanyId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? SearchBy { get; set; }
        public string? TicketSearchType { get; set; }
        public string? SearchByValue { get; set; }
    }
}
