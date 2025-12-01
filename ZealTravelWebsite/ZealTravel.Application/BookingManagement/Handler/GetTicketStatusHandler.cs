using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class GetTicketStatusHandler : IHandlesQueryAsync<int, bool>
    {
        private readonly IBookingManagementService _bookingManagementService;
        public GetTicketStatusHandler(IBookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }
        public async Task<bool> HandleAsync(int bookingRef)
        {
            try
            {
                var status = await _bookingManagementService.GetBookingTicketStatus(bookingRef);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error handling Ticket status for booking {bookingRef}: {ex.Message}", ex);
            }
        }
    }
}
