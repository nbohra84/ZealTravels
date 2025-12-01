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
    public class UpdatePnrMakeDaysHandler : IHandlesCommandAsync<PnrMakeDaysCommand>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly ILogger<UpdatePnrMakeDaysHandler> _logger;

        public UpdatePnrMakeDaysHandler(ZealdbNContext dbContext, ILogger<UpdatePnrMakeDaysHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(PnrMakeDaysCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (command.Id <= 0) throw new ArgumentException("Invalid ID provided.", nameof(command.Id));

            try
            {
                var query = "UPDATE dbo.AirlinePnrMakeDays SET PnrDays = @PnrDays WHERE ID = @Id";

                var parameters = new[]
                {
                    new SqlParameter("@PnrDays", command.PnrDays),
                    new SqlParameter("@Id", command.Id)
                };

                int rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("No record found with ID {Id}.", command.Id);
                    throw new Exception($"No record found with ID {command.Id}.");
                }

                _logger.LogInformation("Successfully updated PnrDays for ID {Id}.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PnrDays for ID {Id}.", command.Id);
                throw new ApplicationException("Error updating PnrDays.", ex);
            }
        }
    }
}
