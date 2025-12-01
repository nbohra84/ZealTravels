using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class SupplierDetails
    {
        public string SupplierID { get; set; }
        public string Fare_Type { get; set; }
        public string Supplier_Type { get; set; }
        public string Supplier_Code { get; set; }
        public string CarrierCode { get; set; }
    }
}
