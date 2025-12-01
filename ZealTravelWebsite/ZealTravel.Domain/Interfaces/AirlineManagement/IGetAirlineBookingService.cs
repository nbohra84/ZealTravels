using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IGetAirlineBookingService
    {
        Task<string> GetAirlineBookingData(string universalRecordLocatorCode, string supplier, string searchID);
    }
}
