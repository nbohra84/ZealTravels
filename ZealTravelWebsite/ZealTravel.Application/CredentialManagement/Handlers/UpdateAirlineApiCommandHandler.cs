using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateAirlineApiCommandHandler : IHandlesCommandAsync<UpdateAirlineApiCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public UpdateAirlineApiCommandHandler(ZealdbNContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UpdateAirlineApiCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var existingSupplier = await _dbContext.Set<SupplierDetailApiAirline>()
                .FirstOrDefaultAsync(s => s.Id == command.Id);

            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found.");
            }

            existingSupplier.UserId = command.UserId;
            existingSupplier.Password = command.Password;
            await _dbContext.SaveChangesAsync();
        }
    }
}
