namespace ZealTravel.Domain.Data.Models
{
    public class CancelFlightBookingReport
    {
        public int BookingRef { get; set; }
        public string flttype { get; set; }
        public string PNR { get; set; }
        public DateTime EventTime { get; set; }
        public string CompanyName { get; set; }
        public string Supplier { get; set; }
        
       
    }
}
