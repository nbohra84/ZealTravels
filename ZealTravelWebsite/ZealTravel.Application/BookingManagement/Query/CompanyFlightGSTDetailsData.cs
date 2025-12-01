using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyFlightGSTDetailsData
    {
        public int Id { get; set; }

        public int? BookingRef { get; set; }

        public string? GstcompanyContactNumber { get; set; }

        public string? GstcompanyName { get; set; }

        public string? Gstnumber { get; set; }

        public string? GstcompanyEmail { get; set; }

        public string? GstcompanyAddress { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
