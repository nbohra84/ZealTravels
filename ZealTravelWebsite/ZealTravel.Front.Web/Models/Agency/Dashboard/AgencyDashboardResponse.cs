namespace ZealTravel.Front.Web.Models.Agency.Dashboard
{
    public class AgencyDashboardResponse
    {
        public List<AgencyRepresentative> Agency { get; set; }
        public decimal AvailableBalance { get; set; }
        public List<DashboardLedgerResponse> LedgerStatement { get; set; }
        public List<DashboardNotificationResponse> Notification { get; set; }
        public string DashboardChartXML { get; set; }
        public List<DashboardCorporateResponse> DashboardCorporate { get; set; }



    }
}
