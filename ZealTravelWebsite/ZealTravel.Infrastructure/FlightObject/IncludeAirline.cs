using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class IncludeAirline
    {
        [XmlArrayItem("MA", IsNullable = false)]
        public string[] MA;
    }
}