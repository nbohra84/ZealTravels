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
    public class CompanyRegisterCorporateUserRepository : RepositoryBase<CompanyRegisterCorporateUser>, ICompanyRegisterCorporateUsersRepository
    {
        private readonly ZealdbNContext _context;

        public CompanyRegisterCorporateUserRepository(ZealdbNContext context) : base(context)
        {
            
        }
    }
}   
