using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;

namespace ZealTravel.Domain.Interfaces.AirlineSupplierManagement
{
    public interface IAirlineSupplierService
    {
        Task<List<AirlineSuppliers>> GetAirSupplierListAsync();
    }
}
