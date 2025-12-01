using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PaymentDetail
    {
        [XmlElement("Payment")]
        public Payment[] Payment { get; set; }
    }
}
