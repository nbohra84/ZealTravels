using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.UserManagement.Queries
{
    public class GstCityByState
    {
        public int Id { get; set; }

        public string? StateCode { get; set; }

        public string? City { get; set; }
    }
}
