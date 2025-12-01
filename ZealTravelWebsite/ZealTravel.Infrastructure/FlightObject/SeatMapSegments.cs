using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SeatMapSegments
    {
        public string SegmentIndex { get; set; }

        public string SeatNumber { get; set; }

        public string SegmentTattoo { get; set; }

        public string DepartureAirport { get; set; }

        public string ArrivalAirport { get; set; }

        public bool SeatAssigned { get; set; }

        public string MarketingAirlineWithFN { get; set; }

        public string SeatKey { get; set; }

        public decimal Amount { get; set; } = default(decimal);


        public decimal SuppAmount { get; set; } = default(decimal);


        public SeatMapSegments()
        {
            SeatAssigned = false;
        }
    }
}
