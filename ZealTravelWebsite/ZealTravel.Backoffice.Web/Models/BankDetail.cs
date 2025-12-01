using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Channels;
using ZealTravel.Application.BankManagement.Query;

namespace ZealTravel.Backoffice.Web.Models
{
    public class BankDetail
    {

        public int Id { get; set; }
        //public int ID { get; set; }
        public string? CompanyId { get; set; }

        public string? BankLogoCode { get; set; }

        [Required(ErrorMessage = "Bank Code is required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "* Special Characters not allowed")]
        public string? BankCode { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        public string? BankName { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        [MinLength(1, ErrorMessage = "At least one Bank Name must be provided.")]
        public List<GetBankNameQuery>? BankNames { get; set; }
        [Required(ErrorMessage = "Branch Name is required")]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        public string AccountNo { get; set; } = null!;

        public bool Status { get; set; } = false; // Default to false
        public bool B2b { get; set; } = false;  // Default to false
        public bool D2b { get; set; } = false;  // Default to false
        public bool B2c { get; set; } = false;  // Default to false
        public bool B2b2b { get; set; } = false; // Default to false
        public bool B2b2c { get; set; } = false;

        public string BankLogo { get; set; } = null!;
        public string? GSTNumber { get; set; }
        public string CompanyName { get; set; }
        public string? PanNo { get; set; }

    }
}
