using Microsoft.AspNetCore.Mvc.Rendering;
using ZealTravel.Application.ReportManagement.Queries;

namespace ZealTravel.Backoffice.Web.Models
{
    public class CancelFlightBookingReportResponse
    {
        public string? SupplierID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<CancelAirBooking> BookingItems { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        public string ErrorMessage { get; set; }
    }
}
