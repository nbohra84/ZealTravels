using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models
{
    public class CityState
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public List<string> Cities { get; set; }

        public CityState()
        {
            Cities = new List<string>();
        }
    }
}
