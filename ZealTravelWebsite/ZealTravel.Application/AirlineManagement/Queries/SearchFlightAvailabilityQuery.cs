using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirelineManagement.Queries
{
    public class SearchFlightAvailabilityQuery
    {
        //JourneyType, SearchID, CompanyID, AirRQ
        public string JourneyType { get; set; }
        public string SearchID { get; set; }
        public string CompanyID { get; set; }
        public string AirRQ { get; set; }

    }
}
