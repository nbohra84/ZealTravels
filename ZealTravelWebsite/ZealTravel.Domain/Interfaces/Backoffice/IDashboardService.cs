using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Backoffice;

namespace ZealTravel.Domain.Interfaces.Backoffice
{
    public interface IDashboardService
    {
        Task<List<DashboardAirlinePendingBooking>> GetAirlinePendingBookings();
        Task<List<DashboardAirlineFlightBooking>> GetTopTenAirlineBookings(string companyID);
    }
}
