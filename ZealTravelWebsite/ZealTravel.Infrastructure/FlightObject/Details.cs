using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Details
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set
            {
                this.propertyValue = value;
                if (this.RSeg == null)
                    return;
                foreach (TIRequestResponse.RSeg rseg in this.RSeg)
                    rseg.ShowPropertyWhileSerialize = this.propertyValue;
            }
        }

        [XmlElement("RSeg")]
        public TIRequestResponse.RSeg[] RSeg { get; set; }
    }
}