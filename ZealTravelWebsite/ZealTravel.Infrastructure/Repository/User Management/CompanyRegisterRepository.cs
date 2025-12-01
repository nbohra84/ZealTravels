using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Infrastructure.Repository
{
    public class CompanyRegisterRepository : RepositoryBase<CompanyRegister>, ICompanyRegisterRepository
    {
        private readonly ZealdbNContext _context;

        public CompanyRegisterRepository(ZealdbNContext context) : base(context)
        {
            
        }
    }
}   
