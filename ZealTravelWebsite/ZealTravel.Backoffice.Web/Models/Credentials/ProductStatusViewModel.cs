using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class ProductStatusViewModel
    {
        public int Id { get; set; }

        public string? SupplierId { get; set; }

        public string? SupplierCode { get; set; }
        public string? SupplierType { get; set; }

        public string? Pcc { get; set; }

        public string? Product { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool? B2b { get; set; }

        public bool? B2c { get; set; }

        public bool? Rt { get; set; }

        public bool? Int { get; set; }

        public bool? MultiCity { get; set; }

        public bool? ImportPnr { get; set; }

        public bool? Pnr { get; set; }

        public bool? Ticketting { get; set; }
    }
}
