using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class VerifyAgentBookingQuery
    {
        public string CompanyId { get; set; }
        public int BookingRef { get; set; }
        public string BookingType { get; set; }
    }
}
