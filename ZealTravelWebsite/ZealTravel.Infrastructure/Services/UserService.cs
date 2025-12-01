using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.AgencyManagement;
using ZealTravelWebsite.Infrastructure.Context;
using log4net;
using Microsoft.Data.SqlClient;
using ZealTravel.Domain.Data.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UserService));

        public UserService(ZealdbNContext context)
        {
            _context = context;
        }
        public async Task<string> GetCompanyIDByAccountID(int accountId)
        {
            try
            {

                var reports = await _context.Database.SqlQuery<string>(
                    $"EXECUTE Company_Register_Proc @ProcNo = 16, @AccountID = {accountId}"
                ).ToListAsync();
                return reports.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }

    }
}
