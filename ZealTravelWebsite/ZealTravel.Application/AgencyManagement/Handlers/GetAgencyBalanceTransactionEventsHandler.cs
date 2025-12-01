using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.Services;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class GetAgencyBalanceTransactionEventsHandler : IHandlesQueryAsync<GetAgencyBalanceTransactionEventQuery, List<GetAgencyBalanceTransactionEvents>>
    {
        private readonly IAgencyService _agencyService;
        private readonly IMapper _mapper;
        public GetAgencyBalanceTransactionEventsHandler(IAgencyService agencyService, IMapper mapper)
        {
            _agencyService = agencyService;
            _mapper = mapper;
        }
        public async Task<List<GetAgencyBalanceTransactionEvents>> HandleAsync(GetAgencyBalanceTransactionEventQuery query)
        {
            var states = await _agencyService.GetCompanyBalanceTransactionDetailEvents(query.EventType);

            return _mapper.Map<List<GetAgencyBalanceTransactionEvents>>(states);
        }
    }
}
