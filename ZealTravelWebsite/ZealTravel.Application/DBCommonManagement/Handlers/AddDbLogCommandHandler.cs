using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Command;
using ZealTravel.Application.BankManagement.Query;
using ZealTravel.Application.DBCommonManagement.Commands;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class AddDbLogCommandHandler : IHandlesCommandAsync<AddDBLogCommand>
    {
        private readonly IDBLoggerService _dBLoggerService;

        public AddDbLogCommandHandler(IDBLoggerService dBLoggerService)
        {
            _dBLoggerService = dBLoggerService;
        }
        public async Task HandleAsync(AddDBLogCommand command)
        {

            var result = await _dBLoggerService.dbLogg(
                  command.CompanyID,
                  command.BookingRef,
                  command.MethodName,
                  command.Location,
                  command.SearchCriteria,
                  command.SearchID,
                  command.ErrorMessage
                  );
        }
    }
}
