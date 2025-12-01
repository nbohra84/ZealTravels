using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class GetFlightRefrenceHandler : IHandlesQueryAsync<BookingRefrenceQuery, List<FlightBookingRefrence>>
    {
        private readonly IBookingManagementService _reportingService;
        private readonly IMapper _mapper;

        public GetFlightRefrenceHandler(IBookingManagementService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<List<FlightBookingRefrence>> HandleAsync(BookingRefrenceQuery query)
        {
            var data = await _reportingService.GetBookingRef(query.StartDate, query.EndDate);
            return _mapper.Map<List<FlightBookingRefrence>>(data);
        }
    }
}
