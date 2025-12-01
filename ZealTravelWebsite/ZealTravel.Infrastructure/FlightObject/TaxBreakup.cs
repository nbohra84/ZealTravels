using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class TaxBreakup
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}