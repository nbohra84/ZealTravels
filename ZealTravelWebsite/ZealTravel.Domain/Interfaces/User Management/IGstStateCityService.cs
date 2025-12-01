using System.Data;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Domain.Interfaces.Services
{
    public interface IGstStateCityService
    {
        Task<List<GstState>> GetStatesAsync();
        Task<List<GstStateCity>> GetCityByStateNameAsync(string stateName);
        Task<List<CityState>> GetCityByStateAsync(string stateName = null);
    }
}
