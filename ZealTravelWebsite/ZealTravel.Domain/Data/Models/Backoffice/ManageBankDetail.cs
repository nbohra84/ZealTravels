using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Backoffice
{
    public class ManageBankDetail
    {
        public int Id { get; set; }
        public string? CompanyId { get; set; }

        [Column("Bank_Logo_Code")]
        public string? BankLogoCode { get; set; }

        //public string? BankCode { get; set; }
        [Column("Bank_Name")]
        public string? BankName { get; set; }
        [Column("Bank_Code")]
        public string? BankCode { get; set; }

        [Column("Branch_Name")]
        public string? BranchName { get; set; }

        public string AccountNo { get; set; } = null!;

        public bool? Status { get; set; }

        public bool? B2b { get; set; }

        public bool? D2b { get; set; }

        public bool? B2c { get; set; }

        public bool? B2b2b { get; set; }

        public bool? B2b2c { get; set; }

        //public string BankLogo { get; set; } = null!;
        [Column("Gst_Number")]
        public string GSTNumber { get; set; }
        public string CompanyName { get; set; }
        [Column("Pan_No")]
        public string PanNo { get; set; }
    }
}
