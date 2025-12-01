using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class AdvanceSearch
    {
        public AdvanceSearch()
        {
            this.NoOfResult = 0;
            this.FilterInResult = false;
        }

        public bool isFlexi { get; set; }

        public bool FilterInResult { get; set; }

        public int NoOfResult { get; set; }

        public bool isFareBreakup { get; set; }

        public bool ShouldSerializeisFareBreakup() => false;

        public bool DirectFlight { get; set; }

        public SpecificAirlines SpecificAirlines { get; set; }

        public bool ForeignResidency { get; set; }

        public bool NonStopFlight { get; set; }

        public bool RefundableFlight { get; set; }

        public bool HighestLoyaltyPoints { get; set; }
    }
}