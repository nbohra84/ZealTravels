using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyFareDetailSegmentAirlines
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public decimal? Basic { get; set; }

        public decimal? Yq { get; set; }

        public decimal? Psf { get; set; }

        public decimal? Udf { get; set; }

        public decimal? Audf { get; set; }

        public decimal? Cute { get; set; }

        public decimal? Gst { get; set; }

        public decimal? Tf { get; set; }

        public decimal? Cess { get; set; }

        public decimal? Ex { get; set; }

        public decimal? Meal { get; set; }  

        public decimal? Seat { get; set; }

        public decimal? Baggage { get; set; }

        public string? PaxType { get; set; }

        public int? No_Of_Passenger { get; set; }

        public decimal? Basic_Deal { get; set; }

        public decimal? Yq_Deal { get; set; }

        public decimal? Cb_Deal { get; set; }

        public decimal? Promo_Deal { get; set; }

        public decimal? Markup { get; set; }

        public decimal? Service_Fee { get; set; }

        public decimal? Import { get; set; }

        public decimal? ServiceTax { get; set; }

        public decimal? Tds { get; set; }

        public decimal? Basic_Deal1 { get; set; }

        public decimal? Yq_Deal1 { get; set; }

        public decimal? Cb_Deal1 { get; set; }

        public decimal? Promo_Deal1 { get; set; }

        public decimal? Markup1 { get; set; }

        public decimal? Service_Fee1 { get; set; }

        public decimal? Import1 { get; set; }

        public decimal? ServiceTax1 { get; set; }

        public decimal? Tds1 { get; set; }

        public string? Conn { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
