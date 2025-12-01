namespace ZealTravel.Front.Web.Models
{
    public class HoldPaymentRequest
    {
        public string BookingRef { get; set; }
        public string PaymentType { get; set; }
        public string? PaxSegmentID { get; set; }
        public string Remarks { get; set; }
        public string? Sector { get; set; }
        public string? PaxSegmentID2 { get; set; }

    }
}
