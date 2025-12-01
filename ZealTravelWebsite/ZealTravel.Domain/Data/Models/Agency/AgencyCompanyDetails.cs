using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Domain.Data.Models.Agency
{
    public class AgencyCompanyDetails
    {
        public List<CompanyDetails> CompanyDetails { get; set; }
        public List<CompanyRegisterProduct> CompanyRegisterProduct { get; set; }
    }
}
