using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class GetProfileQuery
    {
        public string Email { get; set; }

        public GetProfileQuery(string email)
        {
            Email = email;
        }

    }
}
