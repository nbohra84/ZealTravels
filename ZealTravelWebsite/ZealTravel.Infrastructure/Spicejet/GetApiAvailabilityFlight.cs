using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Infrastructure.DBCommon;
//using air_spicejet.svc_booking;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiAvailabilityFlight
    {
        public string errorMessage;
        public DataTable dtOutbound;
        public DataTable dtInbound;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;

        private string Supplierid;
        private string Password;

        private int ContractVersion;

        //-----------------------------------------------------------------------------------------------
        public GetApiAvailabilityFlight(string Searchid,string Companyid, string Supplierid, string Password)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.Supplierid = Supplierid;
            this.Password = Password;

            this.ContractVersion = 420;
        }
        public void GetOneWay(string PriceType, string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string FltType, string Sector)
        {
            dtOutbound = Schema.SchemaFlights;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                int iPaxCount = Adt + Chd;
                GetApiLogin objLogin = new GetApiLogin();
                string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.IBookingManager objBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetAvailabilityRequest objRequest = new svc_booking.GetAvailabilityRequest();
                    objRequest.Signature = Signature;
                    objRequest.ContractVersion = ContractVersion;

                    objRequest.TripAvailabilityRequest = new svc_booking.TripAvailabilityRequest();
                    objRequest.TripAvailabilityRequest.AvailabilityRequests = new svc_booking.AvailabilityRequest[1];


                    svc_booking.AvailabilityRequest objAvailabilityRequest = new svc_booking.AvailabilityRequest();
                 

                    svc_booking.PaxPriceType[] objPaxPriceType = new svc_booking.PaxPriceType[iPaxCount];
                    for (int i = 0; i < Adt; i++)
                    {
                        objPaxPriceType[i] = new svc_booking.PaxPriceType();
                        objPaxPriceType[i].PaxType = "ADT";
                        objPaxPriceType[i].PaxDiscountCode = String.Empty;
                    }
                    for (int i = 0; i < Chd; i++)
                    {
                        objPaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                        objPaxPriceType[Adt + i].PaxType = "CHD";
                        objPaxPriceType[Adt + i].PaxDiscountCode = String.Empty;
                    }

                    objAvailabilityRequest.PaxPriceTypes = objPaxPriceType;
                    objAvailabilityRequest.AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.AvailabilityTypeSpecified = true;

                    objAvailabilityRequest.AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.AvailabilityFilterSpecified = true;



                    objAvailabilityRequest.MaximumFarePrice = 0;
                    objAvailabilityRequest.MaximumFarePriceSpecified = true;

                    objAvailabilityRequest.MinimumFarePrice = 0;
                    objAvailabilityRequest.MinimumFarePriceSpecified = true;

                    objAvailabilityRequest.BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.BeginDateSpecified = true;

                    objAvailabilityRequest.EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.EndDateSpecified = true;

                    objAvailabilityRequest.DepartureStation = Origin;
                    objAvailabilityRequest.ArrivalStation = Destination;

                    objAvailabilityRequest.FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.FlightTypeSpecified = true;



                    objAvailabilityRequest.CurrencyCode = "INR";
                    objAvailabilityRequest.SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.SSRCollectionsModeSpecified = true;


                    objAvailabilityRequest.Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.DowSpecified = true;


                    objAvailabilityRequest.IncludeTaxesAndFees = true;
                    objAvailabilityRequest.IncludeTaxesAndFeesSpecified = true;


                    objAvailabilityRequest.PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.PaxCountSpecified = true;


                    objAvailabilityRequest.CarrierCode = "SG";
                    objAvailabilityRequest.NightsStay = 0;
                    objAvailabilityRequest.InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.InboundOutboundSpecified = true;

                    objAvailabilityRequest.IncludeAllotments = false;
                    objAvailabilityRequest.IncludeAllotmentsSpecified = true;

                    objAvailabilityRequest.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.FareRuleFilterSpecified = true;

                    objAvailabilityRequest.MaximumConnectingFlights = 3;
                    objAvailabilityRequest.MaximumConnectingFlightsSpecified = true;

                    if (PriceType.Equals("TBF"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("MAX"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "MX";
                    }
                    else if (PriceType.Equals("NDF"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("SME"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("SPL"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("COUPON"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("CORPORATE"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "C";
                    }
                    else if (PriceType.Equals("LCC"))  ///-- Under LCC
                    {
                        //dbCommon.Logger.dbLogg(Companyid, 0, "GetAPIAvailability", "air_spicejet", Supplierid, Searchid, "Price Type is =>" + PriceType);

                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.Default;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "R";

                        if (FltType == "I")
                        {
                            string[] _ProductClasses_I = { "XA", "AX" };
                            objAvailabilityRequest.ProductClasses = _ProductClasses_I;
                        }
                        else
                        {
                            string[] _ProductClasses = { "RS", "SS", "SR", "SU" };
                            objAvailabilityRequest.ProductClasses = _ProductClasses;
                        }
                    }
                    else
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.Default;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[4];
                        objAvailabilityRequest.FareTypes[0] = "R";
                        objAvailabilityRequest.FareTypes[1] = "MX";
                        objAvailabilityRequest.FareTypes[2] = "IO";
                        objAvailabilityRequest.FareTypes[3] = "F";

                    }

                    objAvailabilityRequest.PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objRequest.TripAvailabilityRequest.AvailabilityRequests[0] = objAvailabilityRequest;
                    objRequest.TripAvailabilityRequest.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objRequest.TripAvailabilityRequest.LoyaltyFilterSpecified = true;


                    GetApiRequest = GetCommonFunctions.Serialize(objRequest);
                    svc_booking.GetAvailabilityResponse objAvailabilityResponse = objBookingManager.GetAvailability(objRequest);
                    GetApiResponse = GetCommonFunctions.Serialize(objAvailabilityResponse);

                    if (objAvailabilityResponse != null && objAvailabilityResponse.GetTripAvailabilityResponse.Schedules[0].Length > 0)
                    {
                        svc_booking.Journey[] j = objAvailabilityResponse.GetTripAvailabilityResponse.Schedules[0][0].Journeys;

                        SetAvailabilityModifier objModifier = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Origin, Destination, Adt, Chd, Inf, Sector);
                        objModifier.FlightModifier(j, FltType, dtOutbound);
                    }

                    if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                    {
                        GetApiAvailabilityFare objFareRequest = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                        objFareRequest.GetOneWayFares(dtOutbound, false);
                        this.errorMessage = objFareRequest.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(Companyid, 0, "GetOneWay", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(Companyid, 0, "GetOneWay-air_spicejet", "AVAILABILITY", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
        }
        public async Task<DataTable> GetOneWayAsync(string PriceType, string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string FltType, string Sector)
        {
            dtOutbound = Schema.SchemaFlights;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                int iPaxCount = Adt + Chd;
                GetApiLogin objLogin = new GetApiLogin();
                string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.IBookingManager objBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetAvailabilityRequest objRequest = new svc_booking.GetAvailabilityRequest();
                    objRequest.Signature = Signature;
                    objRequest.ContractVersion = ContractVersion;

                    objRequest.TripAvailabilityRequest = new svc_booking.TripAvailabilityRequest();
                    objRequest.TripAvailabilityRequest.AvailabilityRequests = new svc_booking.AvailabilityRequest[1];


                    svc_booking.AvailabilityRequest objAvailabilityRequest = new svc_booking.AvailabilityRequest();


                    svc_booking.PaxPriceType[] objPaxPriceType = new svc_booking.PaxPriceType[iPaxCount];
                    for (int i = 0; i < Adt; i++)
                    {
                        objPaxPriceType[i] = new svc_booking.PaxPriceType();
                        objPaxPriceType[i].PaxType = "ADT";
                        objPaxPriceType[i].PaxDiscountCode = String.Empty;
                    }
                    for (int i = 0; i < Chd; i++)
                    {
                        objPaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                        objPaxPriceType[Adt + i].PaxType = "CHD";
                        objPaxPriceType[Adt + i].PaxDiscountCode = String.Empty;
                    }

                    objAvailabilityRequest.PaxPriceTypes = objPaxPriceType;
                    objAvailabilityRequest.AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.AvailabilityTypeSpecified = true;

                    objAvailabilityRequest.AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.AvailabilityFilterSpecified = true;



                    objAvailabilityRequest.MaximumFarePrice = 0;
                    objAvailabilityRequest.MaximumFarePriceSpecified = true;

                    objAvailabilityRequest.MinimumFarePrice = 0;
                    objAvailabilityRequest.MinimumFarePriceSpecified = true;

                    objAvailabilityRequest.BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.BeginDateSpecified = true;

                    objAvailabilityRequest.EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.EndDateSpecified = true;

                    objAvailabilityRequest.DepartureStation = Origin;
                    objAvailabilityRequest.ArrivalStation = Destination;

                    objAvailabilityRequest.FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.FlightTypeSpecified = true;



                    objAvailabilityRequest.CurrencyCode = "INR";
                    objAvailabilityRequest.SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.SSRCollectionsModeSpecified = true;


                    objAvailabilityRequest.Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.DowSpecified = true;


                    objAvailabilityRequest.IncludeTaxesAndFees = true;
                    objAvailabilityRequest.IncludeTaxesAndFeesSpecified = true;


                    objAvailabilityRequest.PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.PaxCountSpecified = true;


                    objAvailabilityRequest.CarrierCode = "SG";
                    objAvailabilityRequest.NightsStay = 0;
                    objAvailabilityRequest.InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.InboundOutboundSpecified = true;

                    objAvailabilityRequest.IncludeAllotments = false;
                    objAvailabilityRequest.IncludeAllotmentsSpecified = true;

                    objAvailabilityRequest.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.FareRuleFilterSpecified = true;

                    objAvailabilityRequest.MaximumConnectingFlights = 3;
                    objAvailabilityRequest.MaximumConnectingFlightsSpecified = true;

                    if (PriceType.Equals("TBF"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("MAX"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "MX";
                    }
                    else if (PriceType.Equals("NDF"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("SME"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("SPL"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("COUPON"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "T";
                    }
                    else if (PriceType.Equals("CORPORATE"))
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "C";
                    }
                    else if (PriceType.Equals("LCC"))  ///-- Under LCC
                    {
                        //dbCommon.Logger.dbLogg(Companyid, 0, "GetAPIAvailability", "air_spicejet", Supplierid, Searchid, "Price Type is =>" + PriceType);

                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.Default;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[1];
                        objAvailabilityRequest.FareTypes[0] = "R";

                        if (FltType == "I")
                        {
                            string[] _ProductClasses_I = { "XA", "AX" };
                            objAvailabilityRequest.ProductClasses = _ProductClasses_I;
                        }
                        else
                        {
                            string[] _ProductClasses = { "RS", "SS", "SR", "SU" };
                            objAvailabilityRequest.ProductClasses = _ProductClasses;
                        }
                    }
                    else
                    {
                        objAvailabilityRequest.FareClassControl = svc_booking.FareClassControl.Default;
                        objAvailabilityRequest.FareClassControlSpecified = true;
                        objAvailabilityRequest.FareTypes = new string[4];
                        objAvailabilityRequest.FareTypes[0] = "R";
                        objAvailabilityRequest.FareTypes[1] = "MX";
                        objAvailabilityRequest.FareTypes[2] = "IO";
                        objAvailabilityRequest.FareTypes[3] = "F";

                    }

                    objAvailabilityRequest.PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objRequest.TripAvailabilityRequest.AvailabilityRequests[0] = objAvailabilityRequest;
                    objRequest.TripAvailabilityRequest.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objRequest.TripAvailabilityRequest.LoyaltyFilterSpecified = true;


                    GetApiRequest = GetCommonFunctions.Serialize(objRequest);
                    svc_booking.GetAvailabilityResponse objAvailabilityResponse =await objBookingManager.GetAvailabilityAsync(objRequest);
                    GetApiResponse = GetCommonFunctions.Serialize(objAvailabilityResponse);

                    if (objAvailabilityResponse != null && objAvailabilityResponse.GetTripAvailabilityResponse.Schedules[0].Length > 0)
                    {
                        svc_booking.Journey[] j = objAvailabilityResponse.GetTripAvailabilityResponse.Schedules[0][0].Journeys;

                        SetAvailabilityModifier objModifier = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Origin, Destination, Adt, Chd, Inf, Sector);
                        objModifier.FlightModifier(j, FltType, dtOutbound);
                    }

                    if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                    {
                        GetApiAvailabilityFare objFareRequest = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                        objFareRequest.GetOneWayFares(dtOutbound, false);
                        this.errorMessage = objFareRequest.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Companyid, 0, "GetOneWay", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace + "," + ex.StackTrace);
            }
           //dbCommon.Logger.WriteLogg(Companyid, 0, "GetOneWay-air_spicejet", "AVAILABILITY", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return dtOutbound;
        }

        public void GetRT(string PriceType, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            dtOutbound = Schema.SchemaFlights;
            dtInbound = Schema.SchemaFlights;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.GetAvailabilityRequest objAvailabilityRequest = new svc_booking.GetAvailabilityRequest();

                    int iPaxCount = Adt + Chd;

                    int PPT = 1;
                    if (Chd > 0)
                    {
                        PPT = 2;
                    }
                    svc_booking.PaxPriceType[] priceTypes = new svc_booking.PaxPriceType[PPT];

                    priceTypes[0] = new svc_booking.PaxPriceType();
                    priceTypes[0].PaxType = "ADT";
                    priceTypes[0].PaxDiscountCode = String.Empty;

                    if (Chd > 0)
                    {
                        priceTypes[1] = new svc_booking.PaxPriceType();
                        priceTypes[1].PaxType = "CHD";
                        priceTypes[1].PaxDiscountCode = String.Empty;
                    }

                    objAvailabilityRequest.TripAvailabilityRequest = new svc_booking.TripAvailabilityRequest();
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests = new svc_booking.AvailabilityRequest[2];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0] = new svc_booking.AvailabilityRequest();

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = Origin;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = Destination;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "SG";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = new string[4];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[0] = "R";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[1] = "IO";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[2] = "MX";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[3] = "F";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 2;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MinimumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MinimumFarePriceSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumFarePriceSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].SSRCollectionsModeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].InboundOutboundSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].NightsStay = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].NightsStaySpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeAllotments = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeAllotmentsSpecified = false;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = priceTypes;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = false;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1] = new svc_booking.AvailabilityRequest();
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].DepartureStation = Destination;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].ArrivalStation = Origin;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].BeginDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].EndDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FlightTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].CarrierCode = "SG";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes = new string[4];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[0] = "R";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[1] = "MX";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[2] = "IO";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[3] = "F";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxCountSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].DowSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].CurrencyCode = "INR";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityTypeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumConnectingFlights = 2;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumConnectingFlightsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareClassControlSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MinimumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MinimumFarePriceSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumFarePriceSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].SSRCollectionsModeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].InboundOutboundSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].NightsStay = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].NightsStaySpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeAllotments = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeAllotmentsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxPriceTypes = priceTypes;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeTaxesAndFees = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeTaxesAndFeesSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareRuleFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objAvailabilityRequest.Signature = Signature;
                    objAvailabilityRequest.ContractVersion = ContractVersion;

                    GetApiRequest = GetCommonFunctions.Serialize(objAvailabilityRequest);

                    svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetAvailabilityResponse response = objIBookingManager.GetAvailability(objAvailabilityRequest);

                    GetApiResponse = GetCommonFunctions.Serialize(response);

                    if (response.GetTripAvailabilityResponse.Schedules[0].Length > 0)
                    {
                        svc_booking.Journey[] j1 = response.GetTripAvailabilityResponse.Schedules[0][0].Journeys;
                        svc_booking.Journey[] j2 = response.GetTripAvailabilityResponse.Schedules[1][0].Journeys;

                        SetAvailabilityModifier objModifier1 = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Origin, Destination, Adt, Chd, Inf, Sector);
                        objModifier1.FlightModifier(j1, "O", dtOutbound);

                        SetAvailabilityModifier objModifier2 = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Destination, Origin, Adt, Chd, Inf, Sector);
                        objModifier2.FlightModifier(j2, "I", dtInbound);
                    }

                    if (dtOutbound != null && dtInbound != null && dtOutbound.Rows.Count > 0 && dtInbound.Rows.Count > 0)
                    {
                        if (Sector.Equals("D"))
                        {
                            GetApiAvailabilityFare objFareRequest1 = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest1.GetOneWayFares(dtOutbound, false);
                            this.errorMessage = objFareRequest1.errorMessage;

                            GetApiAvailabilityFare objFareRequest2 = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest2.GetOneWayFares(dtInbound, false);
                            this.errorMessage = objFareRequest2.errorMessage;
                        }
                        else
                        {
                            GetCombineAvailability objR = new GetCombineAvailability();
                            objR.CombineTripDT(dtOutbound, dtInbound);
                            dtOutbound = objR.CombineDT;

                            GetApiAvailabilityFare objFareRequest = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest.GetRTFares(dtOutbound, false);
                            this.errorMessage = objFareRequest.errorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Companyid, 0, "GetRT", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            //dbCommon.Logger.WriteLogg(Companyid, 0, "GetRT-air_spicejet", "AVAILABILITY", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
        }
        public async Task<List<DataTable>> GetRT_Async(string PriceType, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            dtOutbound = Schema.SchemaFlights;
            dtInbound = Schema.SchemaFlights;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_booking.GetAvailabilityRequest objAvailabilityRequest = new svc_booking.GetAvailabilityRequest();

                    int iPaxCount = Adt + Chd;

                    int PPT = 1;
                    if (Chd > 0)
                    {
                        PPT = 2;
                    }
                    svc_booking.PaxPriceType[] priceTypes = new svc_booking.PaxPriceType[PPT];

                    priceTypes[0] = new svc_booking.PaxPriceType();
                    priceTypes[0].PaxType = "ADT";
                    priceTypes[0].PaxDiscountCode = String.Empty;

                    if (Chd > 0)
                    {
                        priceTypes[1] = new svc_booking.PaxPriceType();
                        priceTypes[1].PaxType = "CHD";
                        priceTypes[1].PaxDiscountCode = String.Empty;
                    }

                    objAvailabilityRequest.TripAvailabilityRequest = new svc_booking.TripAvailabilityRequest();
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests = new svc_booking.AvailabilityRequest[2];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0] = new svc_booking.AvailabilityRequest();

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].DepartureStation = Origin;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].ArrivalStation = Destination;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].BeginDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].EndDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FlightTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].CarrierCode = "SG";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes = new string[4];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[0] = "R";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[1] = "IO";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[2] = "MX";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareTypes[3] = "F";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxCountSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].DowSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].CurrencyCode = "INR";


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlights = 2;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumConnectingFlightsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].AvailabilityFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareClassControlSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MinimumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MinimumFarePriceSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].MaximumFarePriceSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].SSRCollectionsModeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].InboundOutboundSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].NightsStay = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].NightsStaySpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeAllotments = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeAllotmentsSpecified = false;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes = priceTypes;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFees = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].IncludeTaxesAndFeesSpecified = false;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].FareRuleFilterSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[0].PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1] = new svc_booking.AvailabilityRequest();
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].DepartureStation = Destination;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].ArrivalStation = Origin;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].BeginDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].BeginDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].EndDate = DateTime.Parse(GetCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00:00:00");
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].EndDateSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FlightType = svc_booking.FlightType.All;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FlightTypeSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].CarrierCode = "SG";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes = new string[4];
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[0] = "R";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[1] = "MX";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[2] = "IO";
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareTypes[3] = "F";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxCount = Convert.ToInt16(iPaxCount);
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxCountSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].Dow = svc_booking.DOW.Daily;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].DowSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].CurrencyCode = "INR";

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityType = svc_booking.AvailabilityType.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityTypeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumConnectingFlights = 2;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumConnectingFlightsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityFilter = svc_booking.AvailabilityFilter.ExcludeUnavailable;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].AvailabilityFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareClassControl = svc_booking.FareClassControl.LowestFareClass;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareClassControlSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MinimumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MinimumFarePriceSpecified = true;


                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumFarePrice = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].MaximumFarePriceSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].SSRCollectionsMode = svc_booking.SSRCollectionsMode.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].SSRCollectionsModeSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].InboundOutbound = svc_booking.InboundOutbound.None;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].InboundOutboundSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].NightsStay = 0;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].NightsStaySpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeAllotments = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeAllotmentsSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PaxPriceTypes = priceTypes;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeTaxesAndFees = false;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].IncludeTaxesAndFeesSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareRuleFilter = svc_booking.FareRuleFilter.Default;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].FareRuleFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].LoyaltyFilterSpecified = true;

                    objAvailabilityRequest.TripAvailabilityRequest.AvailabilityRequests[1].PromotionCode = GetCommonFunctions.GetPromotionCodeBySupplier(Supplierid, PriceType);

                    objAvailabilityRequest.Signature = Signature;
                    objAvailabilityRequest.ContractVersion = ContractVersion;

                    GetApiRequest = GetCommonFunctions.Serialize(objAvailabilityRequest);

                    svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetAvailabilityResponse response = await objIBookingManager.GetAvailabilityAsync(objAvailabilityRequest);

                    GetApiResponse = GetCommonFunctions.Serialize(response);

                    if (response.GetTripAvailabilityResponse.Schedules[0].Length > 0)
                    {
                        svc_booking.Journey[] j1 = response.GetTripAvailabilityResponse.Schedules[0][0].Journeys;
                        svc_booking.Journey[] j2 = response.GetTripAvailabilityResponse.Schedules[1][0].Journeys;

                        SetAvailabilityModifier objModifier1 = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Origin, Destination, Adt, Chd, Inf, Sector);
                        objModifier1.FlightModifier(j1, "O", dtOutbound);

                        SetAvailabilityModifier objModifier2 = new SetAvailabilityModifier(Searchid, Companyid, Supplierid, Signature, PriceType, Destination, Origin, Adt, Chd, Inf, Sector);
                        objModifier2.FlightModifier(j2, "I", dtInbound);
                    }

                    if (dtOutbound != null && dtInbound != null && dtOutbound.Rows.Count > 0 && dtInbound.Rows.Count > 0)
                    {
                        if (Sector.Equals("D"))
                        {
                            GetApiAvailabilityFare objFareRequest1 = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest1.GetOneWayFares(dtOutbound, false);
                            this.errorMessage = objFareRequest1.errorMessage;

                            GetApiAvailabilityFare objFareRequest2 = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest2.GetOneWayFares(dtInbound, false);
                            this.errorMessage = objFareRequest2.errorMessage;
                        }
                        else
                        {
                            GetCombineAvailability objR = new GetCombineAvailability();
                            objR.CombineTripDT(dtOutbound, dtInbound);
                            dtOutbound = objR.CombineDT;

                            GetApiAvailabilityFare objFareRequest = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                            objFareRequest.GetRTFares(dtOutbound, false);
                            this.errorMessage = objFareRequest.errorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Companyid, 0, "GetRT", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(Companyid, 0, "GetRT-air_spicejet", "AVAILABILITY", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);

            List<DataTable> _l = new List<DataTable>();
            _l.Add(dtOutbound);
            _l.Add(dtInbound);
            return (_l);
        }
    }
}

