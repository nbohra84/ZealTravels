using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ZealTravel.Application.UserManagement.Handlers
{
    public class GetCitiesByStateQueryHandler : IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>>
    {
        private readonly IGstStateCityService _gstStateCityService;
        private readonly IMapper _mapper;

        public GetCitiesByStateQueryHandler(IGstStateCityService gstStateCityService, IMapper mapper)
        {
            _gstStateCityService = gstStateCityService;
            _mapper = mapper;
        }

        public async Task<List<CityState>> HandleAsync(GetCitiesByStateQuery query)
        {
            var states = await _gstStateCityService.GetCityByStateAsync(query.StateName);
            return _mapper.Map<List<CityState>>(states);
        }
    }
}
