using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.Loggers
{
    public interface ILogger
    {
        public bool dbSearchLogg(string CompanyID, Int32 BookingRef, string StaffID, string SearchID, string Location, string Status, string Place, string Remark, string Remark2, string Host);
        public bool dbLogg(string CompanyID, Int32 BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage);
        public bool dbLoggHotelAPI(string SearchID, string CompanyID, Int32 BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest);
        public bool dbLoggAPI(string SearchID, string CompanyID, Int32 BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest);
    }
}
