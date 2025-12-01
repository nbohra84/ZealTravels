using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BackofficeManagement.Queries.Dashboard;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BackofficeManagement.Handlers.Dashboard
{
    public class DashboardAirlinetopTenBookingQueryHandler  : IHandlesQueryAsync<DashboardAirlinetopTenBookingQuery,List<DashboardAirlineTopTenBookingData>>
    {

        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        public DashboardAirlinetopTenBookingQueryHandler(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }
        public async Task<List<DashboardAirlineTopTenBookingData>> HandleAsync(DashboardAirlinetopTenBookingQuery query)
        {
            var data = await _dashboardService.GetTopTenAirlineBookings(query.CompanyId);

            return _mapper.Map<List<DashboardAirlineTopTenBookingData>>(data);

        }
    }
}

