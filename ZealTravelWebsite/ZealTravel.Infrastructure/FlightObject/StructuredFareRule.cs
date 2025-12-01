using System.ComponentModel;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    [Serializable]
    public class StructuredFareRule
    {
        public string origin { get; set; }

        public string destination { get; set; }

        public Reissue_Refund Reissue_Refund { get; set; }
    }
}