using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class SearchAgencyTextQuery
    {
        public int? AccountId { get; set; }
        public string? CompanyName { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string CompanyID { get; set; }
    }
}
