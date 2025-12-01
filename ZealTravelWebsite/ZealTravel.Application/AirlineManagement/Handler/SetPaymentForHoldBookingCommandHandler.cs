using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirlineManagement.Command;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class SetPaymentForHoldBookingCommandHandler : IHandlesCommandAsync<SetPaymentForHoldBookingCommand>
    {
        private readonly IAirHoldBookingPaymentService _airSendHoldPayment;
        private IDBLoggerService _dbLoggerService;

        public SetPaymentForHoldBookingCommandHandler(IDBLoggerService dbLoggerService, IAirHoldBookingPaymentService airSendHoldPayment)
        {
            _airSendHoldPayment = airSendHoldPayment;
            _dbLoggerService = dbLoggerService;
        }

        public async Task HandleAsync(SetPaymentForHoldBookingCommand command)
        {
            string sendPayment = string.Empty;
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }

            
                string result = await _airSendHoldPayment.SetHoldBookingBookingPaymentAsync(command.BookingRef,command.Remarks,command.PaymentType,command.SearchId);
                

            

        }
    }
}
