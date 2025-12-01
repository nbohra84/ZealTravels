using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirlineManagement.Queries
{
    public class GetAirCommitQuery
    {
        public string JourneyType { get; set; }
        public string SearchID { get; set; }
        public string CompanyID { get; set; }
        public string AirRS { get; set; }
        public Int32 BookingRef { get; set; }
        public string PassengerRS { get; set; }
        public string GstRS { get; set; }
        public string PaymentType { get; set; }
    }
}
