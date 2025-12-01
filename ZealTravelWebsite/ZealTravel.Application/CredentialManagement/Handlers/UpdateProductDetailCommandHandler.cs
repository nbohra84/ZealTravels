using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class UpdateProductDetailCommandHandler : IHandlesCommandAsync<SupplierProductDetailCommand>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly ILogger<UpdateProductDetailCommandHandler> _logger;

        public UpdateProductDetailCommandHandler(ZealdbNContext dbContext, ILogger<UpdateProductDetailCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(SupplierProductDetailCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Id <= 0) throw new ArgumentException("Invalid ID provided.", nameof(command.Id));

            try
            {
                var query = "UPDATE dbo.Supplier_Product_Detail SET ";

                var parameters = new List<SqlParameter>();
                var setClauses = new List<string>();

                if (!string.IsNullOrEmpty(command.SupplierName))
                {
                    setClauses.Add("Supplier_Name = @SupplierName");
                    parameters.Add(new SqlParameter("@SupplierName", command.SupplierName));
                }

                if (command.Status.HasValue)
                {
                    setClauses.Add("Status = @Status");
                    parameters.Add(new SqlParameter("@Status", command.Status.Value));
                }

                if (setClauses.Count == 0)
                {
                    throw new ArgumentException("No fields to update.");
                }

                query += string.Join(", ", setClauses) + " WHERE ID = @Id";
                parameters.Add(new SqlParameter("@Id", command.Id));

                int rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters.ToArray());

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("No changes were saved to the database.");
                    throw new Exception("No changes were saved to the database.");
                }

                _logger.LogInformation("Successfully updated supplier product detail with ID {Id}.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the product detail.");
                throw new Exception("An unexpected error occurred while updating the product detail.", ex);
            }
        }

    }
}