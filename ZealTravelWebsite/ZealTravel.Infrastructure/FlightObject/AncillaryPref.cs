using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class AncillaryPref
    {
        public AncillaryPref() => this.SeatPref = this.MealPref = this.FQFN = false;

        public bool SeatPref { get; set; }

        public bool MealPref { get; set; }

        public bool FQFN { get; set; }
    }
}