namespace ZealTravel.Backoffice.Web.Models
{
    public class DailyBookingReportsListResponse
    {
        public string CompanyID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DailyBookingReportsResponse> BookingReports { get; set; }
        public string ErrorMessage { get; set; }
        public string SearchText { get; set; }
        public string? CompanyNameWithAccountId { get; set; }
    }
}
