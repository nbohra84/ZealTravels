using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Charges
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set
            {
                this.propertyValue = value;
                if (this.Charge == null)
                    return;
                foreach (Charge charge in this.Charge)
                    charge.ShowPropertyWhileSerialize = this.propertyValue;
            }
        }

        [XmlElement("Charge")]
        public Charge[] Charge { get; set; }
    }
}