using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Backoffice;
using ZealTravel.Domain.Data.Models.Country;
using ZealTravel.Domain.Interfaces.CountryManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Country
{
    public class CountryService : ICountryService
    {
        private readonly ZealdbNContext _context;

        public CountryService(ZealdbNContext context)
        {
            _context = context;

        }

        public async Task<List<CountryList>> GetCountryListWithCode()
        {
            try
            {
                var countryList = await _context.Database.SqlQuery<CountryList>($"EXECUTE CarrierDetail_Proc @ProcNo = 3").ToListAsync();
                return countryList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving country list: {ex.Message}", ex);
            }
            
        }

    }
}
