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
    public class AddDbSearchLogCommandHandler : IHandlesCommandAsync<AddDBSearchLogCommand>
    {
        private readonly IDBLoggerService _dBLoggerService;

        public AddDbSearchLogCommandHandler(IDBLoggerService dBLoggerService)
        {
            _dBLoggerService = dBLoggerService;
        }
        public async Task HandleAsync(AddDBSearchLogCommand command)
        {

            var result = await _dBLoggerService.dbSearchLogg(
                  command.CompanyID,
                  command.BookingRef,
                    command.StaffID,
                    command.SearchID,
                    command.Location,
                    command.Status,
                    command.Place,
                    command.Remark,
                    command.Remark2,
                    command.Host
                  );
        }
    }
}
