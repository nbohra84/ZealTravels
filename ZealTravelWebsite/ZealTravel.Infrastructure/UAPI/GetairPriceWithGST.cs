using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetairPriceWithGST
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string CompanyID;
        private DataTable dtBound;
        private DataTable dtPassenger;
        private int BookingRef;
        public GetairPriceWithGST(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string SearchID, string CompanyID, int BookingRef, DataTable dtBound, DataTable dtPassenger)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.UserName = UserName;
            this.Password = Password;

            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
            this.dtBound = dtBound;
            this.dtPassenger = dtPassenger;
        }

        public bool GetBookResponse()
        {
            bool status = false;
            string SearchCriteria = "";
            string BookRequest = "";
            string BookResponse = "";
            string PassengerRequest = "";
            string Trip = "";
            string FltType = "";

            try
            {
                PassengerRequest = DBCommon.CommonFunction.DataTableToString(dtPassenger);
                string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
                string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();
                string BeginDate = dtBound.Rows[0]["DepDate"].ToString().Trim();
                string EndDate = dtBound.Rows[dtBound.Rows.Count - 1]["DepDate"].ToString().Trim().ToUpper();

                short iAdt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
                short iChd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
                short iInf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());

                Trip = dtBound.Rows[0]["Trip"].ToString();
                FltType = dtBound.Rows[0]["FltType"].ToString();
                string AirlineID = dtBound.Rows[0]["AirlineID"].ToString();

                SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, EndDate, iAdt, iChd, iInf);

                BookRequest = GetAirPriceGstRequest();
                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, BookRequest, "AirService", "BookWithGST");
                if (BookResponse != null && BookResponse.IndexOf("SupplierLocator") != -1 && BookResponse.IndexOf("AirReservation") != 1)
                {
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookResponse-GetairPriceWithGST", "air_uapi", SearchCriteria, SearchID, ex.Message);
            }

            DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, FltType, SearchCriteria, "PNR-GetairPriceWithGST" + Trip, BookRequest, BookResponse, PassengerRequest);
            DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "PNR-GetairPriceWithGST", "PNR", BookRequest + Environment.NewLine + BookResponse, SearchCriteria, SearchID);
            return status;
        }
        private string GetAirPriceGstRequest()
        {
            string Request = string.Empty;
            try
            {
                Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ses=""http://www.travelport.com/soa/common/security/SessionContext_v1"" xmlns:air=""http://www.travelport.com/schema/air_v51_0"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"">";
                Request += @"<soapenv:Header></soapenv:Header>";
                Request += @"<soapenv:Body>";
                Request += @"<air:AirPriceReq AuthorizedBy=""ZEALTRAVELS"" CheckOBFees =""All"" FareRuleType=""long""  TraceId=" + "\"" + SearchID + "\"" + "  TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""UAPI""/>";

                //===========================================================================================================================================

                Request += @"<air:AirItinerary>";
                foreach (DataRow dr in dtBound.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);

                    Request += @"<air:AirSegment Key=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " Group=" + "\"" + dr["Classes"].ToString() + "\"" + " Carrier=" + "\"" + dr["CarrierCode"].ToString() + "\"" + " FlightNumber=" + "\"" + dr["FlightNumber"].ToString() + "\"" + " Origin=" + "\"" + dr["DepartureStation"].ToString() + "\"" + " Destination=" + "\"" + dr["ArrivalStation"].ToString() + "\"" + " DepartureTime=" + "\"" + dr["DepDate"].ToString() + "\"" + " ArrivalTime=" + "\"" + dr["ArrDate"].ToString() + "\"" + "  HostTokenRef=" + "\"" + HostToken + "\"" + " ProviderCode=" + "\"" + "ACH" + "\"" + " ClassOfService=" + "\"" + dr["ClassOfService"].ToString() + "\"" + " SupplierCode=" + "\"" + dr["CarrierCode"].ToString() + "\"" + ">";
                    Request += @"</air:AirSegment>";
                }
                foreach (DataRow dr in dtBound.Rows)
                {
                    string HostToken_Text = "";
                    string HostToken = GetBookFunctions.GetHostToken(dr["API_BookingFareID"].ToString(), out HostToken_Text);
                    Request += @"<com:HostToken Key=" + "\"" + HostToken + "\"" + ">" + HostToken_Text + "</com:HostToken>";
                }
                Request += @"</air:AirItinerary>";
                Request += @"<air:AirPricingModifiers/>";

                string BookingTravelerRef = dtBound.Rows[0]["API_AirlineID"].ToString();
                GetBookFunctions cs = new GetBookFunctions();
                cs.SetBookingTravelerRefPassenegers(SearchID, dtPassenger, BookingTravelerRef);

                DataRow[] dtAdults = dtPassenger.Select("PaxType='" + "ADT" + "'");
                DataRow[] dtChilds = dtPassenger.Select("PaxType='" + "CHD" + "'");
                DataRow[] dtInfants = dtPassenger.Select("PaxType='" + "INF" + "'");

                for (int i = 0; i < dtAdults.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow dr = dtAdults.CopyToDataTable().Rows[i];
                    Request += @"<com:SearchPassenger Code=""ADT"" BookingTravelerRef=" + "\"" + dr["BookingTravelerRef"].ToString() + "\"" + "/>";
                }

                if (dtInfants.Length > 0)
                {
                    for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtInfants.CopyToDataTable().Rows[i];
                        Request += @"<com:SearchPassenger Code=""INF"" BookingTravelerRef=" + "\"" + dr["BookingTravelerRef"].ToString() + "\"" + "/>";
                    }
                }

                if (dtChilds.Length > 0)
                {
                    for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.CopyToDataTable().Rows[i];
                        Request += @"<com:SearchPassenger Code=""CHD"" BookingTravelerRef=" + "\"" + dr["BookingTravelerRef"].ToString() + "\"" + "/>";
                    }
                }

                Request += @"<air:AirPricingCommand>";
                foreach (DataRow dr in dtBound.Rows)
                {
                    Request += @"<air:AirSegmentPricingModifiers AirSegmentRef=" + "\"" + dr["JourneySellKey"].ToString() + "\"" + " FareBasisCode=" + "\"" + dr["FareBasisCode"].ToString() + "\"" + "/>";
                }
                Request += @"</air:AirPricingCommand>";

                Request += @"<com:FormOfPayment Key = ""1"" Type = ""AgencyPayment"">";
                //Request += @"<com:AgencyPayment AgencyBillingIdentifier="""" AgencyBillingPassword=""""/>";
                Request += @"<com:AgencyPayment AgencyBillingIdentifier=" + "\"" + UserName + "\"" + " AgencyBillingPassword=" + "\"" + Password + "\"" + "/>";
                Request += @"</com:FormOfPayment>";

                string GSTCompanyContactNumber = dtPassenger.Rows[0]["GSTCompanyContactNumber"].ToString().Trim();
                string GSTCompanyName = dtPassenger.Rows[0]["GSTCompanyName"].ToString().Trim();
                string GSTNumber = dtPassenger.Rows[0]["GSTNumber"].ToString().Trim();
                string GSTCompanyEmail = dtPassenger.Rows[0]["GSTCompanyEmail"].ToString().Trim();
                //if (GSTNumber.Length.Equals(0))
                //{
                //    //GSTCompanyAddress = "707 Arunachal Building 19 Barakhamba Road New Delhi 110001";
                //    GSTCompanyContactNumber = "8882233320";
                //    GSTCompanyName = "Tourista International Pvt Ltd";
                //    GSTNumber = "07AAFCT0556N1Z0";
                //    GSTCompanyEmail = "accounts@tourista.asia";
                //}

                string CarrierCode = "6E";
                string[] split = GSTCompanyEmail.Split('@');
                Request += @"<com:SSR Key=" + "\"" + "11" + "\"" + " Type=" + "\"" + "GSTN" + "\"" + "  Carrier=" + "\"" + CarrierCode + "\"" + " Status=" + "\"" + "HK" + "\"" + " FreeText=" + "\"" + "/IND/" + GSTNumber + "/" + GSTCompanyName + "\"" + "/>";
                Request += @"<com:SSR Key=" + "\"" + "22" + "\"" + " Type=" + "\"" + "GSTE" + "\"" + "  Carrier=" + "\"" + CarrierCode + "\"" + " Status=" + "\"" + "HK" + "\"" + " FreeText=" + "\"" + "/IND/" + split[0].ToString() + "//" + split[1].ToString() + "\"" + "/>";

                Request += @"</air:AirPriceReq>";

                Request += @"</soapenv:Body>";
                Request += @"</soapenv:Envelope>";
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetairPriceWithGST-GetAirPriceGstRequest", "air_uapi", Request, SearchID, ex.Message);
            }
            return Request;
        }
    }
}
