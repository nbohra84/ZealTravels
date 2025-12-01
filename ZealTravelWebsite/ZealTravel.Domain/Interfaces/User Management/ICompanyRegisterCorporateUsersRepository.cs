using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Domain.Interfaces.IRepository
{
    public interface ICompanyRegisterCorporateUsersRepository : IRepositoryBase<CompanyRegisterCorporateUser>
    {
    }
}
