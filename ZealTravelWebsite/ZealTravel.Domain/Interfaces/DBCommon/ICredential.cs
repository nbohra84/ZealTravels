using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.DBCommon
{
    public interface ICredential
    {
        Task<List<T>> AirlineCredentialDetail<T>(string supplierType, string supplierid) where T : class;
        List<SupplierDetails> GetSuppliers();
        List<SupplierApiAirline> InactiveCarriersAPI(string SupplierID);
    }
}
