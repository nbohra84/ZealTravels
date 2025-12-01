using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.UserManagement.Queries
{
    public class GstStates
    {
        public int Id { get; set; }

        public string? State { get; set; }

        public string? GstCode { get; set; }

        public string? StateCode { get; set; }

        public bool? Ut { get; set; }
    }
}
