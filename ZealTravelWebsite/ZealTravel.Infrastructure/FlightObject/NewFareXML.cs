using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class NewFareXML
    {
        public NewFareXML() => this.ChangeFare = this.IsClassChange = false;

        public bool IsClassChange { get; set; }

        public bool ChangeFare { get; set; }

        public double TAmt { get; set; }

        public double TTax { get; set; }

        public double SuppABF { get; set; }

        public double SuppATX { get; set; }

        public double SuppAYQ { get; set; }

        public double SuppCBF { get; set; }

        public double SuppCTX { get; set; }

        public double SuppCYQ { get; set; }

        public double SuppIBF { get; set; }

        public double SuppITX { get; set; }

        public double SuppIYQ { get; set; }

        public string ATXBreakup { get; set; }

        public string CTXBreakup { get; set; }

        public string ITXBreakup { get; set; }

        public string ValidatingCarrier { get; set; }
    }
}