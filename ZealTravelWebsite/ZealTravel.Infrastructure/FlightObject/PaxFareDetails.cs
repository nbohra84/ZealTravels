using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PaxFareDetails
    {
        public PaxFareDetails() => this.Type = this.Value = "";

        public string Type { get; set; }

        public string Value { get; set; }
    }
}