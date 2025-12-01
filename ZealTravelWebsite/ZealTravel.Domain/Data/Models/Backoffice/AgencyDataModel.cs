using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Backoffice
{
    public class AgencyDataModel
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int AccountID { get; set; }
        public string City { get; set; }
        public string UserType { get; set; }
        public bool Active_Status { get; set; }
        public DateTime EventTime { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
