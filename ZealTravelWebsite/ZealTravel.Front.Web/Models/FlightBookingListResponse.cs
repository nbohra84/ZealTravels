using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Front.Web.Models
{
    public class FlightBookingListResponse
    {
        public string? CompanyID { get; set; }
        public string? PNR { get; set; }
        public string? TicketNumber { get; set; }
        public string? PassengerName { get; set; }
        public string? BookingReference { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public DateTime FilterFromDate { get; set; }
        [Required]
        public DateTime FilterToDate { get; set; }
        public string? TicketSearchType { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ReportTableHTML { get; set; }
        public string? ReportTableCompanyPopup { get; set; }
        public string? SalesTable { get; set; }
        public string? Hdntktdetails { get; set; }
        public string? HdnhostName { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public bool? ShowLogo { get; set; } = true;
        public bool? HideFare { get; set; } = false;
        public bool? IsSingleFare { get; set; } = false;
    }
}
