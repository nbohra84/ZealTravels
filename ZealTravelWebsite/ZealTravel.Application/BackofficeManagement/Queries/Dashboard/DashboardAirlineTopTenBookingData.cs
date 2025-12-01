using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BackofficeManagement.Queries.Dashboard
{
    public class DashboardAirlineTopTenBookingData
    {
        public string CompanyName { get; set; }
        public int AccountID { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string Pan_No { get; set; }
        public string UserType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int BookingRef { get; set; }
        public DateTime EventTime { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool TKTstatus { get; set; }
        public bool PNRstatus { get; set; }
        public string BookingStatus { get; set; }
        public string? RejectDetail { get; set; }
    }
}
