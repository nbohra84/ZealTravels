using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class ServiceCost
    {
        public ServiceCost()
        {
            this.Amount = this.SupplierAmount = "0.00";
            this.ROE = 1M;
            this.SuppCurr = "";
            this.DecimalPlaces = 2;
        }

        public string Amount { get; set; }

        public string SupplierAmount { get; set; }

        public string SuppCurr { get; set; }

        public Decimal ROE { get; set; }

        public int DecimalPlaces { get; set; }
    }
}