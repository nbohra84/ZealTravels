using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IGetAirFareRulesService
    {
        Task<string> GetAirFareRules(AirAvaibilityModel parameters);
    }
}
