using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetBookPriceItinerarySSR
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        public int TotalPrice;
        public int TotalService;

        public GetBookPriceItinerarySSR(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.UserName = UserName;
            this.Password = Password;
        }
        public string GetPriceItinerarySSRResponse(string SearchID, string CompanyID, int BookingRef, string TResponse, string TPassenger)
        {
            string PriceRequest = "";
            string PriceResponse = "";
            string FltType = string.Empty;

            try
            {
                TotalPrice = 0;
                TotalService = 0;

                DataTable dtBound = DBCommon.CommonFunction.StringToDataSet(TResponse).Tables[0];
                DataTable dtPassenger = DBCommon.CommonFunction.StringToDataSet(TPassenger).Tables[0];
                if (dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    FltType = dtBound.Rows[0]["FltType"].ToString();
                    PriceRequest = GetPriceWithSSRRequest(SearchID, CompanyID, BookingRef, dtBound.Copy(), dtPassenger.Copy());
                    CommonUapi cs = new CommonUapi();
                    PriceResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, PriceRequest, "AirService", "PriceWithSSR");
                    GetTotalPriceAfterSSR(PriceResponse);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetPriceItinerarySSRResponse", "GetBookPriceItinerarySSR", TResponse, SearchID, ex.Message);
            }

            DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, FltType, "", "PNR", PriceRequest, PriceResponse, TResponse + Environment.NewLine + TPassenger);
            DBCommon.Logger.WriteLogg(CompanyID, 0, "PNR-SSR_UAPI", "PNR", PriceRequest + Environment.NewLine + PriceResponse, "", SearchID);
            return PriceResponse;
        }
        private string GetPriceWithSSRRequest(string SearchID, string CompanyID, int BookingRef, DataTable dtSelect, DataTable dtPassenger)
        {
            string Request = "";
            try
            {
                string JSK1 = dtSelect.Select("FltType='" + "O" + "'").CopyToDataTable().Rows[0]["JourneySellKey"].ToString();
                string JSK2 = "";
                if (dtSelect.Select("FltType='" + "I" + "'").Length > 0)
                {
                    JSK2 = dtSelect.Select("FltType='" + "I" + "'").CopyToDataTable().Rows[0]["JourneySellKey"].ToString();
                }

                string BookingTravelerRef = dtSelect.Rows[0]["API_AirlineID"].ToString();
                bool Is6e = false;
                if (dtSelect.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    Is6e = true;
                }

                Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
                Request += @"<soapenv:Body>";
                Request += @"<air:AirPriceReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" CheckOBFees =""All"" FareRuleType=""long"" AuthorizedBy=""ZEALTRAVELS"" TraceId=" + "\"" + SearchID + "\"" + " TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

                //===========================================================================================================================================
                Request += @"<air:AirItinerary>";
                foreach (DataRow dr in dtSelect.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);

                    Request += @"<air:AirSegment Key=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " Group=" + "\"" + dr["Classes"].ToString() + "\"" + " Carrier=" + "\"" + dr["CarrierCode"].ToString() + "\"" + " FlightNumber=" + "\"" + dr["FlightNumber"].ToString() + "\"" + " Origin=" + "\"" + dr["DepartureStation"].ToString() + "\"" + " Destination=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + " DepartureTime=" + "\"" + dr["DepDate"].ToString() + "\"" + " ArrivalTime=" + "\"" + dr["ArrDate"].ToString() + "\"" + "  HostTokenRef=" + "\"" + HostToken + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " ClassOfService=" + "\"" + dr["ClassOfService"].ToString() + "\"" + ">";
                    Request += @"</air:AirSegment>";
                }
                foreach (DataRow dr in dtSelect.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);
                    Request += @"<com:HostToken Key=" + "\"" + HostToken + "\"" + ">" + HostToken_Text + "</com:HostToken>";
                }
                Request += @"</air:AirItinerary>";
                //===========================================================================================================================================
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (dr["MealCode_O"].ToString().Trim().Length > 2 || dr["BaggageCode_O"].ToString().Trim().Length > 2 || dr["MealCode_I"].ToString().Trim().Length > 2 || dr["BaggageCode_I"].ToString().Trim().Length > 2)
                    {
                        Request += @"<com:SearchPassenger Code=" + "\"" + dr["PaxType"].ToString() + "\"" + " Key=" + "\"" + CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString())) + "\"" + "/>";
                    }
                }
                //===========================================================================================================================================
                Request += @"<air:AirPricingCommand>";
                foreach (DataRow dr in dtSelect.Rows)
                {
                    Request += @"<air:AirSegmentPricingModifiers AirSegmentRef=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " FareBasisCode=" + "\"" + dr["FareBasisCode"].ToString() + "\"" + "/>";
                }
                Request += @"</air:AirPricingCommand>";
                //===========================================================================================================================================

                Request += @"<air:OptionalServices>";
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (dr["MealCode_O"].ToString().Trim().Length > 2)
                    {
                        string OptionalServicesRuleRef = "";
                        string OptionalServicesKey = GetBookFunctions.GetOptionalServices(dr["MealDetail_O"].ToString(), int.Parse(dr["RowID"].ToString()), BookingTravelerRef, out OptionalServicesRuleRef);
                        string Type = "MealOrBeverage";
                        string Code = dr["MealCode_O"].ToString();
                        string PassengerType = dr["PaxType"].ToString();

                        Request += @"<air:OptionalService Key=" + "\"" + OptionalServicesKey + "\"" + " Source =" + "\"" + "ACH" + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " SupplierCode=" + "\"" + "6E" + "\"" + " Type=" + "\"" + Type + "\"" + " Quantity =" + "\"" + "1" + "\"" + " ProviderDefinedType=" + "\"" + Code + "\"" + ">";
                        if (Is6e)
                        {
                            Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + JSK1 + "\"" + " BookingTravelerRef =" + "\"" + CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString())) + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                        }
                        else
                        {
                            Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + JSK1 + "\"" + " BookingTravelerRef =" + "\"" + dr["RowID"].ToString() + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                        }
                        Request += @"</air:OptionalService>";
                    }
                    if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                    {
                        string OptionalServicesRuleRef = "";
                        string OptionalServicesKey = GetBookFunctions.GetOptionalServices(dr["BaggageDetail_O"].ToString(), int.Parse(dr["RowID"].ToString()), BookingTravelerRef, out OptionalServicesRuleRef);
                        string Type = "Baggage";
                        string Code = dr["BaggageCode_O"].ToString();
                        string PassengerType = dr["PaxType"].ToString();

                        Request += @"<air:OptionalService Key=" + "\"" + OptionalServicesKey + "\"" + " Source =" + "\"" + "ACH" + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " SupplierCode=" + "\"" + "6E" + "\"" + " Type=" + "\"" + Type + "\"" + " Quantity =" + "\"" + "1" + "\"" + " ProviderDefinedType=" + "\"" + Code + "\"" + ">";

                        DataTable dtOutbound = dtSelect.Select("FltType='" + "O" + "'").CopyToDataTable();
                        foreach (DataRow drAirSegment in dtOutbound.Rows)
                        {
                            if (Is6e)
                            {
                                Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + drAirSegment["JourneySellKey"].ToString() + "\"" + " BookingTravelerRef =" + "\"" + CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString())) + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                            }
                            else
                            {
                                Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + drAirSegment["JourneySellKey"].ToString() + "\"" + " BookingTravelerRef =" + "\"" + dr["RowID"].ToString() + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                            }
                        }

                        Request += @"</air:OptionalService>";
                    }
                    //.==============================================================================================================================================
                    if (dr["MealCode_I"].ToString().Trim().Length > 2)
                    {
                        string OptionalServicesRuleRef = "";
                        string OptionalServicesKey = GetBookFunctions.GetOptionalServices(dr["MealDetail_I"].ToString(), int.Parse(dr["RowID"].ToString()), BookingTravelerRef, out OptionalServicesRuleRef);
                        string Type = "MealOrBeverage";
                        string Code = dr["MealCode_I"].ToString();
                        string PassengerType = dr["PaxType"].ToString();

                        Request += @"<air:OptionalService Key=" + "\"" + OptionalServicesKey + "\"" + " Source =" + "\"" + "ACH" + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " SupplierCode=" + "\"" + "6E" + "\"" + " Type=" + "\"" + Type + "\"" + " Quantity =" + "\"" + "1" + "\"" + " ProviderDefinedType=" + "\"" + Code + "\"" + ">";
                        if (Is6e)
                        {
                            Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + JSK2 + "\"" + " BookingTravelerRef =" + "\"" + CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString())) + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                        }
                        else
                        {
                            Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + JSK2 + "\"" + " BookingTravelerRef =" + "\"" + dr["RowID"].ToString() + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                        }
                        Request += @"</air:OptionalService>";
                    }
                    if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                    {
                        string OptionalServicesRuleRef = "";
                        string OptionalServicesKey = GetBookFunctions.GetOptionalServices(dr["BaggageDetail_I"].ToString(), int.Parse(dr["RowID"].ToString()), BookingTravelerRef, out OptionalServicesRuleRef);
                        string Type = "Baggage";
                        string Code = dr["BaggageCode_I"].ToString();
                        string PassengerType = dr["PaxType"].ToString();

                        Request += @"<air:OptionalService Key=" + "\"" + OptionalServicesKey + "\"" + " Source =" + "\"" + "ACH" + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " SupplierCode=" + "\"" + "6E" + "\"" + " Type=" + "\"" + Type + "\"" + " Quantity =" + "\"" + "1" + "\"" + " ProviderDefinedType=" + "\"" + Code + "\"" + ">";

                        DataTable dtInbound = dtSelect.Select("FltType='" + "I" + "'").CopyToDataTable();
                        foreach (DataRow drAirSegment in dtInbound.Rows)
                        {
                            if (Is6e)
                            {
                                Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + drAirSegment["JourneySellKey"].ToString() + "\"" + " BookingTravelerRef =" + "\"" + CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString())) + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                            }
                            else
                            {
                                Request += @"<com:ServiceData xmlns=""http://www.travelport.com/schema/common_v51_0"" AirSegmentRef = " + "\"" + drAirSegment["JourneySellKey"].ToString() + "\"" + " BookingTravelerRef =" + "\"" + dr["RowID"].ToString() + "\"" + " TravelerType = " + "\"" + PassengerType + "\"" + "/>";
                            }
                        }

                        Request += @"</air:OptionalService>";
                    }
                }
                Request += @"</air:OptionalServices>";

                //===========================================================================================================================================

                //Request += @"<com:FormOfPayment Type=""Cash""/>";
                Request += @"</air:AirPriceReq>";
                Request += @"</soapenv:Body>";
                Request += @"</soapenv:Envelope>";
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetPriceWithSSRRequest", "air_uapi", Request, SearchID, ex.Message);
            }
            return Request;
        }
        //Request += @"<OptionalService AssessIndicator=""MileageOrCurrency"" DisplayText=""Meal, Vegetarian"" 
        //Key=""0q6+Gu4R2BKAGV0pFAAAAA=="" Source =""ACH"" ApproximateTotalPrice =""GBP3.00"" ProviderCode=""ACH"" SupplierCode=""6E""
        //Type=""MealOrBeverage"" ServiceStatus=""Offered"" Quantity =""1"" ProviderDefinedType=""ML__VG"" TotalPrice =""GBP3.00"" BasePrice =""GBP3.00"">";

        private void GetTotalPriceAfterSSR(string PriceResponse)
        {
            TotalPrice = 0;
            TotalService = 0;

            XmlDocument xmlflt = new XmlDocument();
            xmlflt.LoadXml(PriceResponse);
            XmlElement root = xmlflt.DocumentElement;
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    string s12 = root.ChildNodes[i].InnerXml;

                    XmlDocument xmlflt1 = new XmlDocument();
                    xmlflt1.LoadXml(s12);
                    XmlElement root1 = xmlflt1.DocumentElement;
                    if (root1.HasChildNodes)
                    {
                        for (int j = 0; j < root1.ChildNodes.Count; j++)
                        {
                            string s123 = root1.ChildNodes[j].OuterXml;
                            if (s123.IndexOf("air:AirItinerary") != -1)
                            {

                            }
                            else if (s123.IndexOf("air:AirPriceResult") != -1)
                            {
                                XmlDocument xmlflt2 = new XmlDocument();
                                xmlflt2.LoadXml(s123);
                                XmlElement root2 = xmlflt2.DocumentElement;

                                if (root2.HasChildNodes)
                                {
                                    for (int k = 0; k < root2.ChildNodes.Count; k++)
                                    {
                                        string Nodes = root2.ChildNodes[k].OuterXml;

                                        XmlDocument xmlflt3 = new XmlDocument();
                                        xmlflt3.LoadXml(Nodes);
                                        XmlElement root3 = xmlflt3.DocumentElement;
                                        if (root3.HasChildNodes)
                                        {
                                            for (int n = 0; n < root3.ChildNodes.Count; n++)
                                            {
                                                string Nodes1 = root3.ChildNodes[n].OuterXml;
                                                if (Nodes1.IndexOf("<air:AirPricingInfo") != -1)
                                                {
                                                    DataSet ResponseDs = new DataSet();
                                                    ResponseDs.ReadXml(new System.IO.StringReader(Nodes1));
                                                    if (ResponseDs.Tables["AirPricingInfo"] != null)
                                                    {
                                                        if (ResponseDs.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString().IndexOf("INR") != -1)
                                                        {
                                                            TotalPrice = Decimal.ToInt32(Convert.ToDecimal(ResponseDs.Tables["AirPricingInfo"].Rows[0]["TotalPrice"].ToString().Replace("INR", "").ToString().Trim()));
                                                        }
                                                        else if (ResponseDs.Tables["AirPricingInfo"].Rows[0]["ApproximateTotalPrice"].ToString().IndexOf("INR") != -1)
                                                        {
                                                            TotalPrice = Decimal.ToInt32(Convert.ToDecimal(ResponseDs.Tables["AirPricingInfo"].Rows[0]["ApproximateTotalPrice"].ToString().Replace("INR", "").ToString().Trim()));
                                                        }

                                                        if (ResponseDs.Tables["AirPricingInfo"].Rows[0]["Services"].ToString().IndexOf("INR") != -1)
                                                        {
                                                            TotalService = Decimal.ToInt32(Convert.ToDecimal(ResponseDs.Tables["AirPricingInfo"].Rows[0]["Services"].ToString().Replace("INR", "").ToString().Trim()));
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}