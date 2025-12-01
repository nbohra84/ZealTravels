using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class SetBookingQuery
    {
        public string SearchID { get; set; }
        public string CompanyID { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsCombi { get; set; }
        public bool IsRTfare { get; set; }
        public bool IsQueue { get; set; }
        public bool IsOffline { get; set; }
        public string PaymentType { get; set; }
        public string PaymentID { get; set; }
        public string PassengerResponse { get; set; }
        public string AvailabilityResponse { get; set; }
        public string RefID_O { get; set; }
        public string RefID_I { get; set; } 
        public string GstInfo { get; set; }

    }
}
