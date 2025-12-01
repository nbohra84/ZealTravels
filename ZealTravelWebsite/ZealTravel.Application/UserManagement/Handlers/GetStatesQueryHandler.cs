using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Infrastructure.Services;
using ZealTravel.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ZealTravel.Domain.Interfaces.Services;


namespace ZealTravel.Application.UserManagement.Handlers
{
    public class GetStatesQueryHandler : IHandlesQueryAsync<GetStatesQuery, List<GstStates>>
    {
        private readonly IGstStateCityService _gstStateCityService;
        private readonly IMapper _mapper;

        public GetStatesQueryHandler(IGstStateCityService gstStateCityService, IMapper mapper)
        {
            _gstStateCityService = gstStateCityService;
            _mapper = mapper;
        }

        public async Task<List<GstStates>> HandleAsync(GetStatesQuery query)
        {
            var states = await _gstStateCityService.GetStatesAsync();

            return _mapper.Map<List<GstStates>>(states);
        }
    }
}