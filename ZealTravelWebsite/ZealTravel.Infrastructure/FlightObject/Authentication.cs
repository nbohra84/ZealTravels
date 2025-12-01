using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Authentication
    {
        public string UrlAuthentication { get; set; }

        public string URL { get; set; }

        public string ServiceType { get; set; }
    }
}