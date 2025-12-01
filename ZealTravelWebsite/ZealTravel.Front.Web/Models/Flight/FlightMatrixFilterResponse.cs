namespace ZealTravel.Front.Web.Models.Flight
{
    public class FlightMatrixFilterResponse
    {
        public string Stops { get; set; }
        public int NoOfStops { get; set; }
        public string Airlines { get; set; }
        public string Airlinespath { get; set; }
        public string Fare { get; set; }
        public string type { get; set; }

        public string FareM { get; set; }
        public string FareA { get; set; }
        public string FareN { get; set; }
        public string FareMN { get; set; }
        public string FlightRefid { get; set; }

        public string FlightRefidM { get; set; }
        public string FlightRefidA { get; set; }
        public string FlightRefidN { get; set; }
        public string FlightRefidMN { get; set; }
    }
}
