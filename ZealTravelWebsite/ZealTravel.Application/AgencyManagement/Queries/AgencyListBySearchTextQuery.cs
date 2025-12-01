using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class AgencyListBySearchTextQuery
    {
        public int AccountId { get; set; }
        public string SearchText { get; set; }

    }
}
