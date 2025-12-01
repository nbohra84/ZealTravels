using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.PaymentGatewayManagement;
using ZealTravel.Domain.Models;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Aireline
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly ZealdbNContext _context;

        public PaymentGatewayService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentGatewayDisplayOption>> GetPaymentGatewayCardChargesDetailAsync(string companyId, string adminId)
        {
            var result = new List<PaymentGatewayDisplayOption>();
            try
            {
                result = await _context.Database.SqlQuery<PaymentGatewayDisplayOption>($"EXEC Payment_Gateway_Card_Charges_Detail_Proc @ProcNo = 15, @CompanyID = {companyId}, @AdminID = {adminId}").ToListAsync();
            }
            catch (Exception ex)
            {
               // Logger.WriteLogg(companyId, 0, "GetPaymentGatewayCardChargesDetailAsync", "PaymentGatewayCardChargesService", "", "", ex.Message);
                //return new List<PaymentGatewayCardCharge>();
            }

            return result;
        }

        public async Task<bool> GetPGOurOwnerStatusAsync(string merchantCode)
        {
            string companyID = string.Empty;
            try
            {
                var companyIDList = await _context.Database.SqlQuery<string>($"EXEC Payment_Gateway_Card_Charges_Detail_Proc @ProcNo = 12, @Merchant_Code = {merchantCode}").ToListAsync();
                companyID = companyIDList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, 0, "", "PG_Our_Owner_Status", "clsDB", merchantCode, ex.Message);
            }

            if (companyID.Equals("AD-101"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Int32> SetPaymentGatewayLogger(string Merchant_Code, string CompanyID, Int32 BookingRef, Decimal Amount, Decimal SurchargeAmount, Decimal Surcharge, string CardType, string TransactionType, string RequestRemark, string Mobile, string Email, string Address, string Host, string IP, string Card_Name)
        {
            Int32 PaymentID = 0;
            try
            {
                var result = await _context.Database.SqlQuery<Int32>(
                    $"EXECUTE Payment_Gateway_Logger_Proc @ProcNo = 1, @Merchant_Code = {Merchant_Code}, @CompanyID = {CompanyID}, @BookingRef = {BookingRef}, @Amount = {Amount}, @SurchargeAmount = {SurchargeAmount}, @Surcharge = {Surcharge}, @CardType = {CardType}, @TransactionType = {TransactionType}, @Mobile = {Mobile}, @Email = {Email}, @Address = {Address}, @Host = {Host}, @IP = {IP}, @RequestRemark = {RequestRemark}, @Card_Name = {Card_Name}"
                ).ToListAsync();

                if (result.FirstOrDefault() > 0)
                {
                    PaymentID = result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Payment_Gateway_Logger", TransactionType, Amount.ToString() + "," + SurchargeAmount.ToString(), Host + "," + IP, ex.Message);
            }

            return PaymentID;
        }
    }
}
