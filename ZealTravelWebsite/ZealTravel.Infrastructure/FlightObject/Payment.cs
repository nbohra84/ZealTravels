using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class Payment
    {
        public string PayMode { get; set; }

        public string CardType { get; set; }

        [DefaultValue(0)]
        public decimal PayAmount { get; set; }

        public string PayCurrency { get; set; }

        public string CardId { get; set; }

        public string PGType { get; set; }

        public string CardCode { get; set; }

        public string CardNumber { get; set; }

        public string NameOnCard { get; set; }

        public string ExpiryMonth { get; set; }

        [DefaultValue("0")]
        public string ExpiryYear { get; set; }

        public string CVV { get; set; }

        public string PGCode { get; set; }

        [DefaultValue(false)]
        public bool PGAutoRefund { get; set; }

        public string UccfCard { get; set; }

        [DefaultValue(0)]
        public double CCCharge { get; set; }

        public string PGResponseCode { get; set; }

        public string PGResponseDesc { get; set; }

        public string AuthCode { get; set; }

        public string TerminalId { get; set; }

        public string BankId { get; set; }

        public string BatchNo { get; set; }

        public string MerchantTxnCode { get; set; }

        public string PGTxnCode { get; set; }

        public string EftSequence { get; set; }

        public string TxnCurrency { get; set; }

        [DefaultValue(0)]
        public decimal TxnRoe { get; set; }

        public string TravelerAccountId { get; set; } = "0";


        public string TravelerAccountChannelCode { get; set; }

        public string PaymentModeType { get; set; }

        public string MeCode { get; set; }

        public string Type { get; set; }

        public string Charge { get; set; }

        public string Discount { get; set; }

        public string Address { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string CityCode { get; set; }

        public string CityName { get; set; }

        public string PinCode { get; set; }

        public string Email { get; set; }

        public string CCType { get; set; }

        public decimal CCValue { get; set; } = default(decimal);


        public Payment()
        {
            CCCharge = 0.0;
            ExpiryYear = "0";
            PGAutoRefund = false;
            TxnRoe = 1m;
            PayMode = (CardType = (CardId = (PGType = (CardCode = (CardNumber = (NameOnCard = (ExpiryMonth = (CVV = (UccfCard = (PGResponseCode = (PGResponseDesc = (AuthCode = (PayCurrency = "")))))))))))));
            TerminalId = (BankId = (BatchNo = (MerchantTxnCode = (PGTxnCode = (EftSequence = (TxnCurrency = (TravelerAccountId = (TravelerAccountChannelCode = ""))))))));
            PaymentModeType = (MeCode = (Type = (Charge = (Discount = ""))));
            Address = (CountryCode = (CountryName = (CityCode = (CityName = (PinCode = (Email = ""))))));
        }
    }
}
