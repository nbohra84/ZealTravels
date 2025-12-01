using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZealTravel.Domain.Interfaces.AirCalculation;

namespace ZealTravel.Infrastructure.AirCalculations
{
    public class rr_Layer: IRR_Layer
    {
        public string GetAvailabilityCal(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, string AvailabilityResponse)
        {
            Calculation Call = new Calculation();
            return Call.Calculate_Fare_Commission(IsOnline, IsDeal, SearchID, CompanyID, AvailabilityResponse);
        }
        public DataTable GetAvailabilityCal(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, DataTable dtBound)
        {
            Calculation Call = new Calculation();
            return Call.Calculate_Fare_Commission(IsOnline, IsDeal, SearchID, CompanyID, dtBound);
        }
    }
}
