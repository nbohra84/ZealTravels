using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface IGetAirSSRService
    {
        Task<string> GetAirSSR(AirAvaibilityModel parameters);
    }
}
