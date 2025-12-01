using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateProductStatusCommandHandler : IHandlesCommandAsync<UpdateProductStatusCommand>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly ILogger<UpdateProductStatusCommandHandler> _logger;

        public UpdateProductStatusCommandHandler(ZealdbNContext dbContext, ILogger<UpdateProductStatusCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(UpdateProductStatusCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Id <= 0) throw new ArgumentException("Invalid ID provided.", nameof(command.Id));

            if (!command.B2b.HasValue)
            {
                throw new ArgumentException("B2B value is required for the update.", nameof(command.B2b));
            }

            try
            {
                string query = "UPDATE dbo.Supplier_Product_Status SET B2B = @B2B WHERE ID = @Id";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@B2B", command.B2b.Value),
            new SqlParameter("@Id", command.Id)
        };

                int rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters.ToArray());

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("No changes were saved to the database.");
                    throw new Exception("No changes were saved to the database.");
                }

                _logger.LogInformation("Successfully updated B2B field for product status with ID {Id}.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the B2B field.");
                throw new Exception("An unexpected error occurred while updating the B2B field.", ex);
            }
        }

    }
}
