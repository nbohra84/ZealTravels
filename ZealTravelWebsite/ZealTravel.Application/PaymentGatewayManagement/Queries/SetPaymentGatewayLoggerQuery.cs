using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.PaymentGatewayManagement.Queries
{
    public class SetPaymentGatewayLoggerQuery
    {
        public string MerchantCode { get; set; }
        public string CompanyID { get; set; }
        public int BookingRef { get; set; }
        public decimal Amount { get; set; }
        public decimal SurchargeAmount { get; set; }
        public decimal Surcharge { get; set; }
        public string CardType { get; set; }
        public string TransactionType { get; set; }
        public string RequestRemark { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Host { get; set; }
        public string IP { get; set; }
        public string CardName { get; set; }
    }
}
