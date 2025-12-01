using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class FlightOwnSegmentsPnrsData
    {
        public int Id { get; set; }

        public int? BookingRef { get; set; }

        public string? Pnr_Galileo { get; set; }

        public string? Pnr_Airline { get; set; }

        public string? FltType { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
