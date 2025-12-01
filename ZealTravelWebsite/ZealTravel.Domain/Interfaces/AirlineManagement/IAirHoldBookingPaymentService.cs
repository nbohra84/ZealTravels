using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IAirHoldBookingPaymentService
    {
        Task<string> SetHoldBookingBookingPaymentAsync(string BookingRef, string Remarks,string PaymentType, string SearchId);
    }
}
