using DocumentFormat.OpenXml.Drawing;
using ZealTravel.Application.BackofficeManagement.Queries.Dashboard;

namespace ZealTravel.Backoffice.Web.Models.Dashboard
{
    public class DashboardResponse
    {
        public List<DashboardAirlinePendingBookingData> DashboardAirlinePendingBookings { get; set; }
        public List<DashboardAirlineTopTenBookingData> DashboardAirlinetopTenBookings { get; set; }

    }
}
