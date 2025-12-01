using System;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Command;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Application.BankManagement.Query;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class DeleteBankDetailHandler
    {
        private readonly IBankService _bankService;

        public DeleteBankDetailHandler(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task HandleAsync(GetBankDetailQuery command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {

                await _bankService.DeleteBank(command.CompanyId, command.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"UN-Success");
            }
        }
    }
}
