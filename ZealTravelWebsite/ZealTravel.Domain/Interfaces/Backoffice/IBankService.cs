using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models.Backoffice;

namespace ZealTravel.Domain.Interfaces.Backoffice
{
    public interface IBankService
    {
        Task<List<ManageBankDetail>> GetBankDetails(string companyId);
        Task<bool> DeleteBank(string companyId, int Id);

        Task<List<BankNameCode>> GetBankNameCode();

        Task<bool> SetBankDetail(string companyId, string bankName, string bankCode, string branchName,
                                                   string bankLogoCode, string accountNo, bool? status, bool? b2b, bool? d2b,
                                                   bool? b2c, bool? b2b2b, bool? b2b2c, int id);

        Task<bool> AddBankDetail(string companyId, string bankName, string bankCode, string branchName,
                                                   string bankLogoCode, string accountNo, bool? status, bool? b2b, bool? d2b,
                                                   bool? b2c, bool? b2b2b, bool? b2b2c);
    }
}
