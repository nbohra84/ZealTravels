using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirlineManagement.Queries
{
    public class GetAirFareRulesQuery
    {
        public string JourneyType { get; set; }
        public string SearchID { get; set; }
        public string CompanyID { get; set; }
        public string AirRS { get; set; }
    }
}
