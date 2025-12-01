using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZealTravel.Infrastructure.Akaasa;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Flight
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set
            {
                this.propertyValue = value;
                if (this.Details != null)
                    this.Details.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.BaggageInformation != null)
                    this.BaggageInformation.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.FaresRulesReply != null)
                    this.FaresRulesReply.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.AdditionalServices != null)
                    this.AdditionalServices.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.SSRDetails != null)
                    this.SSRDetails.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.Charges != null)
                    this.Charges.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.FCanxCharge != null)
                    this.FCanxCharge.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.PriceInfo == null)
                    return;
                foreach (PriceInfo priceInfo in this.PriceInfo)
                    priceInfo.ShowPropertyWhileSerialize = this.propertyValue;
            }
        }

        public Flight()
        {
            this.Idx = new short?((short)0);
            this.TAmt = this.TTax = this.SearchMU = this.SearchMUAmt = this.DummyMarkup = 0.0;
            this.Comp_Curr = this.Agent_Curr = this.JourneyType = "";
            this.Gross_ROE = this.Agent_ROE = 1.0;
            this.OtherCharges = new double?(1.0);
            this.UpgradeClass = false;
            this.FlightReservationId = this.SupplierId = 0;
        }

        public bool ShowFlight { get; set; }

        public string SameFlightIdx { get; set; }

        public string FlightType { get; set; }

        public bool IsFreeMeal { get; set; }

        public string FareFamily { get; set; }

        public short? Idx { get; set; }

        public int FlightReservationId { get; set; }

        public string FlightIndexKey { get; set; }

        public string FlightRefNo { get; set; }

        public bool ShouldSerializeFlightReservationId() => this.FlightReservationId != 0;

        public double TAmt { get; set; }

        public double TTax { get; set; }

        public double DummyMarkup { get; set; }

        public string Gds { get; set; }

        public string Gross_Curr { get; set; }

        public double Gross_ROE { get; set; }

        public string MA { get; set; }

        public string OA { get; set; }

        public string MVNm { get; set; }

        public string MSLogo { get; set; }

        public string LTD { get; set; }

        public bool Refundable { get; set; }

        public string JourneyType { get; set; }

        public string LastTktInfo { get; set; }

        public string FareType { get; set; }

        public string SupplierName { get; set; }

        public int SupplierId { get; set; }

        public string OfficeId { get; set; }

        public bool ShouldSerializeLastTktInfo() => this.LastTktInfo != null;

        public Details Details { get; set; }

        public BaggageInformation BaggageInformation { get; set; }

        public bool ShouldSerializeBaggageInformation() => this.BaggageInformation != null;

        public FaresRulesReply FaresRulesReply { get; set; }

        public bool ShouldSerializeFaresRulesReply() => this.FaresRulesReply != null;

        public AdditionalServices AdditionalServices { get; set; }

        public PnrDetails PnrDetails { get; set; }

        public bool ShouldSerializePnrDetails() => this.PnrDetails != null;

        public GDSDetails GDSDetails { get; set; }

        public bool ShouldSerializeGDSDetails() => this.ShowPropertyWhileSerialize;

        public NewFareXML OldFareXML { get; set; }

        public bool ShouldSerializeOldFareXML() => this.ShowPropertyWhileSerialize;

        [XmlElement("PriceInfo")]
        public PriceInfo[] PriceInfo { get; set; }

        public bool ShouldSerializePriceInfo()
        {
            return this.PriceInfo != null && this.ShowPropertyWhileSerialize;
        }

        public Charges Charges { get; set; }

        public bool ShouldSerializeCharges() => this.ShowPropertyWhileSerialize;

        public FCanxCharge FCanxCharge { get; set; }

        public bool ShouldSerializeFCanxCharge() => this.ShowPropertyWhileSerialize;

        public bool UpgradeClass { get; set; }

        public bool ShouldSerializeUpgradeClass() => this.ShowPropertyWhileSerialize;

        public string Agent_Curr { get; set; }

        public bool ShouldSerializeAgent_Curr() => this.ShowPropertyWhileSerialize;

        public double Agent_ROE { get; set; }

        public bool ShouldSerializeAgent_ROE() => this.ShowPropertyWhileSerialize;

        public string GrossCurrSym { get; set; }

        public bool ShouldSerializeGrossCurrSym() => this.ShowPropertyWhileSerialize;

        public double SearchMUAmt { get; set; }

        public bool ShouldSerializeSearchMUAmt() => this.ShowPropertyWhileSerialize;

        public string FIC { get; set; }

        public bool ShouldSerializeFIC() => this.ShowPropertyWhileSerialize;

        public string FT { get; set; }

        public bool ShouldSerializeFT() => this.ShowPropertyWhileSerialize;

        public string PT { get; set; }

        public bool ShouldSerializePT() => this.ShowPropertyWhileSerialize;

        public string Comp_Curr { get; set; }

        public bool ShouldSerializeComp_Curr() => this.ShowPropertyWhileSerialize;

        public int SuppCommRuleId { get; set; }

        public bool ShouldSerializeSuppCommRuleId() => this.ShowPropertyWhileSerialize;

        public int SalesCommRuleID { get; set; }

        public bool ShouldSerializeSalesCommRuleID() => this.ShowPropertyWhileSerialize;

        public int SalesRuleID_SF { get; set; }

        public bool ShouldSerializeSalesRuleID_SF() => this.ShowPropertyWhileSerialize;

        public bool IsCustSupRule { get; set; }

        public bool ShouldSerializeIsCustSupRule() => this.ShowPropertyWhileSerialize;

        public double SearchMU { get; set; }

        public bool ShouldSerializeSearchMU() => this.ShowPropertyWhileSerialize;

        public double? OtherCharges { get; set; }

        public bool ShouldSerializeOtherCharges() => this.ShowPropertyWhileSerialize;

        public string SupplierSessionKey { get; set; }

        public bool ShouldSerializeSupplierSessionKey() => this.ShowPropertyWhileSerialize;

        public string SupplierSecurityToken { get; set; }

        public bool ShouldSerializeSupplierSecurityToken() => this.ShowPropertyWhileSerialize;

        public AncillaryPref Preferences { get; set; }

        public bool ShouldSerializePreferences() => this.ShowPropertyWhileSerialize;

        public SSRDetails SSRDetails { get; set; }

        public bool ShouldSerializeSSRDetails() => this.SSRDetails != null;

        public bool LCC { get; set; } = false;

        public int LoyaltyPoints { get; set; } = 0;

        public bool RestrictFareAllow { get; set; } = false;
    }
}
