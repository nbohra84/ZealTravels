using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard
{
    public class DashboardLedgerData
    {
        public int PaxSegmentID { get; set; }
        public int BookingRef { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string PaymentType { get; set; }
        public DateTime EventTime { get; set; }
        public string Remark { get; set; }
        public string Airline_PNR_D { get; set; }
        public string? BookingStatus { get; set; }
        public string? CompanyName { get; set; }
    }
}
