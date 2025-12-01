using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Backoffice
{
    public class DashboardAirlinePendingBooking
    {
        public bool Status { get; set; }
        public string CompanyName { get; set; }
        public string Sector { get; set; }
        public string CompanyID { get; set; }
        public int BookingRef { get; set; }
        public string? StaffID { get; set; }
        public string? StaffName { get; set; }
        public DateTime EventTime { get; set; }
        public string Trip { get; set; }
    }
}
