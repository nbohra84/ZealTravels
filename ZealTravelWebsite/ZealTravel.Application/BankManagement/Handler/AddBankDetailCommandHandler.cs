using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Command;
using ZealTravel.Application.BankManagement.Query;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class AddBankDetailCommandHandler : IHandlesCommandAsync<BankDetailCommand>
    {
        private readonly IBankService _bankService;

        public AddBankDetailCommandHandler(IBankService bankService)
        {
            _bankService = bankService;
        }
        public async Task HandleAsync(BankDetailCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }

            try
            {
                
                await _bankService.AddBankDetail(
                    command.CompanyId,
                    command.BankName,
                    command.BankCode,
                    command.BranchName,
                    command.BankLogoCode,
                    command.AccountNo,
                    command.Status,
                    command.B2b,
                    command.D2b,
                    command.B2c,
                    command.B2b2b,
                    command.B2b2c
                    );
            }
            catch (Exception ex)
            {
                throw new Exception($"UN-Success");
            }
        }
    }
}
