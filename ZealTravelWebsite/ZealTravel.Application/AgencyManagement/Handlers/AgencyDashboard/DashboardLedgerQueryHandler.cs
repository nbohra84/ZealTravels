using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers.AgencyDashboard
{
    public class DashboardLedgerQueryHandler : IHandlesQueryAsync<DashboardLedgerQuery, List<DashboardLedgerData>>
    {
        private readonly IAgencyDashboardService _reportingService;
        private readonly IMapper _mapper;
        public DashboardLedgerQueryHandler(IAgencyDashboardService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }
        public async Task<List<DashboardLedgerData>> HandleAsync(DashboardLedgerQuery query)
        {
            var data = await _reportingService.LedgerDescriptiveDashboardReportAsync(
                query.CompanyId,
                query.FromDate,
                query.ToDate,
                query.SearchBy,
                query.TicketSearchType,
                query.SearchByValue
            );

            return _mapper.Map<List<DashboardLedgerData>>(data);

        }
    }
}
