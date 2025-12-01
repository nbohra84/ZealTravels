using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyFlightOwnSegmentsPnrsData
    {
        public int Id { get; set; }

        public int? BookingRef { get; set; }

        public string? PNR { get; set; }

        public string? FltType { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
