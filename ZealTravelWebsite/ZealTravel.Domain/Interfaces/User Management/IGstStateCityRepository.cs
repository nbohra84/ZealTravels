using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.User_Management
{
    public interface IGstStateCityRepository
    {
        Task<DataTable> GetStatesAsync();
        Task<DataTable> GetCityByStateNameAsync(string stateName);
    }
}
