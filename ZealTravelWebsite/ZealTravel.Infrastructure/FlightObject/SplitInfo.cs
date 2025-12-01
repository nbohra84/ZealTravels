using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SplitInfo
    {
        public SplitInfo()
        {
            this.FlightId = 0;
            this.IsDivideImport = false;
            this.SplitStatus = false;
        }

        public int FlightId { get; set; }

        public bool IsDivideImport { get; set; }

        public bool SplitStatus { get; set; }

        public string SplitFrom { get; set; }
    }
}