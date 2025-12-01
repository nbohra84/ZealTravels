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
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetPriceItinerary
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;

        private string SearchID;
        private string CompanyID;
        private Int32 BookingRef;
        public DataTable dtSSRItinerary;
        IRR_Layer _rr_Layer;
        public GetPriceItinerary(string NetworkUserName, string NetworkPassword, string TargetBranch, string SearchID, string CompanyID, int BookingRef)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;


            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
            _rr_Layer = new rr_Layer();
        }
        public DataTable GetPriceItineraryResponse(DataTable dtBound)
        {
            string PriceRequest = "";
            string PriceResponse = "";
            string SearchCriteria = "";
            try
            {
                string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
                string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();
                string BeginDate = dtBound.Rows[0]["DepDate"].ToString().Trim();
                string EndDate = dtBound.Rows[dtBound.Rows.Count - 1]["DepDate"].ToString().Trim().ToUpper();

                short iAdt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
                short iChd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
                short iInf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());

                string Trip = dtBound.Rows[0]["Trip"].ToString();
                string FltType = dtBound.Rows[0]["FltType"].ToString();
                string AirlineID = dtBound.Rows[0]["AirlineID"].ToString();

                if (Trip.Equals("O"))
                {
                    SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, iAdt, iChd, iInf);
                }
                else
                {
                    SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, EndDate, iAdt, iChd, iInf);
                }

                if (dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                {
                    GetAirPriceRequest objPrcRQ = new GetAirPriceRequest(NetworkUserName, NetworkPassword, TargetBranch, SearchID, CompanyID);
                    PriceRequest = objPrcRQ.GetPriceRequest_6E(dtBound);
                }
                else
                {
                    GetAirPriceRequest objPrcRQ = new GetAirPriceRequest(NetworkUserName, NetworkPassword, TargetBranch,SearchID,CompanyID);
                    PriceRequest = objPrcRQ.GetPriceRequest(dtBound);
                }

                CommonUapi cs = new CommonUapi();
                PriceResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, PriceRequest, "AirService", "Price");
                if (PriceResponse.IndexOf("AirPriceRsp") != -1 && PriceResponse.IndexOf("AirSegment") != -1)
                {
                    if (dtBound.Rows[0]["CarrierCode"].ToString().Equals("6E"))
                    {
                        GetPriceFilter_LCC objfltr = new GetPriceFilter_LCC();
                        objfltr.SetPriceUpdate(SearchID, CompanyID, dtBound, PriceResponse);
                        dtSSRItinerary = objfltr.dtSSRItinerary;
                    }
                    else
                    {
                        GetPriceFilter_GDS objfltr = new GetPriceFilter_GDS();
                        objfltr.SetPriceUpdate(SearchID, CompanyID, dtBound, PriceResponse);
                    }
                }
                else
                {
                    foreach (DataRow dr in dtBound.Rows)
                    {
                        dr["IsPriceChanged"] = false;
                        dr["FareStatus"] = false;
                        dr["FareQuote"] = "";
                    }
                    dtBound.AcceptChanges();
                }

                //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                dtBound = _rr_Layer.GetAvailabilityCal(true, true, SearchID, CompanyID, dtBound);
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceItineraryResponse", "GetPriceItinerary_NEW", SearchCriteria, SearchID, ex.Message);
            }
            DBCommon.Logger.WriteLogg(CompanyID, 0, "FareUpdate", "FARE", PriceRequest + Environment.NewLine + PriceResponse, SearchCriteria, SearchID);
            return dtBound;
        }
        private string GetPriceRequest_6E(DataTable dtSelect)
        {
            string Request = "";
            try
            {
                Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
                Request += @"<soapenv:Body>";
                Request += @"<air:AirPriceReq xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" CheckOBFees =""All"" FareRuleType=""long"" AuthorizedBy=""ZEALTRAVELS""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";
                Request += @"<air:AirItinerary>";

                foreach (DataRow dr in dtSelect.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);

                    Request += @"<air:AirSegment SupplierCode=""6E"" Key=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " Group=" + "\"" + dr["Classes"].ToString() + "\"" + " Carrier=" + "\"" + dr["CarrierCode"].ToString() + "\"" + " FlightNumber=" + "\"" + dr["FlightNumber"].ToString() + "\"" + " Origin=" + "\"" + dr["DepartureStation"].ToString() + "\"" + " Destination=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + " DepartureTime=" + "\"" + dr["DepDate"].ToString() + "\"" + " ArrivalTime=" + "\"" + dr["ArrDate"].ToString() + "\"" + "  HostTokenRef=" + "\"" + HostToken + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " ClassOfService=" + "\"" + dr["ClassOfService"].ToString() + "\"" + ">";
                    Request += @"</air:AirSegment>";

                }
                foreach (DataRow dr in dtSelect.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);
                    Request += @"<com:HostToken Key=" + "\"" + HostToken + "\"" + ">" + HostToken_Text + "</com:HostToken>";
                }
                Request += @"</air:AirItinerary>";

                if (Convert.ToDecimal(dtSelect.Rows[0]["Adt_Import"].ToString()) < 0)
                {
                    Request += @"<air:AirPricingModifiers AccountCodeFaresOnly = ""false"">";
                    //Request += @"<air:PromoCodes>";
                    //Request += @"<air:PromoCode Code =""CUITYCN"" ProviderCode = ""ACH"" SupplierCode = ""6E"" />";
                    //Request += @"</air:PromoCodes>";
                    Request += @"</air:AirPricingModifiers>";
                }

                int passengerid = 1;
                int Adt = Convert.ToInt32(dtSelect.Rows[0]["Adt"].ToString());
                int Chd = Convert.ToInt32(dtSelect.Rows[0]["Chd"].ToString());
                int Inf = Convert.ToInt32(dtSelect.Rows[0]["Inf"].ToString());

                for (int i = 0; i < Adt; i++)
                {
                    Request += @"<com:SearchPassenger Code=""ADT"" Key=" + "\"" + passengerid.ToString() + "\"" + "/>";
                    passengerid++;
                }
                for (int i = 0; i < Inf; i++)
                {
                    Request += @"<com:SearchPassenger Code=""INF"" PricePTCOnly=""true"" Key=" + "\"" + passengerid.ToString() + "\"" + "/>";
                    passengerid++;
                }
                for (int i = 0; i < Chd; i++)
                {
                    Request += @"<com:SearchPassenger Code=""CHD"" Key=" + "\"" + passengerid.ToString() + "\"" + "/>";
                    passengerid++;
                }



                Request += @"<air:AirPricingCommand>";
                foreach (DataRow dr in dtSelect.Rows)
                {
                    Request += @"<air:AirSegmentPricingModifiers AirSegmentRef=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " FareBasisCode=" + "\"" + dr["FareBasisCode"].ToString() + "\"" + "/>";
                }
                Request += @"</air:AirPricingCommand>";

                Request += @"<com:FormOfPayment Type=""Cash""/>";
                Request += @"</air:AirPriceReq>";
                Request += @"</soapenv:Body>";
                Request += @"</soapenv:Envelope>";
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GetPriceRequest_6E", "GetPriceItinerary_NEW", "", SearchID, ex.Message);
            }
            return Request;
        }
    }
}
