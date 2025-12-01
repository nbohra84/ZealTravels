using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.FlightManagement.Akasaa
{
    public interface IGetApiFlightFareRule
    {
        public string GetFareRule(DataTable dtBound, string Searchid, string CompanyID, Int32 BookingRef);
        public string GetFareRule(DataTable dtBound, string _Token);
    }
}
