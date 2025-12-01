using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.BankManagement.Command
{
    public class BankDetailCommand
    {
        public int Id { get; set; }
        public string? CompanyId { get; set; }

        public string? BankLogoCode { get; set; }

        public string? BankCode { get; set; }

        public string? BankName { get; set; }

        public string? BranchName { get; set; }

        public string AccountNo { get; set; } = null!;

        public bool? Status { get; set; }

        public bool? B2b { get; set; }

        public bool? D2b { get; set; }

        public bool? B2c { get; set; }

        public bool? B2b2b { get; set; }

        public bool? B2b2c { get; set; }

        public string BankLogo { get; set; } = null!;
        public string GSTNumber { get; set; }
        public string CompanyName { get; set; }
        public string PanNo { get; set; }
    }
}
