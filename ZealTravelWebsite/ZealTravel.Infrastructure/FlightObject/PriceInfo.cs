using System.Xml.Serialization;
using ZealTravel.Infrastructure.Akaasa;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class PriceInfo
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set => this.propertyValue = value;
        }

        public PriceInfo()
        {
            this.SupplierCurrency = "";
            this.BaseAmt = this.Taxes = this.TotalGovTaxes = this.CompServiceFee = 0.0;
            Decimal num = 0M;
            this.SupplierTaxes = num;
            this.SupplierBaseAmt = num;
            this.PriceCount = (byte)0;
            this.ValidatingCarrier = this.Origin = this.Destination = this.FareType = "";
            this.FareQuoteStatus = false;
            this.GovernmentTaxes = (GovernmentTaxes)null;
        }

        public string AirPricingSolutionKey { get; set; }

        public string AirPricingInfoKey { get; set; }

        public double BaseAmt { get; set; }

        public Decimal SupplierBaseAmt { get; set; }

        public double Taxes { get; set; }

        public Decimal SupplierTaxes { get; set; }

        public TaxBreakup TaxBreakup { get; set; }

        public TaxBreakup SupplierTaxBreakup { get; set; }

        public string SupplierCurrency { get; set; }

        public Decimal ROE { get; set; }

        public double TotalMU { get; set; }

        public byte PriceCount { get; set; }

        public double TotalGovTaxes { get; set; }

        public double CompServiceFee { get; set; }

        public string PaxType { get; set; }

        public string PaxSubType { get; set; }

        public string ValidatingCarrier { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public bool FareQuoteStatus { get; set; }

        public int LoyaltyPoints { get; set; } = 0;

        public FareDetail FareDetail { get; set; }

        public PaxFareDetails PaxFareDetails { get; set; }

        public SegmentDetail SegmentDetail { get; set; }

        public PriceDetail PriceDetail { get; set; }

        public string FareType { get; set; }

        public string SegmentRute { get; set; }

        public FareRuleComm FareRuleComm { get; set; }

        public GovernmentTaxes GovernmentTaxes { get; set; }

        public bool ShouldSerializeGovernmentTaxes()
        {
            return this.GovernmentTaxes != null && this.ShowPropertyWhileSerialize;
        }
    }
}