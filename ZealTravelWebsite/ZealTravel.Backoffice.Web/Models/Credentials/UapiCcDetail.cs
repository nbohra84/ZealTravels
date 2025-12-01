using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class UapiCcDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? BankCountryCode { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? BankName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Cvv { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? ExpDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Number { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? AddressName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Carriers { get; set; }
    }
}
