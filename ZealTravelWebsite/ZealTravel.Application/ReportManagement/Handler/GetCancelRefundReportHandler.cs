using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.ReportManagement.Handler
{
    public class GetCancelRefundReportHandler : IHandlesQueryAsync<CancelAirRefrenceQuery, List<FlightBookingRefrence>>
    {
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public GetCancelRefundReportHandler(IReportingService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<FlightBookingRefrence>> HandleAsync(CancelAirRefrenceQuery query)
        {
            var data = await _reportingService.GetCancelRefundReport(query.StartDate, query.EndDate);
            return _mapper.Map<List<FlightBookingRefrence>>(data);
        }
    

    }
}
