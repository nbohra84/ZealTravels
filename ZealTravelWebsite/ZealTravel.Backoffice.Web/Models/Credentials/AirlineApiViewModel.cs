using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class AirlineApiViewModel
    {
        public int Id { get; set; }

        public string? SupplierId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Password { get; set; }
    }
}
