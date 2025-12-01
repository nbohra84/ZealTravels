using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class BookingTicketQueryHandler : IHandlesQueryAsync<BookingDetailQuery, BookingDetailData>
    {
        private readonly IBookingManagementService _reportingService;
        private readonly IMapper _mapper;

        public BookingTicketQueryHandler(IBookingManagementService reportingService, IMapper mapper)
        {
            _reportingService = reportingService;
            _mapper = mapper;
        }

        public async Task<BookingDetailData> HandleAsync(BookingDetailQuery query)
        {
            var data = await _reportingService.GetBooking(
                query.BookingRef
            );

            return _mapper.Map<BookingDetailData>(data);
        }
    }
}
