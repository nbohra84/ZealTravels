using System;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Domain.Data.Models; 
using ZealTravel.Domain.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class InsertUapiCcHandler : IHandlesCommandAsync<InsertUapiCcCommand>
    {
        private readonly ZealdbNContext _dbContext;

        public InsertUapiCcHandler(ZealdbNContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task HandleAsync(InsertUapiCcCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var sql = @"INSERT INTO dbo.UAPI_CC_Details (BankCountryCode, BankName, Cvv, ExpDate, Name, Number, Type, AddressName, Street, City, State, PostalCode, Country, Carriers) 
            VALUES (@BankCountryCode, @BankName, @Cvv, @ExpDate, @Name, @Number, @Type, @AddressName, @Street, @City, @State, @PostalCode, @Country, @Carriers)";


            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@BankCountryCode", (object)command.BankCountryCode ?? DBNull.Value),
                    new SqlParameter("@BankName", (object)command.BankName ?? DBNull.Value),
                    new SqlParameter("@Cvv", (object)command.Cvv ?? DBNull.Value),
                    new SqlParameter("@ExpDate", (object)command.ExpDate ?? DBNull.Value),
                    new SqlParameter("@Name", (object)command.Name ?? DBNull.Value),
                    new SqlParameter("@Number", (object)command.Number ?? DBNull.Value),
                    new SqlParameter("@Type", (object)command.Type ?? DBNull.Value),
                    new SqlParameter("@AddressName", (object)command.AddressName ?? DBNull.Value),
                    new SqlParameter("@Street", (object)command.Street ?? DBNull.Value),
                    new SqlParameter("@City", (object)command.City ?? DBNull.Value),
                    new SqlParameter("@State", (object)command.State ?? DBNull.Value),
                    new SqlParameter("@PostalCode", (object)command.PostalCode ?? DBNull.Value),
                    new SqlParameter("@Country", (object)command.Country ?? DBNull.Value),
                    new SqlParameter("@Carriers", (object)command.Carriers ?? DBNull.Value)
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
