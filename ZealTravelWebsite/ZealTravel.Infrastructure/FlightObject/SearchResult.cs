using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class SearchResult
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set
            {
                this.propertyValue = value;
                if (this.Airlines == null)
                    return;
                this.Airlines.ShowPropertyWhileSerialize = this.propertyValue;
            }
        }

        public Airlines Airlines { get; set; }

        public FlightError Error { get; set; }

        [XmlElement("UniqueStop")]
        public TIRequestResponse.UniqueStop[] UniqueStop { get; set; }

        [XmlElement("UniqueAirline")]
        public TIRequestResponse.UniqueAirline[] UniqueAirline { get; set; }

        public TIRequestResponse.LayoverAirport[] LayoverAirport { get; set; }
    }
}