using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Command
{
    public class UpdateBalanceTransactionCommand
    {
        public string CompanyId { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public int BookingRef { get; set; }
        public string UpdatedBy { get; set; }
        public string EventId { get; set; }
        public string Remark { get; set; }
        public bool IsAirline { get; set; }
        public bool IsHotel { get; set; }
        public int PassengerId { get; set; }

    }
}
