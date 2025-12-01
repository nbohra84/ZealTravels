using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.PaymentGatewayManagement;

namespace ZealTravel.Application.PaymentGatewayManagement.Handlers
{
    public class GetPGOurOwnerStatusQueryHandler : IHandlesQueryAsync<string, bool>
    {
        private readonly IPaymentGatewayService _paymentGatewayCardChargesService;

        public GetPGOurOwnerStatusQueryHandler(IPaymentGatewayService paymentGatewayCardChargesService)
        {
            _paymentGatewayCardChargesService = paymentGatewayCardChargesService;
        }

        public async Task<bool> HandleAsync(string merchantID)
        {
            return await _paymentGatewayCardChargesService.GetPGOurOwnerStatusAsync(merchantID);
        }
    }
}
