using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;

namespace ZealTravel.Application.UserManagement.Queries
{
    public class GetUserDetailsQuery
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string UserType { get; set; }
    }
}
