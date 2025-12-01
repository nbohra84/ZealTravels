using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PnrDetails
    {
        public PnrDetails()
        {
            this.PnrNumber = this.Status = this.NewPNR = "";
            this.IsNewPNR = false;
        }

        public string PnrNumber { get; set; }

        public string Status { get; set; }

        public bool IsNewPNR { get; set; }

        public string NewPNR { get; set; }
    }
}