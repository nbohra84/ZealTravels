using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Infrastructure.Services.Agency;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Repository.AirlineManagement
{
    public class CityRepository : RepositoryBase<AirportCity>, ICityRepository
    {
        private readonly ZealdbNContext _context;
        private readonly ILog _logger;

        public CityRepository(ZealdbNContext context) : base(context)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(AgencyService));
        }
        public async Task<List<string>> GetSectorAsync(string searchTerms)
        {
            try
            {
                var cityList = await _context.Database.SqlQuery<string>($"EXECUTE USP_ST_GetSectorDetails @Searchtext ={searchTerms}").ToListAsync();
                return cityList;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving fetch sectors: {ex.Message}");
            }

            return null;
        }
    }
}
