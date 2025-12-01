using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Repository
{
    public class CompanyFlightDetailAirlinesService : RepositoryBase<CompanyFlightDetailAirline>, ICompanyFlightDetailAirlinesService
    {
        private readonly ZealdbNContext _context;
        public CompanyFlightDetailAirlinesService(ZealdbNContext context) : base(context) 
        {
            
        }
    }
}
