using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class GSTDetails
    {
        public string GSTNumber { get; set; }

        public string RegisteredCompanyName { get; set; }

        public string RegisteredCompanyAddress { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public bool isMyCompanyPay { get; set; } = false;


        public bool IsRegisteredCustomer { get; set; } = false;

    }
}
