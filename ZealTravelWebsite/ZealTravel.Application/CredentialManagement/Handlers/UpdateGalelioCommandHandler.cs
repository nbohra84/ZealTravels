using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
//using ZealTravel.Domain.Interfaces.User_Management;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateGalelioCommandHandler : IHandlesCommandAsync<UpdateGalelioCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public UpdateGalelioCommandHandler(ZealdbNContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UpdateGalelioCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var existingSupplier = await _dbContext.Set<SupplierDetailGalileoAirline>()
                .FirstOrDefaultAsync(s => s.Id == command.Id);

            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found.");
            }

            existingSupplier.Hap = command.Hap;
            existingSupplier.ImportQueue = command.ImportQueue;
            existingSupplier.Pcc = command.Pcc;
            existingSupplier.Password = command.Password;
            existingSupplier.SoapUrl = command.SoapUrl;
            existingSupplier.TicketIfFareGaurantee = command.TicketIfFareGaurantee;
            existingSupplier.TktdQueue = command.TktdQueue;

            await _dbContext.SaveChangesAsync();
        }
    }
}
