using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.PaymentGatewayManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.PaymentGatewayManagement;

namespace ZealTravel.Application.PaymentGatewayManagement.Handlers
{
    public class SetPaymentGatewayLoggerQueryHandler : IHandlesQueryAsync<SetPaymentGatewayLoggerQuery, Int32>
    {
        private readonly IPaymentGatewayService _paymentGatewayCardChargesService;

        public SetPaymentGatewayLoggerQueryHandler(IPaymentGatewayService paymentGatewayCardChargesService)
        {
            _paymentGatewayCardChargesService = paymentGatewayCardChargesService;
        }

        public async Task<Int32> HandleAsync(SetPaymentGatewayLoggerQuery query)
        {
            var result = 0;
            try
            {
                result = await _paymentGatewayCardChargesService.SetPaymentGatewayLogger(query.MerchantCode, query.CompanyID, query.BookingRef, query.Amount, query.SurchargeAmount, query.SurchargeAmount, query.CardType, query.TransactionType, query.RequestRemark,
                    query.Mobile, query.Email, query.Address, query.Host, query.IP, query.CardName);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
