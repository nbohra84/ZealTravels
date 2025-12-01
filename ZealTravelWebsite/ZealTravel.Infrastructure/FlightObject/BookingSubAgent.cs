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
    public class BookingSubAgent
    {
        [DefaultValue(0)]
        public int Id { get; set; }

        [DefaultValue(0)]
        public int UserId { get; set; }

        [DefaultValue(0)]
        public int BranchId { get; set; }

        [DefaultValue(0)]
        public int SaBranchId { get; set; }

        public string IsCreditCust { get; set; }

        public string AgentRef { get; set; }
    }
}
