using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using ZealTravel.Domain.Interfaces.TBO;

namespace ZealTravel.Infrastructure.TBO
{
    public struct GetApiServiceURL
    {
        public static string GetApiClientid
        {
            get
            {
                return "tboprod";
                //return "ApiIntegrationNew";
            }
        }
        public static string getauthenticate_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/authenticate";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/Authenticate";
            }
        }
        public static string getAgencyBalance_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/getAgencyBalance";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/GetAgencyBalance";
            }
        }
        public static string getlogout_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/Logout";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/Logout";
            }
        }
        public static string getsearch_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Search";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Search/";
            }
        }
        public static string getfarerule_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/FareRule";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/FareRule/";
            }
        }
        public static string getfarequote_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/FareQuote";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/FareQuote/";
            }
        }
        public static string getssr_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/SSR";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/SSR/";
            }
        }
        public static string getbook_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Book";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Book/";
            }
        }
        public static string getticket_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Ticket";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Ticket/";
            }
        }
        public static string getbookingdetails_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/GetBookingDetails/";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/GetBookingDetails/";
            }
        }
        public static string getcalendarfare_url
        {
            get
            {
                return "http://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/GetCalendarFare/";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/GetCalendarFare/";
            }
        }
        public static string getpricerbd_url
        {
            get
            {
                //return "";
                return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/PriceRBD/";
            }
        }
    }
    public class GetApiRequests 
    {
        public string errorMessage;
        //LOGIN======================================================================================================================================
        public static string GetLoginRequest(string UserName, string Password, string EndUserIp)
        {
            string json = JsonConvert.SerializeObject(new
            {
                ClientId = GetApiServiceURL.GetApiClientid,
                UserName = UserName,
                Password = Password,
                EndUserIp = EndUserIp
            });
            return json;
        }
        public static string GetAgencyBalanceRequest(string TokenId, string TokenMemberId, string TokenAgencyId, string EndUserIp)
        {
            string json = JsonConvert.SerializeObject(new
            {
                ClientId = GetApiServiceURL.GetApiClientid,
                TokenAgencyId = TokenAgencyId,
                TokenMemberId = TokenMemberId,
                TokenId = TokenId,
                EndUserIp = EndUserIp
            });

            return json;
        }
        public static string GetLogoutRequest(string TokenId, string TokenMemberId, string TokenAgencyId, string EndUserIp)
        {
            string json = JsonConvert.SerializeObject(new
            {
                ClientId = GetApiServiceURL.GetApiClientid,
                TokenAgencyId = TokenAgencyId,
                TokenMemberId = TokenMemberId,
                TokenId = TokenId,
                EndUserIp = EndUserIp
            });

            return json;
        }

        //SEARCH======================================================================================================================================
        public string GetOneWaySearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append(" \"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");

            sb.Append("\"AdultCount\": \"" + Adt + "\",");
            sb.Append("\"ChildCount\": \"" + Chd + "\",");
            sb.Append("\"InfantCount\": \"" + Inf + "\",");

            sb.Append("\"DirectFlight\": \"" + "false" + "\",");
            sb.Append("\"OneStopFlight\":\"" + "false" + "\",");
            sb.Append("\"JourneyType\": \"" + "1" + "\",");
            sb.Append("\"PreferredAirlines\": \"" + null + "\",");

            sb.Append("\"Segments\":[");
            sb.Append("{");
            sb.Append("\"Origin\": \"" + Origin + "\",");
            sb.Append("\"Destination\":\"" + Destination + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\"");
            sb.Append("}");
            sb.Append("],");

            //sb.Append("\"Sources\": [\"SG\",\"GDS\",\"6E\",\"G8\"]");

            sb.Append("\"Sources\": [");
            for (int i = 0; i < CarrierList.Count; i++)
            {
                if (i.Equals(CarrierList.Count - 1))
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\"");
                }
                else
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\",");
                }
            }
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }
        public string GetRoundWaySearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");

            sb.Append("\"AdultCount\": \"" + Adt + "\",");
            sb.Append("\"ChildCount\": \"" + Chd + "\",");
            sb.Append("\"InfantCount\": \"" + Inf + "\",");

            sb.Append("\"DirectFlight\": \"" + "false" + "\",");
            sb.Append("\"OneStopFlight\":\"" + "false" + "\",");
            sb.Append("\"JourneyType\": \"" + "2" + "\",");
            sb.Append("\"PreferredAirlines\": \"" + null + "\",");

            sb.Append("\"Segments\":[");
            sb.Append("{");
            sb.Append("\"Origin\": \"" + Origin + "\",");
            sb.Append("\"Destination\":\"" + Destination + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\"");
            sb.Append("},");

            sb.Append("{");
            sb.Append("\"Origin\": \"" + Destination + "\",");
            sb.Append("\"Destination\":\"" + Origin + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00: 00: 00" + "\"");
            sb.Append("}");

            sb.Append("],");

            sb.Append("\"Sources\": [");
            for (int i = 0; i < CarrierList.Count; i++)
            {
                if (i.Equals(CarrierList.Count - 1))
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\"");
                }
                else
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\",");
                }
            }
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }
        public string GetRoundWaySpecialDomesticSearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp)
        {
            ArrayList ArayCarrierList = new ArrayList();
            if (CarrierList.Contains("6E"))
            {
                ArayCarrierList.Add("6E");
            }
            if (CarrierList.Contains("G8"))
            {
                ArayCarrierList.Add("G8");
            }
            if (CarrierList.Contains("SG"))
            {
                ArayCarrierList.Add("SG");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append(" \"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");

            sb.Append("\"AdultCount\": \"" + Adt + "\",");
            sb.Append("\"ChildCount\": \"" + Chd + "\",");
            sb.Append("\"InfantCount\": \"" + Inf + "\",");

            sb.Append("\"DirectFlight\": \"" + "false" + "\",");
            sb.Append("\"OneStopFlight\":\"" + "false" + "\",");
            sb.Append("\"JourneyType\": \"" + "5" + "\",");
            sb.Append("\"PreferredAirlines\": \"" + null + "\",");

            sb.Append("\"Segments\":[");
            sb.Append("{");
            sb.Append("\"Origin\": \"" + Origin + "\",");
            sb.Append("\"Destination\":\"" + Destination + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\"");
            sb.Append("},");

            sb.Append("{");
            sb.Append("\"Origin\": \"" + Destination + "\",");
            sb.Append("\"Destination\":\"" + Origin + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(EndDate) + "T00: 00: 00" + "\"");
            sb.Append("}");

            sb.Append("],");

            sb.Append("\"Sources\": [");
            for (int i = 0; i < ArayCarrierList.Count; i++)
            {
                if (i.Equals(ArayCarrierList.Count - 1))
                {
                    sb.Append("\"" + ArayCarrierList[i].ToString() + "\"");
                }
                else
                {
                    sb.Append("\"" + ArayCarrierList[i].ToString() + "\",");
                }
            }
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }
        public string GetMulticitySearchRequest(string Tokenid, string Request, string EndUserIp)
        {
            DataSet dsRequest = GetCommonFunctions.StringToDataSet(Request);
            int Adt = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
            int Chd = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
            int Inf = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
            string Cabin = dsRequest.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();

            ArrayList CarrierList = new ArrayList();
            foreach (DataRow dr in dsRequest.Tables["AirVInfo"].Rows)
            {
                CarrierList.Add(dr["AirV"].ToString());
            }
            CarrierList = GetCommonFunctions.RemoveDuplicates(CarrierList);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append(" \"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");

            sb.Append("\"AdultCount\": \"" + Adt + "\",");
            sb.Append("\"ChildCount\": \"" + Chd + "\",");
            sb.Append("\"InfantCount\": \"" + Inf + "\",");

            sb.Append("\"DirectFlight\": \"" + "false" + "\",");
            sb.Append("\"OneStopFlight\":\"" + "false" + "\",");
            sb.Append("\"JourneyType\": \"" + "3" + "\",");
            sb.Append("\"PreferredAirlines\": \"" + null + "\",");

            sb.Append("\"Segments\":[");

            foreach (DataRow dr in dsRequest.Tables["AirSrchInfo"].Rows)
            {
                sb.Append("{");
                sb.Append("\"Origin\": \"" + dr["DepartureStation"].ToString() + "\",");
                sb.Append("\"Destination\":\"" + dr["ArrivalStation"].ToString() + "\",");
                sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
                sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(dr["StartDate"].ToString()) + "T00: 00: 00" + "\",");
                sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(dr["StartDate"].ToString()) + "T00: 00: 00" + "\"");
                sb.Append("},");
            }
            sb.Append("],");

            sb.Append("\"Sources\": [");
            for (int i = 0; i < CarrierList.Count; i++)
            {
                if (i.Equals(CarrierList.Count - 1))
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\"");
                }
                else
                {
                    sb.Append("\"" + CarrierList[i].ToString() + "\",");
                }
            }
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }

        //FARE RULE======================================================================================================================================
        public string GetFareRuleFareQuoteSSRRequest(string Tokenid, string TraceId, string ResultIndex, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");
            sb.Append("\"TraceId\": \"" + TraceId + "\",");
            sb.Append("\"ResultIndex\": \"" + ResultIndex + "\"");
            sb.Append("}");

            return sb.ToString();
        }
        //BOOK======================================================================================================================================

        public string GetLCCpnrRTRequest(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Tokenid, string Traceid, DataTable dtBound, DataTable dtPassengerInfo, DataTable dtCompanyInfo, DataTable dtGstInfo, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                bool IsGSTApplicable = false;
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();

                    if (GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0 && GSTCompanyName.Length > 0)
                    {
                        IsGSTApplicable = true;
                    }
                }

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                string CarrierCode = dtBound.Rows[0]["CarrierCode"].ToString();
                string ResultIndex = dtBound.Rows[0]["BookingFareID"].ToString();
                string Sector = dtBound.Rows[0]["Sector"].ToString();

                int iPax = 0;
                sb.Append("{");
                sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
                sb.Append("\"PreferredCurrency\":\"" + "INR" + "\",");
                sb.Append("\"IsBaseCurrencyRequired\": \"" + "true" + "\",");
                sb.Append("\"TokenId\":\"" + Tokenid + "\",");
                sb.Append("\"TraceId\": \"" + Traceid + "\",");
                sb.Append("\"ResultIndex\": \"" + ResultIndex + "\",");

                sb.Append("\"Passengers\":[");
                foreach (DataRow dr in dtPassengerInfo.Rows)
                {
                    sb.Append("{");

                    sb.Append("\"Title\": \"" + dr["Title"].ToString().Trim() + "\",");
                    sb.Append("\"FirstName\": \"" + dr["First_Name"].ToString().Trim() + "\",");
                    sb.Append("\"LastName\": \"" + dr["Last_Name"].ToString().Trim() + "\",");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"PaxType\": \"" + 1 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"PaxType\": \"" + 2 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"PaxType\": \"" + 3 + "\",");
                    }

                    sb.Append("\"DateOfBirth\": \"" + dr["DOB"].ToString().Trim() + "T00:00:00" + "\",");

                    if (dr["Title"].ToString().Trim().ToUpper().Equals("MR"))
                    {
                        sb.Append("\"Gender\": \"" + "1" + "\",");
                    }
                    else
                    {
                        sb.Append("\"Gender\": \"" + "2" + "\",");
                    }

                    if (Sector.Equals("I"))
                    {
                        sb.Append("\"PassportNo\": \"" + dr["PpNumber"].ToString().Trim() + "\",");
                        sb.Append("\"PassportExpiry\": \"" + dr["PPExpirayDate"].ToString().Trim() + "\",");
                    }
                    else
                    {
                        sb.Append("\"PassportNo\": \"" + "" + "\",");
                        sb.Append("\"PassportExpiry\": \"" + "" + "\",");
                    }

                    sb.Append("\"AddressLine1\": \"" + CompanyName + "\",");
                    sb.Append("\"AddressLine2\": \"" + CityName + "\",");
                    sb.Append("\"City\": \"" + CityName + "\",");
                    sb.Append("\"CountryCode\": \"" + CountryCode + "\",");
                    sb.Append("\"CountryName\": \"" + CountryName + "\",");
                    sb.Append("\"ContactNo\": \"" + MobileNo + "\",");
                    sb.Append("\"Email\": \"" + Email + "\",");

                    if (iPax.Equals(0))
                    {
                        sb.Append("\"IsLeadPax\": \"" + "true" + "\",");
                    }
                    else
                    {
                        sb.Append("\"IsLeadPax\": \"" + "false" + "\",");
                    }

                    if (dr["FFN"].ToString().Trim().Length > 0)
                    {
                        string Code = "";
                        string Number = "";
                        GetFFN(Companyid, BookingRef, dr["FFN"].ToString().Trim(), "O", out Code, out Number);

                        if (Code.Length > 0 && Number.Length > 0)
                        {
                            sb.Append("\"FFAirline\": \"" + Code + "\",");
                            sb.Append("\"FFNumber\": \"" + Number + "\",");
                        }
                        else
                        {
                            sb.Append("\"FFAirline\": \"" + "" + "\",");
                            sb.Append("\"FFNumber\": \"" + "" + "\",");
                        }
                    }
                    else
                    {
                        sb.Append("\"FFAirline\": \"" + "" + "\",");
                        sb.Append("\"FFNumber\": \"" + "" + "\",");
                    }

                    if (IsGSTApplicable)
                    {
                        sb.Append("\"GSTCompanyAddress\": \"" + GSTCompanyAddress + "\",");
                        sb.Append("\"GSTCompanyContactNumber\": \"" + GSTCompanyContactNumber + "\",");
                        sb.Append("\"GSTCompanyName\": \"" + GSTCompanyName + "\",");
                        sb.Append("\"GSTNumber\": \"" + GSTNumber + "\",");
                        sb.Append("\"GSTCompanyEmail\": \"" + GSTCompanyEmail + "\",");
                    }

                    sb.Append("\"Fare\":");
                    sb.Append("{");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + 0 + "\",");
                        sb.Append("\"YQTax\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }


                    bool IsMeal = false;
                    bool IsBagg = false;

                    if (dr["MealCode_O"].ToString().Trim().Length > 2)
                    {
                        IsMeal = true;
                    }
                    if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                    {
                        IsBagg = true;
                    }

                    if (IsMeal || IsBagg)
                    {
                        sb.Append("},");
                    }
                    else
                    {
                        sb.Append("}");
                    }

                    if (dr["MealCode_O"].ToString().Trim().Length > 2)
                    {
                        Hashtable SSRht = getSSRdetail(dr["MealDetail_O"].ToString());
                        if (SSRht.Count > 0)
                        {
                            string Description = SSRht["Description"].ToString();
                            string WayType = SSRht["WayType"].ToString();
                            string Weight = SSRht["Weight"].ToString();
                            string Currency = SSRht["Currency"].ToString();
                            string Origin = SSRht["Origin"].ToString();
                            string Destination = SSRht["Destination"].ToString();
                            string AirlineDescription = SSRht["AirlineDescription"].ToString();

                            string FlightNumber = string.Empty;
                            string AirlineCode = string.Empty;

                            if (SSRht["FlightNumber"] != null)
                            {
                                FlightNumber = SSRht["FlightNumber"].ToString();
                            }
                            if (SSRht["AirlineCode"] != null)
                            {
                                AirlineCode = SSRht["AirlineCode"].ToString();
                            }

                            sb.Append("\"MealDynamic\":");
                            sb.Append("[{");

                            if (FlightNumber.Length > 0)
                            {
                                sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                            }
                            if (AirlineCode.Length > 0)
                            {
                                sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                            }

                            sb.Append("\"WayType\": \"" + WayType + "\",");
                            sb.Append("\"Code\": \"" + dr["MealCode_O"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + Description + "\",");
                            sb.Append("\"AirlineDescription\": \"" + AirlineDescription + "\",");
                            sb.Append("\"Quantity\": \"" + "1" + "\",");
                            sb.Append("\"Price\": \"" + dr["MealChg_O"].ToString().Trim() + "\",");
                            sb.Append("\"Currency\": \"" + "INR" + "\",");
                            sb.Append("\"Origin\": \"" + Origin + "\",");
                            sb.Append("\"Destination\": \"" + Destination + "\"");

                            if (dr["MealCode_I"].ToString().Trim().Length > 2)
                            {
                                sb.Append("},");

                                SSRht = getSSRdetail(dr["MealDetail_I"].ToString());
                                if (SSRht.Count > 0)
                                {
                                    Description = SSRht["Description"].ToString();
                                    WayType = SSRht["WayType"].ToString();
                                    Weight = SSRht["Weight"].ToString();
                                    Currency = SSRht["Currency"].ToString();
                                    Origin = SSRht["Origin"].ToString();
                                    Destination = SSRht["Destination"].ToString();
                                    AirlineDescription = SSRht["AirlineDescription"].ToString();

                                    FlightNumber = string.Empty;
                                    AirlineCode = string.Empty;

                                    if (SSRht["FlightNumber"] != null)
                                    {
                                        FlightNumber = SSRht["FlightNumber"].ToString();
                                    }
                                    if (SSRht["AirlineCode"] != null)
                                    {
                                        AirlineCode = SSRht["AirlineCode"].ToString();
                                    }

                                    sb.Append("{");
                                    if (FlightNumber.Length > 0)
                                    {
                                        sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                                    }
                                    if (AirlineCode.Length > 0)
                                    {
                                        sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                                    }

                                    sb.Append("\"WayType\": \"" + WayType + "\",");
                                    sb.Append("\"Code\": \"" + dr["MealCode_I"].ToString().Trim() + "\",");
                                    sb.Append("\"Description\": \"" + Description + "\",");
                                    sb.Append("\"AirlineDescription\": \"" + AirlineDescription + "\",");
                                    sb.Append("\"Quantity\": \"" + "1" + "\",");
                                    sb.Append("\"Price\": \"" + dr["MealChg_I"].ToString().Trim() + "\",");
                                    sb.Append("\"Currency\": \"" + "INR" + "\",");
                                    sb.Append("\"Origin\": \"" + Origin + "\",");
                                    sb.Append("\"Destination\": \"" + Destination + "\"");
                                    sb.Append("}");
                                }
                            }
                            else
                            {
                                sb.Append("}");
                            }

                            if (IsBagg)
                            {
                                sb.Append("],");
                            }
                            else
                            {
                                sb.Append("]");
                            }
                        }
                    }

                    if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                    {
                        Hashtable SSRht = getSSRdetail(dr["BaggageDetail_O"].ToString());
                        if (SSRht.Count > 0)
                        {
                            string Description = SSRht["Description"].ToString();
                            string WayType = SSRht["WayType"].ToString();
                            string Weight = SSRht["Weight"].ToString();
                            string Currency = SSRht["Currency"].ToString();
                            string Origin = SSRht["Origin"].ToString();
                            string Destination = SSRht["Destination"].ToString();
                            string AirlineDescription = SSRht["AirlineDescription"].ToString();

                            string FlightNumber = string.Empty;
                            string AirlineCode = string.Empty;

                            if (SSRht["FlightNumber"] != null)
                            {
                                FlightNumber = SSRht["FlightNumber"].ToString();
                            }
                            if (SSRht["AirlineCode"] != null)
                            {
                                AirlineCode = SSRht["AirlineCode"].ToString();
                            }

                            sb.Append("\"Baggage\":");
                            sb.Append("[{");

                            if (FlightNumber.Length > 0)
                            {
                                sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                            }
                            if (AirlineCode.Length > 0)
                            {
                                sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                            }

                            sb.Append("\"WayType\": \"" + WayType + "\",");
                            sb.Append("\"Code\": \"" + dr["BaggageCode_O"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + Description + "\",");
                            sb.Append("\"Weight\": \"" + Weight + "\",");
                            sb.Append("\"Price\": \"" + dr["BaggageChg_O"].ToString().Trim() + "\",");
                            sb.Append("\"Currency\": \"" + "INR" + "\",");
                            sb.Append("\"Origin\": \"" + Origin + "\",");
                            sb.Append("\"Destination\": \"" + Destination + "\"");


                            if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                            {
                                sb.Append("},");

                                SSRht = getSSRdetail(dr["BaggageDetail_I"].ToString());
                                if (SSRht.Count > 0)
                                {
                                    Description = SSRht["Description"].ToString();
                                    WayType = SSRht["WayType"].ToString();
                                    Weight = SSRht["Weight"].ToString();
                                    Currency = SSRht["Currency"].ToString();
                                    Origin = SSRht["Origin"].ToString();
                                    Destination = SSRht["Destination"].ToString();
                                    AirlineDescription = SSRht["AirlineDescription"].ToString();

                                    sb.Append("{");
                                    sb.Append("\"WayType\": \"" + WayType + "\",");
                                    sb.Append("\"Code\": \"" + dr["BaggageCode_I"].ToString().Trim() + "\",");
                                    sb.Append("\"Description\": \"" + Description + "\",");
                                    sb.Append("\"Weight\": \"" + Weight + "\",");
                                    sb.Append("\"Price\": \"" + dr["BaggageChg_I"].ToString().Trim() + "\",");
                                    sb.Append("\"Currency\": \"" + "INR" + "\",");
                                    sb.Append("\"Origin\": \"" + Origin + "\",");
                                    sb.Append("\"Destination\": \"" + Destination + "\"");
                                    sb.Append("}");
                                }
                            }
                            else
                            {
                                sb.Append("}");
                            }

                            sb.Append("]");
                        }
                    }

                    sb.Append("},");
                    iPax++;
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append("}");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetTicketRTRequest", "air_tbo-GetApiRequests", sb.ToString(), Searchid, errorMessage);
            }
            return sb.ToString();
        }
        public string GetLCCpnrRequest(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Tokenid, string Traceid, string FltType, DataTable dtBound, DataTable dtPassengerInfo, DataTable dtCompanyInfo, DataTable dtGstInfo, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                bool IsGSTApplicable = false;
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();

                    if (GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0 && GSTCompanyName.Length > 0)
                    {
                        IsGSTApplicable = true;
                    }
                }

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                string CarrierCode = dtBound.Rows[0]["CarrierCode"].ToString();
                string ResultIndex = dtBound.Rows[0]["BookingFareID"].ToString();
                string Sector = dtBound.Rows[0]["Sector"].ToString();

                int iPax = 0;
                sb.Append("{");
                sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
                sb.Append("\"PreferredCurrency\":\"" + "INR" + "\",");
                sb.Append("\"IsBaseCurrencyRequired\": \"" + "true" + "\",");
                sb.Append("\"TokenId\":\"" + Tokenid + "\",");
                sb.Append("\"TraceId\": \"" + Traceid + "\",");
                sb.Append("\"ResultIndex\": \"" + ResultIndex + "\",");

                sb.Append("\"Passengers\":[");
                foreach (DataRow dr in dtPassengerInfo.Rows)
                {
                    sb.Append("{");

                    sb.Append("\"Title\": \"" + dr["Title"].ToString().Trim() + "\",");
                    sb.Append("\"FirstName\": \"" + dr["First_Name"].ToString().Trim() + "\",");
                    sb.Append("\"LastName\": \"" + dr["Last_Name"].ToString().Trim() + "\",");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"PaxType\": \"" + 1 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"PaxType\": \"" + 2 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"PaxType\": \"" + 3 + "\",");
                    }

                    sb.Append("\"DateOfBirth\": \"" + dr["DOB"].ToString().Trim() + "T00:00:00" + "\",");

                    if (dr["Title"].ToString().Trim().ToUpper().Equals("MR"))
                    {
                        sb.Append("\"Gender\": \"" + "1" + "\",");
                    }
                    else
                    {
                        sb.Append("\"Gender\": \"" + "2" + "\",");
                    }

                    if (Sector.Equals("I"))
                    {
                        sb.Append("\"PassportNo\": \"" + dr["PpNumber"].ToString().Trim() + "\",");
                        sb.Append("\"PassportExpiry\": \"" + dr["PPExpirayDate"].ToString().Trim() + "\",");
                    }
                    else
                    {
                        sb.Append("\"PassportNo\": \"" + "" + "\",");
                        sb.Append("\"PassportExpiry\": \"" + "" + "\",");
                    }

                    sb.Append("\"AddressLine1\": \"" + CompanyName + "\",");
                    sb.Append("\"AddressLine2\": \"" + CityName + "\",");
                    sb.Append("\"City\": \"" + CityName + "\",");
                    sb.Append("\"CountryCode\": \"" + CountryCode + "\",");
                    sb.Append("\"CountryName\": \"" + CountryName + "\",");
                    sb.Append("\"ContactNo\": \"" + MobileNo + "\",");
                    sb.Append("\"Email\": \"" + Email + "\",");

                    if (iPax.Equals(0))
                    {
                        sb.Append("\"IsLeadPax\": \"" + "true" + "\",");
                    }
                    else
                    {
                        sb.Append("\"IsLeadPax\": \"" + "false" + "\",");
                    }

                    if (dr["FFN"].ToString().Trim().Length > 0)
                    {
                        if (FltType.Equals("O"))
                        {
                            string Code = "";
                            string Number = "";
                            GetFFN(Companyid, BookingRef, dr["FFN"].ToString().Trim(), "O", out Code, out Number);

                            if (Code.Length > 0 && Number.Length > 0)
                            {
                                sb.Append("\"FFAirline\": \"" + Code + "\",");
                                sb.Append("\"FFNumber\": \"" + Number + "\",");
                            }
                            else
                            {
                                sb.Append("\"FFAirline\": \"" + "" + "\",");
                                sb.Append("\"FFNumber\": \"" + "" + "\",");
                            }
                        }
                        else
                        {
                            string Code = "";
                            string Number = "";
                            GetFFN(Companyid, BookingRef, dr["FFN"].ToString().Trim(), "I", out Code, out Number);

                            if (Code.Length > 0 && Number.Length > 0)
                            {
                                sb.Append("\"FFAirline\": \"" + Code + "\",");
                                sb.Append("\"FFNumber\": \"" + Number + "\",");
                            }
                            else
                            {
                                sb.Append("\"FFAirline\": \"" + "" + "\",");
                                sb.Append("\"FFNumber\": \"" + "" + "\",");
                            }
                        }
                    }
                    else
                    {
                        sb.Append("\"FFAirline\": \"" + "" + "\",");
                        sb.Append("\"FFNumber\": \"" + "" + "\",");
                    }

                    if (IsGSTApplicable)
                    {
                        sb.Append("\"GSTCompanyAddress\": \"" + GSTCompanyAddress + "\",");
                        sb.Append("\"GSTCompanyContactNumber\": \"" + GSTCompanyContactNumber + "\",");
                        sb.Append("\"GSTCompanyName\": \"" + GSTCompanyName + "\",");
                        sb.Append("\"GSTNumber\": \"" + GSTNumber + "\",");
                        sb.Append("\"GSTCompanyEmail\": \"" + GSTCompanyEmail + "\",");
                    }


                    sb.Append("\"Fare\":");
                    sb.Append("{");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + 0 + "\",");
                        sb.Append("\"YQTax\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }


                    bool IsMeal = false;
                    bool IsBagg = false;

                    if (FltType.Equals("O"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                        if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                        {
                            IsBagg = true;
                        }
                    }
                    else
                    {
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                        if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                        {
                            IsBagg = true;
                        }
                    }

                    if (IsMeal || IsBagg)
                    {
                        sb.Append("},");
                    }
                    else
                    {
                        sb.Append("}");
                    }

                    if (FltType.Equals("O"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            Hashtable SSRht = getSSRdetail(dr["MealDetail_O"].ToString());
                            if (SSRht.Count > 0)
                            {
                                string Description = SSRht["Description"].ToString();
                                string WayType = SSRht["WayType"].ToString();
                                string Weight = SSRht["Weight"].ToString();
                                string Currency = SSRht["Currency"].ToString();
                                string Origin = SSRht["Origin"].ToString();
                                string Destination = SSRht["Destination"].ToString();
                                string AirlineDescription = SSRht["AirlineDescription"].ToString();

                                string FlightNumber = string.Empty;
                                string AirlineCode = string.Empty;

                                if (SSRht["FlightNumber"] != null)
                                {
                                    FlightNumber = SSRht["FlightNumber"].ToString();
                                }
                                if (SSRht["AirlineCode"] != null)
                                {
                                    AirlineCode = SSRht["AirlineCode"].ToString();
                                }

                                sb.Append("\"MealDynamic\":");
                                sb.Append("[{");

                                if (FlightNumber.Length > 0)
                                {
                                    sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                                }
                                if (AirlineCode.Length > 0)
                                {
                                    sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                                }

                                sb.Append("\"WayType\": \"" + WayType + "\",");
                                sb.Append("\"Code\": \"" + dr["MealCode_O"].ToString().Trim() + "\",");
                                sb.Append("\"Description\": \"" + Description + "\",");
                                sb.Append("\"AirlineDescription\": \"" + AirlineDescription + "\",");
                                sb.Append("\"Quantity\": \"" + "1" + "\",");
                                sb.Append("\"Price\": \"" + dr["MealChg_O"].ToString().Trim() + "\",");
                                sb.Append("\"Currency\": \"" + "INR" + "\",");
                                sb.Append("\"Origin\": \"" + Origin + "\",");
                                sb.Append("\"Destination\": \"" + Destination + "\"");
                                sb.Append("}");

                                if (IsBagg)
                                {
                                    sb.Append("],");
                                }
                                else
                                {
                                    sb.Append("]");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            Hashtable SSRht = getSSRdetail(dr["MealDetail_I"].ToString());
                            if (SSRht.Count > 0)
                            {
                                string Description = SSRht["Description"].ToString();
                                string WayType = SSRht["WayType"].ToString();
                                string Weight = SSRht["Weight"].ToString();
                                string Currency = SSRht["Currency"].ToString();
                                string Origin = SSRht["Origin"].ToString();
                                string Destination = SSRht["Destination"].ToString();
                                string AirlineDescription = SSRht["AirlineDescription"].ToString();

                                string FlightNumber = string.Empty;
                                string AirlineCode = string.Empty;

                                if (SSRht["FlightNumber"] != null)
                                {
                                    FlightNumber = SSRht["FlightNumber"].ToString();
                                }
                                if (SSRht["AirlineCode"] != null)
                                {
                                    AirlineCode = SSRht["AirlineCode"].ToString();
                                }

                                sb.Append("\"MealDynamic\":");
                                sb.Append("[{");

                                if (FlightNumber.Length > 0)
                                {
                                    sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                                }
                                if (AirlineCode.Length > 0)
                                {
                                    sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                                }

                                sb.Append("\"WayType\": \"" + WayType + "\",");
                                sb.Append("\"Code\": \"" + dr["MealCode_I"].ToString().Trim() + "\",");
                                sb.Append("\"Description\": \"" + Description + "\",");
                                sb.Append("\"AirlineDescription\": \"" + AirlineDescription + "\",");
                                sb.Append("\"Quantity\": \"" + "1" + "\",");
                                sb.Append("\"Price\": \"" + dr["MealChg_I"].ToString().Trim() + "\",");
                                sb.Append("\"Currency\": \"" + "INR" + "\",");
                                sb.Append("\"Origin\": \"" + Origin + "\",");
                                sb.Append("\"Destination\": \"" + Destination + "\"");
                                sb.Append("}");

                                if (IsBagg)
                                {
                                    sb.Append("],");
                                }
                                else
                                {
                                    sb.Append("]");
                                }
                            }
                        }
                    }

                    if (FltType.Equals("O"))
                    {
                        if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                        {
                            Hashtable SSRht = getSSRdetail(dr["BaggageDetail_O"].ToString());
                            if (SSRht.Count > 0)
                            {
                                string Description = SSRht["Description"].ToString();
                                string WayType = SSRht["WayType"].ToString();
                                string Weight = SSRht["Weight"].ToString();
                                string Currency = SSRht["Currency"].ToString();
                                string Origin = SSRht["Origin"].ToString();
                                string Destination = SSRht["Destination"].ToString();
                                string AirlineDescription = SSRht["AirlineDescription"].ToString();

                                string FlightNumber = string.Empty;
                                string AirlineCode = string.Empty;

                                if (SSRht["FlightNumber"] != null)
                                {
                                    FlightNumber = SSRht["FlightNumber"].ToString();
                                }
                                if (SSRht["AirlineCode"] != null)
                                {
                                    AirlineCode = SSRht["AirlineCode"].ToString();
                                }

                                sb.Append("\"Baggage\":");
                                sb.Append("[{");

                                if (FlightNumber.Length > 0)
                                {
                                    sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                                }
                                if (AirlineCode.Length > 0)
                                {
                                    sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                                }

                                sb.Append("\"WayType\": \"" + WayType + "\",");
                                sb.Append("\"Code\": \"" + dr["BaggageCode_O"].ToString().Trim() + "\",");
                                sb.Append("\"Description\": \"" + Description + "\",");
                                sb.Append("\"Weight\": \"" + Weight + "\",");
                                sb.Append("\"Price\": \"" + dr["BaggageChg_O"].ToString().Trim() + "\",");
                                sb.Append("\"Currency\": \"" + "INR" + "\",");
                                sb.Append("\"Origin\": \"" + Origin + "\",");
                                sb.Append("\"Destination\": \"" + Destination + "\"");
                                sb.Append("}");
                                sb.Append("]");
                            }
                        }
                    }
                    else
                    {
                        if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                        {
                            Hashtable SSRht = getSSRdetail(dr["BaggageDetail_I"].ToString());
                            if (SSRht.Count > 0)
                            {
                                string Description = SSRht["Description"].ToString();
                                string WayType = SSRht["WayType"].ToString();
                                string Weight = SSRht["Weight"].ToString();
                                string Currency = SSRht["Currency"].ToString();
                                string Origin = SSRht["Origin"].ToString();
                                string Destination = SSRht["Destination"].ToString();
                                string AirlineDescription = SSRht["AirlineDescription"].ToString();

                                string FlightNumber = string.Empty;
                                string AirlineCode = string.Empty;

                                if (SSRht["FlightNumber"] != null)
                                {
                                    FlightNumber = SSRht["FlightNumber"].ToString();
                                }
                                if (SSRht["AirlineCode"] != null)
                                {
                                    AirlineCode = SSRht["AirlineCode"].ToString();
                                }

                                sb.Append("\"Baggage\":");
                                sb.Append("[{");

                                if (FlightNumber.Length > 0)
                                {
                                    sb.Append("\"FlightNumber\": \"" + FlightNumber + "\",");
                                }
                                if (AirlineCode.Length > 0)
                                {
                                    sb.Append("\"AirlineCode\": \"" + AirlineCode + "\",");
                                }

                                sb.Append("\"WayType\": \"" + WayType + "\",");
                                sb.Append("\"Code\": \"" + dr["BaggageCode_I"].ToString().Trim() + "\",");
                                sb.Append("\"Description\": \"" + Description + "\",");
                                sb.Append("\"Weight\": \"" + Weight + "\",");
                                sb.Append("\"Price\": \"" + dr["BaggageChg_I"].ToString().Trim() + "\",");
                                sb.Append("\"Currency\": \"" + "INR" + "\",");
                                sb.Append("\"Origin\": \"" + Origin + "\",");
                                sb.Append("\"Destination\": \"" + Destination + "\"");
                                sb.Append("}");
                                sb.Append("]");
                            }
                        }
                    }

                    sb.Append("},");
                    iPax++;
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append("}");

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetTicketRequest", "air_tbo-GetApiRequests", sb.ToString(), Searchid, errorMessage);
            }
            return sb.ToString();
        }
        public string GetGDSticketnumberRequest(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string PNR, string Bookingid, string Tokenid, string Traceid, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("{");
                sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
                sb.Append("\"TokenId\":\"" + Tokenid + "\",");
                sb.Append("\"TraceId\": \"" + Traceid + "\",");
                sb.Append("\"PNR\": \"" + PNR + "\",");
                sb.Append("\"BookingId\": \"" + Bookingid + "\"");
                sb.Append("}");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetGDSticketnumberRequest", "air_tbo-GetApiRequests", sb.ToString(), Searchid, errorMessage); ;
            }
            return sb.ToString();
        }
        public string GetGDSpnrRequest(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Tokenid, string Traceid, string FltType, DataTable dtBound, DataTable dtPassengerInfo, DataTable dtCompanyInfo, DataTable dtGstInfo, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                bool IsGSTApplicable = false;
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGstInfo != null && dtGstInfo.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGstInfo.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGstInfo.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGstInfo.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGstInfo.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGstInfo.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();

                    if (GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0 && GSTCompanyName.Length > 0)
                    {
                        IsGSTApplicable = true;
                    }
                }

                string CompanyName = dtCompanyInfo.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompanyInfo.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompanyInfo.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompanyInfo.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string CountryName = dtCompanyInfo.Rows[0]["CountryName"].ToString().Trim().ToUpper();
                string Email = dtCompanyInfo.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompanyInfo.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string MobileNo = dtCompanyInfo.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string PhoneNo = dtCompanyInfo.Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompanyInfo.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                string CarrierCode = dtBound.Rows[0]["CarrierCode"].ToString();
                string ResultIndex = dtBound.Rows[0]["BookingFareID"].ToString();
                string Sector = dtBound.Rows[0]["Sector"].ToString();

                int iPax = 0;

                sb.Append("{");

                sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
                sb.Append("\"PreferredCurrency\":\"" + "INR" + "\",");
                sb.Append("\"IsBaseCurrencyRequired\": \"" + "true" + "\",");
                sb.Append("\"TokenId\":\"" + Tokenid + "\",");
                sb.Append("\"TraceId\": \"" + Traceid + "\",");
                sb.Append("\"ResultIndex\": \"" + ResultIndex + "\",");//-----------may be issue

                sb.Append("\"Passengers\":[");
                foreach (DataRow dr in dtPassengerInfo.Rows)
                {
                    sb.Append("{");

                    sb.Append("\"Title\": \"" + dr["Title"].ToString().Trim().ToUpper() + "\",");
                    sb.Append("\"FirstName\": \"" + dr["First_Name"].ToString().Trim().ToUpper() + "\",");
                    sb.Append("\"LastName\": \"" + dr["Last_Name"].ToString().Trim().ToUpper() + "\",");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"PaxType\": \"" + 1 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"PaxType\": \"" + 2 + "\",");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"PaxType\": \"" + 3 + "\",");
                    }

                    sb.Append("\"DateOfBirth\": \"" + dr["DOB"].ToString().Trim() + "T00:00:00" + "\",");

                    if (dr["Title"].ToString().Trim().ToUpper().Equals("MR"))
                    {
                        sb.Append("\"Gender\": \"" + "1" + "\",");
                    }
                    else
                    {
                        sb.Append("\"Gender\": \"" + "2" + "\",");
                    }
                    if (Sector.Equals("I"))
                    {
                        sb.Append("\"PassportNo\": \"" + dr["PpNumber"].ToString().Trim() + "\",");
                        sb.Append("\"PassportExpiry\": \"" + dr["PPExpirayDate"].ToString().Trim() + "\",");
                    }
                    else
                    {
                        sb.Append("\"PassportNo\": \"" + "" + "\",");
                        sb.Append("\"PassportExpiry\": \"" + "" + "\",");
                    }


                    sb.Append("\"AddressLine1\": \"" + CompanyName + "\",");
                    sb.Append("\"AddressLine2\": \"" + CityName + "\",");
                    sb.Append("\"City\": \"" + CityName + "\",");
                    sb.Append("\"CountryCode\": \"" + CountryCode + "\",");
                    sb.Append("\"CountryName\": \"" + CountryName + "\",");
                    sb.Append("\"ContactNo\": \"" + MobileNo + "\",");
                    sb.Append("\"Email\": \"" + Email + "\",");


                    if (iPax.Equals(0))
                    {
                        sb.Append("\"IsLeadPax\": \"" + "true" + "\",");
                    }
                    else
                    {
                        sb.Append("\"IsLeadPax\": \"" + "false" + "\",");
                    }

                    if (dr["FFN"].ToString().Trim().Length > 0)
                    {
                        string Code = "";
                        string Number = "";
                        GetFFN(Companyid, BookingRef, dr["FFN"].ToString().Trim(), FltType, out Code, out Number);

                        if (Code.Length > 0 && Number.Length > 0)
                        {
                            sb.Append("\"FFAirline\": \"" + Code + "\",");
                            sb.Append("\"FFNumber\": \"" + Number + "\",");
                        }
                        else
                        {
                            sb.Append("\"FFAirline\": \"" + "" + "\",");
                            sb.Append("\"FFNumber\": \"" + "" + "\",");
                        }
                    }
                    else
                    {
                        sb.Append("\"FFAirline\": \"" + "" + "\",");
                        sb.Append("\"FFNumber\": \"" + "" + "\",");
                    }


                    if (GSTCompanyAddress.Length > 0 && GSTCompanyContactNumber.Length > 0 && GSTCompanyName.Length > 0 && GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0)
                    {
                        sb.Append("\"GSTCompanyAddress\": \"" + GSTCompanyAddress + "\",");
                        sb.Append("\"GSTCompanyContactNumber\": \"" + GSTCompanyContactNumber + "\",");
                        sb.Append("\"GSTCompanyName\": \"" + GSTCompanyName + "\",");
                        sb.Append("\"GSTNumber\": \"" + GSTNumber + "\",");
                        sb.Append("\"GSTCompanyEmail\": \"" + GSTCompanyEmail + "\",");
                    }

                    sb.Append("\"Fare\":");
                    sb.Append("{");

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["AdtTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Adt_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("CHD"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["ChdTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_TF"].ToString()) + "\",");
                        sb.Append("\"YQTax\": \"" + Convert.ToInt32(dtBound.Rows[0]["Chd_YQ"].ToString()) + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }
                    else if (dr["PaxType"].ToString().Trim().Equals("INF"))
                    {
                        sb.Append("\"BaseFare\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalBasic"].ToString()) + "\",");
                        sb.Append("\"Tax\": \"" + Convert.ToInt32(dtBound.Rows[0]["InfTotalTax"].ToString()) + "\",");
                        sb.Append("\"TransactionFee\": \"" + 0 + "\",");
                        sb.Append("\"YQTax\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeeOfrd\": \"" + 0 + "\",");
                        sb.Append("\"AdditionalTxnFeePub\": \"" + 0 + "\",");
                        sb.Append("\"AirTransFee\": \"" + 0 + "\"");
                    }

                    bool IsMeal = false;
                    if (FltType.Equals("O"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                    }
                    else if (FltType.Equals("I"))
                    {
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                    }
                    else if (FltType.Equals("C"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            IsMeal = true;
                        }
                    }

                    if (IsMeal)
                    {
                        sb.Append("},");
                    }
                    else
                    {
                        sb.Append("}");
                    }

                    if (FltType.Equals("O"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            sb.Append("\"Meal\":");
                            sb.Append("{");
                            sb.Append("\"Code\": \"" + dr["MealCode_O"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + dr["MealDesc_O"].ToString().Trim() + "\"");
                            sb.Append("}");
                        }
                    }
                    else if (FltType.Equals("I"))
                    {
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            sb.Append("\"Meal\":");
                            sb.Append("{");
                            sb.Append("\"Code\": \"" + dr["MealCode_I"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + dr["MealDesc_I"].ToString().Trim() + "\"");
                            sb.Append("}");
                        }
                    }
                    else if (FltType.Equals("C"))
                    {
                        if (dr["MealCode_O"].ToString().Trim().Length > 2)
                        {
                            sb.Append("\"Meal\":");
                            sb.Append("{");
                            sb.Append("\"Code\": \"" + dr["MealCode_O"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + dr["MealDesc_O"].ToString().Trim() + "\"");
                            sb.Append("}");
                        }
                        if (dr["MealCode_I"].ToString().Trim().Length > 2)
                        {
                            sb.Append("\"Meal\":");
                            sb.Append("{");
                            sb.Append("\"Code\": \"" + dr["MealCode_I"].ToString().Trim() + "\",");
                            sb.Append("\"Description\": \"" + dr["MealDesc_I"].ToString().Trim() + "\"");
                            sb.Append("}");
                        }
                    }

                    sb.Append("},");
                    iPax++;
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append("}");
            }
            catch (Exception ex)
            {
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetGDSpnrRequest", "air_tbo-GetApiRequests", sb.ToString(), Searchid, errorMessage); ;
            }
            return sb.ToString();
        }
        private void GetFFN(string CompanyID, int BookingRef, string FFN, string FltType, out string Code, out string Number)
        {
            Code = string.Empty;
            Number = string.Empty;

            try
            {
                if (FFN.IndexOf("#") != -1)
                {
                    string[] s = FFN.Split('#');

                    if (FltType.Equals("O"))
                    {
                        string[] s1 = s[0].ToString().Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                    else
                    {
                        string[] s1 = s[1].ToString().Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                }
                else
                {
                    if (FFN.IndexOf("-") != -1)
                    {
                        string[] s1 = FFN.Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static Hashtable getSSRdetail(string SSRdetail)
        {
            Hashtable SSRht = new Hashtable();
            try
            {
                string FlightNumber = string.Empty;
                string AirlineCode = string.Empty;

                string Description = string.Empty;
                string WayType = string.Empty;
                string Weight = string.Empty;
                string Currency = string.Empty;
                string Origin = string.Empty;
                string Destination = string.Empty;
                string AirlineDescription = string.Empty;

                string[] split = SSRdetail.Split('-');
                for (int i = 0; i < split.Length; i++)
                {
                    string[] split1 = split[i].ToString().Split(':');
                    if (split1.Length.Equals(2))
                    {
                        if (split1[0].ToString().IndexOf("DESC") != -1)
                        {
                            Description = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("WT") != -1)
                        {
                            WayType = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("AD") != -1)
                        {
                            AirlineDescription = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("CU") != -1)
                        {
                            Currency = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("OR") != -1)
                        {
                            Origin = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("DE") != -1)
                        {
                            Destination = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("W") != -1)
                        {
                            Weight = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("AC") != -1)
                        {
                            AirlineCode = split1[1].ToString();
                        }
                        else if (split1[0].ToString().IndexOf("FN") != -1)
                        {
                            FlightNumber = split1[1].ToString();
                        }
                    }
                }

                SSRht.Add("AirlineCode", AirlineCode);
                SSRht.Add("FlightNumber", FlightNumber);

                SSRht.Add("Description", Description);
                SSRht.Add("WayType", WayType);
                SSRht.Add("Weight", Weight);
                SSRht.Add("Currency", Currency);
                SSRht.Add("Origin", Origin);
                SSRht.Add("Destination", Destination);
                SSRht.Add("AirlineDescription", AirlineDescription);
            }
            catch
            {

            }
            return SSRht;
        }

        //BOOKING DETAILS======================================================================================================================================
        public string GetBookingDetailRequest(string Tokenid, string PNR, string BookingId, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");
            sb.Append("\"PNR\": \"" + PNR + "\",");
            sb.Append("\"BookingId\": \"" + BookingId + "\"");
            sb.Append("}");

            return sb.ToString();
        }
        public string GetBookingDetailRequest(string Tokenid, string Traceid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");
            sb.Append("\"Traceid\": \"" + Traceid + "\"");
            sb.Append("}");

            return sb.ToString();
        }

        //public string GetBookingDetailRequest(string Tokenid, string PNR, string FirstName, string LastName)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("{");
        //    //sb.Append("\"TokenId\":\"" + Tokenid + "\",");
        //    sb.Append("\"PNR\": \"" + PNR + "\",");
        //    sb.Append("\"FirstName\": \"" + FirstName + "\",");
        //    sb.Append("\"LastName\": \"" + LastName + "\"");
        //    sb.Append("}");

        //    return sb.ToString();
        //}
        //CAALENDER DETAILS======================================================================================================================================
        public string GetFareCalenderRequest(string Tokenid, string Origin, string Destination, string BeginDate, string Cabin, string EndUserIp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.Append("\"EndUserIp\": \"" + EndUserIp + "\",");
            sb.Append("\"TokenId\":\"" + Tokenid + "\",");
            sb.Append("\"JourneyType\": \"" + "1" + "\",");
            sb.Append("\"PreferredAirlines\": \"" + null + "\",");

            sb.Append("\"Segments\":[");
            sb.Append("{");
            sb.Append("\"Origin\": \"" + Origin + "\",");
            sb.Append("\"Destination\":\"" + Destination + "\",");
            sb.Append("\"FlightCabinClass\": \"" + GetApiCommonFunctions.GetCabinClass(Cabin) + "\",");
            sb.Append("\"PreferredDepartureTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\",");
            sb.Append("\"PreferredArrivalTime\":\"" + GetApiCommonFunctions.GetApiRequestDateFormat(BeginDate) + "T00: 00: 00" + "\"");
            sb.Append("}");
            sb.Append("],");
            sb.Append("\"Sources\":null");
            //sb.Append("\"Sources\": \"" + null + "\"");
            sb.Append("}");

            return sb.ToString();
        }
    }
}
