using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class SetBookingAirlineLogForPGQuery
    {
        public bool IsCombi { get; set; }
        public bool IsRT { get; set; }
        public bool IsMC { get; set; }
        public string SearchID { get; set; }
        public string CompanyID { get; set; }
        public int BookingRef { get; set; }
        public string AvailabilityResponse { get; set; }
        public string PassengerResponse { get; set; }
        public string RefID_O { get; set; }
        public string RefID_I { get; set; }
        public int PaymentID { get; set; }
        public string PaymentType { get; set; }
        public string Currency { get; set; }
        public double CurrencyValue { get; set; }
    }
}
