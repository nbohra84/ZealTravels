using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirlineManagement.Queries
{
    public class GetUAPIAirlineBookingQuery
    {
        public string UniversalRecordLocatorCode { get; set; }

        public string? Supplier { get; set; }

        public string? SearchId { get; set; }

        public GetUAPIAirlineBookingQuery(string universalRecordLocatorCode, string supplier, string? searchId)
        {
            UniversalRecordLocatorCode = universalRecordLocatorCode;
            Supplier = supplier;
            SearchId = searchId;
        }
    }
}
