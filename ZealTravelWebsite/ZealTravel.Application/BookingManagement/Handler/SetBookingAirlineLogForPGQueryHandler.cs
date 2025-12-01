using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class SetBookingAirlineLogForPGQueryHandler : IHandlesQueryAsync<SetBookingAirlineLogForPGQuery, bool>
    {
        private readonly IBookingManagementService _bookingService;
        private IDBLoggerService _dbLoggerService;

        public SetBookingAirlineLogForPGQueryHandler(IBookingManagementService bookingService, IDBLoggerService dbLoggerService)
        {
            _bookingService = bookingService;
            _dbLoggerService = dbLoggerService;
        }
        public async Task<bool> HandleAsync(SetBookingAirlineLogForPGQuery query)
        {
            var status = false;
            try
            {
                status = await _bookingService.SetBookingAirlineLogForPG(query.IsCombi, query.IsRT, query.IsMC, query.SearchID, query.CompanyID, query.BookingRef, query.AvailabilityResponse, query.PassengerResponse, query.RefID_O, query.RefID_I, query.PaymentID, query.PaymentType, query.Currency, query.CurrencyValue);
            }
            catch ( Exception ex)
            {
                await _dbLoggerService.dbLogg(query.CompanyID, query.BookingRef, " SetBookingAirlineLogForPG", "SetBookingAirlineLogForPGQueryHandler","SAVE", query.SearchID, ex.Message);
            }

            return status;
;
        }
    }
}
