using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard
{
    public class DashboardChartData
    {
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public int BookingRef { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal TotalYQ { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalCommission { get; set; }
        public int NoOfPassenger { get; set; }
    }
}
