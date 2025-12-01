using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class FCanxCharge
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public FCanxCharge()
        {
            double? nullable = new double?(0.0);
            this.CompCharge = nullable;
            this.airlineCharge = nullable;
        }

        public double? airlineCharge { get; set; }

        public double? CompCharge { get; set; }
    }
}