using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class GovernmentTaxes
    {
        [XmlElement("Tax")]
        [JsonProperty(PropertyName = "Tax")]
        public List<TIRequestResponse.GovTaxes> GovTaxes { get; set; }
    }
}