using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Front.Web.Models
{
    public class ProfileDetail
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyMobile { get; set; }
        public string CompanyPhone { get; set; }
        public decimal AvailableBalance { get; set; }
        public string PanCardNo { get; set; }
        public string GstNo { get; set; }
        public string GstName { get; set; }

        [Required(ErrorMessage = "Logo URL is required.")]
        public string CompanyLogo { get; set; }
        public IFormFile LogoFile { get; set; }
    }
}
