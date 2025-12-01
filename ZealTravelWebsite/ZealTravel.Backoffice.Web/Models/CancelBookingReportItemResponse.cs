namespace ZealTravel.Backoffice.Web.Models
{
    public class CancelBookingReportItemResponse
    {
        public string CarrierCode { get; set; }     
        public string CarrierName { get; set; }
        public decimal TotalBasic { get; set; }
        public decimal TotalYQ { get; set; }
        public decimal TotalFare { get; set; }
        public decimal TotalCommission { get; set; }
        public int NoOfPassengers { get; set; }
        public int NoOfBookings { get; set; }
    }
}
