using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class VerifyTicketBalanceQuery
    {
        public string CompanyID { get; set; }
        public decimal TicketAmount { get; set; }
    }
}
