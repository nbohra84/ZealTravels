using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.ReportManagement.Queries
{
    public class DailyBooking
    {
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal TotalYQ { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalCommission { get; set; }
        public int NoOfPassengers { get; set; }
        public int BookingRef { get; set; }
    }
}
