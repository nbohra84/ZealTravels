using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.DBCommon
{
    public interface IDBLoggerService
    {
        Task<bool> dbSearchLogg(string CompanyID, Int32 BookingRef, string StaffID, string SearchID, string Location, string Status, string Place, string Remark, string Remark2, string Host);
        Task<bool> dbLogg(string CompanyID, Int32 BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage);
    }
}
