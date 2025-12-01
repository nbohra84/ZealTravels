using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class SupplierProductDetailsViewModel
    {
        public int Id { get; set; }

        public string SupplierId { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
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

        [Required(ErrorMessage = "Required")]
        public bool? Status { get; set; }
    }
}
