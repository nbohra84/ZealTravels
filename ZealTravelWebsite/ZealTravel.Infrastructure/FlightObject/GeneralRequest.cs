using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class GeneralRequest
    {
        public string IssuedTicketPCC { get; set; }
    }
}