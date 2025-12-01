using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Infrastructure.Services;

namespace ZealTravel.Application.ReportManagement.Handler
{
    public class DailyBookingQueryHandler : IHandlesQueryAsync<DailyBookingReportsQuery, List<DailyBooking>>
    {
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public DailyBookingQueryHandler(IReportingService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<DailyBooking>> HandleAsync(DailyBookingReportsQuery query)
        {
            var data = await _reportingService.GetDailyBookingReportsAsync(query.CompanyId, query.StartDate, query.EndDate);
            return _mapper.Map<List<DailyBooking>>(data);
        }

    }

}
