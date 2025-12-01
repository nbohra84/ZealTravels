using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IAirCommitService
    {
        Task<bool> GetConfirm(AirAvaibilityModel airAvaibility, Int32 BookingRef,  string PassengerRS, string GstRS,string PaymentType);
    }
}
