using System.ComponentModel.DataAnnotations;
using ZealTravel.Common.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models
{
    public class Registration
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Letters only, if required use only space")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Letters only, if required use only space.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special character not allowed, if required use only space.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Special character not allowed, if required use only space.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Not a Valid integer")]
        public int PostalCode { get; set; }


        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string Company_Mobile { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string Company_PhoneNo { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\d{2}[A-Za-z]{5}\d{4}[A-Za-z]{1}\d[Zz]{1}[A-Za-z\d]{1}$", ErrorMessage = "Please enter a valid GST number.")]
        public string GST { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$", ErrorMessage = "Please enter valid Pan Number.")]
        public string PanNo { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "No Special character allowed, if required use only space.")]
        public string Pan_Name { get; set; }

        [BooleanRequired(ErrorMessage = "This checkbox is required")]
        public bool AgreeToTerms { get; set; }
        public string? UserType { get; set; }

    }
}
