namespace ZealTravel.Front.Web.Models.Flight
{
    public class FlightTravellersInfoResponse
    {
        public string CurrencyType { get; set; }
        public string AccountNo { get; set; }
        public string CompanyId { get; set; }
        public string HostName { get; set; }
        public string FlightSelectedXml { get; set; }
        public string FlightSelectedXmlInbond { get; set; }
        public string Adult { get; set; }
        public string Child { get; set; }
        public string Infant { get; set; }
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
        public string TravelDateChdInf { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Fare Details
        public decimal? GrossFare { get; set; } 

        // GST Details
        public bool HasGST { get; set; }
        public string GSTRegisteredCompany { get; set; }
        public string GSTNumber { get; set; }
        public string GSTCompanyContactNo { get; set; }
        public string GSTCompanyEmail { get; set; }
        public string GSTCompanyAddress { get; set; }

        // Agreement and Actions
        public bool TermsAndConditionsAccepted { get; set; }
        public string OutbountItinary { get; set; }
        public string Faredetaildiv { get; set; }

        public string Pageredirect { get; set; }

    }
}
