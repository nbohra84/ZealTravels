using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Charge
    {
        private bool propertyValue;

        public Charge()
        {
            this.AdtAmount = this.ChdAmount = this.InfAmount = this.Amount = this.ChargeCurrROE = 0.0;
        }

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Currency { get; set; }

        public double AdtAmount { get; set; }

        public double ChdAmount { get; set; }

        public double InfAmount { get; set; }

        public double Amount { get; set; }

        public double ChargeCurrROE { get; set; }
    }
}