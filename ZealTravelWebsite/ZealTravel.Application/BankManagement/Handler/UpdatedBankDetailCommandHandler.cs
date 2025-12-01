using System;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Command;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class UpdateBankDetailCommandHandler : IHandlesCommandAsync<UpdateBankCommand>
    {
        private readonly IBankService _bankService;
        public UpdateBankDetailCommandHandler(IBankService bankService)
        {
            _bankService = bankService;
        }
        public async Task HandleAsync(UpdateBankCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null");
            }
            try
            {
                await _bankService.SetBankDetail(
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
                    command.B2b2c,
                    command.Id
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"UN-Success");
            }
        }
    }
}
