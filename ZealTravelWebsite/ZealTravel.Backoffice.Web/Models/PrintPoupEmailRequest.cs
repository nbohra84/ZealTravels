namespace ZealTravel.Backoffice.Web.Models
{
    public class PrintPoupEmailRequest
    {
        public string BookingRef { get; set; }
        public string Email { get; set; }
        public string Tax { get; set; }
        public string ShowLogo { get; set; }
        public string HideFare { get; set; }
        public string Disc { get; set; }
        public string Pax_SegmentId { get; set; }
    }
}
