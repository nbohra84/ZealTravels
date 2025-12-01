using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class AdditionalService
    {
        public AdditionalService()
        {
            this.Idx = (short)1;
            this.Isselected = false;
            this.IsMandatory = false;
            this.Type = this.Description = this.ServiceId = this.PaxType = this.Origin = this.Destination = this.Behaviour = "";
            this.Quantity = "1";
        }

        public short Idx { get; set; }

        public bool Isselected { get; set; }

        public string Type { get; set; }

        public bool IsMandatory { get; set; }

        public string Description { get; set; }

        public string ServiceId { get; set; }

        public string PaxType { get; set; }

        public string Behaviour { get; set; }

        public string Origin { get; set; }

        public string Quantity { get; set; }

        public string Destination { get; set; }

        public string[] IncludedServies { get; set; }

        [JsonProperty(PropertyName = "ServiceCost")]
        [XmlElement("ServiceCost")]
        public ServiceCost AddServiceCost { get; set; }
    }
}