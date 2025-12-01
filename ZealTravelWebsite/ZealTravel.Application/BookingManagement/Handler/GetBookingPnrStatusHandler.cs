using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class GetBookingPnrStatusHandler : IHandlesQueryAsync<int, bool>
    {
        private readonly IBookingManagementService _bookingManagementService;
        public GetBookingPnrStatusHandler(IBookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }
        public async Task<bool> HandleAsync(int bookingRef)
        {
            try
            {
                var status = await _bookingManagementService.GetBookingPnrStatus(bookingRef);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error handling PNR status for booking {bookingRef}: {ex.Message}", ex);
            }
        }
    }
}
