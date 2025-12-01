using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyFlightOwnSegmentsPnrs
    {
        public int Id { get; set; }

        public int? BookingRef { get; set; }

        public string? PNR { get; set; }

        public string? FltType { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
