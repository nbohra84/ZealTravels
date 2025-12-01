using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class CompanyFareDetailAirlines
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public string? PriceType { get; set; }

        public int? Adt { get; set; }

        public int? Chd { get; set; }

        public int? Inf { get; set; }

        public decimal? TotalTax { get; set; }

        public decimal? TotalBasic { get; set; }

        public decimal? TotalYq { get; set; }

        public decimal? TotalFare { get; set; }

        public decimal? TotalServiceTax { get; set; }

        public decimal? TotalMarkup { get; set; }

        public decimal? TotalBasic_Deal { get; set; }

        public decimal? TotalYq_Deal { get; set; }

        public decimal? TotalCb_Deal { get; set; }

        public decimal? TotalPromo_Deal { get; set; }

        public decimal? TotalServiceFee_Deal { get; set; }

        public decimal? TotalCommission { get; set; }

        public decimal? TotalTds { get; set; }

        public decimal? TotalSeat { get; set; }

        public decimal? TotalMeal { get; set; }

        public decimal? TotalBaggage { get; set; }

        public decimal? TotalQueue { get; set; }

        public string? Conn { get; set; }

        public DateTime? EventTime { get; set; }
        public bool? IsPaymentHold { get; set; }
    }
}
