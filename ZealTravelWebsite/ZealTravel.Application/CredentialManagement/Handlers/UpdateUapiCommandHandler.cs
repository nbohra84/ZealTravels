using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateUapiCommandHandler : IHandlesCommandAsync<UpdateUapiFopCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public UpdateUapiCommandHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task HandleAsync(UpdateUapiFopCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await _dbContext.UapiFormOfPayments
       .ExecuteUpdateAsync(s => s.SetProperty(f => f.Status, false));
            var selectedFop = await _dbContext.UapiFormOfPayments
    .FirstOrDefaultAsync(f => f.Id == command.Id);

            if (selectedFop == null)
            {
                throw new Exception($"Supplier with FOP {command.Id} not found.");
            }

            await _dbContext.UapiFormOfPayments
    .Where(f => f.Id == command.Id)
    .ExecuteUpdateAsync(s => s.SetProperty(f => f.Status, true));

        }


    }
}