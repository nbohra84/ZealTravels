using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class GDS
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        public ExcludeAirline ExcludeAirline { get; set; }

        public IncludeAirline IncludeAirline { get; set; }
    }
}