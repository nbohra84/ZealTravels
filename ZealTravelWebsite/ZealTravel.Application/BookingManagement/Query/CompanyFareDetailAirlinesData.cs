using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyFareDetailAirlinesData
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
        public string? BaggageDetail { get; set; }

        public DateTime? EventTime { get; set; }

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

        public int? NoOfPassenger { get; set; }

        public decimal? BasicDeal { get; set; }

        public decimal? YqDeal { get; set; }

        public decimal? CbDeal { get; set; }

        public decimal? PromoDeal { get; set; }

        public decimal? Markup { get; set; }

        public decimal? ServiceFee { get; set; }

        public decimal? Import { get; set; }

        public decimal? ServiceTax { get; set; }

        public decimal? Tds { get; set; }

        public decimal? BasicDeal1 { get; set; }

        public decimal? YqDeal1 { get; set; }

        public decimal? CbDeal1 { get; set; }

        public decimal? PromoDeal1 { get; set; }

        public decimal? Markup1 { get; set; }

        public decimal? ServiceFee1 { get; set; }

        public decimal? Import1 { get; set; }

        public decimal? ServiceTax1 { get; set; }

        public decimal? Tds1 { get; set; }
        public bool? IsPaymentHold { get; set; }

    }
}
