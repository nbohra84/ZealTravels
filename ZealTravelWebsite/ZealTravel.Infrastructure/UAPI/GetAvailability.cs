using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;
using iText.Commons.Utils;

namespace ZealTravel.Infrastructure.UAPI
{
    public class GetAvailability
    {
        private string TargetBranch;
        public GetAvailability(string TargetBranch)
        {
            this.TargetBranch = TargetBranch;
        }
        private string RandomString(int size, bool lowerCase = false)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        public string GetAvailabilityRequestMC(string SearchID, string TRequest, string Sector, ref string AdultBTR, ref string ChildBTR, ref string InfantBTR)
        {

            string btr = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 10)));

            DataSet dsRequest = new DataSet();
            dsRequest.ReadXml(new System.IO.StringReader(TRequest));

            int Adt = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
            int Chd = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
            int Inf = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
            string Cabin = dsRequest.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();
  
            ArrayList AirlineList = new ArrayList();
            foreach (DataRow dr in dsRequest.Tables["AirVInfo"].Rows)
            {
                AirlineList.Add(dr["AirV"].ToString());
            }

            string Request = string.Empty;

            Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soapenv:Header/>";
            Request += @"<soapenv:Body>";

            Request += @"<air:LowFareSearchReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" AuthorizedBy=""ZEALTRAVELS"" SolutionResult=""false"" xmlns:com =""http://www.travelport.com/schema/common_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
            Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

            //===================================================================================================
            foreach (DataRow dr in dsRequest.Tables["AirSrchInfo"].Rows)
            {
                Request += @"<air:SearchAirLeg>";

                Request += @"<air:SearchOrigin>";
                Request += @"<com:CityOrAirport Code=" + "\"" + dr["DepartureStation"].ToString() + "\"" + "/>";
                Request += @"</air:SearchOrigin>";
                Request += @"<air:SearchDestination>";
                Request += @"<com:CityOrAirport Code=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + "/>";
                Request += @"</air:SearchDestination>";
                Request += @"<air:SearchDepTime PreferredTime=" + "\"" + GetAvailabilityFunctions.GetFilterDate(dr["StartDate"].ToString()) + "\"" + "></air:SearchDepTime>";

                Request += @"<air:AirLegModifiers>";

                //if (AirlineList != null && AirlineList.Contains("GDS"))
                //{
                //    Request += @"<air:PreferredCabins>";
                //    if (Cabin.Equals("Y"))
                //    {
                //        Request += @"<com:CabinClass Type=""Economy""/>";
                //    }
                //    else if (Cabin.Equals("A"))
                //    {
                //        Request += @"<com:CabinClass Type=""PremiumEconomy""/>";
                //    }
                //    else if (Cabin.Equals("C"))
                //    {
                //        Request += @"<com:CabinClass Type=""Business""/>";
                //    }
                //    else if (Cabin.Equals("B"))
                //    {
                //        Request += @"<com:CabinClass Type=""PremiumBusiness""/>";
                //    }
                //    Request += @"</air:PreferredCabins>";
                //}

                //Request += @"<air:PermittedCarriers>";
                //Request += @"<com:Carrier Code=""AI""/>";
                //Request += @"<com:Carrier Code=""AI""/>";
                //Request += @"</air:PermittedCarriers>";

                Request += @"</air:AirLegModifiers>";
                Request += @"</air:SearchAirLeg>";

                //Request = Request.Replace("SSS", dr["DepartureStation"].ToString()).Trim();
                //Request = Request.Replace("DDD", dr["ArrivalStation"].ToString()).Trim();
                //Request = Request.Replace("YYYY-MM-DD", GetFilterDate(dr["StartDate"].ToString())).Trim();
            }
            //==============================================================================================

            Request += @"<air:AirSearchModifiers IncludeFlightDetails=""true"">";
            Request += @"<air:PreferredProviders>";

            string childName = "CHD";
            if (AirlineList != null && AirlineList.Contains("GDS"))
            {
                childName = "CNN";
                Request += @"<com:Provider Code=""1G""/>";
            }

            if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<com:Provider Code=""ACH""/>";
            }

            Request += @"</air:PreferredProviders>";
            Request += @"</air:AirSearchModifiers>";
            //==============================================================================================

            for (int i = 0; i < Adt; i++)
            {
                string AdtBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                if (AdtBTR.Length < 24)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR += RandomString(len, false);
                }
                else if (AdtBTR.Length > 26)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR = AdtBTR.Substring(0, 24);
                }

                Request += @"<com:SearchPassenger Code=""ADT"" BookingTravelerRef=" + "\"" + AdtBTR + "\"" + "/>";
                AdultBTR += AdtBTR + "?";
            }
            AdultBTR = AdultBTR.Substring(0, AdultBTR.Length - 1);

            if (Inf > 0)
            {
                for (int i = 0; i < Inf; i++)
                {
                    string InfBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (InfBTR.Length < 24)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR += RandomString(len, false);
                    }
                    else if (InfBTR.Length > 26)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR = InfBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=""INF"" PricePTCOnly=""true"" BookingTravelerRef=" + "\"" + InfBTR + "\"" + "/>";// Age=" + "\"" + "1" + "\"" + "
                    InfantBTR += InfBTR + "?";
                }
                InfantBTR = InfantBTR.Substring(0, InfantBTR.Length - 1);
            }

            if (Chd > 0)
            {
                for (int i = 0; i < Chd; i++)
                {
                    string ChdBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (ChdBTR.Length < 24)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR += RandomString(len, false);
                    }
                    else if (ChdBTR.Length > 26)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR = ChdBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=" + "\"" + childName + "\"" + " BookingTravelerRef=" + "\"" + ChdBTR + "\"" + "/>";//" Age = " + "\"" + "10" + "\"" + "
                    ChildBTR += ChdBTR + "?";
                }
                ChildBTR = ChildBTR.Substring(0, ChildBTR.Length - 1);
            }

            if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<air:AirPricingModifiers FaresIndicator= ""PublicFaresOnly"" AccountCodeFaresOnly = ""false"">";
                Request += @"<air:PromoCodes>";
                Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                Request += @"</air:PromoCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else
            {
                Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
            }

            Request += @"</air:LowFareSearchReq>";
            Request += @"</soapenv:Body>";
            Request += @"</soapenv:Envelope>";
            return Request;
        }
        public string GetAvailabilityRequestRT(string SearchID, string TRequest, string Sector, ref string AdultBTR, ref string ChildBTR, ref string InfantBTR, bool smeRatedOnly = false)
        {
            // string btr = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 10)));
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(TRequest);

            string DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
            string ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
            string BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
            string EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
            int Adt = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
            int Chd = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
            int Inf = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
            string Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;

            ArrayList AirlineList = new ArrayList();
            XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
            foreach (XmlNode no in nodeList)
            {
                AirlineList.Add(no.FirstChild.InnerText);
            }

            string Request = string.Empty;
            Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soapenv:Header/>";
            Request += @"<soapenv:Body>";

            Request += @"<air:LowFareSearchReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" AuthorizedBy=""ZEALTRAVELS"" SolutionResult=""false"" xmlns:com =""http://www.travelport.com/schema/common_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
            Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

            //===================================================================================================
            Request += @"<air:SearchAirLeg>";

            Request += @"<air:SearchOrigin>";
            Request += @"<com:CityOrAirport Code=" + "\"" + DepartureStation + "\"" + "/>";
            Request += @"</air:SearchOrigin>";
            Request += @"<air:SearchDestination>";
            Request += @"<com:CityOrAirport Code=" + "\"" + ArrivalStation + "\"" + "/>";
            Request += @"</air:SearchDestination>";
            Request += @"<air:SearchDepTime PreferredTime=" + "\"" + GetAvailabilityFunctions.GetFilterDate(BeginDate) + "\"" + "></air:SearchDepTime>";

            Request += @"<air:AirLegModifiers>";

            //if (AirlineList != null && AirlineList.Contains("GDS"))
            //{
            //    Request += @"<air:PreferredCabins>";
            //    if (Cabin.Equals("Y"))
            //    {
            //        Request += @"<com:CabinClass Type=""Economy""/>";
            //    }
            //    else if (Cabin.Equals("A"))
            //    {
            //        Request += @"<com:CabinClass Type=""PremiumEconomy""/>";
            //    }
            //    else if (Cabin.Equals("C"))
            //    {
            //        Request += @"<com:CabinClass Type=""Business""/>";
            //    }
            //    else if (Cabin.Equals("B"))
            //    {
            //        Request += @"<com:CabinClass Type=""PremiumBusiness""/>";
            //    }
            //    Request += @"</air:PreferredCabins>";
            //}

            //Request += @"<air:PermittedCarriers>";
            //Request += @"<com:Carrier Code=""UK""/>";
            //Request += @"<com:Carrier Code=""AI""/>";
            //Request += @"</air:PermittedCarriers>";

            Request += @"</air:AirLegModifiers>";

            Request += @"</air:SearchAirLeg>";

            //===================================================================================================
            Request += @"<air:SearchAirLeg>";

            Request += @"<air:SearchOrigin>";
            Request += @"<com:CityOrAirport Code=" + "\"" + ArrivalStation + "\"" + "/>";
            Request += @"</air:SearchOrigin>";
            Request += @"<air:SearchDestination>";
            Request += @"<com:CityOrAirport Code=" + "\"" + DepartureStation + "\"" + "/>";
            Request += @"</air:SearchDestination>";
            Request += @"<air:SearchDepTime PreferredTime=" + "\"" + GetAvailabilityFunctions.GetFilterDate(EndDate) + "\"" + "></air:SearchDepTime>";

            Request += @"<air:AirLegModifiers>";

            Request += @"<air:PreferredCabins>";
            if (Cabin.Equals("Y"))
            {
                Request += @"<com:CabinClass Type=""Economy""/>";
            }
            else if (Cabin.Equals("A"))
            {
                Request += @"<com:CabinClass Type=""PremiumEconomy""/>";
            }
            else if (Cabin.Equals("C"))
            {
                Request += @"<com:CabinClass Type=""Business""/>";
            }
            else if (Cabin.Equals("B"))
            {
                Request += @"<com:CabinClass Type=""PremiumBusiness""/>";
            }
            Request += @"</air:PreferredCabins>";

            //Request += @"<air:PermittedCarriers>";
            //Request += @"<com:Carrier Code=""UK""/>";
            //Request += @"<com:Carrier Code=""AI""/>";
            //Request += @"</air:PermittedCarriers>";


            Request += @"</air:AirLegModifiers>";

            Request += @"</air:SearchAirLeg>";
            //==============================================================================================

            Request += @"<air:AirSearchModifiers IncludeFlightDetails=""true"">";
            Request += @"<air:PreferredProviders>";

            string childName = "CHD";
            if (AirlineList != null && AirlineList.Contains("GDS"))
            {
                childName = "CNN";
                Request += @"<com:Provider Code=""1G""/>";
            }
            else if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<com:Provider Code=""ACH""/>";
            }

            Request += @"</air:PreferredProviders>";
            Request += @"</air:AirSearchModifiers>";

            for (int i = 0; i < Adt; i++)
            {
                string AdtBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                if (AdtBTR.Length < 24)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR += RandomString(len, false);
                }
                else if (AdtBTR.Length > 26)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR = AdtBTR.Substring(0, 24);
                }

                Request += @"<com:SearchPassenger Code=""ADT"" BookingTravelerRef=" + "\"" + AdtBTR + "\"" + "/>";
                AdultBTR += AdtBTR + "?";
            }
            AdultBTR = AdultBTR.Substring(0, AdultBTR.Length - 1);

            if (Inf > 0)
            {
                for (int i = 0; i < Inf; i++)
                {
                    string InfBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (InfBTR.Length < 24)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR += RandomString(len, false);
                    }
                    else if (InfBTR.Length > 26)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR = InfBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=""INF"" PricePTCOnly=""true"" BookingTravelerRef=" + "\"" + InfBTR + "\"" + "/>";
                    InfantBTR += InfBTR + "?";
                }
                InfantBTR = InfantBTR.Substring(0, InfantBTR.Length - 1);
            }

            if (Chd > 0)
            {
                for (int i = 0; i < Chd; i++)
                {
                    string ChdBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (ChdBTR.Length < 24)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR += RandomString(len, false);
                    }
                    else if (ChdBTR.Length > 26)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR = ChdBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=" + "\"" + childName + "\"" + " BookingTravelerRef=" + "\"" + ChdBTR + "\"" + "/>";
                    ChildBTR += ChdBTR + "?";
                }
                ChildBTR = ChildBTR.Substring(0, ChildBTR.Length - 1);
            }

            if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<air:AirPricingModifiers FaresIndicator= ""PublicFaresOnly"" AccountCodeFaresOnly = ""false"">";
                Request += @"<air:PromoCodes>";
                Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                Request += @"</air:PromoCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else
            {
               // commented on 15Jan2024
               CommonUapi objuapi = new CommonUapi();
                if (objuapi.GetAccountCodeUK().Length > 0)
                {
                    Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""false"">";
                    Request += @"<air:AccountCodes>";
                    Request += @"<com:AccountCode Code=" + "\"" + objuapi.GetAccountCodeUK() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "UK" + "\"" + " />";
                    Request += @"</air:AccountCodes>";
                    Request += @"</air:AirPricingModifiers>";
                }
                else if (objuapi.GetAccountCodeAI().Length > 0 && smeRatedOnly)
                {
                    Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""true"">";
                    Request += @"<air:AccountCodes>";
                    Request += @"<com:AccountCode Code=" + "\"" + objuapi.GetAccountCodeAI() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "AI" + "\"" + " />";
                    Request += @"</air:AccountCodes>";
                    Request += @"</air:AirPricingModifiers>";
                }
                else
                {
                    Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
                }
            //  commented on 15Jan2024
            //Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
            }

            Request += @"</air:LowFareSearchReq>";
            Request += @"</soapenv:Body>";
            Request += @"</soapenv:Envelope>";
            return Request;
        }
        public string GetAvailabilityRequest(string SearchID, string TRequest, string FltType, string Sector, ref string AdultBTR, ref string ChildBTR, ref string InfantBTR, bool smeRatesOnly = false)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(TRequest);

            string DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
            string ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
            string BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
            string EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
            int Adt = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
            int Chd = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
            int Inf = int.Parse(xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
            string Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;
            
            ArrayList AirlineList = new ArrayList();
            XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
            foreach (XmlNode no in nodeList)
            {
                AirlineList.Add(no.FirstChild.InnerText);
            }

            string Request = string.Empty;
            Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soapenv:Header/>";
            Request += @"<soapenv:Body>";

            Request += @"<air:LowFareSearchReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" AuthorizedBy=""ZEALTRAVELS"" SolutionResult=""false"" xmlns:com =""http://www.travelport.com/schema/common_v51_0""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
            Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

            Request += @"<air:SearchAirLeg>";
            Request += @"<air:SearchOrigin>";

            if (FltType.Equals("O"))
            {
                Request += @"<com:CityOrAirport Code=" + "\"" + DepartureStation + "\"" + "/>";
            }
            else
            {
                Request += @"<com:CityOrAirport Code=" + "\"" + ArrivalStation + "\"" + "/>";
            }

            Request += @"</air:SearchOrigin>";
            Request += @"<air:SearchDestination>";

            if (FltType.Equals("O"))
            {
                Request += @"<com:CityOrAirport Code=" + "\"" + ArrivalStation + "\"" + "/>";
            }
            else
            {
                Request += @"<com:CityOrAirport Code=" + "\"" + DepartureStation + "\"" + "/>";
            }

            Request += @"</air:SearchDestination>";

            if (FltType.Equals("O"))
            {
                Request += @"<air:SearchDepTime PreferredTime=" + "\"" + GetAvailabilityFunctions.GetFilterDate(BeginDate) + "\"" + "></air:SearchDepTime>";
            }
            else
            {
                Request += @"<air:SearchDepTime PreferredTime=" + "\"" + GetAvailabilityFunctions.GetFilterDate(EndDate) + "\"" + "></air:SearchDepTime>";
            }

            Request += @"<air:AirLegModifiers>";


            if (AirlineList != null && AirlineList.Contains("GDS"))
            {
                Request += @"<air:PreferredCabins>";
                if (Cabin.Equals("Y"))
                {
                    Request += @"<com:CabinClass Type=""Economy""/>";
                }
                else if (Cabin.Equals("A"))
                {
                    Request += @"<com:CabinClass Type=""PremiumEconomy""/>";
                }
                else if (Cabin.Equals("C"))
                {
                    Request += @"<com:CabinClass Type=""Business""/>";
                }
                else if (Cabin.Equals("B"))
                {
                    Request += @"<com:CabinClass Type=""PremiumBusiness""/>";
                }
                Request += @"</air:PreferredCabins>";
            }

            //if (AirlineList != null && AirlineList.Contains("GDS"))
            //{
            //    Request += @"<air:PermittedCarriers>";
            //    Request += @"<com:Carrier Code=""UK""/>";
            //    Request += @"<com:Carrier Code=""AI""/>";
            //    Request += @"</air:PermittedCarriers>";
            //}

            if (Sector.Equals("D"))
            {
                Request += @"<air:ProhibitedCarriers>";
                Request += @"<com:Carrier Code=""H1""/>";
                Request += @"</air:ProhibitedCarriers>";
            }

            Request += @"</air:AirLegModifiers>";
            Request += @"</air:SearchAirLeg>";

            Request += @"<air:AirSearchModifiers IncludeFlightDetails=""true"">";
            Request += @"<air:PreferredProviders>";

            string childName = "CHD";
            if (AirlineList != null && AirlineList.Contains("GDS"))
            {
                childName = "CNN";
                Request += @"<com:Provider Code=""1G""/>";
            }
           else if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<com:Provider Code=""ACH""/>";
            }
            Request += @"</air:PreferredProviders>";
            Request += @"</air:AirSearchModifiers>";

            for (int i = 0; i < Adt; i++)
            {
                string AdtBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                if (AdtBTR.Length < 24)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR += RandomString(len, false);
                }
                else if (AdtBTR.Length > 26)
                {
                    int len = 26 - AdtBTR.Length;
                    AdtBTR = AdtBTR.Substring(0, 24);
                }

                Request += @"<com:SearchPassenger Code=""ADT"" BookingTravelerRef=" + "\"" + AdtBTR + "\"" + "/>";
                AdultBTR += AdtBTR + "?";
            }
            AdultBTR = AdultBTR.Substring(0, AdultBTR.Length - 1);

            if (Inf > 0)
            {
                for (int i = 0; i < Inf; i++)
                {
                    string InfBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (InfBTR.Length < 24)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR += RandomString(len, false);
                    }
                    else if (InfBTR.Length > 26)
                    {
                        int len = 26 - InfBTR.Length;
                        InfBTR = InfBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=""INF"" PricePTCOnly=""true"" BookingTravelerRef=" + "\"" + InfBTR + "\"" + "/>";
                    InfantBTR += InfBTR + "?";
                }
                InfantBTR = InfantBTR.Substring(0, InfantBTR.Length - 1);
            }

            if (Chd > 0)
            {
                for (int i = 0; i < Chd; i++)
                {
                    string ChdBTR = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString("N").Substring(0, 16)));
                    if (ChdBTR.Length < 24)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR += RandomString(len, false);
                    }
                    else if (ChdBTR.Length > 26)
                    {
                        int len = 26 - ChdBTR.Length;
                        ChdBTR = ChdBTR.Substring(0, 24);
                    }

                    Request += @"<com:SearchPassenger Code=" + "\"" + childName + "\"" + " BookingTravelerRef=" + "\"" + ChdBTR + "\"" + "/>";
                    ChildBTR += ChdBTR + "?";
                }
                ChildBTR = ChildBTR.Substring(0, ChildBTR.Length - 1);
            }

            //Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" CurrencyType=""INR"" ETicketability=""Yes""/>";

            if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Request += @"<air:AirPricingModifiers FaresIndicator= ""PublicFaresOnly"" AccountCodeFaresOnly = ""false"">";
                Request += @"<air:PromoCodes>";
                Request += @"<air:PromoCode Code =""CUITYCS"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                Request += @"</air:PromoCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else
            {
               // commented on 15Jan2024
              CommonUapi objuapi = new CommonUapi();
                if (objuapi.GetAccountCodeUK().Length > 0)
                {
                    Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""false"">";
                    Request += @"<air:AccountCodes>";
                    Request += @"<com:AccountCode Code=" + "\"" + objuapi.GetAccountCodeUK() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "UK" + "\"" + " />";
                    Request += @"</air:AccountCodes>";
                    Request += @"</air:AirPricingModifiers>";
                }
                else if (objuapi.GetAccountCodeAI().Length > 0 && smeRatesOnly)
                {
                    Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""true"">";
                    Request += @"<air:AccountCodes>";
                    Request += @"<com:AccountCode Code=" + "\"" + objuapi.GetAccountCodeAI() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "AI" + "\"" + " />";
                    Request += @"</air:AccountCodes>";
                    Request += @"</air:AirPricingModifiers>";
                }
                else
                {
                    Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
                }
           // }
            //  commented on 15Jan2024
            //Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" AccountCodeFaresOnly=""false""  ETicketability=""Yes"">";
            //Request += @"<air:AccountCodes>";
            //Request += @"<com:AccountCode Code = ""SME"" />";
            //Request += @"</air:AccountCodes>";
            //Request += @"</air:AirPricingModifiers>";
            //   Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
        }

        //Request += @"<air:PenaltyFareInformation ProhibitPenaltyFares=""true""/>";
        //Request += @"</air:AirPricingModifiers>";

        Request += @"</air:LowFareSearchReq>";
            Request += @"</soapenv:Body>";
            Request += @"</soapenv:Envelope>";
            return Request;
        }
    }
}
