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
    public class CanelAirBookingQueryHandler : IHandlesQueryAsync<CancelAirBookingReportsQuery, List<CancelAirBooking>>
    {
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public CanelAirBookingQueryHandler(IReportingService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<CancelAirBooking>> HandleAsync(CancelAirBookingReportsQuery query)
        {
            var data = await _reportingService.GetCancelFlightBookingReportsAsync(query.SupplierId, query.FromDate, query.ToDate);
            return _mapper.Map<List<CancelAirBooking>>(data);
        }

    }

}
