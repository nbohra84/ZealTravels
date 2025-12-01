using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IBookingService
    {
        Task<Int32> SetBooking(string SearchID, string CompanyID, string UpdatedBy, bool IsCombi, bool IsRTfare, bool IsQueue, bool IsOffline, string PaymentType, string PaymentID, string PassengerResponse, string AvailabilityResponse, string RefID_O, string RefID_I, bool IsUserGateway, string GstInfo);
    }
}
