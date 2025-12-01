using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class PaymentGatewayDisplayOption
    {
        public string CompanyID { get; set; }
        public string Card_Type_Name { get; set; }
        public string Merchant_Code { get; set; }
        public string Card_Name { get; set; }
        public string Card_Type { get; set; }
        public string PG_Name { get; set; }
        public decimal Charges { get; set; }
        public bool FIXED { get; set; }
        public bool PERCNT { get; set; }
    }
}
    