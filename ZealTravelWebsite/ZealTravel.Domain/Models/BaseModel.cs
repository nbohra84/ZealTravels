using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class BaseModel
    {
        public string Supplierid{get;set;}
        public string Password { get; set; }
        public string Searchid { get; set; }
        public string Companyid { get; set; }
        public string EndUserIp { get; set; }
        public string SupplierType { get; set; }
        public string JourneyType { get; set; }
    }
}
