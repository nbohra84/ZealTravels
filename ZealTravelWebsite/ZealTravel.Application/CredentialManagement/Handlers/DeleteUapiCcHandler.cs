using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;
using Microsoft.Data.SqlClient; 

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class DeleteUapiCcHandler : IHandlesCommandAsync<int>
    {
        private readonly ZealdbNContext _dbContext;

        public DeleteUapiCcHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task HandleAsync(int id)
        {
            var sql = "DELETE FROM UAPI_CC_Details WHERE Id = @id";

            var affectedRows = await _dbContext.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@id", id));

            if (affectedRows == 0)
            {
                throw new KeyNotFoundException($"UAPI CC details with ID {id} not found.");
            }
        }
    }
}