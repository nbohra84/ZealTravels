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
    public class Passenger
    {
        private bool propertyValue;

        public bool ShowPropertyWhileSerialize
        {
            get
            {
                return propertyValue;
            }
            set
            {
                propertyValue = value;
            }
        }

        public int PaxIndex { get; set; }

        [DefaultValue(0)]
        public int PaxId { get; set; }

        public string PaxType { get; set; }

        public string PaxSubType { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Age { get; set; }

        public string DOB { get; set; }

        public string FFQNo { get; set; }

        public string MealPref { get; set; }

        public string SeatPref { get; set; }

        public string Baggage { get; set; }

        public string PassportNo { get; set; }

        public string PassportIssueDate { get; set; }

        public string PassportExpiryDate { get; set; }

        public string PassportIssuingCountry { get; set; }

        public string Gender { get; set; }

        public string Nationalty { get; set; }

        public string FrequentFlyer { get; set; }

        public string PhoneNo { get; set; }

        public string DailingCode { get; set; }

        public string MobileNumber { get; set; }

        public string EMailID { get; set; }

        [DefaultValue(0)]
        public int Room { get; set; } = 0;


        public string AssociatedOrderID { get; set; }

        public string SeatNo { get; set; }

        public string SeatStatus { get; set; }

        public string MealStatus { get; set; }

        public string IdentityType { get; set; }

        public string CommunicationLanguage { get; set; }

        [XmlElement("SeatMapSegments")]
        public List<SeatMapSegments> SeatMapSegments { get; set; }

        public string OutboundBaggIdx { get; set; }

        public string InboundBaggIdx { get; set; }

        public string OBMealIdx { get; set; }

        public string IBMealIdx { get; set; }

        public string SSRDocDetail { get; set; }

        public string SSRPersonName { get; set; }

        public string AirPricingKey { get; set; }

        public string PaxTatto { get; set; }

        [DefaultValue(0)]
        public decimal TotalCommision { get; set; }

        [DefaultValue(false)]
        public bool CommissionType { get; set; }

        public string TourCode { get; set; }

        [DefaultValue(false)]
        public bool isNetRemmit { get; set; }

        public string CommissionTypeEntry { get; set; }

        [DefaultValue(false)]
        public bool IsPricing { get; set; }

        public string FareType { get; set; }

        public string OriginDestination { get; set; }

        public string PaymentMode { get; set; }

        public string CNP { get; set; }

        public string UID { get; set; }

        public string CompanyName { get; set; }

        public string RegistrationNo { get; set; }

        public string CompanyAddress { get; set; }

        public string PreExistingDiseases { get; set; }

        public PersonAddress PersonAddress { get; set; }

        public string PANNumber { get; set; }

        public bool Status { get; set; }

        public bool ShouldSerializeShowPropertyWhileSerialize()
        {
            return false;
        }

        public Passenger()
        {
            PaxId = (Room = (PaxIndex = 0));
            TotalCommision = 0m;
            Title = (PaxType = (PaxSubType = (FirstName = (MiddleName = (LastName = (DOB = (FFQNo = (MealPref = (SeatPref = (Baggage = (FrequentFlyer = (PassportNo = (PassportIssueDate = (SSRDocDetail = (SSRPersonName = "")))))))))))))));
            PassportExpiryDate = (PassportIssuingCountry = (Gender = (Nationalty = (PhoneNo = (MobileNumber = (EMailID = (PaxTatto = (AssociatedOrderID = (DailingCode = (Age = (CNP = (UID = (CompanyName = (RegistrationNo = (CompanyAddress = "")))))))))))))));
            CommissionType = (isNetRemmit = (IsPricing = false));
        }

        public bool ShouldSerializeOutboundBaggIdx()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeInboundBaggIdx()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeOBMealIdx()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeIBMealIdx()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeSSRDocDetail()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeSSRName()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeAirPricingKey()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializePaxTatto()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeTotalCommision()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeCommissionType()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeTourCode()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeisNetRemmit()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeCommissionTypeEntry()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeIsPricing()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeFareType()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeOriginDestination()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializePaymentMode()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializePID()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeUID()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeCompanyName()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeRegistrationNo()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializeCompanyAddress()
        {
            return ShowPropertyWhileSerialize;
        }

        public bool ShouldSerializePreExistingDiseases()
        {
            return ShowPropertyWhileSerialize;
        }
    }
}
