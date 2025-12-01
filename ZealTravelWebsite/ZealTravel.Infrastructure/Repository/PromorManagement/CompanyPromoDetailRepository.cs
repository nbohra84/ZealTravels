using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.PromoManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Repository.PromorManagement
{
    public class CompanyPromoDetailRepository : RepositoryBase<CompanyPromoDetail>, ICompanyPromoDetailRepository
    {
        private readonly ZealdbNContext _context;
        public CompanyPromoDetailRepository(ZealdbNContext context) : base(context)
        {
        }
    }
}   

