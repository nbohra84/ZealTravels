using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.FlightManagement.Akasaa
{
    public interface IGetFare
    {
        public string GetFareOneWay(string Searchid, string Companyid, string RS_Availability);
        public string GetFareRT(string Searchid, string Companyid, string RS_Availability);
    }
}
