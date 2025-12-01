using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyPaxSegmentDetailAirlines
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public int? Pax_SegmentId { get; set; }

        public string? Conn { get; set; }

        public string? ChargeType { get; set; }

        public string? ChargeCode { get; set; }

        public string? ChargeDescription { get; set; }

        public decimal? Charge_Amount { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
