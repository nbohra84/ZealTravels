using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class PnrMakeDaysDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? CarrierCode { get; set; }
        public string? Sector { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? PnrDays { get; set; }
    }
}
