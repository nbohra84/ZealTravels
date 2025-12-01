namespace ZealTravel.Front.Web.Models.Flight
{
    public class FlightPaymentInfoResponse
    {
        public string CurrencyType { get; set; }
        public string AccountNo { get; set; }
        public string CompanyId { get; set; }
        public string HostName { get; set; }
        public string FlightSelectedXml { get; set; }
        public string FlightSelectedXmlInbond { get; set; }
        public string AdultCount { get; set; }
        public string ChildCount { get; set; }
        public string InfantCount { get; set; }
        public string TotalMarkup { get; set; }
        public string TotalTDS { get; set; }
        public string SearchType { get; set; }
        public string RegisterEmail { get; set; }
        public string SSRAvailabilityOut { get; set; }
        public string SSRAvailabilityIn { get; set; }
        public string PaymentCharge { get; set; }
        public string CardName { get; set; }
        public string PaymentMethod { get; set; }
        public string ShowWallet { get; set; }
        public string IntlQueryString { get; set; }
        public bool IsPayDivAppend { get; set; }
        public string Nationality { get; set; }
        public string Sector { get; set; }
        public string RoundTrip { get; set; }
        public string LivePathImage { get; set; }
        public string TotalCfee { get; set; }
        public string ValidCfee { get; set; }
        public string TotalFare { get; set; }
        public string Discount { get; set; }
        public string PaxXmlResult { get; set; }
        public string TravelDateChildInfant { get; set; }

        public string OutbountItinary { get; set; }
        public string Faredetaildiv { get; set; }

        public string Pageredirect { get; set; }

        public string PaxInformation { get; set; }

        public string PaymentTabMenu { get; set; }
        public string PaymentTabContent { get; set; }
    }
}
