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
    public class DashboardCorporateQueryHandler : IHandlesQueryAsync<DashboardCorporateQuery, List<DashboardCorporateData>>
    {
        private readonly IAgencyDashboardService _reportingService;
        private readonly IMapper _mapper;
        public DashboardCorporateQueryHandler(IAgencyDashboardService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }
        public async Task<List<DashboardCorporateData>> HandleAsync(DashboardCorporateQuery query)
        {
            var data = await _reportingService.DashboardCorporateData(
                query.CompanyId
            );

            return _mapper.Map<List<DashboardCorporateData>>(data);

        }
    }
}
