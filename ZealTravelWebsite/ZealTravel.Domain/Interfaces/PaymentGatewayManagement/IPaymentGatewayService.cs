using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.PaymentGatewayManagement
{
    public interface IPaymentGatewayService
    {
        Task<List<PaymentGatewayDisplayOption>> GetPaymentGatewayCardChargesDetailAsync(string companyId, string adminId);
        Task<bool> GetPGOurOwnerStatusAsync(string merchantCode);
        Task<Int32> SetPaymentGatewayLogger(string Merchant_Code, string CompanyID, Int32 BookingRef, Decimal Amount, Decimal SurchargeAmount, Decimal Surcharge, string CardType, string TransactionType, string RequestRemark, string Mobile, string Email, string Address, string Host, string IP, string Card_Name);
    }
}
