using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class ExcludeAirline
    {
        [XmlArrayItem("MA", IsNullable = false)]
        public string[] MA { get; set; }
    }
}