using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Booking
{
    public class BookingAirlineLogForPGs
    {
        public int Id { get; set; }

        public int? PaymentId { get; set; }

        public string? UpdatedBy { get; set; }

        public string? CompanyId { get; set; }

        public int BookingRef { get; set; }

        public string? AvailabilityResponse { get; set; }

        public string? PassengerResponse { get; set; }

        public string? RefIdO { get; set; }

        public string? RefIdI { get; set; }

        public string? SearchId { get; set; }

        public bool? IsCombi { get; set; }

        public bool? IsRt { get; set; }

        public bool? IsMc { get; set; }

        public string? PaymentType { get; set; }

        public string? Currency { get; set; }

        public decimal? CurrencyValue { get; set; }

        public DateTime? EventTime { get; set; }


        public int? PaxSegmentId { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public decimal? Balance { get; set; }

        public string? Remark { get; set; }


        public bool? Status { get; set; }

        public string? EventId { get; set; }

        public bool? IsAirline { get; set; }

        public bool? IsHotel { get; set; }

        public bool? IsAirlineOff { get; set; }

        public bool? IsHotelOff { get; set; }

    }
}
