using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.CallCenterManagement
{
    public interface ICallCenterService
    {
        Task<bool> FallInToAirlineCallCenter(Int32 BookingRef, string CompanyID);
    }
}
