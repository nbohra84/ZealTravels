using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class DeletePnrMakeDaysHandler : IHandlesCommandAsync<int>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly ILogger<DeletePnrMakeDaysHandler> _logger;

        public DeletePnrMakeDaysHandler(ZealdbNContext dbContext, ILogger<DeletePnrMakeDaysHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid ID provided.", nameof(id));

            try
            {
                var sql = "DELETE FROM dbo.AirlinePnrMakeDays WHERE ID = @id";
                var affectedRows = await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@id", id));

                if (affectedRows == 0)
                {
                    _logger.LogWarning("No record found with ID {Id}.", id);
                    throw new KeyNotFoundException($"AirlinePnrMakeDays record with ID {id} not found.");
                }

                _logger.LogInformation("Successfully deleted record with ID {Id}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting AirlinePnrMakeDays record with ID {Id}.", id);
                throw new ApplicationException("Error deleting PNR Make Days record.", ex);
            }
        }
    }
}
