using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SegmentDetail
    {
        [XmlElement("FromTo")]
        public TIRequestResponse.FromTo[] FromTo { get; set; }
    }
}