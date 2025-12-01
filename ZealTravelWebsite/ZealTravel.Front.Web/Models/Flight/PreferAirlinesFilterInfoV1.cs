namespace ZealTravel.Front.Web.Models.Flight
{
    public class PreferAirlinesFilterInfoV1
    {
        public string RowId { get; set; }
        public string RefId { get; set; }
        public string Logo { get; set; }
        public string Provider { get; set; }
        public string ProviderType { get; set; }
        public string DepDateDesc { get; set; }
        public string ArrDateDesc { get; set; }
        public string Description { get; set; }
        public int TotalFare { get; set; }
        public int TotalTds { get; set; }
        public int TotalTdsSA { get; set; }
        public string FlightCode { get; set; }
    }
}
