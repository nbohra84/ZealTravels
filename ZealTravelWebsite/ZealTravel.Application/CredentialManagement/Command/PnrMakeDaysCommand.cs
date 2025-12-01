using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.CredentialManagement.Command
{
    public class PnrMakeDaysCommand
    {
        public int Id { get; set; }

        public string? CarrierCode { get; set; }

        public string? Sector { get; set; }

        public int? PnrDays { get; set; }
    }
}
