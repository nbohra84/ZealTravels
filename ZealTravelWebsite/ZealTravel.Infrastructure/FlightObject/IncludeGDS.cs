using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class IncludeGDS
    {
        [XmlElement("GDS")]
        public GDS[] GDS { get; set; }
    }
}