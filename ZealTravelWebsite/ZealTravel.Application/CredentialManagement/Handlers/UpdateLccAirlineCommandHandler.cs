using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateLccAirlineCommandHandler : IHandlesCommandAsync<UpdateLccAirlineCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public UpdateLccAirlineCommandHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UpdateLccAirlineCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (command.Id <= 0)
                throw new ArgumentException("Invalid airline ID.");

            var existingSupplier = await _dbContext.Set<SupplierDetailLccAirline>()
                .FirstOrDefaultAsync(s => s.Id == command.Id);

            if (existingSupplier == null)
                throw new Exception($"Supplier with ID {command.Id} not found.");

            existingSupplier.SupplierName = command.SupplierName ?? existingSupplier.SupplierName;
            existingSupplier.SupplierId = command.SupplierId ?? existingSupplier.SupplierId;
            existingSupplier.SupplierCode = command.SupplierCode ?? existingSupplier.SupplierCode;
            existingSupplier.TargetBranch = command.TargetBranch ?? existingSupplier.TargetBranch;
            existingSupplier.OrganizationCode = command.OrganizationCode ?? existingSupplier.OrganizationCode;
            existingSupplier.CarrierCode = command.CarrierCode ?? existingSupplier.CarrierCode;
            existingSupplier.AgentId = command.AgentId ?? existingSupplier.AgentId;
            existingSupplier.AgentDomain = command.AgentDomain ?? existingSupplier.AgentDomain;
            existingSupplier.Password = command.Password ?? existingSupplier.Password;
            existingSupplier.LocationCode = command.LocationCode ?? existingSupplier.LocationCode;
            existingSupplier.ContractVersion = command.ContractVersion ?? existingSupplier.ContractVersion;
            existingSupplier.PromoCode = command.PromoCode ?? existingSupplier.PromoCode;
            existingSupplier.CorporateCode = command.CorporateCode ?? existingSupplier.CorporateCode;
            existingSupplier.Currency = command.Currency ?? existingSupplier.Currency;
            existingSupplier.LoginId = command.LoginId ?? existingSupplier.LoginId;
            existingSupplier.Pwd = command.Pwd ?? existingSupplier.Pwd;
            existingSupplier.SessionUrl = command.SessionUrl ?? existingSupplier.SessionUrl;
            existingSupplier.BookingUrl = command.BookingUrl ?? existingSupplier.BookingUrl;
            existingSupplier.FareUrl = command.FareUrl ?? existingSupplier.FareUrl;
            existingSupplier.ContentUrl = command.ContentUrl ?? existingSupplier.ContentUrl;
            existingSupplier.AgentUrl = command.AgentUrl ?? existingSupplier.AgentUrl;
            existingSupplier.LookupUrl = command.LookupUrl ?? existingSupplier.LookupUrl;
            existingSupplier.OperationUrl = command.OperationUrl ?? existingSupplier.OperationUrl;

            try
            {
                // Save the changes
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the supplier data.", ex);
            }
        }
    }
}
