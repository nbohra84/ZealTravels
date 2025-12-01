using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models.Country;

namespace ZealTravel.Domain.Interfaces.CountryManagement
{
    public interface ICountryService
    {
        Task<List<CountryList>> GetCountryListWithCode(); 
    }
}
