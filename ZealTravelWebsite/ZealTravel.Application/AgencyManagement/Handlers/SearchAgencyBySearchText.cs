using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class SearchAgencyBySearchTextQueryHandler : IHandlesQueryAsync<SearchAgencyTextQuery, List<SearchAgencyListData>>
    {
        private readonly IAgencyService _agencyService;
        private readonly IMapper _mapper;

        public SearchAgencyBySearchTextQueryHandler(IAgencyService agencyService, IMapper mapper)
        {
            _agencyService = agencyService;
            _mapper = mapper;
        }

        public async Task<List<SearchAgencyListData>> HandleAsync(SearchAgencyTextQuery query)
        {
            var data = await _agencyService.SearchAgencyBySearchText(
             query.AccountId,
                query.CompanyID,
                query.CompanyName,
                query.State,
                query.City
            );

            return _mapper.Map<List<SearchAgencyListData>>(data);
        }
    }
}
