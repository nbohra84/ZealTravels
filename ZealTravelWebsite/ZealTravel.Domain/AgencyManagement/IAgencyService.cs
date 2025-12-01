using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models.Backoffice;

namespace ZealTravel.Domain.AgencyManagement
{
    public interface IAgencyService
    {
        Task<decimal> GetAvailableBalanceAsync(string companyId);
        Task<List<string>> GetAgencyListbySerachText(int accountId, string searchText);
        Task<bool> VerifyTicketBalance(string CompanyID, Decimal Transaction_Amount);
        Task<bool> SetGetCompanyAmountTransaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark);
        Task<bool> SETDebitCompanyAmountTransaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark);
        Task<bool> SetTransactionDetail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel);
        Task<DataTable> GetCompanyDetailbyCompanyID(string CompanyID);

        Task<List<AgencyDataModel>> SearchAgencyBySearchText(int? accountId, string companyId, string? companyName, string? state, string? city);
        Task<List<CompanyBalanceTransactionDetailEvent>> GetCompanyBalanceTransactionDetailEvents(string eventType);
        Task<bool> UpdateCompanyTransactionAmount(string companyId, Decimal Transaction_Amount, string TransType, int BookingRef, string UpdatedBy, string Remark, string EventID, bool IsAirline, bool IsHotel, int Passengerid);
        Task<bool> VerifyAgentBooking(string companyId, int BookingRef, string BookingType);
    }
}
