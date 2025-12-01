using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class GalileoSupplierDetailViewModel
    {
        public int Id { get; set; }

        public string? Hap { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Userid { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Password { get; set; }

        public string? SoapUrl { get; set; }

        public string? TargetBranch { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Pcc { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ImportQueue { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? TktdQueue { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool? TicketIfFareGaurantee { get; set; }

        public bool? Status { get; set; }
    }
}
