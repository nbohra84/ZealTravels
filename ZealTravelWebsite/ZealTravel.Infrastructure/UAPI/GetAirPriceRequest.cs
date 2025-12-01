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

namespace ZealTravel.Infrastructure.UAPI
{
    class GetAirPriceRequest
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string SearchID;
        private string CompanyID;
        public GetAirPriceRequest(string NetworkUserName, string NetworkPassword, string TargetBranch, string SearchID, string CompanyID)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
        }
        public string GetPriceRequest_6E(DataTable dtSelect)
        {
            string Carrier = dtSelect.Rows[0]["CarrierCode"].ToString().Trim();
            int k1 = 0;
            int k2 = 0;

            string Request = "";
            Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soapenv:Body>";

            Request += @"<air:AirPriceReq xmlns:air=""http://www.travelport.com/schema/air_v50_0"" CheckOBFees=""All"" AuthorizedBy=""ZEALTRAVELS"" FareRuleType=""long""   TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
            Request += @"<com:BillingPointOfSaleInfo OriginApplication =""uAPI"" xmlns:com=""http://www.travelport.com/schema/common_v50_0""/>";

            Request += @"<air:AirItinerary>";
            foreach (DataRow dr in dtSelect.Rows)
            {
                string[] split = dr["BookingCode"].ToString().Split(',');
                string CodeshareInfoOperatingCarrier = dr["CodeshareInfoOperatingCarrier"].ToString();
                string CodeshareInfoOperatingFlightNumber = dr["CodeshareInfoOperatingFlightNumber"].ToString();
                string CodeshareInfo = dr["CodeshareInfo"].ToString();

                Request += @"<air:AirSegment Key=" + "\"" + dr["SegmentRef"].ToString() + "\"" + "";
                if(dr["OptionalServicesIndicator"].ToString().Length>0)
                {
                    Request += @" OptionalServicesIndicator =" + "\"" + dr["OptionalServicesIndicator"].ToString() + "\"" + "";
                }
                if (dr["AvailabilityDisplayType"].ToString().Length > 0)
                {
                    Request += @" AvailabilityDisplayType =" + "\"" + dr["AvailabilityDisplayType"].ToString() + "\"" + "";
                }
                if (dr["FlightNumber"].ToString().Length > 0)
                {
                    Request += @" FlightNumber =" + "\"" + dr["FlightNumber"].ToString() + "\"" + "";
                }
                if (dr["Group"].ToString().Length > 0)
                {
                    Request += @" Group=" + "\"" + dr["Group"].ToString() + "\"" + "";
                }
                if (dr["CarrierCode"].ToString().Length > 0)
                {
                    Request += @" Carrier=" + "\"" + dr["CarrierCode"].ToString() + "\"" + "";
                }
                if (dr["DepartureStation"].ToString().Length > 0)
                {
                    Request += @" Origin=" + "\"" + dr["DepartureStation"].ToString() + "\"" + "";
                }
                if (dr["ArrivalStation"].ToString().Length > 0)
                {
                    Request += @" Destination=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + "";
                }
                if (dr["DepDate"].ToString().Length > 0)
                {
                    Request += @" DepartureTime=" + "\"" + dr["DepDate"].ToString() + "\"" + "";
                }
                if (dr["ArrDate"].ToString().Length > 0)
                {
                    Request += @" ArrivalTime=" + "\"" + dr["ArrDate"].ToString() + "\"" + "";
                }
                if (dr["FlightTime"].ToString().Length > 0)
                {
                    Request += @" FlightTime=" + "\"" + dr["FlightTime"].ToString() + "\"" + "";
                }
                if (dr["TravelTime"].ToString().Length > 0)
                {
                    Request += @" TravelTime =" + "\"" + dr["TravelTime"].ToString() + "\"" + "";
                }
                if (dr["Distance"].ToString().Length > 0)
                {
                    Request += @" Distance=" + "\"" + dr["Distance"].ToString() + "\"" + "";
                }
                if (dr["ProviderCode"].ToString().Length > 0)
                {
                    Request += @" ProviderCode=" + "\"" + dr["ProviderCode"].ToString() + "\"" + "";
                }
                if (dr["ClassOfService"].ToString().Length > 0)
                {
                    Request += @" ClassOfService=" + "\"" + dr["ClassOfService"].ToString() + "\"" + "";
                }
                if (dr["AvailabilitySource"].ToString().Length > 0)
                {
                    Request += @" AvailabilitySource=" + "\"" + dr["AvailabilitySource"].ToString() + "\"" + "";
                }
                if (dr["ETicketability"].ToString().Length > 0)
                {
                    Request += @" ETicketability=" + "\"" + dr["ETicketability"].ToString() + "\"" + "";
                }
                if (dr["EquipmentType"].ToString().Length > 0)
                {
                    Request += @" Equipment=" + "\"" + dr["EquipmentType"].ToString() + "\"" + "";
                }
                if (dr["ChangeOfPlane"].ToString().Length > 0)
                {
                    Request += @" ChangeOfPlane=" + "\"" + dr["ChangeOfPlane"].ToString() + "\"" + "";
                }
                if (dr["LinkAvailability"].ToString().Length > 0)
                {
                    Request += @" LinkAvailability=" + "\"" + dr["LinkAvailability"].ToString() + "\"" + "";
                }
                if (dr["PolledAvailabilityOption"].ToString().Length > 0)
                {
                    Request += @" PolledAvailabilityOption =" + "\"" + dr["PolledAvailabilityOption"].ToString() + "\"" + "";
                }
                if (dr["ParticipantLevel"].ToString().Length > 0)
                {
                    Request += @" ParticipantLevel=" + "\"" + dr["ParticipantLevel"].ToString() + "\"" + "";
                }
                if (dr["AdtHTR"].ToString().Length > 0)
                {
                    Request += @" HostTokenRef=" + "\"" + dr["AdtHTR"].ToString() + "\"" + "";
                }
                if (dr["TempData1"].ToString().Length > 0)
                {
                    Request += @" APISRequirementsRef=" + "\"" + dr["TempData1"].ToString() + "\"" + "";
                }
                Request += @">";
                

                if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length > 0)
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length.Equals(0))
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + "></air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length.Equals(0))
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + "</air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length > 0)
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                }


                if (dr["FltType"].ToString().Equals("O"))
                {
                    if (k1.Equals(0))
                    {
                        k1++;
                        if (dr["Connection_O"].ToString().Length > 0)
                        {
                            string[] Conn = dr["Connection_O"].ToString().Split('?');
                            for (int i = 0; i < Conn.Length; i++)
                            {
                                Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                            }
                        }
                        else
                        {
                            Request += @"<air:Connection/>";
                        }
                    }
                }
                else
                {
                    if (k2.Equals(0))
                    {
                        k2++;
                        if (dr["Connection_I"].ToString().Length > 0)
                        {
                            string[] Conn = dr["Connection_I"].ToString().Split('?');
                            for (int i = 0; i < Conn.Length; i++)
                            {
                                Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                            }
                        }
                        else
                        {
                            Request += @"<air:Connection/>";
                        }
                    }
                }

                Request += @"</air:AirSegment>";
            }

            ArrayList ArHostTokenRef= new ArrayList();
            ArrayList ArHTR = DBCommon.CommonFunction.DataTable2ArrayList(dtSelect, "AdtHTR", false);
            string HostTokenRef = dtSelect.Rows[0]["HostTokenRef"].ToString().Trim();
            string[] SplitHostTokenRef = HostTokenRef.Split('?');
            for (int d = 0; d < SplitHostTokenRef.Length; d++)
            {
                string[] SplitKey = SplitHostTokenRef[d].ToString().Trim().Split('@');
                if (ArHTR.Contains(SplitKey[0].ToString().Trim()))
                {
                    ArHostTokenRef.Add(SplitHostTokenRef[d].ToString().Trim());
                }
            }

            for (int d = 0; d < ArHostTokenRef.Count; d++)
            {
                string[] SplitKey = ArHostTokenRef[d].ToString().Trim().Split('@');
                Request += @"<HostToken xmlns=""http://www.travelport.com/schema/common_v50_0""  Key=" + "\"" + SplitKey[0].ToString().Trim() + "\"" + ">" + SplitKey[1].ToString().Trim() + "</HostToken>";
            }
            Request += @"</air:AirItinerary>";

            if (Carrier.Equals("6E"))
            {
                Request += @"<air:AirPricingModifiers FaresIndicator= ""PublicFaresOnly"" AccountCodeFaresOnly = ""false"">";
                //Request += @"<air:PromoCodes>";
                //Request += @"<air:PromoCode Code =""CUITYCN"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                //Request += @"</air:PromoCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else
            {
                Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
            }


            //Request += @"<AirPricingModifiers InventoryRequestType=""DirectAccess"">";
            //Request += @"<BrandModifiers ModifierType=""FareFamilyDisplay""/>";
            //Request += @"</AirPricingModifiers>";

            //int passengerid = 1;
            int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
            int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
            int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());

            string AdtBTR = dtSelect.Rows[0]["AdtBTR"].ToString();
            string ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString();
            string InfBTR = dtSelect.Rows[0]["InfBTR"].ToString();

            for (int i = 0; i < Adt; i++)
            {
                string[] split = AdtBTR.Split('?');
                Request += @"<SearchPassenger Code=""ADT"" xmlns =""http://www.travelport.com/schema/common_v50_0"" BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";
                //Request = Request.Replace("AAA", passengerid.ToString()).Trim();
                //passengerid++;
            }
            for (int i = 0; i < Inf; i++)
            {
                string[] split = InfBTR.Split('?');
                Request += @"<SearchPassenger Code=""INF"" xmlns =""http://www.travelport.com/schema/common_v50_0"" BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";  // Age=" + "\"" + "1" + "\"" + "
                //Request = Request.Replace("III", passengerid.ToString()).Trim();
                //passengerid++;
            }
            for (int i = 0; i < Chd; i++)
            {
                string ChildCode = "CNN";
                if (dtSelect.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    ChildCode = "CHD";
                }

                string[] split = ChdBTR.Split('?');
                Request += @"<SearchPassenger  xmlns=""http://www.travelport.com/schema/common_v50_0""  Code=" + "\"" + ChildCode + "\"" + " BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";// Age=" + "\"" + "10" + "\"" + "
                //Request = Request.Replace("CCC", passengerid.ToString()).Trim();
                //passengerid++;
            }

            Request += @"<air:AirPricingCommand>";
            foreach (DataRow dr in dtSelect.Rows)
            {
                Request += @"<air:AirSegmentPricingModifiers AirSegmentRef=" + "\"" + dr["SegmentRef"].ToString() + "\"" + " FareBasisCode=" + "\"" + dr["FareBasisCode"].ToString() + "\"" + "/>";
            }
            Request += @"</air:AirPricingCommand>";

            Request += @"<FormOfPayment xmlns=""http://www.travelport.com/schema/common_v50_0"" Type=""Credit""/>";

            Request += @" </air:AirPriceReq>";

            Request += @"</soapenv:Body>";
            Request += @"</soapenv:Envelope>";

            return Request;
        }
        public string GetPriceRequest(DataTable dtSelect)
        {
            string Carrier = dtSelect.Rows[0]["CarrierCode"].ToString().Trim();
            string PriceType = dtSelect.Rows[0]["PriceType"].ToString().Trim();
            int k1 = 0;
            int k2 = 0;

            string Request = "";
            Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
            Request += @"<soapenv:Body>";

            Request += @"<air:AirPriceReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" CheckOBFees=""All"" AuthorizedBy=""ZEALTRAVELS"" FareRuleType=""long""   TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
            Request += @"<com:BillingPointOfSaleInfo OriginApplication =""uAPI"" xmlns:com=""http://www.travelport.com/schema/common_v51_0""/>";

            Request += @"<air:AirItinerary>";
            foreach (DataRow dr in dtSelect.Rows)
            {
                string[] split = dr["BookingCode"].ToString().Split(',');
                string CodeshareInfoOperatingCarrier = dr["CodeshareInfoOperatingCarrier"].ToString();
                string CodeshareInfoOperatingFlightNumber = dr["CodeshareInfoOperatingFlightNumber"].ToString();
                string CodeshareInfo = dr["CodeshareInfo"].ToString();

                Request += @"<air:AirSegment Key=" + "\"" + dr["SegmentRef"].ToString() + "\"" + "";

                if (dr["OptionalServicesIndicator"].ToString().Length > 0)
                {
                    Request += @" OptionalServicesIndicator =" + "\"" + dr["OptionalServicesIndicator"].ToString() + "\"" + "";
                }
                if (dr["AvailabilityDisplayType"].ToString().Length > 0)
                {
                    Request += @" AvailabilityDisplayType =" + "\"" + dr["AvailabilityDisplayType"].ToString() + "\"" + "";
                }
                if (dr["FlightNumber"].ToString().Length > 0)
                {
                    Request += @" FlightNumber =" + "\"" + dr["FlightNumber"].ToString() + "\"" + "";
                }
                if (dr["Group"].ToString().Length > 0)
                {
                    Request += @" Group=" + "\"" + dr["Group"].ToString() + "\"" + "";
                }
                if (dr["CarrierCode"].ToString().Length > 0)
                {
                    Request += @" Carrier=" + "\"" + dr["CarrierCode"].ToString() + "\"" + "";
                }
                if (dr["DepartureStation"].ToString().Length > 0)
                {
                    Request += @" Origin=" + "\"" + dr["DepartureStation"].ToString() + "\"" + "";
                }
                if (dr["ArrivalStation"].ToString().Length > 0)
                {
                    Request += @" Destination=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + "";
                }
                if (dr["DepDate"].ToString().Length > 0)
                {
                    Request += @" DepartureTime=" + "\"" + dr["DepDate"].ToString() + "\"" + "";
                }
                if (dr["ArrDate"].ToString().Length > 0)
                {
                    Request += @" ArrivalTime=" + "\"" + dr["ArrDate"].ToString() + "\"" + "";
                }
                if (dr["FlightTime"].ToString().Length > 0)
                {
                    Request += @" FlightTime=" + "\"" + dr["FlightTime"].ToString() + "\"" + "";
                }
                if (dr["TravelTime"].ToString().Length > 0)
                {
                    Request += @" TravelTime =" + "\"" + dr["TravelTime"].ToString() + "\"" + "";
                }
                if (dr["Distance"].ToString().Length > 0)
                {
                    Request += @" Distance=" + "\"" + dr["Distance"].ToString() + "\"" + "";
                }
                if (dr["ProviderCode"].ToString().Length > 0)
                {
                    Request += @" ProviderCode=" + "\"" + dr["ProviderCode"].ToString() + "\"" + "";
                }
                if (dr["ClassOfService"].ToString().Length > 0)
                {
                    Request += @" ClassOfService=" + "\"" + dr["ClassOfService"].ToString() + "\"" + "";
                }
                if (dr["AvailabilitySource"].ToString().Length > 0)
                {
                    Request += @" AvailabilitySource=" + "\"" + dr["AvailabilitySource"].ToString() + "\"" + "";
                }
                if (dr["ETicketability"].ToString().Length > 0)
                {
                    Request += @" ETicketability=" + "\"" + dr["ETicketability"].ToString() + "\"" + "";
                }
                if (dr["EquipmentType"].ToString().Length > 0)
                {
                    Request += @" Equipment=" + "\"" + dr["EquipmentType"].ToString() + "\"" + "";
                }
                if (dr["ChangeOfPlane"].ToString().Length > 0)
                {
                    Request += @" ChangeOfPlane=" + "\"" + dr["ChangeOfPlane"].ToString() + "\"" + "";
                }
                if (dr["LinkAvailability"].ToString().Length > 0)
                {
                    Request += @" LinkAvailability=" + "\"" + dr["LinkAvailability"].ToString() + "\"" + "";
                }
                if (dr["PolledAvailabilityOption"].ToString().Length > 0)
                {
                    Request += @" PolledAvailabilityOption =" + "\"" + dr["PolledAvailabilityOption"].ToString() + "\"" + "";
                }
                if (dr["ParticipantLevel"].ToString().Length > 0)
                {
                    Request += @" ParticipantLevel=" + "\"" + dr["ParticipantLevel"].ToString() + "\"" + "";
                }
                if (dr["AdtHTR"].ToString().Length > 0)
                {
                    Request += @" HostTokenRef=" + "\"" + dr["AdtHTR"].ToString() + "\"" + "";
                }
                if (dr["TempData1"].ToString().Length > 0)
                {
                    Request += @" APISRequirementsRef=" + "\"" + dr["TempData1"].ToString() + "\"" + "";
                }
                Request += @">";


                if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length > 0)
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length > 0 && CodeshareInfo.Length.Equals(0))
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + " OperatingFlightNumber=" + "\"" + CodeshareInfoOperatingFlightNumber + "\"" + "></air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length.Equals(0))
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + "</air:CodeshareInfo>";
                }
                else if (CodeshareInfoOperatingCarrier.Length > 0 && CodeshareInfoOperatingFlightNumber.Length.Equals(0) && CodeshareInfo.Length > 0)
                {
                    Request += @"<air:CodeshareInfo OperatingCarrier=" + "\"" + CodeshareInfoOperatingCarrier + "\"" + ">" + CodeshareInfo + "</air:CodeshareInfo>";
                }


                if (dr["FltType"].ToString().Equals("O"))
                {
                    if (k1.Equals(0))
                    {
                        k1++;
                        if (dr["Connection_O"].ToString().Length > 0)
                        {
                            string[] Conn = dr["Connection_O"].ToString().Split('?');
                            for (int i = 0; i < Conn.Length; i++)
                            {
                                Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                            }
                        }
                        else
                        {
                            Request += @"<air:Connection/>";
                        }
                    }
                }
                else
                {
                    if (k2.Equals(0))
                    {
                        k2++;
                        if (dr["Connection_I"].ToString().Length > 0)
                        {
                            string[] Conn = dr["Connection_I"].ToString().Split('?');
                            for (int i = 0; i < Conn.Length; i++)
                            {
                                Request += @"<air:Connection SegmentIndex=" + "\"" + Conn[i].ToString() + "\"" + "/>";
                            }
                        }
                        else
                        {
                            Request += @"<air:Connection/>";
                        }
                    }
                }

                Request += @"</air:AirSegment>";
            }
            Request += @"</air:AirItinerary>";


            CommonUapi objuapi = new CommonUapi();
            if (objuapi.GetAccountCodeUK().Length > 0 && Carrier.Equals("UK"))
            {
                Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""false"">";
                Request += @"<air:AccountCodes>";
                Request += @"<com:AccountCode xmlns:com =""http://www.travelport.com/schema/common_v51_0"" Code=" + "\"" + objuapi.GetAccountCodeUK() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "UK" + "\"" + " />";
                Request += @"</air:AccountCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else if (objuapi.GetAccountCodeAI().Length > 0 && Carrier.Equals("AI") && PriceType.ToLower().Contains("sme"))
            {
                Request += @"<air:AirPricingModifiers AccountCodeFaresOnly=""true"">";
                Request += @"<air:AccountCodes>";
                Request += @"<com:AccountCode xmlns:com =""http://www.travelport.com/schema/common_v51_0"" Code=" + "\"" + objuapi.GetAccountCodeAI() + "\"" + " ProviderCode=" + "\"" + "1G" + "\"" + " SupplierCode=" + "\"" + "AI" + "\"" + " />";
                Request += @"</air:AccountCodes>";
                Request += @"</air:AirPricingModifiers>";
            }
            else
            {
                Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
            }

            //if(Carrier.Equals("6E"))
            //{
            //    Request += @"<air:AirPricingModifiers FaresIndicator= ""PublicFaresOnly"" AccountCodeFaresOnly = ""false"">";
            //    //Request += @"<air:PromoCodes>";
            //    //Request += @"<air:PromoCode Code =""CUITYCN"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
            //    //Request += @"</air:PromoCodes>";
            //    Request += @"</air:AirPricingModifiers>";
            //}
            //else
            //{
            //    Request += @"<air:AirPricingModifiers FaresIndicator=""AllFares"" ETicketability=""Yes"" />";
            //}


            //Request += @"<AirPricingModifiers InventoryRequestType=""DirectAccess"">";
            //Request += @"<BrandModifiers ModifierType=""FareFamilyDisplay""/>";
            //Request += @"</AirPricingModifiers>";

            //int passengerid = 1;
            int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
            int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
            int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());

            string AdtBTR = dtSelect.Rows[0]["AdtBTR"].ToString();
            string ChdBTR = dtSelect.Rows[0]["ChdBTR"].ToString();
            string InfBTR = dtSelect.Rows[0]["InfBTR"].ToString();

            for (int i = 0; i < Adt; i++)
            {
                string[] split = AdtBTR.Split('?');
                Request += @"<com:SearchPassenger Code=""ADT"" xmlns:com =""http://www.travelport.com/schema/common_v51_0"" BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";
                //Request = Request.Replace("AAA", passengerid.ToString()).Trim();
                //passengerid++;
            }
            for (int i = 0; i < Inf; i++)
            {
                string[] split = InfBTR.Split('?');
                Request += @"<com:SearchPassenger Code=""INF"" PricePTCOnly=""true"" xmlns:com =""http://www.travelport.com/schema/common_v51_0"" BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";// Age=" + "\"" + "1" + "\"" + "
                //Request = Request.Replace("III", passengerid.ToString()).Trim();
                //passengerid++;
            }
            for (int i = 0; i < Chd; i++)
            {
                string ChildCode = "CNN";
                if(dtSelect.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    ChildCode = "CHD";
                }

                string[] split = ChdBTR.Split('?');
                Request += @"<com:SearchPassenger  xmlns:com =""http://www.travelport.com/schema/common_v51_0""  Code=" + "\"" + ChildCode + "\"" + " BookingTravelerRef=" + "\"" + split[i].ToString().Trim() + "\"" + "/>";// Age=" + "\"" + "10" + "\"" + "
                //Request = Request.Replace("CCC", passengerid.ToString()).Trim();
                //passengerid++;
            }


            Request += @"<air:AirPricingCommand/>";
            Request += @"</air:AirPriceReq>";

            Request += @"</soapenv:Body>";
            Request += @"</soapenv:Envelope>";

            return Request;
        }
    }
}
