namespace ZealTravel.Backoffice.Web.Models
{
    public class SearchAgencyData
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int AccountID { get; set; }
        public string City { get; set; }
        public string UserType { get; set; }
        public bool Active_Status { get; set; }
        public DateTime EventTime { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
