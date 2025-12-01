using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Application.BackofficeManagement.Queries.Dashboard;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BackofficeManagement.Handlers.Dashboard
{
    public class DashboardAirlinePendingBookingQueryHandler : IHandlesQueryAsync<List<DashboardAirlinePendingBookingData>>
    {

        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        public DashboardAirlinePendingBookingQueryHandler(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }
        public async Task<List<DashboardAirlinePendingBookingData>> HandleAsync()
        {
            var data = await _dashboardService.GetAirlinePendingBookings();

            return _mapper.Map<List<DashboardAirlinePendingBookingData>>(data);

        }
    }
}

