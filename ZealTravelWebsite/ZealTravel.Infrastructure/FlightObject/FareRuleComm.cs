using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class FareRuleComm
    {
        [XmlElement("EventsComm")]
        public EventsComm[] EventsComm { get; set; }
    }
}