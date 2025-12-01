using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class GeneralInfo
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public GeneralInfo()
        {
            this.AdditionalMarkup = 0.0;
            this.AgentId = this.BranchId = this.TravelerId = this.DecPrefrences = 0;
            this.Currency = this.Agent_Curr = this.Gross_Curr;
            this.Channel = this.JourneyType = "";
            this.OnBehalfBooking = this.IsCSBTUser = this.IsApiUser = this.ImportPNR = false;
            this.IsTaxCalculateInAPI = true;
            this.AirFlowOpt = "OTA";
            this.LanguageCode = "en";
            this.SupplierId = 0;
            this.CombinationType = "IN";
            this.IsRW = false;
        }

        public string GUID { get; set; }

        public string Currency { get; set; }

        public string Agent_Curr { get; set; }

        public string Gross_Curr { get; set; }

        public string Cabin { get; set; }

        public string Nationality { get; set; }

        public string TripType { get; set; }

        public string CompanyId { get; set; }

        public string Channel { get; set; }

        public string AirFlowOpt { get; set; }

        public bool IsApiUser { get; set; }

        public string LanguageCode { get; set; }

        public string Suppliers { get; set; }

        public int SupplierId { get; set; }

        public int DecPrefrences { get; set; }

        public string CombinationType { get; set; }

        public string ClientIP { get; set; }

        public bool ShouldSerializeClientIP() => this.ShowPropertyWhileSerialize;

        public string JourneyType { get; set; }

        public bool ShouldSerializeJourneyType() => this.ShowPropertyWhileSerialize;

        public int AgentId { get; set; }

        public bool ShouldSerializeAgentId() => this.ShowPropertyWhileSerialize;

        public int BranchId { get; set; }

        public bool ShouldSerializeBranchId() => this.ShowPropertyWhileSerialize;

        public int TravelerId { get; set; }

        public bool ShouldSerializeTravelerId() => this.ShowPropertyWhileSerialize;

        public int ProcessedBy { get; set; }

        public bool ShouldSerializeProcessedBy() => this.ShowPropertyWhileSerialize;

        public double AdditionalMarkup { get; set; }

        public bool ShouldSerializeAdditionalMarkup() => this.ShowPropertyWhileSerialize;

        public bool OnBehalfBooking { get; set; }

        public bool ShouldSerializeOnBehalfBooking() => this.ShowPropertyWhileSerialize;

        public bool IsCSBTUser { get; set; }

        public bool ShouldSerializeIsCSBTUser() => this.ShowPropertyWhileSerialize;

        public bool ImportPNR { get; set; }

        public bool ShouldSerializeImportPNR() => this.ShowPropertyWhileSerialize;

        public bool IsTaxCalculateInAPI { get; set; }

        public bool ShouldSerializeIsTaxCalculateInAPI() => this.ShowPropertyWhileSerialize;

        public string PCC { get; set; }

        public bool ShouldSerializePCC() => this.ShowPropertyWhileSerialize;

        public string POS { get; set; }

        public bool ShouldSerializePOS() => this.ShowPropertyWhileSerialize;

        public string GDSPnr { get; set; }

        public bool ShouldSerializeGDSPnr() => this.ShowPropertyWhileSerialize;

        public SubAgent SubAgent { get; set; }

        public bool ShouldSerializeSubAgent() => this.ShowPropertyWhileSerialize;

        public GeneralRequest GeneralRequest { get; set; }

        public bool ShouldSerializeGeneralRequest() => this.ShowPropertyWhileSerialize;

        public bool IsRW { get; set; }
    }
}