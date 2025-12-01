using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface IGetServices
    {
        public string GetFlights(string JourneyType, string Supplierid, string Password, string Searchid, string Companyid, string AirRQ, string EndUserIp);
        public Task<string> GetFlightsAsync(string JourneyType, string Supplierid, string Password, string Searchid, string Companyid, string AirRQ, string EndUserIp);
    }
}
