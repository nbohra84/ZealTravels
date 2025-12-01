using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.AirlineManagement.Akasaa
{
    public interface IGetApiAvailabilityFare
    {
        bool GetRTFares(DataTable dtBound, bool IsfareMethod, string Searchid, string CompanyID);
        bool GetOneWayFares(string SearchID, DataTable dtBound, bool IsfareMethod);
    }
}
