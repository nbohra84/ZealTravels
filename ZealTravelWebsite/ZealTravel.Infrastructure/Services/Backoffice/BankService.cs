using CommonComponents;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models.Backoffice;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Backoffice
{
    public class BankService : IBankService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BankService));

        public BankService(ZealdbNContext context)
        {
            _context = context;
        }



        public async Task<List<ManageBankDetail>> GetBankDetails(string companyId)
        {
            try
            {
                var bankDetail = await _context.Database.SqlQuery<ManageBankDetail>(
                    $"EXECUTE Bank_Detail_Proc @ProcNo = 3, @CompanyID = {companyId}"
                ).ToListAsync();

                return bankDetail;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving airline pending booking reports: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteBank(string companyId, int id)
        {
            bool rowsAffectedStatus = false;
            try
            {
                string sql = @"EXECUTE Bank_Detail_Proc 
                       @ProcNo = {0}, @CompanyID = {1}, @ID = {2}";

                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    4, // ProcNo for deletion
                    companyId,
                    id
                );

                if (rowsAffected > 0)
                {
                    rowsAffectedStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting bank details: {ex.Message}", ex);
            }
            return rowsAffectedStatus;
        }


        public async Task<List<BankNameCode>> GetBankNameCode()
        {
            try
            {
                var bankDetail = await _context.Database.SqlQuery<BankNameCode>(
                    $"EXECUTE Bank_Detail_Proc @ProcNo = 6"
                ).ToListAsync();

                return bankDetail;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving airline pending booking reports: {ex.Message}", ex);
            }
        }

        public async Task<bool> SetBankDetail(
     string companyId,
     string bankName,
     string bankCode,
     string branchName,
     string bankLogoCode,
     string accountNo,
     bool? status,
     bool? b2b,
     bool? d2b,
     bool? b2c,
     bool? b2b2b,
     bool? b2b2c,
     int id)
        {
            bool rowsAffectedStatus = false;
            try
            {
                string sql = @"EXECUTE Bank_Detail_Proc 
                       @ProcNo = {0}, @CompanyID = {1}, @Bank_Name = {2}, 
                       @Bank_Code = {3}, @Branch_Name = {4}, @Bank_Logo_Code = {5}, 
                       @AccountNo = {6}, @Status = {7}, @B2B = {8}, 
                       @D2B = {9}, @B2C = {10}, @B2B2B = {11}, 
                       @B2B2C = {12}, @ID = {13}";

                // Execute the stored procedure
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    2, // ProcNo for updating
                    companyId,
                    bankName,
                    bankCode,
                    branchName,
                    bankLogoCode,
                    accountNo,
                    status,
                    b2b,
                    d2b,
                    b2c,
                    b2b2b,
                    b2b2c,
                    id
                );

                if (rowsAffected > 0)
                {
                    rowsAffectedStatus = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating bank details: {ex.Message}", ex);
            }
            return rowsAffectedStatus;
        }



        public async Task<bool> AddBankDetail(string companyId, string bankName, string bankCode, string branchName, string bankLogoCode, string accountNo, bool? status, bool? b2b, bool? d2b, bool? b2c, bool? b2b2b, bool? b2b2c)
        {
            bool rowsAffectedStatus = false;
            try
            {
                string sql = @"EXECUTE Bank_Detail_Proc 
                   @ProcNo = {0}, @CompanyID = {1}, @Bank_Name = {2}, 
                   @Bank_Code = {3}, @Branch_Name = {4}, @Bank_Logo_Code = {5}, 
                   @AccountNo = {6}, @Status = {7}, @B2B = {8}, 
                   @D2B = {9}, @B2C = {10}, @B2B2B = {11}, @B2B2C = {12}";
                // Execute the stored procedure
                var rowsAffected = _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    companyId,
                    bankName,
                    bankCode,
                    branchName,
                    bankLogoCode,
                    accountNo,
                    status,
                    b2b,
                    d2b,
                    b2c,
                    b2b2b,
                    b2b2c
                );
                if (rowsAffected.Result > 0)
                {
                    rowsAffectedStatus = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving bank details: {ex.Message}", ex);
            }
            return rowsAffectedStatus;
        }



    }
}
