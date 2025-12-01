using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateUapiCcHandler : IHandlesCommandAsync<UpdateUapiCcCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public UpdateUapiCcHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task HandleAsync(UpdateUapiCcCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Id <= 0) throw new ArgumentException("Invalid ID provided.", nameof(command.Id));

        
            var rowsAffected = await _dbContext.UapiCcDetails
                .Where(u => u.Id == command.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(x => x.BankCountryCode, command.BankCountryCode)
                    .SetProperty(x => x.BankName, command.BankName)
                    .SetProperty(x => x.Cvv, command.Cvv)
                    .SetProperty(x => x.ExpDate, command.ExpDate)
                    .SetProperty(x => x.Name, command.Name)
                    .SetProperty(x => x.Number, command.Number)
                    .SetProperty(x => x.Type, command.Type)
                    .SetProperty(x => x.AddressName, command.AddressName)
                    .SetProperty(x => x.Street, command.Street)
                    .SetProperty(x => x.City, command.City)
                    .SetProperty(x => x.State, command.State)
                    .SetProperty(x => x.PostalCode, command.PostalCode)
                    .SetProperty(x => x.Country, command.Country)
                    .SetProperty(x => x.Carriers, command.Carriers)
                );

            if (rowsAffected == 0)
            {
                throw new KeyNotFoundException($"Supplier with ID {command.Id} not found.");
            }
        }
    }
}
