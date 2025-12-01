namespace ZealTravel.Front.Web.Models.Agency.Dashboard
{
    public class DashboardChartResponse
    {
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public int NoOfBookings { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal TotalYQ { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalCommission { get; set; }
        public int NoOfPassenger { get; set; }
        public int BookingRef { get; set; }
    }
}
