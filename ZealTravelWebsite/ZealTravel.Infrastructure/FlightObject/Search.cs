using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Search
    {
        private bool propertyValue;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public string DepartSelected { get; set; }

        public bool ShouldSerializeDepartSelected() => this.ShowPropertyWhileSerialize;

        public string ArrivalSelected { get; set; }

        public bool ShouldSerializeArrivalSelected() => this.ShowPropertyWhileSerialize;

        public string DepartCFN { get; set; }

        public bool ShouldSerializeDepartCFN() => this.ShowPropertyWhileSerialize;

        public string ArrivalCFN { get; set; }

        public bool ShouldSerializeArrivalCFN() => this.ShowPropertyWhileSerialize;

        public string DepartAP { get; set; }

        public string ArrivalAP { get; set; }

        public string DepartTime { get; set; }

        public string ArrivalTime { get; set; }

        public string DepartDate { get; set; }

        public string IconType { get; set; }
    }
}