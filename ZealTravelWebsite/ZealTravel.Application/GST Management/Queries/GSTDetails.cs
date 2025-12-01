using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.GSTManagement.Queries
{
    public class GSTDetails
    {
        public int Id { get; set; }

        public string? CompanyId { get; set; }

        public string? GstcompanyName { get; set; }

        public string? GstcompanyEmail { get; set; }

        public string? GstcompanyContactNumber { get; set; }

        public string? GstcompanyAddress { get; set; }

        public string? Gstnumber { get; set; }

        public DateTime? EventTime { get; set; }
    }
}
