using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.CredentialManagement.Command
{
    public class UpdateAirlineApiCommand
    {
        public int Id { get; set; }

        public string? SupplierId { get; set; }

        public string? UserId { get; set; }

        public string? Password { get; set; }
    }
}
