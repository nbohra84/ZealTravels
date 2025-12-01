using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class CompanySetting
    {
        public CompanySetting()
        {
            this.AirFlowOptionAllowed = "BOTH";
            this.CompanyCountryCode = "";
            this.MultiCityAllowed = false;
            this.FlexiSearchAllowed = false;
            this.PageMarkUpEnable = false;
            this.CompanyCountryCode = this.Name = this.PhoneNo = "";
            this.IPAddress = "";
            this.BrowserAgent = "";
            this.CountryName = this.Email = "";
        }

        public bool MultiCityAllowed { get; set; }

        public bool FlexiSearchAllowed { get; set; }

        public bool PageMarkUpEnable { get; set; }

        public string AirFlowOptionAllowed { get; set; }

        public string CompanyCountryCode { get; set; }

        public string IPAddress { get; set; }

        public string BrowserAgent { get; set; }

        public string CountryName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string Name { get; set; }
    }
}