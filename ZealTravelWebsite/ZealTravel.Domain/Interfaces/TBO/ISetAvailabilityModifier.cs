using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface ISetAvailabilityModifier
    {
        public string errorMessage { get; set; }
        public string Supplierid { get; set; }
        public string Searchid { get; set; }
        public string Companyid { get; set; }
           
        public string Tokenid { get; set; }
        public string Traceid { get; set; }
        
        public string Cabin { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Sector { get; set; }
        public DataTable FlightModifier(string Response, bool IsCombi);
    }
}
