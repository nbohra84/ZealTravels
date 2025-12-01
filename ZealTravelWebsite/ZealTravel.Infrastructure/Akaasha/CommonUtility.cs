using System;
using System.Collections;
using System.Collections.Generic;
using TIRequestResponse;
//using ZealTravel.Infrastructure.FlightObject;

namespace ZealTravel.Infrastructure.Akaasa
{

    public class CommonUtility
    {
        public static SearchRqQpV2 ConvertOldRequest2V2Request(SearchRQ _SearchRQ)
        {
            SearchRqQpV2 request = new SearchRqQpV2();
            var search = _SearchRQ.Search[0];

            request.Origin = search.DepartAP;
            request.Destination = search.ArrivalAP;
            request.SearchDestinationMacs = true;
            request.SearchOriginMacs = true;
            request.BeginDate = search.DepartDate;
            request.EndDate = null;
            /*if (_SearchRQ.Search.Length > 1)
            {
                var searchR = _SearchRQ.Search[1];
                request.EndDate = "2024-02-29"; //_SearchRQ.Search[1].a;
            }
            else
            {
                request.EndDate = null;
            }*/
            request.GetAllDetails = true;
            request.TaxesAndFees = "TaxesAndFees";  // "Taxes";  // ;


            if (_SearchRQ.PaxDetails.Adults > 0)
            {
                request.Passengers.Types.Add(new Passenger { Type = "ADT", Count = _SearchRQ.PaxDetails.Adults });
            }
            if (Convert.ToInt16(_SearchRQ.PaxDetails.Childs) > 0)
            {
                request.Passengers.Types.Add(new Passenger { Type = "CHD", Count = Convert.ToInt16(_SearchRQ.PaxDetails.Childs) });
            }
            if (Convert.ToInt16(_SearchRQ.PaxDetails.Infant) > 0)
            {
                request.Passengers.Types.Add(new Passenger { Type = "INFT", Count = Convert.ToInt16(_SearchRQ.PaxDetails.Infant) });
            }

            Codes _codes = new Codes();
            _codes.CurrencyCode = _SearchRQ.GeneralInfo.Currency;
            _codes.PromotionCode = "";
            request.Codes = _codes;

            request.NumberOfFaresPerJourney = 8;

            Filters _filters = new Filters();
            _filters.CompressionType = 1;
            _filters.GroupByDate = false;
            _filters.CarrierCode = "QP";
            _filters.Type = "ALL";
            _filters.MaxConnections = 4;
            //List<string> _productClasses = new List<string>();
            _filters.ProductClasses = new string[4];
            _filters.ProductClasses[0] = "EC";
            _filters.ProductClasses[1] = "AV";
            _filters.ProductClasses[2] = "SP";
            //_filters.ProductClasses[3] = "CP";
            _filters.ProductClasses[3] = "SM";

            //List<string> _sortOptions = new List<string>();
            //_sortOptions.Add("NoSort");
            _filters.SortOptions = new string[1];
            _filters.SortOptions[0] = "NoSort";

            //List<string> _fareTypes = new List<string>();
            //_fareTypes.Add("R");
            //_fareTypes.Add("V");
            //_fareTypes.Add("S");
            _filters.FareTypes = new string[4];
            _filters.FareTypes[0] = "R";
            _filters.FareTypes[1] = "V";
            _filters.FareTypes[2] = "S";
            _filters.FareTypes[3] = "SE";

            request.Filters = _filters;


            return request;
        }
    }




    public class Passenger
    {
        public string Type { get; set; }
        public int Count { get; set; }
    }

    public class Passengers
    {
        public List<Passenger> Types { get; set; }
        public Passengers()
        {
            Types = new List<Passenger>();
        }
    }

    public class Codes
    {
        public string CurrencyCode { get; set; }
        public string PromotionCode { get; set; }
    }

    public class Filters
    {
        public int CompressionType { get; set; }
        public bool GroupByDate { get; set; }
        public string CarrierCode { get; set; }
        public string Type { get; set; }
        public int MaxConnections { get; set; }
        public string[] ProductClasses { get; set; }
        public string[] SortOptions { get; set; }
        public string[] FareTypes { get; set; }

        public Filters()
        {
            //ProductClasses = new List<string>();
            //SortOptions = new List<string>();
            //FareTypes = new List<string>();
        }
    }

    public class SearchRqQpV2
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool SearchDestinationMacs { get; set; }
        public bool SearchOriginMacs { get; set; }
        public string BeginDate { get; set; }
        public object EndDate { get; set; }
        public bool GetAllDetails { get; set; }
        public string TaxesAndFees { get; set; }
        public Passengers Passengers { get; set; }
        public Codes Codes { get; set; }
        public int NumberOfFaresPerJourney { get; set; }
        public Filters Filters { get; set; }

        public SearchRqQpV2()
        {
            this.Passengers = new Passengers();
            this.Codes = new Codes();
            this.Filters = new Filters();

        }

    }


    //============== API Respose  =======================================================================
    //========= response as the API v2  [~/v4/availability/search/simple"]

    public class SearchRsQpV2
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        //public Result Results { get; set; }
        public List<TripList> Results { get; set; }
        //public bool faresAvailable { get; set; }
        //public string currencyCode { get; set; }
        //public bool includeTaxesAndFees { get; set; }
        //public string[] bundleOffers { get; set; }

        //public FaresAvailableList FaresAvailable { get; set; }
        public Dictionary<string, FareDetails> FaresAvailable { get; set; }
        public string CurrencyCode { get; set; }
        public bool IncludeTaxesAndFees { get; set; }
        public string BundleOffers { get; set; }
        // ... other properties
    }




    public class TripList
    {
        public List<Trip> Trips { get; set; }
    }

    public class Trip
    {
        public bool MultipleOriginStations { get; set; }
        public bool MultipleDestinationStations { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, List<Journey>> JourneysAvailableByMarket { get; set; }
    }


    public class Journey
    {
        public Designator Designator { get; set; }
        public int FlightType { get; set; }
        public int Stops { get; set; }
        public string JourneyKey { get; set; }
        public List<SegmentDetails> Segments { get; set; }
        public List<Fare1> Fares { get; set; }
        public bool NotForGeneralUser { get; set; }
    }


    public class Designator
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
    }



    public class SegmentDetails
    {
        public bool IsChangeOfGauge { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsHosted { get; set; }
        public Designator Designator { get; set; }
        public bool IsSeatmapViewable { get; set; }
        public string SegmentKey { get; set; }
        public Identifier Identifier { get; set; }
        public string CabinOfService { get; set; }
        public string ExternalIdentifier { get; set; }
        public bool International { get; set; }
        public int SegmentType { get; set; }
        public List<Leg> Legs { get; set; }
    }


    public class Identifier
    {
        public string identifier { get; set; }
        public string CarrierCode { get; set; }
        public string OpSuffix { get; set; }
    }


    public class Leg
    {
        public string LegKey { get; set; }
        public object OperationsInfo { get; set; }
        public Designator Designator { get; set; }
        public LegInfo LegInfo { get; set; }
        public List<object> Nests { get; set; }
        public List<object> Ssrs { get; set; }
        public string SeatmapReference { get; set; }
        public string FlightReference { get; set; }
    }
    public class LegInfo
    {
        public DateTime ArrivalTimeUtc { get; set; }
        public DateTime DepartureTimeUtc { get; set; }
        public int AdjustedCapacity { get; set; }
        public string ArrivalTerminal { get; set; }
        public int ArrivalTimeVariant { get; set; }
        public int BackMoveDays { get; set; }
        public int Capacity { get; set; }
        public bool ChangeOfDirection { get; set; }
        public int CodeShareIndicator { get; set; }
        public string DepartureTerminal { get; set; }
        public int DepartureTimeVariant { get; set; }
        public string EquipmentType { get; set; }
        public object EquipmentTypeSuffix { get; set; }
        public bool ETicket { get; set; }
        public bool Irop { get; set; }
        public int Lid { get; set; }
        public object MarketingCode { get; set; }
        public bool MarketingOverride { get; set; }
        public object OperatedByText { get; set; }
        public object OperatingCarrier { get; set; }
        public object OperatingFlightNumber { get; set; }
        public object OperatingOpSuffix { get; set; }
        public int OutMoveDays { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string PrbcCode { get; set; }
        public string ScheduleServiceType { get; set; }
        public int Sold { get; set; }
        public int Status { get; set; }
        public bool SubjectToGovtApproval { get; set; }
    }


    public class Fare1
    {
        public string FareAvailabilityKey { get; set; }
        public List<Detail> Details { get; set; }
        public bool IsSumOfSector { get; set; }
    }

    public class Detail
    {
        public int AvailableCount { get; set; }
        public int Status { get; set; }
        public string Reference { get; set; }
        public object ServiceBundleSetCode { get; set; }
        public object BundleReferences { get; set; }
        public object SsrReferences { get; set; }
    }



    //public class FaresAvailableList
    //{
    //   public Dictionary<string, FareDetails> FaresAvailable { get; set; }
    //}

    public class FareDetails
    {
        public Totals Totals { get; set; }
        public bool IsSumOfSector { get; set; }
        public string FareAvailabilityKey { get; set; }
        public List<Fare> Fares { get; set; }
    }

    public class Totals
    {
        public decimal FareTotal { get; set; }
        public decimal DiscountedTotal { get; set; }
        public decimal RevenueTotal { get; set; }
        public decimal PublishedTotal { get; set; }
        public decimal LoyaltyTotal { get; set; }
    }


    public class Fare
    {
        public bool IsGoverning { get; set; }
        public string FareBasisCode { get; set; }
        public string ClassOfService { get; set; }
        public string ClassType { get; set; }
        public int FareApplicationType { get; set; }
        public int FareStatus { get; set; }
        public string ProductClass { get; set; }
        public string RuleNumber { get; set; }
        public string RuleTariff { get; set; }
        public List<PassengerFare> PassengerFares { get; set; }
        public string TravelClassCode { get; set; }
        public bool IsAllotmentMarketFare { get; set; }
        public string Reference { get; set; }
    }



    public class PassengerFare
    {
        public string FareDiscountCode { get; set; }
        public string PassengerDiscountCode { get; set; }
        public string PassengerType { get; set; }
        public decimal FareAmount { get; set; }
        public decimal RevenueFare { get; set; }
        public decimal PublishedFare { get; set; }
        public decimal LoyaltyPoints { get; set; }
        public decimal DiscountedFare { get; set; }
        public List<ServiceCharge> ServiceCharges { get; set; }
        public int Multiplier { get; set; }
    }


    public class ServiceCharge
    {
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
        public int CollectType { get; set; }
        public string CurrencyCode { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public decimal ForeignAmount { get; set; }
        public string TicketCode { get; set; }
    }

    //======== end API v2 response

    //====== Quoat API V2 start

    /*public class Identifier
    {
        public string identifier { get; set; }
        public string carrierCode { get; set; }
    }
    */
    public class Market
    {
        public Identifier identifier { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public DateTime departureDate { get; set; }
    }

    public class Designator4Quot : Designator
    {
        //public string Destination { get; set; }
        //public string Origin { get; set; }
        public DateTime departureDate { get; set; }
    }

    public class SsrItem
    {
        public string ssrCode { get; set; }
        public int count { get; set; }
        public Designator Designator { get; set; }

        public SsrItem()
        {
            Designator = new Designator4Quot();
        }
    }

    public class Item
    {
        public string passengerType { get; set; }
        public List<SsrItem> ssrs { get; set; }
        public Item()
        {
            ssrs = new List<SsrItem>();
        }
    }

    public class Key
    {
        public string journeyKey { get; set; }
        public string fareAvailabilityKey { get; set; }
        public string standbyPriorityCode { get; set; }
        public string inventoryControl { get; set; }
    }

    public class Type
    {
        public string type { get; set; }
        public int count { get; set; }
    }

    public class Passengers4Quot
    {
        public List<Type> types { get; set; }
        public string residentCountry { get; set; }
        public Passengers4Quot()
        {
            types = new List<Type>();
        }
    }
    public class Ssr
    {
        public Market Market { get; set; }
        public List<Item> items { get; set; }

        public Ssr()
        {
            items = new List<Item>();
        }
    }
    public class QuoteRqQpV1
    {
        public List<Ssr> ssrs { get; set; }
        public List<Key> keys { get; set; }
        public Passengers4Quot passengers { get; set; }
        public string currencyCode { get; set; }
        public QuoteRqQpV1()
        {
            ssrs = new List<Ssr>();
            keys = new List<Key>();
        }


    }


    public class RequestQuoteV4
    {
        public List<Key> keys { get; set; }
        public Passengers4Quot passengers { get; set; }
        public string currencyCode { get; set; }
        public string promotionCode { get; set; }
    }

    //======= Quoat API V2 End

    //====== FareRule RTP format
    public class passengerPriceType
    {
        public string passengerType { get; set; }
        public string passengerDiscountCode { get; set; }
        public int passengerCount { get; set; }
    }
    public class RequestQuoteRTF
    {
        public string fareAvailabilityKey { get; set; }
        public List<passengerPriceType> passengerPriceTypes { get; set; }

        public RequestQuoteRTF()
        {
            passengerPriceTypes = new List<passengerPriceType>();
        }

    }

    //=========== start Fares objects



    public class ServiceCharge4Fare
    {   
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
        public int CollectType { get; set; }
        public string CurrencyCode { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public double ForeignAmount { get; set; }
        public string TicketCode { get; set; }
    }

    public class InfantFee
    {
        public bool IsConfirmed { get; set; }
        public bool IsConfirming { get; set; }
        public bool IsConfirmingExternal { get; set; }
        public string Code { get; set; }
        public object Detail { get; set; } // Modify this based on the actual type
        public string PassengerFeeKey { get; set; }
        public bool Override { get; set; }
        public string FlightReference { get; set; }
        public object Note { get; set; } // Modify this based on the actual type
        public DateTime? CreatedDate { get; set; }
        public bool IsProtected { get; set; }
        public List<ServiceCharge4Fare> ServiceCharges { get; set; }

   

        public InfantFee()
        {
            this.ServiceCharges = new List<ServiceCharge4Fare>();
        }
    }


    public class InfantFeeList
    {
        public List<InfantFee> InfantFees { get; set; }
        public object Nationality { get; set; } // Modify this based on the actual type
        public object DateOfBirth { get; set; } // Modify this based on the actual type
        public object TravelDocuments { get; set; } // Modify this based on the actual type
        public object ResidentCountry { get; set; } // Modify this based on the actual type
        public int Gender { get; set; }
        public object Type { get; set; } // Modify this based on the actual type
        public InfantFeeList()
        {
            this.InfantFees = new List<InfantFee>();
        }
    }

    public class Passenger4Fare
    {
        public string PassengerKey { get; set; }
        public string PassengerAlternateKey { get; set; } // Modify this based on the actual type
        public string CustomerNumber { get; set; } // Modify this based on the actual type
        public ArrayList Fees { get; set; } // Modify this based on the actual type
        public object Name { get; set; } // Modify this based on the actual type
        public string PassengerTypeCode { get; set; }
        public object DiscountCode { get; set; } // Modify this based on the actual type
        public ArrayList Bags { get; set; } // Modify this based on the actual type
        public object Program { get; set; } // Modify this based on the actual type
        public InfantFeeList Infant { get; set; }

        public Info4FateRs Info { get; set; }
        public ArrayList TravelDocuments { get; set; }
        public ArrayList Addresses { get; set; }
        public int WeightCategory { get; set; }
        public ArrayList EmdCoupons { get; set; }

        public Passenger4Fare()
        {
            this.Fees = new ArrayList();
            this.Bags = new ArrayList();
            this.Addresses = new ArrayList();
            this.EmdCoupons = new ArrayList();
            TravelDocuments = new ArrayList();
            Addresses = new ArrayList();
            EmdCoupons = new ArrayList();
        }
    }
    public class Info4FateRs
    {
        public string Nationality { get; set; }
        public string ResidentCountry { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string FamilyNumber { get; set; }
    }

    public class InfoTotal4FateRs
    {
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }
        public string Adjustments { get; set; }
        public List<ServiceCharge4Fare> Charges { get; set; }
    }


    public class PassengerData
    {
        //public Passenger4Fare MCFBRFQ { get; set; }
        public List<Passenger4Fare> PassengerList { get; set; }
        
        public PassengerData()
        {
            this.PassengerList = new List<Passenger4Fare>();
        }
    }


    /*public class Designator
    {
        public string Destination { get; set; }
        public string Origin { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
    }*/

    /*public class Fare
    {
        // Properties omitted for brevity; include properties from JSON
    }*/

    public class FareObject4FareQuote
    {
        public bool IsGoverning { get; set; }
        public bool DowngradeAvailable { get; set; }
        public string CarrierCode { get; set; }
        public string FareKey { get; set; }
        public string ClassOfService { get; set; }
        public string FareApplicationType { get; set; }
        public string FareClassOfService { get; set; }
        public string FareBasisCode { get; set; }
        public int FareSequence { get; set; }
        public int InboundOutBound { get; set; }
        public int FareStatus { get; set; }
        public bool IsAllotmentMarketFare { get; set; }
        public object OriginalClassOfService { get; set; }
        public string RuleNumber { get; set; }
        public string ProductClass { get; set; }
        public object RuleTariff { get; set; }
        public object TravelClassCode { get; set; }
        public object CrossReferenceClassOfService { get; set; }
        public List<PassengerFare> PassengerFares { get; set; }
        public string PassengerType { get; set; }
        public object DiscountCode { get; set; }
        public object FareDiscountCode { get; set; }
        public string FareLink { get; set; }
    }

    public class SeatPreferences
    {
        public int Seat { get; set; }
        public int TravelClass { get; set; }
        public List<object> AdvancedPreferences { get; set; }
    }

    public class SegmentDetail4Fare
    {
        public List<object> Seats { get; set; }
        public string PassengerKey { get; set; }
        public object ActivityDate { get; set; }
        public object BoardingSequence { get; set; }
        public object CreatedDate { get; set; }
        public int LiftStatus { get; set; }
        public object ModifiedDate { get; set; }
        public int OverBookIndicator { get; set; }
        public object PriorityDate { get; set; }
        public bool TimeChanged { get; set; }
        public object VerifiedTravelDocs { get; set; }
        public object SourcePointOfSale { get; set; }
        public object PointOfSale { get; set; }
        public List<object> Ssrs { get; set; }
        public List<object> Tickets { get; set; }
        public List<object> Bags { get; set; }
        public List<object> Scores { get; set; }
        public object BoardingPassDetail { get; set; }
        public bool HasInfant { get; set; }
        public SeatPreferences SeatPreferences { get; set; }
        public object BundleCode { get; set; }
        public object VerifiedTravelDocuments { get; set; }
        public int ReferenceNumber { get; set; }
        public object BaggageGroupNumber { get; set; }
    }



    public class Segment
    {
        public bool IsStandby { get; set; }
        public bool IsConfirming { get; set; }
        public bool IsConfirmingExternal { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsHosted { get; set; }
        public bool IsChangeOfGauge { get; set; }
        public Designator Designator { get; set; }
        public bool IsSeatmapViewable { get; set; }
        public List<FareObject4FareQuote> Fares { get; set; }

        public string SegmentKey { get; set; }
        public Identifier Identifier { get; set; }
        public SegmentDetail4Fare PassengerSegment { get; set; }

        public int ChannelType { get; set; }
        public object CabinOfService { get; set; }
        public object ExternalIdentifier { get; set; }
        public object PriorityCode { get; set; }
        public int ChangeReasonCode { get; set; }
        public int SegmentType { get; set; }
        public object SalesDate { get; set; }
        public bool International { get; set; }
        public string FlightReference { get; set; }
        public List<LegInfo> Legs { get; set; }
        public int Status { get; set; }

        // Other properties omitted for brevity
        public Segment()
        {
            this.Fares = new List<FareObject4FareQuote>();
        }
    }

    public class JourneyDtl4Fare
    {
        public int FlightType { get; set; }
        public int Stops { get; set; }
        public Designator Designator { get; set; }
        public object Move { get; set; }
        public List<Segment> Segments { get; set; }
        public string JourneyKey { get; set; }
        public bool NotForGeneralUser { get; set; }
        // Other properties omitted for brevity

        public JourneyDtl4Fare()
        {
            this.Segments = new List<Segment>();
        }
    }

    public class JourneyDetails
    {
        public string journeyKey { get; set; }
        public decimal totalAmount { get; set; }
        public double totalPoints { get; set; }
        public double totalTax { get; set; }
        public double totalDiscount { get; set; }
    }
    /*public class Journeys4Fare
    {
        public List<JourneyDtl4Fare> Journeys { get; set; }
        public Journeys4Fare()
        {
            this.Journeys = new List<JourneyDtl4Fare>();
        }
    }*/
    public class JourneyTotals
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
    }
    public class JourneyAddOnTotal
    {
        public string Car { get; set; }
        public string Hotel { get; set; }
        public string Activities { get; set; }

    }


public class PassengerTotal
    {
        public object Services { get; set; }
        public object SpecialServices { get; set; }
        public object Seats { get; set; }
        public object Upgrades { get; set; }
        public object Spoilage { get; set; }
        public object NameChanges { get; set; }
        public object Convenience { get; set; }
        public InfoTotal4FateRs Infant { get; set; }
    }


    public class Passenger4BrkUp4Fare
    {
        public string PassengerKey { get; set; }
        public object Services { get; set; }
        public object SpecialServices { get; set; }
        public object Seats { get; set; }
        public object Upgrades { get; set; }
        public object Spoilage { get; set; }
        public object NameChanges { get; set; }
        public object Convenience { get; set; }
        public InfoTotal4FateRs Infant { get; set; }
    }

    public class Breakdown
    {
        public decimal balanceDue { get; set; }
        public double pointsBalanceDue { get; set; }
        public decimal authorizedBalanceDue { get; set; }
        public decimal totalAmount { get; set; }
        public double totalPoints { get; set; }
        public decimal totalToCollect { get; set; }
        public double totalPointsToCollect { get; set; }
        public decimal totalCharged { get; set; }
        public PassengerTotal passengerTotals { get; set; }

        public Dictionary<string, Passenger4BrkUp4Fare> Passengers { get; set; }
        public JourneyTotals JourneyTotals { get; set; }
        public Dictionary<string, JourneyDetails> Journeys { get; set; }


        public Dictionary<string, JourneyAddOnTotal> AddOnTotals { get; set; }
    }

    //public class Passengers4Fare
    //{
    //    public Dictionary<string, PassengerData> PassengerList { get; set; }
    //}

    public class Data4Fare
    {
        //public List<PassengerData> Passengers { get; set; }
        public Dictionary<string, PassengerData> Passengers { get; set; }
        //public Journeys4Fare Journeys { get; set; }
        public List<JourneyDtl4Fare> Journeys { get; set; }
        public Breakdown Breakdown { get; set; }
        public Data4Fare()
        {
            this.Journeys = new List<JourneyDtl4Fare>();
        }

    }

    public class FareQpRS
    {
        public Data4Fare Data { get; set; }
    }



    //============= End Fare Objects

}