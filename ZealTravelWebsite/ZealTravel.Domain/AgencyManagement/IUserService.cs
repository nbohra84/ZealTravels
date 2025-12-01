using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models;

namespace ZealTravel.Domain.AgencyManagement
{
    public interface IUserService
    {
        Task<string> GetCompanyIDByAccountID(int accountId);
    }
}
