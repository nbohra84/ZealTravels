using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.PaymentGatewayManagement.Queries
{
    public class GetPaymentGatewayCardChargesQuery
    {
        public string CompanyID { get; set; }
        public string AdminID { get; set; }
    }
}
