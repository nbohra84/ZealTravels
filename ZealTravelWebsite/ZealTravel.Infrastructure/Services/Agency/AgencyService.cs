using Microsoft.Data.SqlClient;
using System;
using log4net;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.AgencyManagement;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Domain.Data.Models.Agency;
using Dapper;
using ZealTravel.Domain.Data.Models.Backoffice;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Infrastructure.Services.Agency
{

    public class AgencyService : IAgencyService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AgencyService));

        public AgencyService(ZealdbNContext context)
        {
            _context = context;
        }
        public async Task<decimal> GetAvailableBalanceAsync(string companyId)
        {
            decimal availableBalance = 0;

            try
            {
                var amout = await _context.Database.SqlQuery<decimal>($"EXECUTE Company_Balance_Transaction_Detail_Proc @ProcNo = 1, @CompanyID = {companyId}").ToListAsync();
                availableBalance = amout.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving available balance: {ex.Message}");
            }

            return availableBalance;
        }

        public async Task<List<string>> GetAgencyListbySerachText(int accountId, string SearchText)
        {
            try
            {

                var reports = await _context.Database.SqlQuery<string>(
                    $"EXECUTE Company_Register_Proc @ProcNo = 15, @AccountID = {accountId},@SearchText = {SearchText}"
                ).ToListAsync();
                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }

        public async Task<bool> VerifyTicketBalance(string CompanyID, Decimal Transaction_Amount)
        {
            bool Status = false;

            try
            {
                var result = await _context.Database.SqlQuery<Int32>($"EXECUTE Company_Balance_Transaction_Detail_Proc @ProcNo = 3, @CompanyID = {CompanyID}, @Transaction_Amount = {Transaction_Amount}").ToListAsync();

                if (result.FirstOrDefault() > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error verifying ticket balance: {ex.Message}");
            }

            return Status;
        }

        public async Task<bool> SetGetCompanyAmountTransaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark)
        {
            bool bStatus = false;

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXECUTE Company_Balance_Transaction_Detail_Proc @ProcNo = 2, @CompanyID = {0}, @Transaction_Amount = {1}, @TransType = {2}, @BookingRef = {3}, @UpdatedBy = {4}, @Remark = {5}",
                    CompanyID, Transaction_Amount, TransType, BookingRef, UpdatedBy, Remark
                );

                if (result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                bStatus = false;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_GET_Company_Amount_Transaction", "db_Company", SearchCriteria, SearchID, TransType + Environment.NewLine + Remark + Environment.NewLine + ex.Message);
            }
            return bStatus;
        }

        public async Task<bool> SETDebitCompanyAmountTransaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark)
        {
            bool bStatus = false;

            try
            {
                string sql = @"EXECUTE Company_Balance_Transaction_Detail_Proc @ProcNo = {0}, @CompanyID = {1}, @Transaction_Amount = {2}, @TransType = {3}, @BookingRef = {4}, @UpdatedBy = {5}, @Remark = {6}";
                int result = await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    6, // ProcNo
                    CompanyID,
                    Transaction_Amount,
                    TransType,
                    BookingRef,
                    UpdatedBy,
                    Remark);

                if (result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                bStatus = false;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Debit_Company_Amount_Transaction", "db_Company", SearchCriteria, SearchID, TransType + Environment.NewLine + Remark + Environment.NewLine + ex.Message);
            }
            return bStatus;
        }

        public async Task<bool> SetTransactionDetail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel)
        {
            bool bStatus = false;

            try
            {
                string sql = @"EXECUTE Company_Transaction_Detail_Proc @ProcNo = {0}, @CompanyID = {1}, @BookingRef = {2}, @Debit = {3}, @Credit = {4}, @PaymentType = {5}, @PaymentID = {6}, @UpdatedBy = {7}, @Remark = {8}, @IsAirline = {9}, @IsHotel = {10}";
                var result =  _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    Debit,
                    Credit,
                    PaymentType,
                    PaymentID,
                    UpdatedBy,
                    Remark,
                    IsAirline,
                    IsHotel);

                if (result.Result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                bStatus = false;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Transaction_Detail", "db_Company", SearchCriteria, SearchID, "Debit- " + Debit.ToString() + "- Credit- " + Credit.ToString() + "- PaymentType-" + PaymentType + "- UpdatedBy-" + UpdatedBy + "-" + "- Remark-" + Remark + "-" + ex.Message);
            }
            return bStatus;
        }

        public async Task<DataTable> GetCompanyDetailbyCompanyID(string CompanyID)
        {
            DataTable CompanyDT = new DataTable();
            try
            {
                //var CompanyDetail = _context.CompanyRegisters.FromSqlRaw("EXEC Company_Register_Proc @ProcNo, @CompanyID", 
                //    new SqlParameter("@ProcNo", 21),
                //    new SqlParameter("@CompanyID", CompanyID)).ToList<PNRCompanyDetails>();

                var CompanyDetail = await _context.Database.SqlQuery<PNRCompanyDetails>($"EXEC Company_Register_Proc @ProcNo = 21, @CompanyID = {CompanyID}").ToListAsync();
                CompanyDT = CompanyDetail.ToDataTable();
                CompanyDT.TableName = "CompanyDetail";
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "GetCompanyDetailbyCompanyID", "Company_Register_Proc", "db_Company", "", ex.Message);
            }
            return CompanyDT;
        }


        public async Task<List<AgencyDataModel>> SearchAgencyBySearchText(int? accountId, string companyId, string? companyName, string? state, string? city)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<AgencyDataModel>(
                    $"EXECUTE Company_Register_Search_Proc @AccountID = {accountId},@CompanyID = {companyId},@CompanyName = {companyName},@State ={state},@City = {city}" 
                ).ToListAsync();
                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ledger descriptive report: {ex.Message}", ex);
            }
        }


        public async Task<List<CompanyBalanceTransactionDetailEvent>> GetCompanyBalanceTransactionDetailEvents(string eventType)
        {
            try
            {
                var eventDetails = await _context.Database.SqlQuery<CompanyBalanceTransactionDetailEvent>(
                    $"Company_Balance_Transaction_Detail_Events_Proc @ProcNo = 2, @EventType = {eventType}"
                ).ToListAsync();
                return eventDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving event type details: {ex.Message}", ex);
            }
        }


        public async Task<bool> UpdateCompanyTransactionAmount(string companyId, decimal transactionAmount, string transType, int bookingRef, string updatedBy, string remark, string eventId, bool isAirline, bool isHotel, int passengerId)
        {
            try
            {
                string sql = $"EXEC Company_Balance_Transaction_Detail_Proc @ProcNo = @p0, @CompanyID = @p1, @Transaction_Amount = @p2, @TransType = @p3, @BookingRef = @p4, @UpdatedBy = @p5, @EventID = @p6, @Remark = @p7, @IsAirline = @p8, @IsHotel = @p9, @Passengerid = @p10";

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    sql,
                    2, companyId, transactionAmount, transType, bookingRef, updatedBy, eventId, remark, isAirline, isHotel, passengerId
                );

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating company transaction: {ex.Message}", ex);
            }
        }



        public async Task<bool> VerifyAgentBooking(string CompanyId, int BookingRef, string BookingType)
        {
            try
            {
                var result = await _context.Database
                    .SqlQueryRaw<int>(
                        "EXEC Verify_Agency_By_BookingRef @ProcNo = 1, @CompanyID = {0}, @BookingRef = {1}, @BookingType = {2}",
                        CompanyId, BookingRef, BookingType
                    ).ToListAsync();

                return result.FirstOrDefault() == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
