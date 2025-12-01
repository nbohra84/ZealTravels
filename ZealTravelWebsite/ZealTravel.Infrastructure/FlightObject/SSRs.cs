using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SSRs
    {
        public SSRs()
        {
            this.SSRIdx = 0;
            this.Isselected = false;
            this.GroupName = this.Code = this.Name = "";
            this.Quantity = 1;
            this.Markup = 0M;
        }

        public int SSRIdx { get; set; }

        public string GroupName { get; set; }

        public bool Isselected { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public Decimal Amount { get; set; }

        public Decimal SupplierAmount { get; set; }

        public Decimal Markup { get; set; }

        public string OptionalKey { get; set; }

        public string OptionalServicesRuleRef { get; set; }

        public string AirSegmentRef { get; set; }

        public string BookingTravelerRef { get; set; }

        public string TempDetail { get; set; }

        public string AssessIndicator { get; set; }

        public string Currency { get; set; }
    }
}
