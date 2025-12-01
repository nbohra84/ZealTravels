using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirlineSupplierManagement.Queries;
using ZealTravel.Application.PaymentGatewayManagement.Queries;
using ZealTravel.Domain.Interfaces.AirlineSupplierManagement;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.PaymentGatewayManagement;

namespace ZealTravel.Application.PaymentGatewayManagement.Handlers
{
    public class GetPaymentGatewayCardChargesQueryHandler : IHandlesQueryAsync<GetPaymentGatewayCardChargesQuery, List<PaymentGatewayDisplayOption>>
    {
        private readonly IPaymentGatewayService _paymentGatewayCardChargesService;
        private readonly IMapper _mapper;   

        public GetPaymentGatewayCardChargesQueryHandler(IPaymentGatewayService paymentGatewayCardChargesService, IMapper mapper)
        {
            _paymentGatewayCardChargesService = paymentGatewayCardChargesService;
            _mapper = mapper;
        }   

        public async Task<List<PaymentGatewayDisplayOption>> HandleAsync(GetPaymentGatewayCardChargesQuery query)
        {
            var paymentDisplayOptions = await _paymentGatewayCardChargesService.GetPaymentGatewayCardChargesDetailAsync(query.CompanyID, query.AdminID);
            return _mapper.Map<List<PaymentGatewayDisplayOption>>(paymentDisplayOptions);
        }
    }
}
