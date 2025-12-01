using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.PNRManagement
{
    public interface IPNRService
    {
        Task<int> GetPNRstatusByPriceTypeFare(string CompanyID, string Supplierid, string CarrierCode, string Sector, string PriceType);
        Task<bool> GetPNRstatus(string CompanyID, string CarrierCode, string Sector);
    }
}
