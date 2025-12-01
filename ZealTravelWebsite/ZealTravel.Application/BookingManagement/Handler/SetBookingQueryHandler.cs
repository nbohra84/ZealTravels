using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.Handlers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class SetBookingQueryHandler : IHandlesQueryAsync<SetBookingQuery, Int32>
    {
        private readonly IBookingService _bookingService;
        private IDBLoggerService _dbLoggerService;

        public SetBookingQueryHandler(IBookingService bookingService, IDBLoggerService dbLoggerService)
        {   
            _bookingService = bookingService;
            _dbLoggerService = dbLoggerService;
        }
        public async Task<Int32> HandleAsync(SetBookingQuery query)
        {
            var bookingRef = 0;
            try
            {
              bookingRef = await _bookingService.SetBooking(query.SearchID, query.CompanyID, query.UpdatedBy, query.IsCombi, query.IsRTfare, query.IsQueue, query.IsOffline, query.PaymentType, query.PaymentID, query.PassengerResponse, query.AvailabilityResponse, query.RefID_O, query.RefID_I, false, query.GstInfo);
            }
            catch ( Exception ex)
            {
                await _dbLoggerService.dbLogg(query.CompanyID, bookingRef, " SetBooking", "SetBookingQueryHandler", "SAVE", query.SearchID, ex.Message);
                throw new Exception("SetBooking - " + ex.Message);
            }

            return bookingRef;
        }
    }
}
