namespace ZealTravel.Backoffice.Web.Models
{
    public class AirSheet
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? BillingType { get; set; }
        public string CompanyID { get; set; }
        public string ReportType { get; set; }
    }
}
