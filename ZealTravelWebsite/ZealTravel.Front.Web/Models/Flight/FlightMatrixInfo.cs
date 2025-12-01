namespace ZealTravel.Front.Web.Models.Flight
{
    public class FlightMatrixInfo
    {
        public string FlightIndexRef { get; set; }
        public string Src { get; set; }
        public string Dest { get; set; }
        public string CarrierCode { get; set; }
        public int TotalAmount { get; set; }
        public string DeptTime { get; set; }
        public string TimeType { get; set; } = "N"; // Default value
        public int Stops { get; set; }
    }
}
