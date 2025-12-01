using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Repository
{
    public class CompanyFareDetailAirlineService : RepositoryBase<CompanyFareDetailAirline>, ICompanyFareDetailAirlineService
    {
        private readonly ZealdbNContext _context;
        public CompanyFareDetailAirlineService(ZealdbNContext context) : base(context)
        {
            
        }
    }
}
