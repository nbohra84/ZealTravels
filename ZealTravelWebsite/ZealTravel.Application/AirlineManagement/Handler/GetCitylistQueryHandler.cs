using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class GetCitylistQueryHandler : IHandlesQueryAsync<GetCitySearchTermQuery, List<string>>
    {
        ICityRepository _cityRepository;
        public GetCitylistQueryHandler(ICityRepository cityRepository)
        {
            _cityRepository= cityRepository;
        }
        public Task<List<string>> HandleAsync(GetCitySearchTermQuery query)
        {
           return _cityRepository.GetSectorAsync(query.SearchTerm);
        }
    }
}
