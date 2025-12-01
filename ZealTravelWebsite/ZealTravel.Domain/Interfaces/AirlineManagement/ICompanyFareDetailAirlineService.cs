using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.IRepository;

namespace ZealTravel.Domain.Interfaces.AirlineManagement
{
    public interface ICompanyFareDetailAirlineService :  IRepositoryBase<CompanyFareDetailAirline>
    {
    }
}
