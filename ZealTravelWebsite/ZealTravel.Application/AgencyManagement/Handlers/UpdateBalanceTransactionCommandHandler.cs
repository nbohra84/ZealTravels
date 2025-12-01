using System;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class UpdateBalanceTransactionCommandHandler : IHandlesCommandAsync<UpdateBalanceTransactionCommand>
    {
        private readonly IAgencyService _agencyService;

        public UpdateBalanceTransactionCommandHandler(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }

        public async Task HandleAsync(UpdateBalanceTransactionCommand command)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                bool isTransactionSuccessful = await _agencyService.UpdateCompanyTransactionAmount(
                    command.CompanyId,
                    command.TransactionAmount,
                    command.TransactionType,
                    command.BookingRef,
                    command.UpdatedBy,
                    command.Remark,
                    command.EventId,
                    command.IsAirline,
                    command.IsHotel,
                    command.PassengerId
                );

                if (!isTransactionSuccessful)
                {
                    throw new Exception("Transaction failed or no records were affected.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in BalanceTransactionCommandHandler: {ex.Message}", ex);
            }
        }

    }
}
