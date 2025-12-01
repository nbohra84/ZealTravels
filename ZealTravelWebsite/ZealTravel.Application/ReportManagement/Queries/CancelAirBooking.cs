using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.ReportManagement.Queries
{
    public class CancelAirBooking
    {
        public int BookingRef { get; set; }
        public string flttype { get; set; }
        public string PNR { get; set; }
        public DateTime EventTime { get; set; }
        public string CompanyName { get; set; }
        public string Supplier { get; set; }
    }
}
