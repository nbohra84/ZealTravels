using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PriceDetail
    {
        private bool propertyValue;

        public PriceDetail() => this.ASuppMU = 0.0;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public string Type { get; set; }

        public double ASuppComm { get; set; }

        public double ASuppMU { get; set; }

        public double ASuppFee { get; set; }

        public double APassTransFee { get; set; }

        public double ACompComm { get; set; }

        public double ACompMU { get; set; }

        public double ATDS { get; set; }

        public double ASAMU { get; set; }

        public double Comp_Price_Net { get; set; }

        public double Comp_Price_Gross { get; set; }

        public double AgentNet { get; set; }

        public double AgentGross { get; set; }

        public double SubAgentNet { get; set; }

        public double SubAgentGross { get; set; }

        public string Agent_Curr { get; set; }

        public double Agent_ROE { get; set; }

        public string Gross_Curr { get; set; }

        public double Gross_ROE { get; set; }

        public double Supp_ROE { get; set; }

        public double GrossBF { get; set; }

        public double GrossYQ { get; set; }

        public double GrossTX { get; set; }

        public SegmentDetail SegmentDetail { get; set; }
    }
}