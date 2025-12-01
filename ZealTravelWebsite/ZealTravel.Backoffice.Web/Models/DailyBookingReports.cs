namespace ZealTravel.Backoffice.Web.Models
{
    public class DailyBookingReports
    {
        public string CompanyID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? CompanyNameWithAccountId{ get; set; }
    }
}
