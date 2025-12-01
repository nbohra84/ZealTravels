namespace ZealTravel.Front.Web.Models.Agency.Dashboard
{
    public class DashboardLedgerResponse
    {
        public int PaxSegmentID { get; set; }
        public int BookingRef { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string PaymentType { get; set; }
        public DateTime EventTime { get; set; }
        public string Remark { get; set; }
        public string Airline_PNR_D { get; set; }
        public string Airline_PNR_A { get; set; }
        public string? BookingStatus { get; set; }
        public string? CompanyName { get; set; }
        public string? Trip { get; set; }

    }
}
