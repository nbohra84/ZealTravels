using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Asn1.Cms;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Xml;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.AirlineManagement.Handler;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Application.Handlers;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Common;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Helper.Flight;
using ZealTravel.Front.Web.Models.Flight;
using ZealTravel.Infrastructure.Akaasha;
using log4net.Core;
using ZealTravel.Application.DBCommonManagement.Commands;
using ZealTravel.Front.Web.Helper;

namespace ZealTravel.Front.Web.Controllers
{
    [Authorize]
    public class FlightMulticityController : AgencyBaseController
    {
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private readonly IConfiguration _configuration;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly IHandlesQueryAsync<IsDomesticFlightSearchQuery, bool> _isDomesticFlightSearchHandler;
        private readonly IHandlesQueryAsync<SearchFlightAvailabilityQuery, string> _SearchFlightAvailabilityHandler;
        private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> _getCompanyRegisterCorporateUserDetailsQueryHandler;
        private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> _getCompanyRegisterCorporateUserLimitQueryHandler;
        private readonly IHandlesQueryAsync<GetAirFareQuery, string> _getAirFareHandler;
        private readonly IHandlesQueryAsync<GetAirFareRulesQuery, string> _getAirFareRulesHandler;
        private IHandlesCommandAsync<AddDBLogCommand> _addDbLogCommandHandler;
        private readonly ILogger<FlightMulticityController> _logger;


        public FlightMulticityController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler,
            IConfiguration configuration, IHandlesQueryAsync<CompanyIdByAccountIdQuery,
            string> getCompanyIDByAccountIDQueryHandler,
            IHandlesQueryAsync<IsDomesticFlightSearchQuery, bool> isDomesticFlightSearchHandler,
            IHandlesQueryAsync<SearchFlightAvailabilityQuery, string> SearchFlightAvailabilityHandler,
            IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler,
            IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler,
            IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler,
            ILogger<FlightMulticityController> logger,
            IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler, IHandlesCommandAsync<AddDBLogCommand> addDbLogCommandHandler) : base(getAvailableBalanceQueryHandler)
        {
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _configuration = configuration;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _isDomesticFlightSearchHandler = isDomesticFlightSearchHandler;
            _SearchFlightAvailabilityHandler = SearchFlightAvailabilityHandler;
            _getCompanyRegisterCorporateUserDetailsQueryHandler = getCompanyRegisterCorporateUserDetailsQueryHandler;
            _getCompanyRegisterCorporateUserLimitQueryHandler = getCompanyRegisterCorporateUserLimitQueryHandler;
            _getAirFareHandler = getAirFareHandler;
            _getAirFareRulesHandler = getAirFareRulesHandler;
            _addDbLogCommandHandler = addDbLogCommandHandler;
            _logger = logger;

        }

        [Route("/flight/multicity")]
        public async Task<IActionResult> Multicity()
        {

            if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null))
            {
                return RedirectToAction("Index", "Flight");
            }
            var multicityResponse = await MulticityGetPageValues();
            return View("~/Views/Flight/Multicity.cshtml", multicityResponse);
           
        }

        private async Task<MulticityResponse> MulticityGetPageValues()
        {
            var multicityResponse = new MulticityResponse();
            var oneAccountId = string.Empty;
            var onePSQXML = string.Empty;
            multicityResponse.HostName = _configuration["SiteURL:BasePath"];
            multicityResponse.URL = HttpContext.Request.Host.Value;
            multicityResponse.CompanyId = string.Empty;

            if (string.IsNullOrEmpty(HttpContext.Request.Query["value"]) && User != null && User.Identity.IsAuthenticated && (_configuration["Company:IsBO"].Equals("0")))
            {
                multicityResponse.CompanyId = UserHelper.GetCompanyID(User);
            }
            else
            {
                oneAccountId = EncodeDecodeHelper.DecodeFrom64(HttpContext.Request.Query["value"].ToString().Trim());
                if (!string.IsNullOrEmpty(oneAccountId))
                {
                    multicityResponse.AccountNumber = oneAccountId;
                    var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(oneAccountId) };
                    multicityResponse.CompanyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
                }
            }

            if (HttpContext.Session.GetString("PSQ") != null)
            {
                int _SrNo = 0;
                if (HttpContext.Session.GetString("CurrSrNo") == null || HttpContext.Session.GetString("CurrSrNo") == ""
                    || HttpContext.Session.GetString("CurrSrNo") == "0")
                {
                    _SrNo = 1;
                    HttpContext.Session.SetString("CurrSrNo", _SrNo.ToString());
                }
                else
                {
                    _SrNo = Convert.ToInt16(HttpContext.Session.GetString("CurrSrNo") ?? "1");
                }
                
                multicityResponse.CurrSrNo = _SrNo.ToString();

                List<clsPSQBO> _l = new List<clsPSQBO>();
                _l.Add(((List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList")).Where(x => x._SrNo == _SrNo).FirstOrDefault());
                clsPSQBORequest clsPSQBORequest = new clsPSQBORequest();
                clsPSQBORequest.objclsPSQ = _l;
                HttpContext.Session.SetComplexObject("OriginalPSQ", clsPSQBORequest);

                if (HttpContext.Session.GetString("OriginalPSQ") != null)
                {
                    var originalPSQ = System.Text.Json.JsonSerializer.Deserialize<clsPSQBORequest>(HttpContext.Session.GetString("OriginalPSQ"));
                    if (originalPSQ != null)
                    {
                        var obj = originalPSQ.objclsPSQ;
                        var arrdep = obj[0]._DepartureStation.Split(',');
                        var arrArr = obj[0]._ArrivalStation.Split(',');
                        var DepartureStation = arrdep[0];
                        var ArrivalStation = arrArr[0];
                        multicityResponse.AdultsCount = obj[0]._NoOfAdult;
                        multicityResponse.ChildrenCount = obj[0]._NoOfChild;
                        multicityResponse.InfantsCount = obj[0]._NoOfInfant;
                        multicityResponse.OriginCity = obj[0]._DepartureStation;
                        multicityResponse.ArrivalCity = obj[0]._ArrivalStation;
                        multicityResponse.DepartureDate = obj[0]._BeginDate;
                        multicityResponse.DepartureStation = obj[0]._DepartureStation;
                        multicityResponse.ArrivalStation = obj[0]._ArrivalStation;
                        multicityResponse.PsqXMl = HttpUtility.HtmlDecode(obj[0]._Multisector);

                        

                        StringBuilder lsPSQBOList_ = new StringBuilder();
                        int TheCounter = 0;
                        lsPSQBOList_.Append("[");
                        var list__ = ((List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList"));
                        foreach (clsPSQBO o1 in list__)
                        {
                            TheCounter++;
                            lsPSQBOList_.Append("{");
                            lsPSQBOList_.Append("\"_Multisector\" : \"" + o1._Multisector + "\",");
                            lsPSQBOList_.Append("\"_DepartureStation\" : \"" + o1._DepartureStation + "\",");
                            lsPSQBOList_.Append("\"_ArrivalStation\" : \"" + o1._ArrivalStation + "\",");
                            lsPSQBOList_.Append("\"_BeginDate\" : \"" + o1._BeginDate + "\",");
                            lsPSQBOList_.Append("\"_EndDate\" : \"" + o1._EndDate + "\",");
                            lsPSQBOList_.Append("\"_NoOfAdult\" : \"" + o1._NoOfAdult + "\",");
                            lsPSQBOList_.Append("\"_NoOfChild\" : \"" + o1._NoOfChild + "\",");
                            lsPSQBOList_.Append("\"_NoOfInfant\" : \"" + o1._NoOfInfant + "\",");
                            lsPSQBOList_.Append("\"_SearchType\" : \"" + o1._SearchType + "\",");
                            lsPSQBOList_.Append("\"_PreferedAirlines\" : \"" + o1._PreferedAirlines + "\",");
                            lsPSQBOList_.Append("\"_TravelClass\" : \"" + o1._TravelClass + "\",");
                            lsPSQBOList_.Append("\"_Place\" : \"" + o1._Place + "\",");
                            lsPSQBOList_.Append("\"_Currency\" : \"" + o1._Currency + "\",");
                            lsPSQBOList_.Append("\"_SpecialFare\" : \"" + o1._SpecialFare + "\",");
                            lsPSQBOList_.Append("\"_SrNo\" : \"" + o1._SrNo + "\"");
                            lsPSQBOList_.Append("}");

                            if (TheCounter != list__.Count())
                            {
                                lsPSQBOList_.Append(",");
                            }

                        }
                        lsPSQBOList_.Append("]");
                        multicityResponse.PSQBOList = lsPSQBOList_.ToString();

                        multicityResponse.Cabin = obj[0]._TravelClass.Equals("C") ? "Business" : "Economy";
                        multicityResponse.CityDeparture = arrdep[1];
                        multicityResponse.CityArrival = arrArr[1];

                        var searchdate = DateHelper.EightDIgit2DateFormat(obj[0]._BeginDate);
                        multicityResponse.CalenderVisibleDate = Convert.ToDateTime(searchdate);
                        multicityResponse.CalenderSelectedDate = Convert.ToDateTime(searchdate);

                        var sb = new StringBuilder();
                        sb.Append("<table>");
                        sb.Append("<tr><td style='padding-left:20px;font-size: 14px;'> Adult </td><td style='padding-left:20px;font-size: 14px;'> Child </td><td style='padding-left:20px;font-size: 14px;'> Infant </td></tr>");
                        sb.Append("<tr><td style='padding-left:30px'>" + obj[0]._NoOfAdult + "</td>");
                        sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfChild) > 0 ? obj[0]._NoOfChild : "---") + "</td>");
                        sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfInfant) > 0 ? obj[0]._NoOfInfant : "---") + "</td></tr>");
                        sb.Append("</table>");
                        multicityResponse.LblNoofPax = sb.ToString();

                        if (HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList") != null)
                        {
                            string FinalResultFirstList__ = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").Where(x => x._SrNo == _SrNo).FirstOrDefault().FinalResultFirstList;
                            if (FinalResultFirstList__ == null || FinalResultFirstList__ == "")
                            {
                                HttpContext.Session.SetString("FinalResult", HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").Where(x => x._SrNo == _SrNo).FirstOrDefault().FinalResult  ?? String.Empty);

                            }
                            else
                            {
                                HttpContext.Session.SetString("FinalResult", HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").Where(x => x._SrNo == _SrNo).FirstOrDefault().FinalResultFirstList);
                            }
                            //hdnCurrSrNo.Value= HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").OrderByDescending(x => x._SrNo).FirstOrDefault()._SrNo.ToString();
                            multicityResponse.CurrSrNo = _SrNo.ToString();
                        }
                        else
                        {
                            multicityResponse.CurrSrNo = "1";
                        }

                        StringBuilder tableHtml = new StringBuilder();
                        tableHtml.Append("<table id=tblBookSummary class='modify_top'>");
                        // Loop to add rows and cells


                        foreach (clsPSQBO o in (List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList"))
                        {

                            string[] arrdep1 = o._DepartureStation.Split(',');
                            string[] arrArr1 = o._ArrivalStation.Split(',');
                            string _FlightName = "--";
                            string _SelectedFlightRefId = "";
                            string _GrossF = "0";
                            string _Stop = "";

                            SessionResult4MC sessionResult4MC;
                            if (HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList") != null)
                            {
                             
                                sessionResult4MC = new SessionResult4MC();
                                sessionResult4MC = ((List<SessionResult4MC>)HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList")).Where(x => x._SrNo == o._SrNo).FirstOrDefault();
                                if (sessionResult4MC != null && sessionResult4MC.SelectedFltOut != null && sessionResult4MC.SelectedFltOut != "")
                                {
                                   
                                    string Outbound = string.Empty;
                                    TravellerInformationHelper.SetXmlNode(sessionResult4MC.SelectedFltOut, sessionResult4MC.FinalResult, out Outbound);
                                    GetFlightDtls(Outbound, out _FlightName, out _GrossF, out _Stop);
                                    _SelectedFlightRefId = sessionResult4MC.SelectedFltOut;

                                }
                            }

                            tableHtml.Append("<tr> ");
                            tableHtml.Append("<td id=bookSummaryNavg" + o._SrNo + " class='modify_captions one' style='width:30%;'>");
                            tableHtml.Append("<a href='javascript: void(0);' class='mobilemodi' onclick='CallNextFlights(" + o._SrNo + ");'><span class='depboxfi'>" + arrdep1[0] + "</span><span class='mydkduq'> <i class='fa fa-plane planeki3'></i>  </span><span class='depboxfi yte'>" + arrArr1[0] + "</span></a>");
                            tableHtml.Append("</td>");
                            tableHtml.Append("<td class='modify_captions two'>");
                            tableHtml.Append("<span class='kdgrw'><i class='fa fa-calendar' style='font-size: 14px;'></i> <span class='trdat'>Date: </span></span>  " + o._BeginDate);
                            tableHtml.Append("</td>");
                            tableHtml.Append("<td id=bookSummary" + o._SrNo + " class='modify_captions two'>");
                            tableHtml.Append("<span class='resultbox4'>" + _FlightName + "<span class='resultbox4' style='font-size:6pt;color:#00008B;'>(" + _Stop + ")</span> | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" + _GrossF + "</span>");
                            tableHtml.Append("</td>");
                            tableHtml.Append("<td class='modify_captions three'>");
                            if (o._SrNo == 1)
                            {
                                tableHtml.Append(sb.ToString());
                            }
                            else if (o._SrNo == 2)
                            {
                                tableHtml.Append("<a href = 'javascript:void (0);' onclick='showmodifystatus();'><span id='pldt1' style='display: block;'>+</span><span id = 'mldt1' style='display: none;'>-</span> modify search</a>");
                            }
                            tableHtml.Append("</td>");
                            tableHtml.Append("<td class='modify_captions four'>");
                            if (o._SrNo == 1)
                            {
                                tableHtml.Append("<a href = 'javascript:void (0);' onclick='Continue();' class='btn-primary-red mywaves'>Continue</a>");
                            }
                            tableHtml.Append("</td>");
                            tableHtml.Append("<td id=selectedFlightRefId" + o._SrNo + " style='display:none;'>");
                            tableHtml.Append("<span>" + _SelectedFlightRefId + "</span>");
                            tableHtml.Append("</td>");


                            tableHtml.Append("</tr>");
                        }
                        tableHtml.Append("</table>");
                        multicityResponse.TableHtml = tableHtml.ToString();
                    }
                }

                onePSQXML = HttpContext.Session.GetString("PSQ");
                multicityResponse.PreferredAirline = onePSQXML;
                multicityResponse.TotalCities = HttpContext.Session.GetString("TotalCities");
            }
            return multicityResponse;
        }

        private async Task<string> GetvalueForRequestMC([FromBody] clsPSQBORequest model)
        {
            // Clear all session values
            var result = string.Empty;
            var Output = string.Empty;
            try
            {

                var sessionKeys = new[]
                {
            "FinalResult", "SearchID", "SelectedFltOut",
            "SelectedFltIn", "PaxXML", "OriginalHotlPSQ", "BOOK_HOTEL",
            "hoteldata", "hotel", "room", "block", "hotelblock",
            "hotelinfo", "paxdetail", "addre", "BOOK", "SearchValue",
            "Guest", "OriginalPSQ", "PSQ"
                };

                foreach (var key in sessionKeys)
                {
                    HttpContext.Session.Remove(key);
                }



                // Store original PSQ if needed
                if (model != null && model.objclsPSQ.Any())
                {

                    var searchValue = string.Empty;
                    HttpContext.Session.SetString("OriginalPSQ", System.Text.Json.JsonSerializer.Serialize(model));

                    string xmlstring = SearchFlightRequestHelper.PSQXMLString(model.objclsPSQ);
                    clsPSQBO objGetAllValue = model.objclsPSQ[0];

                    if (User == null || !User.Identity.IsAuthenticated)
                    {
                        HttpContext.Session.SetString("accidC", "1");
                    }

                    string searchType = objGetAllValue._SearchType;
                    HttpContext.Session.SetString("PSQ", xmlstring);


                    var arrdep = CommonFunction.GetStringInBetween(objGetAllValue._DepartureStation, "(", ")", false, false);
                    string departureStation = arrdep[0];

                    var arrArv = CommonFunction.GetStringInBetween(objGetAllValue._ArrivalStation, "(", ")", false, false);
                    string arrivalStation = arrArv[0];

                    var isDomestic = await _isDomesticFlightSearchHandler.HandleAsync(new IsDomesticFlightSearchQuery { Origin = departureStation, Destination = arrivalStation });
                    string sector = isDomestic ? "D" : "I";
                    string place = objGetAllValue._Place;
                    if (objGetAllValue._Place.Equals("H") || objGetAllValue._Place.Equals("I"))
                    {
                        place = "HOME";
                    }
                    else if (objGetAllValue._Place.Equals("M"))
                    {
                        place = "MODIFY";
                    }
                    string currency = string.Empty;
                    if (objGetAllValue._Currency != null)
                    {
                        currency = objGetAllValue._Currency.Trim();
                    }
                    else
                    {
                        currency = "INR";
                    }

                    HttpContext.Session.SetString("Curr", currency);
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("Curr")) || HttpContext.Session.GetString("Curr").Equals("INR"))
                    {
                        HttpContext.Session.SetString("dCurrency", "1");
                    }
                    else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Curr")))
                    {
                        HttpContext.Session.SetString("dCurrency", await CurrencyHelper.GetCurrencyConvertToINR(currency));
                    }
                    else
                    {
                        HttpContext.Session.SetString("dCurrency", "1");
                    }

                    string companyId = objGetAllValue._companyId;
                    int accountID = 0;
                    if (_configuration["Company:IsBO"].Equals("0"))
                    {
                        companyId = User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                        accountID = User.Identity.IsAuthenticated ? UserHelper.GetStaffAccountID(User) : 0;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(companyId))
                        {
                            int.TryParse(companyId, out accountID);
                            if (accountID > 0)
                            {
                                var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = accountID };
                                companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
                            }
                        }
                    }
                    var query = new SearchFlightAvailabilityQuery();
                    query.SearchID = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString(); ;
                    var dataresult = string.Empty;
                    if (searchType == "O")
                    {

                        query.CompanyID = companyId;
                        query.AirRQ = xmlstring;
                        query.JourneyType = "OW";
                        dataresult = await _SearchFlightAvailabilityHandler.HandleAsync(query);

                        HttpContext.Session.SetString("SearchID", query.SearchID);
                        if (!string.IsNullOrEmpty(dataresult) && dataresult.IndexOf("RefID") != -1)
                        {
                            result = "O";
                            HttpContext.Session.SetString("FinalResult", dataresult);
                            HttpContext.Session.SetString("SearchValue", "OW");
                        }
                        else
                        {
                            result = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
                //log error

            }
            return result;
        }

        [HttpPost]
        [Route("/flight/GetvalueForRequestMulticity")]
        public async Task<IActionResult> GetvalueForRequestMulticity([FromBody] clsPSQBORequest model)
        {
            // Clear all session values
            var result = string.Empty;
            var Output = string.Empty;
            try
            {

                var sessionKeys = new[]
                {
                    "clsPSQBOList", "FlightOutBoundList", "SessionResult4MCList"
                };

                foreach (var key in sessionKeys)
                {
                    HttpContext.Session.Remove(key);
                }


                // Store original PSQ if needed
                if (model != null && model.objclsPSQ.Any())
                {

                    HttpContext.Session.SetString("clsPSQBOList", System.Text.Json.JsonSerializer.Serialize(model.objclsPSQ));
                    HttpContext.Session.SetString("TotalCities", model.objclsPSQ.Count.ToString());
                    HttpContext.Session.SetString("CurrSrNo", "1");

                    var sessionResult4MCList = new List<SessionResult4MC>();
                    SessionResult4MC sessionResult;
                    List<clsPSQBO> lstClsPSQBO;
                    int _SrNo = 1;
                    for (int i = 2; i <= model.objclsPSQ.Count(); i++)
                    {
                        sessionResult = new SessionResult4MC();
                        sessionResult._SrNo = i;
                        lstClsPSQBO = new List<clsPSQBO>();
                        lstClsPSQBO.Add(model.objclsPSQ.Where(x => x._SrNo == i).FirstOrDefault());
                        sessionResult.OriginalPSQ.objclsPSQ = lstClsPSQBO;
                        sessionResult4MCList.Add(sessionResult);
                    }
                    lstClsPSQBO = new List<clsPSQBO>();
                    lstClsPSQBO.Add(((List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList")).Where(x => x._SrNo == _SrNo).FirstOrDefault());


                    HttpContext.Session.SetString("CurrSrNo", _SrNo.ToString());

                    var clsPSQBORequest = new clsPSQBORequest();
                    clsPSQBORequest.objclsPSQ = lstClsPSQBO;
                    Output = await GetvalueForRequestMC(clsPSQBORequest);

                    if (Output.Trim().Equals(string.Empty))
                    {
                        result = string.Empty;
                    }
                    else
                    {
                        result = "O"; // Output;
                        sessionResult4MCList.Add(GetCurrSessionResult());
                        HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Empty;
                //log error

            }
            return Ok(new { d = result });
        }

        [HttpPost]
        [Route("/flight/GetvalueForRequestMulticityBySrNo")]
        public async Task<IActionResult> GetvalueForRequestMulticityBySrNo([FromBody] GetvalueForRequestMulticityRequest getvalueForRequestMulticityRequest)
        {
            string result = "";
            string Output = "";
            //string SelectedFlightRefId = "";
            int _SrNo = getvalueForRequestMulticityRequest._SrNo;
            int _IsNewReuslt = getvalueForRequestMulticityRequest._IsNewReuslt;
            var sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            SessionResult4MC exIstVal = sessionResult4MCList != null ? sessionResult4MCList.Where(x => x._SrNo == _SrNo && x.FinalResult != null && x.FinalResult != "").FirstOrDefault() :  new SessionResult4MC();
            
            if (exIstVal == null)
            {
                exIstVal = sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault();
            }
            else
            {
                await DBLoggerHelper.DBLogAsync("", 0, "GetvalueForRequestMulticityBySrNo", "", "", "", "Second time fetch data Start => " + DateTime.Now.ToLongTimeString(), _addDbLogCommandHandler);
            }

            HttpContext.Session.SetString("CurrSrNo", _SrNo.ToString());
            if (_IsNewReuslt == 1 || exIstVal == null || exIstVal.FinalResult == null)
            {
                List<clsPSQBO> lstclsPSQBO = new List<clsPSQBO>();
                lstclsPSQBO.Add(((List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList")).Where(x => x._SrNo == _SrNo).FirstOrDefault());
                clsPSQBORequest clsPSQBORequest = new clsPSQBORequest();
                clsPSQBORequest.objclsPSQ = lstclsPSQBO;
                HttpContext.Session.SetComplexObject("OriginalPSQ", clsPSQBORequest);
                if (exIstVal != null)
                {
                    sessionResult4MCList.Remove(exIstVal);
                }
                //result = GetvalueForRequest(_l, true);

                string companyId, xmlstring, sector, place, host;
                int accountID = 0;

                xmlstring = SearchFlightRequestHelper.PSQXMLString(lstclsPSQBO);
                clsPSQBO objGetAllValue = lstclsPSQBO[0];


                var arrdep = CommonFunction.GetStringInBetween(objGetAllValue._DepartureStation, "(", ")", false, false);
                string departureStation = arrdep[0];

                var arrArv = CommonFunction.GetStringInBetween(objGetAllValue._ArrivalStation, "(", ")", false, false);
                string arrivalStation = arrArv[0];

                var isDomestic = await _isDomesticFlightSearchHandler.HandleAsync(new IsDomesticFlightSearchQuery { Origin = departureStation, Destination = arrivalStation });
                sector = isDomestic ? "D" : "I";
                place = objGetAllValue._Place;
                if (objGetAllValue._Place.Equals("H") || objGetAllValue._Place.Equals("I"))
                {
                    place = "HOME";
                }
                else if (objGetAllValue._Place.Equals("M"))
                {
                    place = "MODIFY";
                }

               
                companyId = objGetAllValue._companyId;
                
                if (_configuration["Company:IsBO"].Equals("0"))
                {
                    companyId = User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                    accountID = User.Identity.IsAuthenticated ? UserHelper.GetStaffAccountID(User) : 0;
                }
                else
                {
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        int.TryParse(companyId, out accountID);
                        if (accountID > 0)
                        {
                            var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = accountID };
                            companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
                        }
                    }
                }

                DateTime strTime = DateTime.Now.AddMinutes(10);
                while (strTime > DateTime.Now)
                {
                    if (sessionResult4MCList != null || sessionResult4MCList.Count() > 0)
                    {
                        break;
                    }
                }
                if (sessionResult4MCList == null || sessionResult4MCList.Count() < 1)
                {
                    result =  "O";
                }

                string SearchValue = sessionResult4MCList.Where(x => x._SrNo == 1).FirstOrDefault().SearchValue;
                var query = new SearchFlightAvailabilityQuery();
                var searchID = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString(); ;
                query.SearchID = searchID;
                var dataresult = string.Empty;
                
                query.CompanyID = companyId;
                query.AirRQ = xmlstring;
                query.JourneyType = "OW";
                dataresult = await _SearchFlightAvailabilityHandler.HandleAsync(query);

                if (!string.IsNullOrEmpty(dataresult) && dataresult.IndexOf("RefID") != -1)
                {
                    if (sessionResult4MCList.Where(x => x._SrNo == _SrNo).Count() > 0)
                    {
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().OriginalPSQ = clsPSQBORequest;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().FinalResult = dataresult;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().SearchValue = SearchValue;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().SearchID = searchID;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().PSQ = xmlstring;
                    }
                    else
                    {
                        SessionResult4MC _sessionResult4MC = new SessionResult4MC();
                        _sessionResult4MC._SrNo = _SrNo;
                        _sessionResult4MC.OriginalPSQ = clsPSQBORequest;
                        _sessionResult4MC.FinalResult = dataresult;
                        _sessionResult4MC.SearchValue = SearchValue;
                        _sessionResult4MC.SearchID = searchID;
                        _sessionResult4MC.PSQ = xmlstring;
                        sessionResult4MCList.Add(_sessionResult4MC);
                    }


                    exIstVal = new SessionResult4MC();
                    exIstVal = sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault();


                    SetSessionResult2CurrSession(exIstVal);
                    HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);
                    result = exIstVal.FinalResult;

                    HttpContext.Session.SetString("FinalResult", exIstVal.FinalResult ?? "");
                    HttpContext.Session.SetString("StopCheck", exIstVal.StopCheck ?? "");
                }
                else
                {
                    result = string.Empty;
                    HttpContext.Session.SetString("FinalResult", "");
                    HttpContext.Session.Remove("StopCheck");
                }
            }
            else
            {
                if (exIstVal.FinalResultFirstList == null || exIstVal.FinalResultFirstList == "")
                {
                    HttpContext.Session.SetString("FinalResult", exIstVal.FinalResult);
                    HttpContext.Session.SetString("StopCheck", exIstVal.StopCheck ?? "");
                }
                else
                {
                    HttpContext.Session.SetString("FinalResult", exIstVal.FinalResultFirstList);
                    HttpContext.Session.SetString("StopCheck", exIstVal.StopCheck ?? "");
                    exIstVal.FinalResult = exIstVal.FinalResultFirstList;

                }
                SetSessionResult2CurrSession(exIstVal);
                HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);
                result = exIstVal.FinalResult;

                await DBLoggerHelper.DBLogAsync("", 0, "GetvalueForRequestMulticityBySrNo", "", "", "", "Second time fetch data End => " + DateTime.Now.ToLongTimeString() , _addDbLogCommandHandler );
                // dbCommon.Logger.dbLogg("", 0, "GetvalueForRequestMulticityBySrNo", "", "", "", "Second time fetch data End => " + DateTime.Now.ToLongTimeString());
            }
            
            if (result == "")
            {
                return Ok(new { d = string.Empty });
            }
            
            return Ok(new { d = "O" });
        }

        [HttpPost]
        [Route("/flight/ShowOneWayMC")]
        public async Task<IActionResult> ShowOneWayMC([FromBody] ShowOneWayMCRequest showOneWayMCRequest)//MC
        {
            string _d = "";
            string CompanyID = showOneWayMCRequest.CompanyID;
            int _CurrSubPageNo = showOneWayMCRequest._CurrSubPageNo;

            var sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            var resultPageList = new Dictionary<int, List<k_ShowFlightOutBound>>();
            if (_CurrSubPageNo == 0)
            {
                resultPageList =  await GetOneWayMC(CompanyID, sessionResult4MCList);
            }
            _CurrSubPageNo++;
            sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            List<k_ShowFlightOutBound> flightOutBound = new List<k_ShowFlightOutBound>();
            flightOutBound = resultPageList.Where(x => x.Key == _CurrSubPageNo).SelectMany(y => y.Value).ToList();


            int _PageFroRemove = _CurrSubPageNo;
            if (_CurrSubPageNo == resultPageList.OrderByDescending(x => x.Key).First().Key)
            {
                _CurrSubPageNo = 999;
            }
            resultPageList.Remove(_PageFroRemove);

            _d = _CurrSubPageNo.ToString("000") + JsonSerializer.Serialize(flightOutBound);

            int _SrNo = int.Parse(HttpContext.Session.GetString("CurrSrNo") ?? "0");
            sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().StopCheck = HttpContext.Session.GetString("StopCheck") ?? "";
            HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);
            var comp = Compress(_d);
            return Ok(new { d = comp });
            
        }

        [HttpPost]
        [Route("/flight/UpdateAvailablityBackGroundMC")]
        public async Task<IActionResult> UpdateAvailablityBackGroundMC()
        {

           
            int _SrNo = 0;
            int _TotalCities = Convert.ToInt16(HttpContext.Session.GetString("TotalCities") ?? "0");
            int _CurrSrNo = Convert.ToInt16(HttpContext.Session.GetString("CurrSrNo") ?? "0");
            var sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");

            for (int i = (_CurrSrNo + 1); i <= _TotalCities; i++)
            {
                if (sessionResult4MCList.Where(x => x._SrNo == i).FirstOrDefault().FinalResult == null
                    || sessionResult4MCList.Where(x => x._SrNo == i).FirstOrDefault().FinalResult == "")
                {
                    _SrNo = i;
                    break;
                }
            }

            if (_SrNo == 0)
            {
                return Ok();
            }

            if (_SrNo > 0)
            {

                List<clsPSQBO> lstclsPSQBO = new List<clsPSQBO>();
                lstclsPSQBO.Add(((List<clsPSQBO>)HttpContext.Session.GetComplexObject<List<clsPSQBO>>("clsPSQBOList")).Where(x => x._SrNo == _SrNo).FirstOrDefault());
                clsPSQBORequest clsPSQBORequest = new clsPSQBORequest();
                clsPSQBORequest.objclsPSQ = lstclsPSQBO;

                string companyId, xmlstring, sector, place, Host;
                int accountID= 0;

                xmlstring = SearchFlightRequestHelper.PSQXMLString(lstclsPSQBO);
                clsPSQBO objGetAllValue = lstclsPSQBO[0];


                var arrdep = CommonFunction.GetStringInBetween(objGetAllValue._DepartureStation, "(", ")", false, false);
                string departureStation = arrdep[0];

                var arrArv = CommonFunction.GetStringInBetween(objGetAllValue._ArrivalStation, "(", ")", false, false);
                string arrivalStation = arrArv[0];

                var isDomestic = await _isDomesticFlightSearchHandler.HandleAsync(new IsDomesticFlightSearchQuery { Origin = departureStation, Destination = arrivalStation });
                sector = isDomestic ? "D" : "I";
                place = objGetAllValue._Place;
                if (objGetAllValue._Place.Equals("H") || objGetAllValue._Place.Equals("I"))
                {
                    place = "HOME";
                }
                else if (objGetAllValue._Place.Equals("M"))
                {
                    place = "MODIFY";
                }


                companyId = objGetAllValue._companyId;

                if (_configuration["Company:IsBO"].Equals("0"))
                {
                    companyId = User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                    accountID = User.Identity.IsAuthenticated ? UserHelper.GetStaffAccountID(User) : 0;
                }
                else
                {
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        int.TryParse(companyId, out accountID);
                        if (accountID > 0)
                        {
                            var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = accountID };
                            companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
                        }
                    }
                }

                DateTime strTime = DateTime.Now.AddMinutes(10);
                while (strTime > DateTime.Now)
                {
                    if (sessionResult4MCList != null || sessionResult4MCList.Count() > 0)
                    {
                        break;
                    }
                }
                if (sessionResult4MCList == null || sessionResult4MCList.Count() < 1)
                {
                    return Ok();
                }

                string SearchValue = sessionResult4MCList.Where(x => x._SrNo == 1).FirstOrDefault().SearchValue;
                var query = new SearchFlightAvailabilityQuery();
                var searchID = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString(); ;
                query.SearchID = searchID;
                var dataresult = string.Empty;

                query.CompanyID = companyId;
                query.AirRQ = xmlstring;
                query.JourneyType = "OW";
                dataresult = await _SearchFlightAvailabilityHandler.HandleAsync(query);

                if (!string.IsNullOrEmpty(dataresult) && dataresult.IndexOf("RefID") != -1)
                {
                    if (sessionResult4MCList.Where(x => x._SrNo == _SrNo).Count() > 0)
                    {
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().OriginalPSQ = clsPSQBORequest;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().FinalResult = dataresult;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().SearchValue = SearchValue;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().SearchID = searchID;
                        sessionResult4MCList.Where(x => x._SrNo == _SrNo).FirstOrDefault().PSQ = xmlstring;
                    }
                    else
                    {
                        SessionResult4MC _sessionResult4MC = new SessionResult4MC();
                        _sessionResult4MC._SrNo = _SrNo;
                        _sessionResult4MC.OriginalPSQ = clsPSQBORequest;
                        _sessionResult4MC.FinalResult = dataresult;
                        _sessionResult4MC.SearchValue = SearchValue;
                        _sessionResult4MC.SearchID = searchID;
                        _sessionResult4MC.PSQ = xmlstring;
                        sessionResult4MCList.Add(_sessionResult4MC);
                    }
                }

                HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);

            }
            return Ok();
        }

        [HttpPost]
        [Route("/flight/GetSelectedFlightGrossFare")]
        public async Task<IActionResult> GetSelectedFlightGrossFare([FromBody] GetSelectedFlightGrossFareRequest request)
        {

            string totFare = "";

            List<SessionResult4MC> sessionResult4MCList = new List<SessionResult4MC>();
            if (HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList") != null)
            {
                sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            }

            HttpContext.Session.SetString("SelectedFltOut", request.SelectedFlightRefId);
            string Outbound = string.Empty;
            string Inbound = string.Empty;
            TravellerInformationHelper.SetXmlNode(out Outbound, out Inbound, request.SelectedFlightRefId, sessionResult4MCList.Where(x => x._SrNo == request.SrNo).FirstOrDefault().FinalResult);

            string t1_, t2_, t3_;
            totFare = GetFlightDtls(Outbound, out t1_, out t2_, out t3_);

            //-------- set the selected flight
            sessionResult4MCList.Where(x => x._SrNo == request.SrNo).FirstOrDefault().SelectedFltOut = request.SelectedFlightRefId;
            HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);

            return Ok(new { d = totFare });
        }

        [HttpPost]
        [Route("/flight/SelectFlightOneWayMC")]
        public async Task<IActionResult> SelectFlightOneWayMC(string CompanyID)
        {

            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();
            string companyID = CompanyID;
            var sessionResult4MCList = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            int SrNo_ = 0;

            if (HttpContext.Session.GetString("CurrSrNo") == null || HttpContext.Session.GetString("CurrSrNo") == ""
                || HttpContext.Session.GetString("CurrSrNo") == "0")
            {
                SrNo_ = 1;
                HttpContext.Session.SetString("CurrSrNo", SrNo_.ToString());
            }
            else
            {
                SrNo_ = Convert.ToInt16(HttpContext.Session.GetString("CurrSrNo"));
            }


            List<k_ShowFlightOutBound> _l = new List<k_ShowFlightOutBound>();
            try
            {
                if (_configuration["Company:IsBO"].Equals("0"))
                {
                    companyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;

                }

                foreach (SessionResult4MC oMc in sessionResult4MCList)
                {
                    if (oMc.FinalResultFirstList == null || oMc.FinalResultFirstList == "")
                    {
                        HttpContext.Session.SetString("FinalResult", oMc.FinalResult);
                        oMc.FinalResultFirstList = oMc.FinalResult;
                    }
                    else
                    {
                        HttpContext.Session.SetString("FinalResult", oMc.FinalResultFirstList);
                    }

                    HttpContext.Session.SetString("SelectedFltOut", oMc.SelectedFltOut);
                    HttpContext.Session.SetString("PSQ", oMc.PSQ);

                    _l = await ShowFlightDataHelper.SelectOneway("OUTBOUND", "DOM", oMc.SelectedFltOut, companyID, User, _getAirFareHandler, _getAirFareRulesHandler, _getCompanyRegisterCorporateUserDetailsQueryHandler, _getCompanyRegisterCorporateUserLimitQueryHandler);
                    foreach (k_ShowFlightOutBound o in _l)
                    {
                        o._SrNo = oMc._SrNo;
                        FlightOutBound.Add((k_ShowFlightOutBound)o);
                    }
                    oMc.FinalResult = HttpContext.Session.GetString("FinalResult");   // this time fate calclation add some extra values for reauslt page that why re-assgn it
                }

                HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCList);

            }
            catch (Exception ex)
            {

            }
            HttpContext.Session.SetComplexObject("FlightOutBoundList", FlightOutBound);

            HttpContext.Session.SetString("FinalResult", sessionResult4MCList.Where(x => x._SrNo == SrNo_).FirstOrDefault().FinalResult);
            HttpContext.Session.SetString("SelectedFltOut", sessionResult4MCList.Where(x => x._SrNo == SrNo_).FirstOrDefault().SelectedFltOut);

            return Ok(new { d = FlightOutBound.OrderBy(x => x._SrNo).ToList() });
        }


        private SessionResult4MC GetCurrSessionResult()
        {
            SessionResult4MC _aessionResult4MC = new SessionResult4MC();
            _aessionResult4MC._SrNo = Convert.ToInt16(HttpContext.Session.GetString("CurrSrNo") ?? "0");
            _aessionResult4MC.FinalResult = HttpContext.Session.GetString("FinalResult") ?? "";
            _aessionResult4MC.SearchID = HttpContext.Session.GetString("SearchID") ?? "";
            _aessionResult4MC.SelectedFltOut = HttpContext.Session.GetString("SelectedFltOut") ?? "";
            _aessionResult4MC.SelectedFltIn = null;   //HttpContext.Session.GetString("SelectedFltIn") ?? "";
            _aessionResult4MC.PaxXML = HttpContext.Session.GetString("PaxXML") ?? "";
            _aessionResult4MC.OriginalHotlPSQ = HttpContext.Session.GetString("OriginalHotlPSQ") ?? "";
            _aessionResult4MC.BOOK_HOTEL = HttpContext.Session.GetString("BOOK_HOTEL") ?? "";
            _aessionResult4MC.hoteldata = HttpContext.Session.GetString("hoteldata") ?? "";
            _aessionResult4MC.hotel = HttpContext.Session.GetString("hotel") ?? "";
            _aessionResult4MC.room = HttpContext.Session.GetString("room") ?? "";
            _aessionResult4MC.block = HttpContext.Session.GetString("block") ?? "";
            _aessionResult4MC.hotelblock = HttpContext.Session.GetString("hotelblock") ?? "";
            _aessionResult4MC.hotelinfo = HttpContext.Session.GetString("hotelinfo") ?? "";
            _aessionResult4MC.paxdetail = HttpContext.Session.GetString("paxdetail") ?? "";
            _aessionResult4MC.addre = HttpContext.Session.GetString("addre") ?? "";
            _aessionResult4MC.BOOK = HttpContext.Session.GetString("BOOK") ?? "";
            _aessionResult4MC.SearchValue = HttpContext.Session.GetString("SearchValue") ?? "";
            _aessionResult4MC.Guest = HttpContext.Session.GetString("Guest") ?? "";
            _aessionResult4MC.OriginalPSQ = HttpContext.Session.GetComplexObject<clsPSQBORequest> ("OriginalPSQ") ?? new clsPSQBORequest();
            _aessionResult4MC.PSQ = HttpContext.Session.GetString("PSQ") ?? "";
            _aessionResult4MC.StopCheck = HttpContext.Session.GetString("StopCheck") ?? "";
            return _aessionResult4MC;
        }

        private async Task<Dictionary<int, List<k_ShowFlightOutBound>>> GetOneWayMC(string companyID, List<SessionResult4MC> sessionResult4MCs)//MC
        {


            int SrNo_ = Convert.ToInt16(HttpContext.Session.GetString("CurrSrNo"));
            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();
            if ((sessionResult4MCs.Where(x => x._SrNo == SrNo_).FirstOrDefault().ShowFlightOutBoundList).Count() > 0)
            {
               await DBLoggerHelper.DBLogAsync("", 0, "ShowOneWayMC", "", "", "", "Get from memory Start => " + DateTime.Now.ToLongTimeString(), _addDbLogCommandHandler);
                FlightOutBound = sessionResult4MCs.Where(x => x._SrNo == SrNo_).FirstOrDefault().ShowFlightOutBoundList;  //.Take(5).ToList();
               await DBLoggerHelper.DBLogAsync("", 0, "ShowOneWayMC", "", "", "", "Get from memory End => " + DateTime.Now.ToLongTimeString(), _addDbLogCommandHandler);
            }
            else
            {
                try
                {
                    if (_configuration["Company:IsBO"].Equals("0"))
                    {
                        companyID = User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                    }
                   await DBLoggerHelper.DBLogAsync("", 0, "ShowOneWayMC", "", "", "", "set ShowData List Start => " + DateTime.Now.ToLongTimeString(), _addDbLogCommandHandler);
                    FlightOutBound = await ShowFlightDataHelper.ResultDataAsync("OUTBOUND", "DOM", companyID);
                    sessionResult4MCs.Where(x => x._SrNo == SrNo_).FirstOrDefault().ShowFlightOutBoundList = FlightOutBound;
                   await DBLoggerHelper.DBLogAsync("", 0, "ShowOneWayMC", "", "", "", "set ShowData List End => " + DateTime.Now.ToLongTimeString() , _addDbLogCommandHandler);
                }
                catch (Exception ex)
                {

                }
            }
           

            int _index = 0;
            int _page = 0;
            List<k_ShowFlightOutBound> _l = new List<k_ShowFlightOutBound>();
            var resultPageList = new Dictionary<int, List<k_ShowFlightOutBound>>();
            if (FlightOutBound.Count() < 2000)
            {
                resultPageList.Add(1, FlightOutBound);
            }
            else
            {
                foreach (var o in FlightOutBound)
                {
                    _l.Add(o);
                    _index++;
                    if (_index > 5)
                    {
                        _page++;
                        _index = 0;

                        resultPageList.Add(_page, new List<k_ShowFlightOutBound>(_l));
                        _l.Clear();
                        _l = new List<k_ShowFlightOutBound>();

                    }

                }
                if (_index > 0)  /// if last remaing qty
                {
                    _page++;
                    _index = 0;
                    resultPageList.Add(_page, new List<k_ShowFlightOutBound>(_l));
                    _l.Clear();
                    _l = new List<k_ShowFlightOutBound>();

                }
            }

            HttpContext.Session.SetComplexObject("SessionResult4MCList", sessionResult4MCs);
            return resultPageList; 
        }

        private void SetSessionResult2CurrSession(SessionResult4MC _aessionResult4MC)
        {
            HttpContext.Session.SetString("CurrSrNo", _aessionResult4MC._SrNo.ToString());
            HttpContext.Session.SetString("FinalResult", _aessionResult4MC.FinalResult ?? "");
            HttpContext.Session.SetString("SearchID", _aessionResult4MC.SearchID);
            HttpContext.Session.SetString("SelectedFltOut", _aessionResult4MC.SelectedFltOut ?? "");
            HttpContext.Session.Remove("SelectedFltIn");  // _aessionResult4MC.SelectedFltIn;
            HttpContext.Session.SetString("PaxXML", _aessionResult4MC.PaxXML ?? "");
            HttpContext.Session.SetString("OriginalHotlPSQ", _aessionResult4MC.OriginalHotlPSQ ?? "");
            HttpContext.Session.SetString("BOOK_HOTEL", _aessionResult4MC.BOOK_HOTEL ?? "");
            HttpContext.Session.SetString("hoteldata", _aessionResult4MC.hoteldata ?? "");
            HttpContext.Session.SetString("hotel", _aessionResult4MC.hotel ?? "");
            HttpContext.Session.SetString("room", _aessionResult4MC.room ?? "");
            HttpContext.Session.SetString("block", _aessionResult4MC.block ?? "");
            HttpContext.Session.SetString("hotelblock", _aessionResult4MC.hotelblock ?? "");
            HttpContext.Session.SetString("hotelinfo", _aessionResult4MC.hotelinfo ?? "");
            HttpContext.Session.SetString("paxdetail", _aessionResult4MC.paxdetail ?? "");
            HttpContext.Session.SetString("addre", _aessionResult4MC.addre ?? "");
            HttpContext.Session.SetString("BOOK", _aessionResult4MC.BOOK ?? "");
            HttpContext.Session.SetString("SearchValue", _aessionResult4MC.SearchValue);
            HttpContext.Session.SetString("Guest", _aessionResult4MC.Guest ?? "");
            HttpContext.Session.SetComplexObject("OriginalPSQ", _aessionResult4MC.OriginalPSQ);
            HttpContext.Session.SetString("PSQ", _aessionResult4MC.PSQ);

            HttpContext.Session.SetString("StopCheck", _aessionResult4MC.StopCheck ?? "");

        }

        public byte[] Compress(string _Text)
        {
          
            byte[] bytes = Encoding.UTF8.GetBytes(_Text);
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }
                return memoryStream.ToArray();
            }

        }

        private static string GetFlightDtls(string Outbound, out string _FlightName, out string _GrossF, out string _Stop)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Outbound);

            string _totFare = "";
            string FlightName = "";
            int TotalFare = 0;
            int TotalTds = 0;
            int TotalCommission = 0;
            int TotalCommission_SA = 0;
            int Stops = 0;
            foreach (XmlNode node in xmlDoc.SelectNodes("//AvailabilityResponseOut"))
            {
                if (node != null)
                {
                    if (Stops == 0)
                    {
                        FlightName = node.SelectSingleNode("//CarrierCode").InnerText;
                        TotalFare += Convert.ToInt32(node.SelectSingleNode("//TotalFare").InnerText);
                        TotalTds += Convert.ToInt32(node.SelectSingleNode("//TotalTds").InnerText);
                        TotalCommission += Convert.ToInt32(node.SelectSingleNode("//TotalCommission").InnerText);
                        TotalCommission_SA += Convert.ToInt32(node.SelectSingleNode("//TotalCommission_SA").InnerText);
                    }
                    Stops++;
                }

            }
            _totFare = ((TotalFare - TotalTds) + TotalCommission + TotalCommission_SA).ToString("#,###");
            _FlightName = FlightName;
            _GrossF = _totFare;
            _Stop = Stops.ToString();
            return _totFare;
        }
    }
}
