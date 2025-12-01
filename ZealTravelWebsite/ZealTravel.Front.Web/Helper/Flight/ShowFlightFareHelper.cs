using System.Data;
using System.Web;
using System.Xml;
using ZealTravel.Application.AirlineManagement.Handler;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models.Flight;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class ShowFlightFareHelper
    {
        private readonly IHandlesQueryAsync<GetAirFareQuery, string> _getAirFareHandler;
        private readonly IHandlesQueryAsync<GetAirFareRulesQuery, string> _getAirFareRulesHandler;

        public ShowFlightFareHelper()
        {
        }
        public ShowFlightFareHelper(IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler)
        {
            _getAirFareHandler = getAirFareHandler;
            _getAirFareRulesHandler = getAirFareRulesHandler;
        }
        public static async Task<ShowFlightGetfareResponse> GetFare(string CompanyID, string RefID, string FltType, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler , IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler)
        {
            bool FareStatus = false;
            string AvailabilityResponse = string.Empty;
            string SearchID = string.Empty;
            string FareRemark = string.Empty;
            try
            {

                AvailabilityResponse = HttpContextHelper.Current?.Session.GetString("FinalResult").ToString();
                SearchID = HttpContextHelper.Current?.Session.GetString("SearchID").ToString();
                string FareRule =  await GetFareRule(SearchID, CompanyID, AvailabilityResponse, RefID, FltType, getAirFareRulesHandler);

                var showFlightFareHelper = new ShowFlightFareHelper();
                string SelectedAvailabilityResponse = await GetAirFare(SearchID, CompanyID, AvailabilityResponse, RefID, FltType, getAirFareHandler);
                if (SelectedAvailabilityResponse.IndexOf("RefID") != -1)
                {
                    SelectedAvailabilityResponse = HttpUtility.HtmlDecode(SelectedAvailabilityResponse);

                   
                    AvailabilityResponse = FlightUpdateAvailabilityHelper.UpdateAvailability(SearchID, CompanyID, AvailabilityResponse, SelectedAvailabilityResponse, FltType, FareRule);

                    HttpContextHelper.Current.Session.SetString("FinalResult", AvailabilityResponse);

                    FareStatus =await StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
                    if (!FareStatus)
                    {
                        FareRemark = "flight is not available, kindly select another flight";
                    }
                }
                else if (FareRule.Trim().Length > 3)
                {
                    
                    AvailabilityResponse = FlightUpdateAvailabilityHelper.UpdateAvailabilityFareRule(SearchID, CompanyID, AvailabilityResponse, RefID, FltType, FareRule);

                    HttpContextHelper.Current.Session.SetString("FinalResult", AvailabilityResponse);

                    FareStatus = await StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
                    if (!FareStatus)
                    {
                        FareRemark = "flight is not available, kindly select another flight";
                    }
                }
                else
                {
                    if (await IsDepartures(AvailabilityResponse, RefID))
                    {
                        FareStatus = true;
                    }
                    else
                    {
                        FareRemark = "kindly search again or select another flights";
                    }
                }
            }
            catch (Exception ex)
            {
              //  dbCommon.Logger.dbLogg(CompanyID, 0, "GetFare", "clsFareUpdate", AvailabilityResponse, SearchID, ex.Message);
            }

            return new ShowFlightGetfareResponse
            {
                FareStatus = FareStatus,
                FareRemark = FareRemark
            };
            
        }
        public static async Task<ShowFlightGetfareResponse> GetFareCO(string CompanyID, string RefID, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler)
        {
            bool FareStatus = false;
            var FareRemark = string.Empty;
            string AvailabilityResponse = string.Empty;
            string SearchID = string.Empty;
            try
            {


                AvailabilityResponse = HttpContextHelper.Current?.Session.GetString("FinalResult").ToString();
                SearchID = HttpContextHelper.Current?.Session.GetString("SearchID").ToString();

                string FareRule = await GetFareRuleCO(SearchID, CompanyID, AvailabilityResponse, RefID, getAirFareRulesHandler);

              
                string SelectedAvailabilityResponse = await GetAirFareCO(SearchID, CompanyID, AvailabilityResponse, RefID, getAirFareHandler);
                if (SelectedAvailabilityResponse.IndexOf("RefID") != -1)
                {
                    SelectedAvailabilityResponse = HttpUtility.HtmlDecode(SelectedAvailabilityResponse);
                    AvailabilityResponse = FlightUpdateAvailabilityHelper.UpdateAvailabilityCO(SearchID, CompanyID, AvailabilityResponse, SelectedAvailabilityResponse, FareRule);
                    HttpContextHelper.Current.Session.SetString("FinalResult", AvailabilityResponse);
                    FareStatus = await StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
                    if(!FareStatus)
                    {
                        FareRemark = "flight is not available, kindly select another flight";
                    }
                }
                else if (FareRule.Trim().Length > 3)
                {
                    AvailabilityResponse = FlightUpdateAvailabilityHelper.UpdateAvailabilityFareRule(SearchID, CompanyID, AvailabilityResponse, RefID, "", FareRule);
                    HttpContextHelper.Current.Session.SetString("FinalResult", AvailabilityResponse);
                    FareStatus = await StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
                    if (!FareStatus)
                    {
                        FareRemark = "flight is not available, kindly select another flight";
                    }
                }
                else
                {
                    FareRemark = "kindly search again or select another flights";
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareCO", "clsFareUpdate", AvailabilityResponse, SearchID, ex.Message);
            }

            return new ShowFlightGetfareResponse
            {
                FareStatus = FareStatus,
                FareRemark = FareRemark
            };
        }
        //public bool GetFareRT(string CompanyID, string RefID_O, string RefID_I)
        //{
        //    bool FareStatus = false;
        //    FareRemark = string.Empty;
        //    string AvailabilityResponse = string.Empty;
        //    string SearchID = string.Empty;

        //    try
        //    {
        //        if (HttpContext.Current.Session["SearchID"] == null || (HttpContext.Current.Session["accid"] == null && HttpContext.Current.Session["accidC"] == null))
        //        {
        //            HttpContext.Current.Response.Redirect("Index.aspx");
        //        }

        //        AvailabilityResponse = HttpContext.Current.Session["FinalResult"].ToString();
        //        SearchID = HttpContext.Current.Session["SearchID"].ToString();

        //        Air_Svc_Farerule objRule = new Air_Svc_Farerule();
        //        string FareRule = objRule.GetFareRuleRT(SearchID, CompanyID, AvailabilityResponse, RefID_O, RefID_I);

        //        Air_Svc_Fare objfare = new Air_Svc_Fare();
        //        string SelectedAvailabilityResponse = objfare.GetFareRT(SearchID, CompanyID, AvailabilityResponse, RefID_O, RefID_I);
        //        if (SelectedAvailabilityResponse.IndexOf("RefID") != -1)
        //        {
        //            SelectedAvailabilityResponse = HttpUtility.HtmlDecode(SelectedAvailabilityResponse);

        //            Air_Updater objupdater = new Air_Updater();
        //            AvailabilityResponse = objupdater.UpdateAvailabilityRT(SearchID, CompanyID, AvailabilityResponse, SelectedAvailabilityResponse, RefID_O, "O", FareRule);
        //            AvailabilityResponse = objupdater.UpdateAvailabilityRT(SearchID, CompanyID, AvailabilityResponse, SelectedAvailabilityResponse, RefID_I, "I", FareRule);
        //            HttpContext.Current.Session["FinalResult"] = AvailabilityResponse;
        //            FareStatus = StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID_O);
        //        }
        //        else if (FareRule.Trim().Length > 3)
        //        {
        //            Air_Updater objupdater = new Air_Updater();
        //            AvailabilityResponse = objupdater.UpdateAvailability_FareRule(SearchID, CompanyID, AvailabilityResponse, RefID_O, "O", FareRule);
        //            AvailabilityResponse = objupdater.UpdateAvailability_FareRule(SearchID, CompanyID, AvailabilityResponse, RefID_I, "I", FareRule);
        //            HttpContext.Current.Session["FinalResult"] = AvailabilityResponse;
        //            FareStatus = StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID_O);
        //        }
        //        else
        //        {
        //            FareRemark = "kindly search again or select another flights";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "GetFare", "clsFareUpdate", AvailabilityResponse, SearchID, ex.Message);
        //    }
        //    return FareStatus;
        //}
        //public bool GetFareMC(string CompanyID, string RefID)
        //{
        //    bool FareStatus = false;
        //    FareRemark = string.Empty;
        //    string AvailabilityResponse = string.Empty;
        //    string SearchID = string.Empty;
        //    try
        //    {
        //        if (HttpContext.Current.Session["SearchID"] == null || (HttpContext.Current.Session["accid"] == null && HttpContext.Current.Session["accidC"] == null))
        //        {
        //            HttpContext.Current.Response.Redirect("Index.aspx");
        //        }

        //        AvailabilityResponse = HttpContext.Current.Session["FinalResult"].ToString();
        //        SearchID = HttpContext.Current.Session["SearchID"].ToString();

        //        Air_Svc_Farerule objRule = new Air_Svc_Farerule();
        //        string FareRule = objRule.GetFareRuleMC(SearchID, CompanyID, AvailabilityResponse, RefID);

        //        Air_Svc_Fare objfare = new Air_Svc_Fare();
        //        string SelectedAvailabilityResponse = objfare.GetFareMC(SearchID, CompanyID, AvailabilityResponse, RefID);
        //        if (SelectedAvailabilityResponse.IndexOf("RefID") != -1)
        //        {
        //            SelectedAvailabilityResponse = HttpUtility.HtmlDecode(SelectedAvailabilityResponse);

        //            Air_Updater objupdater = new Air_Updater();
        //            AvailabilityResponse = objupdater.UpdateAvailabilityCO(SearchID, CompanyID, AvailabilityResponse, SelectedAvailabilityResponse, FareRule);
        //            HttpContext.Current.Session["FinalResult"] = AvailabilityResponse;
        //            FareStatus = StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
        //        }
        //        else if (FareRule.Trim().Length > 3)
        //        {
        //            Air_Updater objupdater = new Air_Updater();
        //            AvailabilityResponse = objupdater.UpdateAvailability_FareRule(SearchID, CompanyID, AvailabilityResponse, RefID, "O", FareRule);
        //            HttpContext.Current.Session["FinalResult"] = AvailabilityResponse;
        //            FareStatus = StatusFareQuote(SearchID, CompanyID, AvailabilityResponse, RefID);
        //        }
        //        else
        //        {
        //            FareRemark = "kindly search again or select another flights";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareMC", "clsFareUpdate", AvailabilityResponse, SearchID, ex.Message);
        //    }
        //    return FareStatus;
        //}

        public static async Task<string> GetFareRule(string SearchID, string CompanyID, string AvailabilityResponse, string RefID, string FltType, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler)
        {
            string FareRule = string.Empty;
            string SelectedAvailabilityResponse = string.Empty;
            try
            {
                if (FltType.Equals("O"))
                {
                    
                    SelectedAvailabilityResponse = GetSelectedResponse(RefID, "", AvailabilityResponse, false);
                }
                else
                {
                   
                    SelectedAvailabilityResponse = GetSelectedResponse("", RefID, AvailabilityResponse, false);
                }

                string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
                //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);

                //svc_air.AirServiceClient objFR = new svc_air.AirServiceClient();

                var getAirFareRulesQuery = new GetAirFareRulesQuery
                {
                    AirRS = SelectedAvailabilityResponse,
                    CompanyID = CompanyID,
                    JourneyType = "OW",
                    SearchID = SearchID
                };

                FareRule = await getAirFareRulesHandler.HandleAsync(getAirFareRulesQuery);
                //objFR.Close();


                //air_api_collector.GetFareRule objFR = new air_api_collector.GetFareRule();
                //FareRule = objFR.GetAirFareRules("OW", SearchID, CompanyID, SelectedAvailabilityResponse);




                FareRule = RemoveHTMLbr(FareRule);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareRule", "clsFareRule", SelectedAvailabilityResponse, SearchID, ex.Message);
            }
            return FareRule;
        }
        public static async Task<string> GetFareRuleCO(string SearchID, string CompanyID, string AvailabilityResponse, string RefID, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler)
        {
            string FareRule = string.Empty;
            string SelectedAvailabilityResponse = string.Empty;
            try
            {
                
                SelectedAvailabilityResponse = GetSelectedResponse(RefID, "", AvailabilityResponse, true);

                string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
                //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);


                var getAirFareRulesQuery = new GetAirFareRulesQuery
                {
                    AirRS = SelectedAvailabilityResponse,
                    CompanyID = CompanyID,
                    JourneyType = "RT",
                    SearchID = SearchID
                };

                FareRule = await getAirFareRulesHandler.HandleAsync(getAirFareRulesQuery);
                FareRule = RemoveHTMLbr(FareRule);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareRuleCO", "clsFareRule", SelectedAvailabilityResponse, SearchID, ex.Message);
            }
            return FareRule;
        }

        //public string GetFareRuleRT(string SearchID, string CompanyID, string AvailabilityResponse, string RefID_O, string RefID_I)
        //{
        //    string FareRule = string.Empty;
        //    string SelectedAvailabilityResponse = string.Empty;
        //    try
        //    {
        //        Air_Svc_Ssr objSelect = new Air_Svc_Ssr();
        //        SelectedAvailabilityResponse = objSelect.GET_SelectedResponse(RefID_O, RefID_I, AvailabilityResponse, false);

        //        string Host = HttpContext.Current.Request.Url.Host + "," + HttpContext.Current.Request.UserHostAddress;
        //        dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);

        //        //svc_air.AirServiceClient objFR = new svc_air.AirServiceClient();
        //        AirService objFR = new AirService();
        //        FareRule = objFR.GetAirFareRules("RTLCC", SearchID, CompanyID, SelectedAvailabilityResponse);
        //        //objFR.Close();

        //        //air_api_collector.GetFareRule objFR = new air_api_collector.GetFareRule();
        //        //FareRule = objFR.GetAirFareRules("RTLCC", SearchID, CompanyID, SelectedAvailabilityResponse);

        //        FareRule = RemoveHTMLbr(FareRule);
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareRuleRT", "clsFareRule", SelectedAvailabilityResponse, SearchID, ex.Message);
        //    }
        //    return FareRule;
        //}
        //public string GetFareRuleMC(string SearchID, string CompanyID, string AvailabilityResponse, string RefID)
        //{
        //    string FareRule = string.Empty;
        //    string SelectedAvailabilityResponse = string.Empty;
        //    try
        //    {
        //        Air_Svc_Ssr objSelect = new Air_Svc_Ssr();
        //        SelectedAvailabilityResponse = objSelect.GET_SelectedResponse(RefID, "", AvailabilityResponse, false);

        //        string Host = HttpContext.Current.Request.Url.Host + "," + HttpContext.Current.Request.UserHostAddress;
        //        dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);

        //        //svc_air.AirServiceClient objFR = new svc_air.AirServiceClient();
        //        AirService objFR = new AirService();
        //        FareRule = objFR.GetAirFareRules("MC", SearchID, CompanyID, SelectedAvailabilityResponse);
        //        //objFR.Close();


        //        //air_api_collector.GetFareRule objFR = new air_api_collector.GetFareRule();
        //        //FareRule = objFR.GetAirFareRules("MC", SearchID, CompanyID, SelectedAvailabilityResponse);


        //        FareRule = RemoveHTMLbr(FareRule);
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareRuleMC", "clsFareRule", SelectedAvailabilityResponse, SearchID, ex.Message);
        //    }
        //    return FareRule;
        //}
        private static string RemoveHTMLbr(string FareRule)
        {
            if (FareRule.IndexOf("<Br/>") != -1)
            {
                FareRule = FareRule.Replace("<Br/>", "").Trim();
            }
            if (FareRule.IndexOf("</Br>") != -1)
            {
                FareRule = FareRule.Replace("</Br>", "").Trim();
            }
            if (FareRule.IndexOf("</BR>") != -1)
            {
                FareRule = FareRule.Replace("</BR>", "").Trim();
            }
            if (FareRule.IndexOf("</br>") != -1)
            {
                FareRule = FareRule.Replace("</br>", "").Trim();
            }
            if (FareRule.IndexOf("<br>") != -1)
            {
                FareRule = FareRule.Replace("<br>", "").Trim();
            }
            if (FareRule.IndexOf("<br/>") != -1)
            {
                FareRule = FareRule.Replace("<br/>", "").Trim();
            }
            if (FareRule.IndexOf("</b>") != -1)
            {
                FareRule = FareRule.Replace("</b>", "").Trim();
            }
            if (FareRule.IndexOf("<b>") != -1)
            {
                FareRule = FareRule.Replace("<b>", "").Trim();
            }

            if (FareRule.IndexOf("<br/>") != -1)
            {
                FareRule = FareRule.Replace("<br/>", "").Trim();
            }
            if (FareRule.IndexOf("&") != -1)
            {
                FareRule = FareRule.Replace("&", "").Trim();
            }
            return FareRule;
        }

        public static string GetSelectedResponse(string RefID_O, string RefID_I, string TResponse, bool IsCombi)
        {
            string SelectedResponse = string.Empty;

            if (IsCombi.Equals(true))
            {
                if (TResponse.Trim() != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(TResponse);
                    XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + "]");
                    SelectedResponse = "<root>";
                    if (xnList1 != null)
                    {
                        foreach (XmlNode node in xnList1)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    SelectedResponse += "</root>";
                }
            }
            else if (RefID_O.Length > 0 && RefID_I.Length > 0)
            {
                if (TResponse.Trim() != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(TResponse);
                    XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + " and FltType='O']");
                    XmlNodeList xnList2 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_I + " and FltType='I']");

                    SelectedResponse = "<root>";
                    if (xnList1 != null)
                    {
                        foreach (XmlNode node in xnList1)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    if (xnList2 != null)
                    {
                        foreach (XmlNode node in xnList2)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    SelectedResponse += "</root>";
                }
            }
            else
            {
                if (RefID_O.Length > 0)
                {
                    if (TResponse.Trim() != "")
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(TResponse);

                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + " and FltType='O']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                }
                else if (RefID_I.Length > 0)
                {
                    if (TResponse.Trim() != "")
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(TResponse);

                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_I + " and FltType='I']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                }
            }
            return SelectedResponse;
        }

        public static async Task<string> GetAirFare(string SearchID, string CompanyID, string AvailabilityResponse, string RefID, string FltType, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler)
        {
            string fareresponse = string.Empty;
            string selectedAvailabilityResponse = string.Empty;
            try
            {
                if (FltType.Equals("O"))
                {

                    selectedAvailabilityResponse = GetSelectedResponse(RefID, "", AvailabilityResponse, false);
                }
                else
                {

                    selectedAvailabilityResponse = GetSelectedResponse("", RefID, AvailabilityResponse, false);
                }

                string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
                //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);

                var getAirFareQuery = new GetAirFareQuery
                {
                    AirRS = selectedAvailabilityResponse,
                    CompanyID = CompanyID,
                    JourneyType = "OW",
                    SearchID = SearchID
                };
                fareresponse = await getAirFareHandler.HandleAsync(getAirFareQuery);
           
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GetFare", "clsFareUpdate", SelectedAvailabilityResponse, SearchID, ex.Message);
            }

            return fareresponse;
        }

        public static async Task<string> GetAirFareCO(string SearchID, string CompanyID, string AvailabilityResponse, string RefID, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler)
        {
            string fareresponse = string.Empty;
            string selectedAvailabilityResponse = string.Empty;
            try
            {
                selectedAvailabilityResponse = GetSelectedResponse(RefID, "", AvailabilityResponse, true);
                string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
                //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "FARE", "POPOPUP-DISPLAY", "FO", SelectedAvailabilityResponse, "", Host);

                var getAirFareQuery = new GetAirFareQuery
                {
                    AirRS = selectedAvailabilityResponse,
                    CompanyID = CompanyID,
                    JourneyType = "RT",
                    SearchID = SearchID
                };

                fareresponse = await getAirFareHandler.HandleAsync(getAirFareQuery);

            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "GetFareCO", "clsFareUpdate", SelectedAvailabilityResponse, SearchID, ex.Message);
            }
            return fareresponse;
        }

        private static async Task<bool> StatusFareQuote(string SearchID, string CompanyID, string AvailabilityResponse, string RefID)
        {
            bool Status = false;

            try
            {
                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(AvailabilityResponse));
                DataRow[] drSelect = dsAvailability.Tables[0].Select("RefID='" + RefID + "'");
                if (drSelect.Length > 0)
                {
                    Status = Convert.ToBoolean(drSelect.CopyToDataTable().Rows[0]["FareStatus"].ToString());
                   
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "StatusFareQuote", "clsFareUpdate", "", SearchID, ex.Message);
            }
            return Status;
        }
        private static async Task<bool> IsDepartures(string AvailabilityResponse, string RefID)
        {
            bool FareStatus = false;

            DataSet dsAvailability = new DataSet();
            dsAvailability.ReadXml(new System.IO.StringReader(AvailabilityResponse));
            DataRow[] drSelect = dsAvailability.Tables[0].Select("RefID='" + RefID + "'");
            if (drSelect.Length > 0)
            {
                string AirlineID = drSelect.CopyToDataTable().Rows[0]["AirlineID"].ToString().Trim();
                if (AirlineID.IndexOf("OWN") != -1)
                {
                    FareStatus = true;
                }
            }
            return FareStatus;
        }

    }
}
