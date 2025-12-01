using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.ReportManagement.Handler
{
    public class AirSheetReportQueryHandler : IHandlesQueryAsync<AirBookingDetailQuery, AirSheetReportData>
    {
        private readonly IBookingManagementService _reportingService;
        private readonly IMapper _mapper;
        public AirSheetReportQueryHandler(IBookingManagementService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }
        public async Task<AirSheetReportData> HandleAsync(AirBookingDetailQuery query)
        {
            var data = await _reportingService.GetAirBooking(
                query.BookingRef
            );

            return _mapper.Map<AirSheetReportData>(data);
        }
    }
}
