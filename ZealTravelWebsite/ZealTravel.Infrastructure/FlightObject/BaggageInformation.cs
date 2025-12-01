using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class BaggageInformation
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public BagOtherInfo BagOtherInfo { get; set; }

        public bool ShouldSerializeBagOtherInfo() => this.BagOtherInfo != null;

        [XmlElement("BaggageInfoAry")]
        public TIRequestResponse.BaggageInfoAry[] BaggageInfoAry { get; set; }
    }
}