using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers.AgencyDashboard
{
    public  class DashboardChartQueryHandler : IHandlesQueryAsync<DashboardChartQuery, List<DashboardChartData>>
    {

        private readonly IAgencyDashboardService _reportingService;
        private readonly IMapper _mapper;
        public DashboardChartQueryHandler(IAgencyDashboardService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }
        public async Task<List<DashboardChartData>> HandleAsync(DashboardChartQuery query)
        {
            var data = await _reportingService.GetDashboardChart(
                query.CompanyId
            );

            return _mapper.Map<List<DashboardChartData>>(data);

        }
    }
}
