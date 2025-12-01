using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class BookingGeneralInfo
    {
        public string Currency { get; set; }

        public string LangCode { get; set; }

        public string CultureCode { get; set; }

        public string CompanyId { get; set; }

        public string Channel { get; set; }

        public string PaymentReceiptId { get; set; }

        public string LPOETONumber { get; set; }

        public string ReasonCode { get; set; }

        public string TTSPrimaryChannel { get; set; }

        public string ClientIP { get; set; }

        [DefaultValue(0)]
        public int DecimalPreference { get; set; }

        [DefaultValue(0)]
        public int TravelerId { get; set; }

        [DefaultValue(0)]
        public int ProcessedBy { get; set; }

        [DefaultValue(0)]
        public int AgentId { get; set; }

        [DefaultValue(0)]
        public int BranchId { get; set; }

        public Customer Customer { get; set; }

        [XmlElement("SubAgent")]
        public BookingSubAgent SubAgent { get; set; }

        [DefaultValue(false)]
        public bool IsChargeable { get; set; }

        [DefaultValue(false)]
        public bool IsCanxRefund { get; set; }

        [DefaultValue(false)]
        public bool IsEMD { get; set; }

        [DefaultValue(false)]
        public bool OnBehalfBooking { get; set; }

        [DefaultValue(false)]
        public bool IsPaymentRecieved { get; set; }

        [DefaultValue(false)]
        public bool IsRefundable { get; set; }

        [DefaultValue(false)]
        public bool IsCeiling { get; set; }

        [DefaultValue(false)]
        public bool IsCustomerOnCash { get; set; } = false;


        public string ChannelInterface { get; set; }

        public string DomainUrl { get; set; }

        public string CompanyDesign { get; set; }

        public bool IsAutoTicket { get; set; } = false;


        public bool IsCCLogic { get; set; } = false;


        public string AgencyLogo { get; set; }

        public string IsWhiteLabel { get; set; }

        public string PaymentGatewayType { get; set; }

        public string PCC { get; set; }

        public string BookingPcc { get; set; }

        public string AgentPCC { get; set; }

        public string AgentSine { get; set; }

        public bool IsPartPayment { get; set; } = false;


        public bool IsManual { get; set; } = false;


        public string InclusionExclusion { get; set; }

        public string ServiceBrief { get; set; }

        public bool IsSaveTraveller { get; set; }

        public int ClientId { get; set; }

        public BookingGeneralInfo()
        {
            Currency = (LangCode = (CultureCode = (CompanyId = (Channel = (PaymentReceiptId = (LPOETONumber = (TTSPrimaryChannel = (ClientIP = ""))))))));
            DecimalPreference = (TravelerId = (ProcessedBy = (BranchId = (AgentId = 0))));
            IsChargeable = false;
            IsCanxRefund = false;
            IsEMD = false;
            OnBehalfBooking = false;
            IsPaymentRecieved = false;
            IsRefundable = false;
            IsCeiling = false;
            IsCustomerOnCash = false;
            PCC = (BookingPcc = (AgentPCC = (AgentSine = "")));
        }
    }
}
