using System.ComponentModel.DataAnnotations;
using ZealTravel.Common.DataAnnotations;
namespace ZealTravel.Backoffice.Web.Models
{
    public class StaffRegister
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Letters only, if required use only space")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Letters only, if required use only space.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string Company_Mobile { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string Company_PhoneNo { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special character not allowed, if required use only space.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Not a Valid integer")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; }

        public string? StaffType { get; set; }
        [BooleanRequired(ErrorMessage = "This checkbox is required")]
        public bool AgreeToTerms { get; set; }
    }
}
