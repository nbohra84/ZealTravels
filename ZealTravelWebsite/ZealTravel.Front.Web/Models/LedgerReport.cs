namespace ZealTravel.Front.Web.Models
{
    public class LedgerReport
    {
        public string? CompanyID { get; set; }
        public string? PNR { get; set; } 
        public string? TicketNumber { get; set; } 
        public string? PassengerName { get; set; } 
        public string? BookingReference { get; set; } 
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FilterFromDate { get; set; }
        public DateTime FilterToDate { get; set; }
        public string? EventID { get; set; }
    }
}
