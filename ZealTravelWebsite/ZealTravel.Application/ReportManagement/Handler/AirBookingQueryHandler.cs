using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.ReportManagement.Handler
{
    public class AirBookingQueryHandler : IHandlesQueryAsync<AirBookingQuery, List<AirBookingReport>>
    {
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public AirBookingQueryHandler(IReportingService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<AirBookingReport>> HandleAsync(AirBookingQuery query)
        {
            var data = await _reportingService.FilghtBookingReportAsync(
                query.CompanyId,
                query.FromDate,
                query.ToDate,
                query.TicketSearchType,
                query.SearchByValue
            );

            return _mapper.Map<List<AirBookingReport>>(data);
        }
    }
}
