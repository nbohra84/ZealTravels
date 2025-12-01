using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class InsertPnrMakeDaysHandler : IHandlesCommandAsync<InsertPnrMakeDaysCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public InsertPnrMakeDaysHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task HandleAsync(InsertPnrMakeDaysCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var sql = @"INSERT INTO dbo.AirlinePnrMakeDays (CarrierCode, Sector, PnrDays) 
                        VALUES (@CarrierCode, @Sector, @PnrDays)";

            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@CarrierCode", (object)command.CarrierCode ?? DBNull.Value),
                    new SqlParameter("@Sector", (object)command.Sector ?? DBNull.Value),
                    new SqlParameter("@PnrDays", (object)command.PnrDays ?? DBNull.Value)
                );
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw;
            }
        }
    }
}
