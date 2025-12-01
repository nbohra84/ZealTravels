using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Front.Web.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "This field is required")]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Compare("NewPassword", ErrorMessage = "New Password and Retype New Password must match.")]
        public string RetypeNewPassword { get; set; }
    }
}
