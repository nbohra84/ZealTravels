using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class GetAgencyListbySerachTextQueryHandler : IHandlesQueryAsync<AgencyListBySearchTextQuery, List<string>>
    {
        private readonly IAgencyService _reportingService;
        private readonly IMapper _mapper;

        public GetAgencyListbySerachTextQueryHandler(IAgencyService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<string>> HandleAsync(AgencyListBySearchTextQuery query)
        {
            var data = await _reportingService.GetAgencyListbySerachText(
                query.AccountId,
                query.SearchText
            );

            return _mapper.Map<List<string>>(data);
        }
    }
}
