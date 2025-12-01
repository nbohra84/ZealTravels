using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password  is required")]
        public string Password { get; set; }
        public string? UserType { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
