using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BookingManagement.Query
{
    public class CompanyFlightSegmentRuleDetailAirlinesData
    {
        public int Id { get; set; }

        public string? SearchId { get; set; }

        public string? Api_SearchId { get; set; }

        public string? Api_TraceId { get; set; }

        public string? CompanyId { get; set; }

        public int? BookingRef { get; set; }

        public string? Conn { get; set; }

        public string? EquipmentType { get; set; }

        public string? PriceType { get; set; }

        public string? FareRule { get; set; }

        public string? FareRuledb { get; set; }

        public string? BaggageDetail { get; set; }

        public bool? IsTimeChanged { get; set; }

        public bool? IsPriceChanged { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
