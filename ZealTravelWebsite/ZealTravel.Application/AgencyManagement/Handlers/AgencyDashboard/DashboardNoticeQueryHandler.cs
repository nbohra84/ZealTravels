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
    public class DashboardNoticeQueryHandler : IHandlesQueryAsync<DashboardNotificationQuery, List<DashboardNotificationData>>
    {
        private readonly IAgencyDashboardService _reportingService;
        private readonly IMapper _mapper;
        public DashboardNoticeQueryHandler(IAgencyDashboardService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }
        public async Task<List<DashboardNotificationData>> HandleAsync(DashboardNotificationQuery query)
        {
            var data = await _reportingService.GetDashboardNotification(
                query.CompanyId
            );

            return _mapper.Map<List<DashboardNotificationData>>(data);

        }
    }
}
