using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class GetAgencyBalanceTransactionEvents
    {
        public int Id { get; set; }

        public string? EventId { get; set; }

        public string? EventName { get; set; }

        public bool? Status { get; set; }

        public string? EventType { get; set; }
    }
}
