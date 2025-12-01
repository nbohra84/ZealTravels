using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;

namespace ZealTravel.Domain.Interfaces.UniversalAPI.Spicejet
{
    public interface IGetConfirmForHoldBooking
    {
        Task<bool> GetConfirmForHoldBookingResponse(string Supplierid, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, Decimal TotalApiFare, string RecordLocator, int Paxcount,string FltType);
    }
}
