using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface ITBOService
    {
        //public string GetAirAvailability(AirAvaibilityModel model);
        public Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model);
    }
}
        