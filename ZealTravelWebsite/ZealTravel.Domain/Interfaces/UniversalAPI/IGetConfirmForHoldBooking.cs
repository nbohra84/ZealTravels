using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.UniversalAPI
{
    public interface IGetConfirmForHoldBooking
    {
        Task<bool> GetConfirmForHoldBookingResponse(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, string fltType);
        Task<bool> Get6EConfirmForHoldBookingResponse(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string UserName, string Password, string SearchID, int BookingRef, string CompanyID, string fltType);
    }
}
