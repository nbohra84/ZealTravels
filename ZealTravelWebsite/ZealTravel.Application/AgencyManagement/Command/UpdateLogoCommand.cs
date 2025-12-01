using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Command
{
    public class UpdateLogoCommand
    {
        public string CompanyID { get; set; }
        public string? CompanyLogo { get; set; }
    }
}
