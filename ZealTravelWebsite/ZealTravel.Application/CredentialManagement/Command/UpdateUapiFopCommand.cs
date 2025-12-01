using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.CredentialManagement.Command
{
    public class UpdateUapiFopCommand
    {
        public int Id { get; set; }

        public string? Fop { get; set; }

        public bool? Status { get; set; }
    }
}
