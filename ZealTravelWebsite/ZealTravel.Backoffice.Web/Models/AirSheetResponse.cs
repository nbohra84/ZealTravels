namespace ZealTravel.Backoffice.Web.Models
{
    public class AirSheetResponse
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<AirSheetReports> AirSheetItems { get; set; }
        public string ErrorMessage { get; set; }
        public string BillingType { get; set; }
        public string ReportType { get; set; }
    }
}
