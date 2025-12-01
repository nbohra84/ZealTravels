using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Front.Web.Models
{
    public class ForgotPassword
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
