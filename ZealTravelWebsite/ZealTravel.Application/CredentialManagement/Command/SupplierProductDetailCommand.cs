using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.CredentialManagement.Command
{
    public class SupplierProductDetailCommand
    {
        public int Id { get; set; }

        public string SupplierId { get; set; } = null!;

        public string? SupplierName { get; set; }

        public string? SupplierType { get; set; }

        public string? SupplierCode { get; set; }

        public string? FareType { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CityName { get; set; }

        public bool? Flight { get; set; }

        public bool? Hotel { get; set; }

        public string? Remarks { get; set; }

        public bool? Status { get; set; }
    }
}
