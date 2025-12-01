using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.DBCommon
{
    public interface IAirlineDetailService
    {
        Task<bool> IsDomestic(string Origin, string Destination);
        Task<bool> GetDateValidationDays(string CarrierCode, string Sector, string DepartureDate);
    }
}
