using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PaxDetails
    {
        public byte Adults { get; set; }

        public byte? Childs { get; set; }

        public byte? Infant { get; set; }
    }
}