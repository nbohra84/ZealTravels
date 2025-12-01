using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.CredentialManagement.Query
{
    public class PnrMakeDaysDetails
    {
        public int Id { get; set; }

        public string? CarrierCode { get; set; }

        public string? Sector { get; set; }

        public int? PnrDays { get; set; }
    }
}
