using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.AirCalculation
{
    public interface IRR_Layer
    {
        public string GetAvailabilityCal(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, string AvailabilityResponse);
        public DataTable GetAvailabilityCal(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, DataTable dtBound);
    }
}
