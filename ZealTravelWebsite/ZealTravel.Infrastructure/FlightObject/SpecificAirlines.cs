using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SpecificAirlines
    {
        [XmlElement("Airline")]
        public string[] Airline { get; set; }
    }
}