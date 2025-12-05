using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Common.Helpers;
using ZealTravel.Common.Models;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models;
using ZealTravel.Front.Web.Models.Flight;
using ZealTravel.Front.Web.Helper.Flight;
using DocumentFormat.OpenXml.VariantTypes;
using System.Security.Claims;
using ZealTravel.Front.Web.Controllers;
using System.Buffers;
using ZealTravel.Application.UserManagement.Handlers;
using System.Text;
using static System.Net.WebRequestMethods;
using ZealTravel.Front.Web.Models.Flight.SearchFilter;
using Microsoft.AspNetCore.Http.HttpResults;
using ZealTravel.Common;
using ZealTravel.Application.Handlers;
using DocumentFormat.OpenXml.Math;
using System.Xml;
using ZealTravel.Common.Helpers.Flight;
using System.Data;
using Org.BouncyCastle.Asn1.Ocsp;
using ZealTravel.Application.BookingManagement.Query;
using ClosedXML.Excel;
using ZealTravel.Application.CountryManagement.Queries;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Web;
using ZealTravel.Infrastructure.Akaasa;
using ZealTravel.Application.PaymentGatewayManagement.Queries;
using System.Collections;
using ZealTravel.Application.GST_Management.Commands;
using ZealTravel.Application.GST_Management.Handlers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ZealTravel.Application.PaymentGatewayManagement.Handlers;
using Microsoft.AspNetCore.Authorization;
using log4net.Core;
using ZealTravel.Application.GSTManagement.Queries;
using ZealTravel.Application.GSTManagement.Handlers;
using ZealTravel.Application.DBCommonManagement.Commands;
using ZealTravel.Front.Web.Helper;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;
using ZealTravel.Application.BankManagement.Handler;
using DocumentFormat.OpenXml.Bibliography;
using ZealTravel.Application.BookingManagement.Handler;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Services.AirelineManagement;
using AirAvaibilityModel = ZealTravel.Domain.Models.AirAvaibilityModel;
using Microsoft.Extensions.DependencyInjection;

[Authorize]
public class FlightController : AgencyBaseController
{
    private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
    private readonly IHandlesQueryAsync<GetCitySearchTermQuery, List<string>> _getcityListQueryHandler;
    private readonly IHandlesQueryAsync<SearchFlightAvailabilityQuery, string> _SearchFlightAvailabilityHandler;
    private readonly IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> _bookingTicketQueryHandler;
    private readonly IConfiguration _configuration;
    private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
    private readonly IHandlesQueryAsync<IsDomesticFlightSearchQuery, bool> _isDomesticFlightSearchHandler;
    private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> _getCompanyRegisterCorporateUserDetailsQueryHandler;
    private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> _getCompanyRegisterCorporateUserLimitQueryHandler;
    private readonly IHandlesQueryAsync<GetAirFareQuery, string> _getAirFareHandler;
    private readonly IHandlesQueryAsync<GetAirFareRulesQuery, string> _getAirFareRulesHandler;
    private readonly IHandlesQueryAsync<string, string> _gettWhitelabelAdminidFromHostQueryHandler;
    private readonly IHandlesQueryAsync<string, int> _getPromoAmountQueryHandler;
    private readonly IHandlesQueryAsync<IsWalletCfeeQuery, bool> _isWalletCfeeQueryHandler;
    private readonly IHandlesQueryAsync<List<CountryList>> _getCountryListWithCodeQueryHandler;
    private readonly IHandlesQueryAsync<GetAirSSRQuery, string> _getAirSSRHandler;
    private readonly IHandlesQueryAsync<GetPaymentGatewayCardChargesQuery, List<PaymentGatewayDisplayOption>> _getPaymentGatewayCardChargesQueryHandler;
    private readonly IHandlesQueryAsync<VerifyTicketBalanceQuery, bool> _verifyTicketBalanceQueryHandler;
    private readonly IHandlesQueryAsync<string, bool> _getPGOurOwnerStatusQueryHandler;
    private readonly IHandlesCommandAsync<SetGSTdetailCommand> _setGSTdetailCommandHandler;
    private readonly IHandlesQueryAsync<SetBookingQuery, Int32> _setBookingQueryHandler;
    private readonly IHandlesQueryAsync<GetAirCommitQuery, bool> _getAirCommitHandler;
    private readonly IHandlesQueryAsync<GetCfeeQuery, DataTable> _getCFeeQueryHandler;
    private readonly IHandlesQueryAsync<SetPaymentGatewayLoggerQuery, Int32> _setPaymentGatewayLoggerQueryHandler;
    private readonly IHandlesQueryAsync<SetBookingAirlineLogForPGQuery, bool> _setBookingAirlineLogForPGQueryHandler;
    private readonly IHandlesCommandAsync<AddDBLogCommand> _addDbLogCommandHandler;
    private readonly IHandlesCommandAsync<AddDBSearchLogCommand> _addDbSearchLogCommandHandler;
    private readonly IHandlesQueryAsync<string, GSTDetails> _getGSTDetailbyCompanyQueryHandler;
    private readonly IGetOneWayFlightService _oneWayFlightService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger<FlightController> _logger;

    public FlightController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler, IHandlesQueryAsync<GetCitySearchTermQuery, List<string>> getcityListQueryHandler, IHandlesQueryAsync<SearchFlightAvailabilityQuery, string> searchFlightAvailabilityHandler, IConfiguration configuration, IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler, IHandlesQueryAsync<IsDomesticFlightSearchQuery, bool> isDomesticFlightSearchHandler,
        IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler,
        IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler,
        IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler,
        IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler,
        IHandlesQueryAsync<string, string> gettWhitelabelAdminidFromHostQueryHandler,
        IHandlesQueryAsync<string, int> getPromoAmountQueryHandler,
        IHandlesQueryAsync<IsWalletCfeeQuery, bool> isWalletCfeeQueryHandler,
        IHandlesQueryAsync<List<CountryList>> getCountryListWithCodeQueryHandler,
        IHandlesQueryAsync<GetAirSSRQuery, string> getAirSSRHandler,
        IHandlesQueryAsync<GetPaymentGatewayCardChargesQuery, List<PaymentGatewayDisplayOption>> getPaymentGatewayCardChargesQueryHandler,
        IHandlesQueryAsync<VerifyTicketBalanceQuery, bool> verifyTicketBalanceQueryHandler,
        IHandlesQueryAsync<string, bool> getPGOurOwnerStatusQueryHandler,
        IHandlesCommandAsync<SetGSTdetailCommand> setGSTdetailCommandHandler, IHandlesQueryAsync<SetBookingQuery, Int32> setBookingQueryHandler,
        IHandlesQueryAsync<GetAirCommitQuery, bool> getAirCommitHandler, IHandlesQueryAsync<GetCfeeQuery, DataTable> getCFeeQueryHandler,
        IHandlesQueryAsync<SetPaymentGatewayLoggerQuery, Int32> setPaymentGatewayLoggerQueryHandler,
        IHandlesQueryAsync<SetBookingAirlineLogForPGQuery, bool> setBookingAirlineLogForPGQueryHandler,
        IHandlesQueryAsync<string, GSTDetails> getGSTDetailbyCompanyQueryHandler,
        IHandlesCommandAsync<AddDBLogCommand> addDbLogCommandHandler,
        IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> bookingTicketQueryHandler,
        IHandlesCommandAsync<AddDBSearchLogCommand> addDbSearchLogCommandHandler,
        IGetOneWayFlightService oneWayFlightService,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FlightController> logger) : base(getAvailableBalanceQueryHandler)
    {
        _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
        _getcityListQueryHandler = getcityListQueryHandler;
        _SearchFlightAvailabilityHandler = searchFlightAvailabilityHandler;
        _configuration = configuration;
        _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
        _isDomesticFlightSearchHandler = isDomesticFlightSearchHandler;
        _getCompanyRegisterCorporateUserDetailsQueryHandler = getCompanyRegisterCorporateUserDetailsQueryHandler;
        _getCompanyRegisterCorporateUserLimitQueryHandler = getCompanyRegisterCorporateUserLimitQueryHandler;
        _getAirFareHandler = getAirFareHandler;
        _getAirFareRulesHandler = getAirFareRulesHandler;
        _gettWhitelabelAdminidFromHostQueryHandler = gettWhitelabelAdminidFromHostQueryHandler;
        _getPromoAmountQueryHandler = getPromoAmountQueryHandler;
        _isWalletCfeeQueryHandler = isWalletCfeeQueryHandler;
        _getCountryListWithCodeQueryHandler = getCountryListWithCodeQueryHandler;
        _getAirSSRHandler = getAirSSRHandler;
        _getPaymentGatewayCardChargesQueryHandler = getPaymentGatewayCardChargesQueryHandler;
        _verifyTicketBalanceQueryHandler = verifyTicketBalanceQueryHandler;
        _getPGOurOwnerStatusQueryHandler = getPGOurOwnerStatusQueryHandler;
        _setGSTdetailCommandHandler = setGSTdetailCommandHandler;
        _setBookingQueryHandler = setBookingQueryHandler;
        _getAirCommitHandler = getAirCommitHandler;
        _getCFeeQueryHandler = getCFeeQueryHandler;
        _setPaymentGatewayLoggerQueryHandler = setPaymentGatewayLoggerQueryHandler;
        _setBookingAirlineLogForPGQueryHandler = setBookingAirlineLogForPGQueryHandler;
        _getGSTDetailbyCompanyQueryHandler = getGSTDetailbyCompanyQueryHandler;
        _addDbLogCommandHandler = addDbLogCommandHandler;
        _addDbSearchLogCommandHandler = addDbSearchLogCommandHandler;
        _logger = logger;
        _bookingTicketQueryHandler = bookingTicketQueryHandler;
        _oneWayFlightService = oneWayFlightService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IActionResult> Index()
    {
        var flightHome = new FlightHomeResponse();
        flightHome.HostName = _configuration["SiteURL:BasePath"];
        return View(flightHome);
    }

    [HttpGet]
    public async Task<IActionResult> GetSector(SearchCityModel model)
    {
        try
        {
            var obj = new GetCitySearchTermQuery() { SearchTerm = model.SearchTerm };
            var result = await _getcityListQueryHandler.HandleAsync(obj);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> OneWay()
    {

        if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null))
        {
            return RedirectToAction("Index");
        }
        var onewayResponse = await OnewayGetPageValues();

        return View(onewayResponse);
    }

    private async Task<OnewayResponse> OnewayGetPageValues()
    {
        var onewayResponse = new OnewayResponse();
        var oneAccountId = string.Empty;
        var onePSQXML = string.Empty;
        onewayResponse.HostName = _configuration["SiteURL:BasePath"];
        onewayResponse.URL = HttpContext.Request.Host.Value;
        onewayResponse.CompanyId = string.Empty;

        if (string.IsNullOrEmpty(HttpContext.Request.Query["value"]) && User != null && User.Identity.IsAuthenticated && (_configuration["Company:IsBO"].Equals("0")))
        {
            onewayResponse.CompanyId = UserHelper.GetCompanyID(User);
        }
        else
        {
            oneAccountId = EncodeDecodeHelper.DecodeFrom64(HttpContext.Request.Query["value"].ToString().Trim());
            if (!string.IsNullOrEmpty(oneAccountId))
            {
                onewayResponse.AccountNumber = oneAccountId;
                var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(oneAccountId) };
                onewayResponse.CompanyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
            }
        }

        if (HttpContext.Session.GetString("PSQ") != null)
        {
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
                    onewayResponse.AdultsCount = obj[0]._NoOfAdult;
                    onewayResponse.ChildrenCount = obj[0]._NoOfChild;
                    onewayResponse.InfantsCount = obj[0]._NoOfInfant;
                    onewayResponse.OriginCity = obj[0]._DepartureStation;
                    onewayResponse.ArrivalCity = obj[0]._ArrivalStation;
                    onewayResponse.DepartureDate = obj[0]._BeginDate;
                    onewayResponse.DepartureStation = obj[0]._DepartureStation;
                    onewayResponse.ArrivalStation = obj[0]._ArrivalStation;
                    onewayResponse.Cabin = obj[0]._TravelClass.Equals("C") ? "Business" : "Economy";
                    onewayResponse.CityDeparture = arrdep[1];
                    onewayResponse.CityArrival = arrArr[1];
                    onewayResponse.LblModsrcdes = $"<span class='depboxfi'>{DepartureStation}</span><span class='mydkduq'> <i class='fa fa-plane planeki3'></i>  </span><span class='depboxfi yte'>{ArrivalStation}</span>";
                    onewayResponse.LblTravlDate = $"<span class='kdgrw'><i class='fa fa-calendar' style='font-size: 14px;'></i> <span>Onward</span></span> {obj[0]._BeginDate}";
                    var searchdate = DateHelper.EightDIgit2DateFormat(obj[0]._BeginDate);
                    onewayResponse.CalenderVisibleDate = Convert.ToDateTime(searchdate);
                    onewayResponse.CalenderSelectedDate = Convert.ToDateTime(searchdate);

                    var sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr><td style='padding-left:20px;font-size: 14px;'> Adult </td><td style='padding-left:20px;font-size: 14px;'> Child </td><td style='padding-left:20px;font-size: 14px;'> Infant </td></tr>");
                    sb.Append("<tr><td style='padding-left:30px'>" + obj[0]._NoOfAdult + "</td>");
                    sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfChild) > 0 ? obj[0]._NoOfChild : "---") + "</td>");
                    sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfInfant) > 0 ? obj[0]._NoOfInfant : "---") + "</td></tr>");
                    sb.Append("</table>");
                    onewayResponse.LblNoofPax = sb.ToString();
                }
            }

            onePSQXML = HttpContext.Session.GetString("PSQ");
            onewayResponse.PreferredAirline = onePSQXML;
            onewayResponse.CurrencyType = HttpContext.Session.GetString("Curr") ?? "INR";
        }
        return onewayResponse;
    }
    [HttpPost]
    public async Task<IActionResult> GetValueForRequest([FromBody]clsPSQBORequest model)
    {
        // Clear all session values
        var result = string.Empty;
        try
        {
            var sessionKeys = new[]
            {
            "dCurrency", "Curr", "FinalResult", "SearchID", "SelectedFltOut",
            "SelectedFltIn", "PaxXML", "OriginalHotlPSQ", "BOOK_HOTEL",
            "hoteldata", "hotel", "room", "block", "hotelblock",
            "hotelinfo", "paxdetail", "addre", "BOOK", "SearchValue",
            "Guest", "OriginalPSQ", "PSQ" , "SessionResult4MCList"
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
                query.SearchID = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString();
                
                // Determine journey type and search value
                string journeyType = string.Empty;
                if (searchType == "O")
                {
                    query.CompanyID = companyId;
                    query.AirRQ = xmlstring;
                    query.JourneyType = "OW";
                    journeyType = "OW";
                    result = "O";
                    searchValue = "OW";
                    HttpContext.Session.SetString("SEARCH_TYPE", "DOM");
                }
                else if (searchType == "DRT" || searchType == "R")
                {
                    if (sector == "D")
                    {
                        query.CompanyID = companyId;
                        query.AirRQ = xmlstring;
                        query.JourneyType = "RW";
                        journeyType = "RW";
                        result = sector;
                        searchValue = "RW";
                        HttpContext.Session.SetString("SEARCH_TYPE", "DOM");
                    }
                    else
                    {
                        query.CompanyID = companyId;
                        query.AirRQ = xmlstring;
                        query.JourneyType = "RT";
                        journeyType = "RT";
                        result = sector;
                        searchValue = "INT";
                        HttpContext.Session.SetString("SEARCH_TYPE", "INT");
                    }
                }

                // Store search parameters in session
                HttpContext.Session.SetString("SearchID", query.SearchID);
                HttpContext.Session.SetString("SearchValue", searchValue);
                HttpContext.Session.SetString("PSQ", xmlstring);

                // For ONE-WAY only: Start background search and return immediately
                if (searchType == "O")
                {
                    var airModel = new ZealTravel.Domain.Models.AirAvaibilityModel
                    {
                        Searchid = query.SearchID,
                        Companyid = companyId,
                        AirRQ = xmlstring,
                        JourneyType = journeyType,
                        EndUserIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "125.63.102.34"
                    };

                    // Cache will be initialized in GetAvailableFightsAsync after SetAirRQ determines which APIs to call
                    // Start background search (fire and forget)
                    _ = Task.Run(async () =>
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            try
                            {
                                var oneWayFlightService = scope.ServiceProvider.GetRequiredService<IGetOneWayFlightService>();
                                await oneWayFlightService.GetAvailableFightsAsync(airModel);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "Error in background search for SearchID: {SearchID}", query.SearchID);
                                ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchCache.MarkComplete(query.SearchID);
                            }
                        }
                    });

                    // Return immediately with searchId for progressive loading
                    return Ok(new { 
                        d = result,
                        searchId = query.SearchID,
                        progressive = true
                    });
                }
                else if (searchType == "R" || searchType == "DRT")
                {
                    // For ROUND TRIP (RT/RW): Start background search and return immediately (same as one-way)
                    var airModel = new ZealTravel.Domain.Models.AirAvaibilityModel
                    {
                        Searchid = query.SearchID,
                        Companyid = companyId,
                        AirRQ = xmlstring,
                        JourneyType = journeyType,  // "RT" or "RW"
                        EndUserIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "125.63.102.34"
                    };

                    // Cache will be initialized in GetAvailableFightsAsync after SetAirRQ determines which APIs to call
                    // Start background search (fire and forget) - SAME PATTERN AS ONE-WAY
                    _ = Task.Run(async () =>
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            try
                            {
                                var oneWayFlightService = scope.ServiceProvider.GetRequiredService<IGetOneWayFlightService>();
                                await oneWayFlightService.GetAvailableFightsAsync(airModel);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "Error in background search for SearchID: {SearchID}", query.SearchID);
                                ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchCache.MarkComplete(query.SearchID);
                            }
                        }
                    });

                    // Return immediately with searchId for progressive loading
                    return Ok(new { 
                        d = result,
                        searchId = query.SearchID,
                        progressive = true
                    });
                }
                else
                {
                    // For other search types (MC, etc.): Use original synchronous behavior
                    var airModel = new ZealTravel.Domain.Models.AirAvaibilityModel
                    {
                        Searchid = query.SearchID,
                        Companyid = companyId,
                        AirRQ = xmlstring,
                        JourneyType = journeyType,
                        EndUserIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "125.63.102.34"
                    };

                    var dataresult = await _SearchFlightAvailabilityHandler.HandleAsync(query);

                    HttpContext.Session.SetString("SearchID", query.SearchID);
                    if (!string.IsNullOrEmpty(dataresult) && dataresult.IndexOf("RefID") != -1)
                    {
                        result = sector;
                        HttpContext.Session.SetString("FinalResult", dataresult);
                        HttpContext.Session.SetString("SearchValue", searchValue);
                    }
                    else
                    {
                        result = string.Empty;
                    }

                    return Ok(new { d = result });
                }
            }
            else
            {
                // If model is null or empty, return error
                return BadRequest(new { 
                    error = "Invalid request",
                    d = string.Empty
                });
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error in GetvalueForRequest");
            return BadRequest(new { 
                error = ex.Message,
                d = string.Empty
            });
        }
    }

    /// <summary>
    /// Polling endpoint for progressive results (One-Way and Round Trip)
    /// Returns current merged results as APIs complete
    /// </summary>
    [HttpGet]
    [Route("Flight/GetProgressiveResults")]
    public IActionResult GetProgressiveResults([FromQuery] string searchId)
    {
        try
        {
            if (string.IsNullOrEmpty(searchId))
            {
                return BadRequest(new { error = "SearchID is required" });
            }

            var cachedResult = ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchCache.GetResult(searchId);
            
            if (cachedResult == null)
            {
                return Ok(new 
                { 
                    searchId = searchId,
                    status = "notfound",
                    results = string.Empty,
                    isComplete = false,
                    completedApis = new string[0],
                    totalApis = 0
                });
            }

            // Get current merged results from cache
            var qpResult = cachedResult.ApiResults.TryGetValue("Akasa", out var akasa) ? akasa : null;
            var uapiResult = cachedResult.ApiResults.TryGetValue("Galileo", out var galileo) ? galileo : null;
            var uapiSMEResult = cachedResult.ApiResults.TryGetValue("Galileo_SME", out var galileoSME) ? galileoSME : null;
            var uapi6eResult = cachedResult.ApiResults.TryGetValue("IndiGo_6E", out var indigo) ? indigo : null;
            var spicejetResult = cachedResult.ApiResults.TryGetValue("Spicejet", out var spicejet) ? spicejet : null;
            var spicejetMaxResult = cachedResult.ApiResults.TryGetValue("Spicejet_MAX", out var spicejetMax) ? spicejetMax : null;
            var spicejetCouponResult = cachedResult.ApiResults.TryGetValue("Spicejet_COUPON", out var spicejetCoupon) ? spicejetCoupon : null;
            var spicejetCorporateResult = cachedResult.ApiResults.TryGetValue("Spicejet_CORPORATE", out var spicejetCorp) ? spicejetCorp : null;

            // Get merged results from service (merges ALL available results immediately, not waiting for all APIs)
            var responseString = _oneWayFlightService.GetProgressiveResults(searchId, cachedResult.JourneyType, cachedResult.CompanyId);

            // Get completed API names - use ApiResults.Keys to ensure we get all APIs that have completed
            // (even if they returned null/empty, they still count as "completed" for progress tracking)
            var completedApis = cachedResult.ApiResults.Keys.ToArray();
            

            return Ok(new 
            { 
                searchId = searchId,
                status = cachedResult.IsComplete ? "completed" : "inprogress",
                results = responseString,
                isComplete = cachedResult.IsComplete,
                completedApis = completedApis,
                totalApis = cachedResult.TotalApis
            });
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting progressive results for SearchID: {SearchID}", searchId);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// TEST ENDPOINT: Verify route is accessible
    /// GET /Flight/TestOldFunctionSimple
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [Route("Flight/TestOldFunctionSimple")]
    public IActionResult TestOldFunctionSimpleGet()
    {
        return Ok(new { message = "Test endpoint is accessible!", timestamp = DateTime.Now });
    }

    /// <summary>
    /// TEST ENDPOINT: Call old synchronous GetAvailableFights method directly
    /// Simple version: POST /Flight/TestOldFunctionSimple
    /// Body: { "from": "PNQ", "to": "DEL", "date": "19/12/2025", "adults": "1" }
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [Route("Flight/TestOldFunctionSimple")]
    public async Task<IActionResult> TestOldFunctionSimple([FromBody] TestFlightRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.from) || string.IsNullOrEmpty(request.to) || string.IsNullOrEmpty(request.date))
            {
                return BadRequest(new { error = "Missing required parameters: from, to, date" });
            }

            // Get company ID
            string companyId = string.Empty;
            int accountID = 0;
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                companyId = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                accountID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetStaffAccountID(User) : 0;
            }
            else
            {
                companyId = "1"; // Default for testing
                accountID = 1;
            }

            // Build XML request string manually
            // Format: DD/MM/YYYY -> YYYYMMDD
            string[] dateParts = request.date.Split('/');
            string beginDate = dateParts[2] + dateParts[1] + dateParts[0]; // YYYYMMDD
            
            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<AvailabilityRequest>");
            xmlBuilder.Append("<DepartureStation>" + request.from + "</DepartureStation>");
            xmlBuilder.Append("<ArrivalStation>" + request.to + "</ArrivalStation>");
            xmlBuilder.Append("<Cabin>Y</Cabin>");
            xmlBuilder.Append("<AirVAry></AirVAry>");
            xmlBuilder.Append("<StartDate>" + beginDate + "</StartDate>");
            xmlBuilder.Append("<EndDate>" + beginDate + "</EndDate>");
            xmlBuilder.Append("<Adult>" + (request.adults ?? "1") + "</Adult>");
            xmlBuilder.Append("<Child>" + (request.children ?? "0") + "</Child>");
            xmlBuilder.Append("<Infant>" + (request.infants ?? "0") + "</Infant>");
            xmlBuilder.Append("<SplFare>false</SplFare>");
            xmlBuilder.Append("</AvailabilityRequest>");
            
            string xmlstring = xmlBuilder.ToString();
            
            // Determine sector
            var isDomestic = await _isDomesticFlightSearchHandler.HandleAsync(new IsDomesticFlightSearchQuery { Origin = request.from, Destination = request.to });
            string sector = isDomestic ? "D" : "I";
            string journeyType = "OW";

            // Create search ID
            var searchId = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString();

            Console.WriteLine($"[TEST] ========================================");
            Console.WriteLine($"[TEST] Testing OLD synchronous GetAvailableFights");
            Console.WriteLine($"[TEST] SearchID: {searchId}");
            Console.WriteLine($"[TEST] From: {request.from}, To: {request.to}");
            Console.WriteLine($"[TEST] Date: {request.date} -> {beginDate}");
            Console.WriteLine($"[TEST] JourneyType: {journeyType}, Sector: {sector}");
            Console.WriteLine($"[TEST] XML Request length: {xmlstring.Length}");
            Console.WriteLine($"[TEST] ========================================");

            // Call OLD synchronous method directly
            var airModel = new ZealTravel.Domain.Models.AirAvaibilityModel
            {
                Searchid = searchId,
                Companyid = companyId,
                AirRQ = xmlstring,
                JourneyType = journeyType,
                EndUserIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "125.63.102.34"
            };

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = _oneWayFlightService.GetAvailableFights(airModel);
            sw.Stop();

            Console.WriteLine($"[TEST] ========================================");
            Console.WriteLine($"[TEST] Old function completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"[TEST] Result length: {result?.Length ?? 0}");
            Console.WriteLine($"[TEST] HasRefID: {(result?.IndexOf("RefID") ?? -1) != -1}");
            if (result != null && result.IndexOf("RefID") != -1)
            {
                var resultCount = (result.Length - result.Replace("<RefID>", "").Length) / "<RefID>".Length;
                Console.WriteLine($"[TEST] Estimated flight count: {resultCount}");
            }
            Console.WriteLine($"[TEST] ========================================");

            return Ok(new { 
                success = true,
                searchId = searchId,
                resultLength = result?.Length ?? 0,
                hasRefID = (result?.IndexOf("RefID") ?? -1) != -1,
                timeMs = sw.ElapsedMilliseconds,
                resultPreview = result != null && result.Length > 0 ? result.Substring(0, Math.Min(1000, result.Length)) : "",
                result = result
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TEST] Error: {ex.Message}");
            Console.WriteLine($"[TEST] StackTrace: {ex.StackTrace}");
            return BadRequest(new { 
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    /// <summary>
    /// Test request model for simple test
    /// </summary>
    public class TestFlightRequest
    {
        public string from { get; set; }  // e.g., "PNQ" for Pune
        public string to { get; set; }      // e.g., "DEL" for Delhi
        public string date { get; set; }    // e.g., "19/12/2025"
        public string adults { get; set; } = "1";
        public string children { get; set; } = "0";
        public string infants { get; set; } = "0";
    }

    /// <summary>
    /// TEST ENDPOINT: Call old synchronous GetAvailableFights method directly
    /// Use this to test if old function still works
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> TestOldFunction([FromBody] clsPSQBORequest model)
    {
        try
        {
            if (model == null || model.objclsPSQ == null || !model.objclsPSQ.Any())
            {
                return BadRequest(new { error = "Invalid request - objclsPSQ is required" });
            }

            clsPSQBO objGetAllValue = model.objclsPSQ[0];
            if (string.IsNullOrEmpty(objGetAllValue._SearchType))
            {
                return BadRequest(new { error = "Invalid request - SearchType is required" });
            }

            // Get company ID
            string companyId = string.Empty;
            int accountID = 0;
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                companyId = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
                accountID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetStaffAccountID(User) : 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(objGetAllValue._companyId))
                {
                    int.TryParse(objGetAllValue._companyId, out accountID);
                    if (accountID > 0)
                    {
                        var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = accountID };
                        companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
                    }
                }
            }

            // Build XML request (same as GetvalueForRequest)
            string xmlstring = SearchFlightRequestHelper.PSQXMLString(model.objclsPSQ);
            
            // Determine sector from departure/arrival stations
            var arrdep = CommonFunction.GetStringInBetween(objGetAllValue._DepartureStation, "(", ")", false, false);
            string departureStation = arrdep[0];
            var arrArv = CommonFunction.GetStringInBetween(objGetAllValue._ArrivalStation, "(", ")", false, false);
            string arrivalStation = arrArv[0];
            var isDomestic = await _isDomesticFlightSearchHandler.HandleAsync(new IsDomesticFlightSearchQuery { Origin = departureStation, Destination = arrivalStation });
            string sector = isDomestic ? "D" : "I";
            
            string searchType = objGetAllValue._SearchType;

            // Determine journey type
            string journeyType = string.Empty;
            if (searchType == "O")
            {
                journeyType = "OW";
            }
            else if (searchType == "DRT" || searchType == "R")
            {
                journeyType = sector == "D" ? "RW" : "RT";
            }

            // Create search ID
            var query = new SearchFlightAvailabilityQuery();
            query.SearchID = Guid.NewGuid().ToString() + "-" + System.DateTime.Now.Minute.ToString() + "-" + System.DateTime.Now.Second.ToString() + "-" + accountID.ToString();
            query.CompanyID = companyId;
            query.AirRQ = xmlstring;
            query.JourneyType = journeyType;

            Console.WriteLine($"[TEST] Calling OLD synchronous GetAvailableFights method");
            Console.WriteLine($"[TEST] SearchID: {query.SearchID}, JourneyType: {journeyType}, Sector: {sector}");

            // Call OLD synchronous method directly
            var airModel = new ZealTravel.Domain.Models.AirAvaibilityModel
            {
                Searchid = query.SearchID,
                Companyid = companyId,
                AirRQ = xmlstring,
                JourneyType = journeyType,
                EndUserIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "125.63.102.34"
            };

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = _oneWayFlightService.GetAvailableFights(airModel);
            sw.Stop();

            Console.WriteLine($"[TEST] Old function completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"[TEST] Result length: {result?.Length ?? 0}, HasRefID: {(result?.IndexOf("RefID") ?? -1) != -1}");

            return Ok(new { 
                success = true,
                searchId = query.SearchID,
                resultLength = result?.Length ?? 0,
                hasRefID = (result?.IndexOf("RefID") ?? -1) != -1,
                timeMs = sw.ElapsedMilliseconds,
                result = result
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TEST] Error: {ex.Message}");
            Console.WriteLine($"[TEST] StackTrace: {ex.StackTrace}");
            return BadRequest(new { 
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ShowData([FromBody] string CompanyID)//oneway,Roundway,rt
    {

        List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();
        try
        {
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;

            }
            
            // Check if FinalResult is in session (traditional flow)
            var finalResult = HttpContext.Session.GetString("FinalResult");
            var searchId = HttpContext.Session.GetString("SearchID");
            
            // If no FinalResult but we have a searchId, check progressive cache (OW, RT, RW)
            if (string.IsNullOrEmpty(finalResult) && !string.IsNullOrEmpty(searchId))
            {
                var cachedResult = ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchCache.GetResult(searchId);
                if (cachedResult != null && (cachedResult.JourneyType == "OW" || cachedResult.JourneyType == "RT" || cachedResult.JourneyType == "RW"))
                {
                    // Get results from progressive cache
                    var progressiveResults = _oneWayFlightService.GetProgressiveResults(searchId, cachedResult.JourneyType, CompanyID);
                    
                    if (!string.IsNullOrEmpty(progressiveResults) && progressiveResults.IndexOf("RefID") != -1)
                    {
                        // Set in session so ShowFlightDataHelper can use it
                        HttpContext.Session.SetString("FinalResult", progressiveResults);
                        var searchValue = cachedResult.JourneyType == "OW" ? "OW" : (cachedResult.JourneyType == "RW" ? "RW" : "INT");
                        HttpContext.Session.SetString("SearchValue", searchValue);
                    }
                }
            }
            
            FlightOutBound = await ShowFlightDataHelper.ResultDataAsync("OUTBOUND", "DOM", CompanyID);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error in ShowData");
        }

        return Ok(new { d = FlightOutBound });
    }

    [HttpPost]
    public async Task<IActionResult> ShowDataR([FromBody] string CompanyID)//Roundway,rt
       
    {
        List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();
        try
        {
          
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;

            }
            
            // Check if FinalResult is in session (traditional flow)
            var finalResult = HttpContext.Session.GetString("FinalResult");
            var searchId = HttpContext.Session.GetString("SearchID");
            
            // If no FinalResult but we have a searchId, check progressive cache (RT, RW only - for inbound flights)
            if (string.IsNullOrEmpty(finalResult) && !string.IsNullOrEmpty(searchId))
            {
                var cachedResult = ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchCache.GetResult(searchId);
                if (cachedResult != null && (cachedResult.JourneyType == "RT" || cachedResult.JourneyType == "RW"))
                {
                    // Get results from progressive cache (same results contain both outbound and inbound)
                    var progressiveResults = _oneWayFlightService.GetProgressiveResults(searchId, cachedResult.JourneyType, CompanyID);
                    
                    if (!string.IsNullOrEmpty(progressiveResults) && progressiveResults.IndexOf("RefID") != -1)
                    {
                        // Set in session so ShowFlightDataHelper can use it
                        HttpContext.Session.SetString("FinalResult", progressiveResults);
                        var searchValue = cachedResult.JourneyType == "RW" ? "RW" : "INT";
                        HttpContext.Session.SetString("SearchValue", searchValue);
                    }
                }
            }
            
            FlightOutBound = await ShowFlightDataHelper.ResultDataAsync("INBOUND", "DOM", CompanyID);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error in ShowDataR");
        }
        return Ok(new { d = FlightOutBound });
    }


    [HttpPost]
    public async Task<IActionResult> ShowDataRoundInternational([FromBody] string CompanyID)//international
    {
        List<k_ShowFlightInternational> FlightOutBound = new List<k_ShowFlightInternational>();
        try
        {
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;

            }
            
            FlightOutBound = await ShowFlightDataHelper.ResultDataIntAsync("OUTBOUND", "INT", CompanyID);
            
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error in ShowDataRoundInternational");
        }

        return Ok(new { d = FlightOutBound });
    }

    [HttpPost]
    public async Task<IActionResult> PreferAirlines()
    {
        List<PreferAirlinesFilterResponse> preferAirlinesFilterResponse = new List<PreferAirlinesFilterResponse>();
        try
        {
            preferAirlinesFilterResponse = await ShowFlightDataHelper.PreferAirlines("OUTBOUND", "DOM");
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = preferAirlinesFilterResponse });
    }


    [HttpPost]
    [Route("/flight/v1/peferairlines")]
    public async Task<IActionResult> PreferAirlinesV1() //all - new 
    {
        List<PreferAirlinesFilterResponseV1> preferAirlinesFilterResponse = new List<PreferAirlinesFilterResponseV1>();
        try
        {
            if (HttpContext.Session.GetString("SEARCH_TYPE") == "DOM")
            {
                preferAirlinesFilterResponse = await ShowFlightDataHelper.PreferAirlinesV1("OUTBOUND", "DOM");
            }
            else
            {
                preferAirlinesFilterResponse = await ShowFlightDataHelper.PreferAirlinesV1("OUTBOUND", "INT");
            }
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = preferAirlinesFilterResponse });
    }

    [HttpPost]
    public async Task<IActionResult> PreferAirlinesR()
    {
        List<PreferAirlinesFilterResponse> preferAirlinesFilterResponse = new List<PreferAirlinesFilterResponse>();
        try
        {
            preferAirlinesFilterResponse = await ShowFlightDataHelper.PreferAirlines("INBOUND", "DOM");
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = preferAirlinesFilterResponse });
    }
    [HttpPost]
    public async Task<IActionResult> FlightStops()
    {
        List<FlightStopsFilterResponse> fightStops = new List<FlightStopsFilterResponse>();
        try
        {
            fightStops = await ShowFlightDataHelper.FlightStop("OUTBOUND");
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = fightStops });
    }

    public async Task<IActionResult> FlightStopsR() //rt,roundway,int
    {
        List<FlightStopsFilterResponse> fightStops = new List<FlightStopsFilterResponse>();
        try
        {
            fightStops = await ShowFlightDataHelper.FlightStop("INBOUND");
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = fightStops });
    }

    [HttpPost]
    public async Task<IActionResult> FlightMatrix()
    {
        List<FlightMatrixFilterResponse> flightMatrixFilterResponse = new List<FlightMatrixFilterResponse>();
        try
        {
            flightMatrixFilterResponse = await ShowFlightDataHelper.FlightMatrix();
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = flightMatrixFilterResponse });

    }

    [HttpPost]
    public async Task<IActionResult> FlightMatrixRoundInt()
    {
        List<FlightMatrixFilterResponse> flightMatrixFilterResponse = new List<FlightMatrixFilterResponse>();
        try
        {
            flightMatrixFilterResponse = await ShowFlightDataHelper.FlightMatrixRoundInt();
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = flightMatrixFilterResponse });

    }

    [HttpPost]
    public async Task<IActionResult> SelectFlight([FromBody] SelectFlightRequest selectFlightRequest)//one
    {
        List<k_ShowFlightOutBound> flightOutBound = new List<k_ShowFlightOutBound>();
        try
        {
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                selectFlightRequest.CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
            }
            flightOutBound = await ShowFlightDataHelper.SelectOneway("OUTBOUND", "DOM", selectFlightRequest.refid, selectFlightRequest.CompanyID, User, _getAirFareHandler, _getAirFareRulesHandler, _getCompanyRegisterCorporateUserDetailsQueryHandler, _getCompanyRegisterCorporateUserLimitQueryHandler);
        }
        catch (Exception ex)
        {
        }
        return Ok(new { d = flightOutBound });
    }

    [HttpPost]
    public async Task<IActionResult> SelectFlightR([FromBody] SelectFlighRoundtRequest selectFlightRequest)//round
    {
        List<k_ShowFlightOutBound> flightOutBound = new List<k_ShowFlightOutBound>();
        try
        {
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                selectFlightRequest.CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
            }
            flightOutBound = await ShowFlightDataHelper.SeletRound(selectFlightRequest.refidOnward, selectFlightRequest.refidReturn, "DOM", selectFlightRequest.CompanyID, User, _getAirFareHandler, _getAirFareRulesHandler, _getCompanyRegisterCorporateUserDetailsQueryHandler, _getCompanyRegisterCorporateUserLimitQueryHandler);
        }
        catch (Exception ex)
        {

        }
        return Ok(new { d = flightOutBound });
    }

    [HttpPost]
    public async Task<IActionResult> SelectFlightIntR([FromBody] SelectFlightRequest selectFlightRequest)//int
    {

        List<k_ShowFlightInternational> flightInt = new List<k_ShowFlightInternational>();
        try
        {
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                selectFlightRequest.CompanyID = User != null && User.Identity.IsAuthenticated ? UserHelper.GetCompanyID(User) : string.Empty;
            }
            flightInt = await ShowFlightDataHelper.SelectInt(selectFlightRequest.refid, selectFlightRequest.CompanyID, User, _getAirFareHandler, _getAirFareRulesHandler, _getCompanyRegisterCorporateUserDetailsQueryHandler, _getCompanyRegisterCorporateUserLimitQueryHandler);
        }
        catch (Exception ex)
        {
        }
        return Ok(new { d = flightInt });
    }

    [HttpGet]
    public async Task<IActionResult> FlightDetailLogin()
    {
        if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null))
        {
            return RedirectToAction("Index");
        }


        var flightDetails = await FlightDetailLoginPageValues();

        return View(flightDetails);
    }

    private async Task<FlightDetailLoginResponse> FlightDetailLoginPageValues()
    {
        var flightDetailLoginResponse = new FlightDetailLoginResponse();
        var SelectedFltIn = string.Empty;
        var pageredirect = string.Empty;
        int totalFare = 0;
        flightDetailLoginResponse.HostName = _configuration["SiteURL:BasePath"];
        flightDetailLoginResponse.CompanyId = string.Empty;


        if (HttpContext.Session.GetString("Curr") != null)
        {
            flightDetailLoginResponse.CurrencyType = HttpContext.Session.GetString("Curr");

        }
        else
        {
            flightDetailLoginResponse.CurrencyType = "INR";
        }

        var SearchID = HttpContext.Session.GetString("SearchID");
        var FinalResult = HttpContext.Session.GetString("FinalResult");
        StringBuilder sbHtmlItinary = new StringBuilder();
        StringBuilder sbHtmlFare = new StringBuilder();
        var SelectedFltOut = HttpContext.Session.GetString("SelectedFltOut");

        //--------- multicity 26Aug2023 
        var ssionResult4MCListData = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
        if (ssionResult4MCListData != null)
        {

            string Outbound = string.Empty;
            string Inbound = string.Empty;


            List<XmlNode> mergedList = new List<XmlNode>();

            //int rowId_ = 0;

            XmlDocument xmlDocFinalResult = new XmlDocument();
            XmlDocument xmlDocSelectedFltOut = new XmlDocument();
            XmlDocument xmlDoc;
            foreach (SessionResult4MC o in HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").OrderBy(x => x._SrNo))
            {
                if (o.FinalResult == null)
                {
                    Response.Redirect("Index");
                }
                HttpContext.Session.SetString("FinalResult", o.FinalResult);
                HttpContext.Session.SetString("SearchID", o.SearchID);
                HttpContext.Session.SetString("SelectedFltOut", o.SelectedFltOut);
                HttpContext.Session.SetString("SearchValue", o.SearchValue);

                string OutboundCurr = string.Empty;
                TravellerInformationHelper.SetXmlNode(out OutboundCurr, out Inbound);
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(OutboundCurr);
                foreach (XmlNode node in xmlDoc.SelectNodes("//AvailabilityResponseOut"))
                {
                    node["RowID"].InnerText = (o._SrNo.ToString() + node["RowID"].InnerText);
                    mergedList.Add(node);
                }

            }
            Outbound = TravellerInformationHelper.Convert2SelectedResponse(mergedList);

            //=============================================
            HttpContext.Session.SetString("SearchValue", "MC");
            StringBuilder OutbountItinary;
            StringBuilder Faredetaildiv;

            TravellerInformationHelper.GetFilghtShow(out OutbountItinary, out Faredetaildiv, false);
            flightDetailLoginResponse.TotalFare = 0;

            pageredirect = "/Flight/multicity";
            flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");

        }
        else
        {

            //--------------

            if (HttpContext.Session.GetString("SelectedFltIn") != null)
            {
                SelectedFltIn = HttpContext.Session.GetString("SelectedFltIn");
            }

            if (HttpContext.Session.GetString("SearchValue").Equals("OW"))
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContext.Session.GetString("SelectedFltOut").Trim(), "", HttpContext.Session.GetString("FinalResult"), false);

                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalFare, false);
                flightDetailLoginResponse.TotalFare = totalFare;
                flightDetailLoginResponse.OutbountItinary = sbHtmlItinary.ToString();
                flightDetailLoginResponse.Faredetaildiv = sbHtmlFare.ToString();
                pageredirect = "k_one.aspx";
                flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");
            }
            else if (HttpContext.Session.GetString("SearchValue").Equals("RW"))
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContext.Session.GetString("SelectedFltOut").Trim(), HttpContext.Session.GetString("SelectedFltIn").Trim(), HttpContext.Session.GetString("FinalResult"), false);

                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalFare, false);
                flightDetailLoginResponse.TotalFare = totalFare;
                flightDetailLoginResponse.OutbountItinary = sbHtmlItinary.ToString();
                flightDetailLoginResponse.Faredetaildiv = sbHtmlFare.ToString();

                pageredirect = "Flight/round";
                flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");
            }
            else if (HttpContext.Session.GetString("SearchValue").Equals("RT"))
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContext.Session.GetString("SelectedFltOut").Trim(), HttpContext.Session.GetString("SelectedFltIn").Trim(), HttpContext.Session.GetString("FinalResult"), false);

                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalFare, false);
                flightDetailLoginResponse.TotalFare = totalFare;
                flightDetailLoginResponse.OutbountItinary = sbHtmlItinary.ToString();
                flightDetailLoginResponse.Faredetaildiv = sbHtmlFare.ToString();

                pageredirect = "k_rt.aspx";
                flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");
            }
            else if (HttpContext.Session.GetString("SearchValue").Equals("INT"))
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContext.Session.GetString("SelectedFltOut").Trim(), "", HttpContext.Session.GetString("FinalResult"), true);

                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalFare, false);
                flightDetailLoginResponse.TotalFare = totalFare;
                flightDetailLoginResponse.OutbountItinary = sbHtmlItinary.ToString();
                flightDetailLoginResponse.Faredetaildiv = sbHtmlFare.ToString();
                pageredirect = "k_int.aspx";
                flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");
            }
            else if (HttpContext.Session.GetString("SearchValue").Equals("MC"))
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContext.Session.GetString("SelectedFltOut").Trim(), "", HttpContext.Session.GetString("FinalResult"), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalFare, false);
                flightDetailLoginResponse.TotalFare = totalFare;
                flightDetailLoginResponse.OutbountItinary = sbHtmlItinary.ToString();
                flightDetailLoginResponse.Faredetaildiv = sbHtmlFare.ToString();
                pageredirect = "/Flight/multicity";
                flightDetailLoginResponse.ReturnType = HttpContext.Session.GetString("SearchValue");
            }
        }

        return flightDetailLoginResponse;
    }

    [HttpGet]
    public async Task<IActionResult> Travellers()
    {
        if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null))
        {
            return RedirectToAction("Index");
        }

        if (HttpContext.Session.GetString("FinalResult") == null || HttpContext.Session.GetString("SelectedFltOut") == null)
        {
            return RedirectToAction("Index");
        }


        var flightTravellersDetails = await FlightTravellersInfoResponse();

        return View(flightTravellersDetails);
    }

    [HttpPost]
    public async Task<IActionResult> Travellers(FlightTravellersInfoResponse flightTravellersInfoResponse)
    {
        string Mobile = flightTravellersInfoResponse.MobileNo.Trim();
        string Address = flightTravellersInfoResponse.Address.Trim();
        string PaxRequest = HttpUtility.HtmlDecode(flightTravellersInfoResponse.PaxXmlResult);


        string strPassenger = TravellerInformationHelper.GetPassengerRequest(HttpUtility.HtmlDecode(flightTravellersInfoResponse.PaxXmlResult));
        if (strPassenger != null && strPassenger.IndexOf("PassengerInfo") != -1 && strPassenger.IndexOf("First_Name") != -1)
        {
            HttpContext.Session.SetString("PaxXML", strPassenger);

            if (Request.Query["regis"].Any())
            {
                return Redirect("/flight/payment?regis=" + Request.Query["regis"].ToString().Trim());
            }
            else
            {
                return Redirect("/flight/payment");
            }
        }
        else
        {
            return View(flightTravellersInfoResponse);
        }
    }

    private async Task<FlightTravellersInfoResponse> FlightTravellersInfoResponse()
    {
        var flightTravellersInfoResponse = new FlightTravellersInfoResponse();
        var isValidCfee = false;

        flightTravellersInfoResponse.HostName = _configuration["SiteURL:BasePath"];
        flightTravellersInfoResponse.LivePathImage = "/assets/img/CommonImages/";


        if (HttpContext.Session.GetString("Curr") != null)
        {
            flightTravellersInfoResponse.CurrencyType = HttpContext.Session.GetString("Curr");

        }
        else
        {
            flightTravellersInfoResponse.CurrencyType = "INR";
        }

        if (HttpContext.Session.GetString("regis") != null)
        {
            flightTravellersInfoResponse.RegisterEmail = HttpContext.Session.GetString("regis");
        }



        if (string.IsNullOrEmpty(Request.Query["value"]) && _configuration["Company:IsBO"].Equals("0"))
        {
            //set compnay
            var cmpid = UserHelper.GetCompanyID(User);
            flightTravellersInfoResponse.CompanyId = cmpid;
            //set promo
            PromoDiscountHelper.AddPromo(_gettWhitelabelAdminidFromHostQueryHandler, _getPromoAmountQueryHandler);

            //set cfee
            string strcmpid = cmpid;
            if (string.IsNullOrEmpty(strcmpid) || string.IsNullOrEmpty(Request.Query["value"]))
            {
                strcmpid = await _gettWhitelabelAdminidFromHostQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Host.Host);
            }

            string Sector = "D";
            if (HttpContext.Session.GetString("SEARCH_TYPE") == "INT")
            {
                Sector = "I";
            }


            isValidCfee = await _isWalletCfeeQueryHandler.HandleAsync(new IsWalletCfeeQuery { CompanyID = strcmpid, BookingType = "AIRLINE", Sector = Sector });
            if (isValidCfee)
            {
                flightTravellersInfoResponse.ValidCfee = "1";
            }
            else
            {
                flightTravellersInfoResponse.ValidCfee = "0";
            }
        }
        else
        {
            flightTravellersInfoResponse.ValidCfee = "0";
        }

        var nationality = await _getCountryListWithCodeQueryHandler.HandleAsync();
        flightTravellersInfoResponse.Nationality = JsonSerializer.Serialize(nationality);

        if (!string.IsNullOrEmpty(Request.Query["regis"]) && Request.Query["regis"].ToString().IndexOf("@") != -1)
        {
            flightTravellersInfoResponse.MobileNo = string.Empty;
            flightTravellersInfoResponse.Email = Request.Query["regis"].ToString().Trim();
            flightTravellersInfoResponse.Address = string.Empty;

        }
        else
        {
            flightTravellersInfoResponse.MobileNo = UserHelper.GetCompanyMobile(User);
            flightTravellersInfoResponse.Email = UserHelper.GetCompanyEmail(User);
            flightTravellersInfoResponse.Address = UserHelper.GetCompanyAddress(User);

        }

        //Set Gst details

        var gstDetails = await _getGSTDetailbyCompanyQueryHandler.HandleAsync(flightTravellersInfoResponse.CompanyId);
        if (gstDetails != null)
        {
            flightTravellersInfoResponse.HasGST = true;
            flightTravellersInfoResponse.GSTNumber = gstDetails.Gstnumber;
            flightTravellersInfoResponse.GSTRegisteredCompany = gstDetails.GstcompanyName;
            flightTravellersInfoResponse.GSTCompanyAddress = gstDetails.GstcompanyAddress;
            flightTravellersInfoResponse.GSTCompanyContactNo = gstDetails.GstcompanyContactNumber;
            flightTravellersInfoResponse.GSTCompanyEmail = gstDetails.GstcompanyEmail;
        }

        string Outbound = string.Empty;
        string Inbound = string.Empty;
        var ssionResult4MCListData = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
        if (ssionResult4MCListData != null)

        {

            List<XmlNode> mergedList = new List<XmlNode>();
            //int rowId_ = 0;
            XmlDocument xmlDocFinalResult = new XmlDocument();
            XmlDocument xmlDocSelectedFltOut = new XmlDocument();
            XmlDocument xmlDoc4SSR = new XmlDocument();
            XmlDocument xmlDoc;
            int _Stops = 0;
            int _titCities = Convert.ToInt16(HttpContextHelper.Current.Session.GetString("TotalCities"));

            flightTravellersInfoResponse.IntlQueryString = string.Empty;

            foreach (SessionResult4MC o in HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").OrderBy(x => x._SrNo))
            {
                if (o.FinalResult == null)
                {
                    Response.Redirect("Index.aspx");
                }
                HttpContext.Session.SetString("FinalResult", o.FinalResult);
                HttpContext.Session.SetString("SearchID", o.SearchID);
                HttpContext.Session.SetString("SelectedFltOut", o.SelectedFltOut);
                HttpContext.Session.SetString("SearchValue", o.SearchValue);

                var clsPSQBORequest = new clsPSQBORequest();
                clsPSQBORequest.objclsPSQ = o.OriginalPSQ.objclsPSQ;
                HttpContext.Session.SetComplexObject<clsPSQBORequest>("OriginalPSQ", clsPSQBORequest);
                HttpContext.Session.SetString("PSQ", o.PSQ);




                if (_titCities == o._SrNo)
                {
                    flightTravellersInfoResponse = SetValueInhiddenField(true, flightTravellersInfoResponse);
                }
                else
                {
                    flightTravellersInfoResponse = SetValueInhiddenField(false, flightTravellersInfoResponse);
                }




                if (xmlDoc4SSR.DocumentElement == null)
                {
                    XmlElement rootElement4SSR = xmlDoc4SSR.CreateElement("root");
                    xmlDoc4SSR.AppendChild(rootElement4SSR);
                }
                flightTravellersInfoResponse = await SetSSR(flightTravellersInfoResponse);
                if (flightTravellersInfoResponse.SSRAvailabilityOut.Trim() != "")
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(flightTravellersInfoResponse.SSRAvailabilityOut);
                    foreach (XmlNode node in xmlDoc.SelectNodes("//SSRInfo"))
                    {
                        //rowId_++;
                        node["RowID"].InnerText = (o._SrNo.ToString() + node["RowID"].InnerText);
                        XmlNode importedNode4SSR = xmlDoc4SSR.ImportNode(node, deep: true);
                        XmlNode targetParentNode = xmlDoc4SSR.DocumentElement;
                        targetParentNode.AppendChild(importedNode4SSR);

                    }
                }
                //------------ End SSr Secton

                string OutboundCurr = string.Empty;
                TravellerInformationHelper.SetXmlNode(out OutboundCurr, out Inbound, o.SelectedFltOut, o.FinalResult);
                if (OutboundCurr.Trim() != "")
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(OutboundCurr);
                    foreach (XmlNode node in xmlDoc.SelectNodes("//AvailabilityResponseOut"))
                    {
                        node["RowID"].InnerText = (o._SrNo.ToString() + node["RowID"].InnerText);
                        mergedList.Add(node);
                        _Stops++;
                    }
                }


            }
            Outbound = TravellerInformationHelper.Convert2SelectedResponse(mergedList);
            if (Outbound.Trim() != "")
            {
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Outbound);
                foreach (XmlNode node in xmlDoc.SelectNodes("//AvailabilityResponseOut"))
                {
                    node["Stops"].InnerText = (_Stops - 1).ToString();
                }
                Outbound = xmlDoc.OuterXml;
            }

            flightTravellersInfoResponse.FlightSelectedXml = Outbound;
            flightTravellersInfoResponse.FlightSelectedXmlInbond = Inbound;
            flightTravellersInfoResponse.SSRAvailabilityOut = xmlDoc4SSR.OuterXml;



            //=============================================
            HttpContextHelper.Current.Session.SetString("SearchValue", "MC");

            StringBuilder OutbountItinary;
            StringBuilder Faredetaildiv;
            TravellerInformationHelper.GetFilghtShow(out OutbountItinary, out Faredetaildiv, false);

            flightTravellersInfoResponse.OutbountItinary = OutbountItinary.ToString();
            flightTravellersInfoResponse.Faredetaildiv = Faredetaildiv.ToString();

            flightTravellersInfoResponse.RoundTrip = HttpContextHelper.Current.Session.GetString("SearchValue");
            flightTravellersInfoResponse.Pageredirect = TravellerInformationHelper.GetBackPageName(flightTravellersInfoResponse.CompanyId, true);

        }
        else //----- non multicity  call
        {

            flightTravellersInfoResponse.IntlQueryString = string.Empty;
            flightTravellersInfoResponse = SetValueInhiddenField(true, flightTravellersInfoResponse);


            TravellerInformationHelper.SetXmlNode(out Outbound, out Inbound);

            flightTravellersInfoResponse.FlightSelectedXml = Outbound;
            flightTravellersInfoResponse.FlightSelectedXmlInbond = Inbound;


            flightTravellersInfoResponse = await SetSSR(flightTravellersInfoResponse);
            //=============================================
            StringBuilder OutbountItinary;
            StringBuilder Faredetaildiv;

            TravellerInformationHelper.GetFilghtShow(out OutbountItinary, out Faredetaildiv, false);
            flightTravellersInfoResponse.OutbountItinary = OutbountItinary.ToString();
            flightTravellersInfoResponse.Faredetaildiv = Faredetaildiv.ToString();
            flightTravellersInfoResponse.RoundTrip = HttpContext.Session.GetString("SearchValue");
            flightTravellersInfoResponse.Pageredirect = TravellerInformationHelper.GetBackPageName(flightTravellersInfoResponse.CompanyId);
        }


        return flightTravellersInfoResponse;
    }

    private FlightTravellersInfoResponse SetValueInhiddenField(bool _IsLastTrip, FlightTravellersInfoResponse flightTravellersInfoResponse)
    {
        if (HttpContext.Session.GetString("OriginalPSQ") != null)
        {
            var originalPSQ = HttpContext.Session.GetComplexObject<clsPSQBORequest>("OriginalPSQ");
            if (originalPSQ != null)
            {
                var obj = originalPSQ.objclsPSQ;

                string[] arrdep = obj[0]._DepartureStation.Split(',');
                string[] arrArr = obj[0]._ArrivalStation.Split(',');
                string DepartureStation = arrdep[0];
                string ArrivalStation = arrArr[0];
                flightTravellersInfoResponse.Adult = obj[0]._NoOfAdult;
                flightTravellersInfoResponse.Child = obj[0]._NoOfChild;
                flightTravellersInfoResponse.Infant = obj[0]._NoOfInfant;

                var intQuery_ = obj[0]._DepartureStation + "!" + obj[0]._ArrivalStation + "#" + obj[0]._BeginDate + "," + obj[0]._EndDate;
                if (_IsLastTrip == false)
                {
                    intQuery_ += "|";
                }

                flightTravellersInfoResponse.IntlQueryString = EncodeDecodeHelper.EncodeTo64(intQuery_);
                if (obj[0]._EndDate.Length > 0)
                {
                    string dt = obj[0]._EndDate;
                    string dd = dt.Substring(0, 2);
                    string mm = dt.Substring(3, 2);
                    string yyyy = dt.Substring(6, 4);
                    flightTravellersInfoResponse.TravelDateChdInf = yyyy + "-" + mm + "-" + dd;

                }
                else
                {
                    string dt = obj[0]._BeginDate;
                    string dd = dt.Substring(0, 2);
                    string mm = dt.Substring(3, 2);
                    string yyyy = dt.Substring(6, 4);
                    flightTravellersInfoResponse.TravelDateChdInf = yyyy + "-" + mm + "-" + dd;

                }
            }
        }
        return flightTravellersInfoResponse;
    }
    private async Task<FlightTravellersInfoResponse> SetSSR(FlightTravellersInfoResponse flightTravellersInfoResponse)
    {
        var searchID = HttpContext.Session.GetString("SearchID");
        var finalResult = HttpContext.Session.GetString("FinalResult");
        var selectedFltOut = HttpContext.Session.GetString("SelectedFltOut");
        var selectedFltIn = string.Empty;
        var getSSRoutbound = string.Empty;
        var getSSRinbound = string.Empty;
        if (HttpContext.Session.GetString("SelectedFltIn") != null)
        {
            selectedFltIn = HttpContext.Session.GetString("SelectedFltIn");
        }

        if (HttpContext.Session.GetString("SearchValue").Equals("INT"))
        {

            flightTravellersInfoResponse.SearchType = "R";

            var flightSSRAvailabilityInfo = await SSRHelper.GetSSRAvailabilityCombine(searchID, flightTravellersInfoResponse.CompanyId, finalResult, selectedFltOut, _getAirSSRHandler);
            flightTravellersInfoResponse.SSRAvailabilityOut = flightSSRAvailabilityInfo.SSRresponseOut;
            flightTravellersInfoResponse.SSRAvailabilityIn = flightSSRAvailabilityInfo.SSRresponseIn;
        }
        else if (HttpContext.Session.GetString("SearchValue").Equals("RT"))
        {
            flightTravellersInfoResponse.SearchType = "R";
            var flightSSRAvailabilityInfo = await SSRHelper.GetSSRAvailabilityRT(searchID, flightTravellersInfoResponse.CompanyId, finalResult, selectedFltOut, selectedFltIn, _getAirSSRHandler);
            flightTravellersInfoResponse.SSRAvailabilityOut = flightSSRAvailabilityInfo.SSRresponseOut;
            flightTravellersInfoResponse.SSRAvailabilityIn = flightSSRAvailabilityInfo.SSRresponseIn;
        }
        else
        {
            if (HttpContext.Session.GetString("SelectedFltOut") != null)
            {
                flightTravellersInfoResponse.SearchType = "O";
                var flightSSRAvailabilityInfo = await SSRHelper.GetSSRAvailability(searchID, flightTravellersInfoResponse.CompanyId, finalResult, selectedFltOut, "O", _getAirSSRHandler);
                flightTravellersInfoResponse.SSRAvailabilityOut = flightSSRAvailabilityInfo.SSRresponseOut;
            }
            if (HttpContext.Session.GetString("SelectedFltIn") != null)
            {
                flightTravellersInfoResponse.SearchType = "R";
                var flightSSRAvailabilityInfo = await SSRHelper.GetSSRAvailability(searchID, flightTravellersInfoResponse.CompanyId, finalResult, selectedFltIn, "I", _getAirSSRHandler);
                flightTravellersInfoResponse.SSRAvailabilityIn = flightSSRAvailabilityInfo.SSRresponseOut;
            }
        }

        return flightTravellersInfoResponse;
    }

    
    [HttpGet]
    public async Task<IActionResult> Payment()
    {
        if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null && string.IsNullOrEmpty(Request.Query["regis"])))
        {
            return RedirectToAction("Index");
        }
        if ((HttpContext.Session.GetString("FinalResult") == null || HttpContext.Session.GetString("SelectedFltOut") == null) &&
                HttpContext.Session.GetString("SessionResult4MCList") == null)
        {
            return RedirectToAction("Index");
        }
        var flightPaymentDetails = await FlightPaymentInfoResponse();
        return View(flightPaymentDetails);
    }

    [HttpPost]
    [Route("Flight/Payment/Finalsubmition")]
    public async Task<IActionResult> Finalsubmition([FromBody] PaymentSubmissionRequest paymentSubmissionRequest)
    {
        List<string> FinalsubmitionList = new List<string>();
        var ssionResult4MCListData = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
        if (ssionResult4MCListData != null)
        {

            foreach (SessionResult4MC o in HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList").OrderBy(x => x._SrNo))
            {
                if (o.FinalResult == null)
                {
                    //Response.Redirect("Index.aspx");
                    //Error = "Payment Method not verified";
                }
                HttpContext.Session.SetString("FinalResult", o.FinalResult);
                HttpContext.Session.SetString("SearchID", o.SearchID);
                HttpContext.Session.SetString("SelectedFltOut", o.SelectedFltOut);
                HttpContext.Session.SetString("SearchValue", o.SearchValue);
                HttpContext.Session.SetString("PSQ", o.PSQ);
                HttpContext.Session.Remove("BOOK");


                HttpContextHelper.Current.Session.SetString("SearchValue", "OW");
                FinalsubmitionList.Add(await Finalsubmition_1(paymentSubmissionRequest.PaxChargeDetails, paymentSubmissionRequest.RegisEmail));
                //--- added by me
               // dbCommon.Logger.WriteLogg(Initializer.get_CompanyID(), 0, "Finalsubmition_Call_Finalsubmition_1 ", "ERROR", "SelectFlightId" + o.SelectedFltOut + " Executed of Trip SrNo:" + o._SrNo.ToString(), o.SearchValue, o.SearchID);
            }
            return Ok(new { d = FinalsubmitionList });
        }
        else
        {
            FinalsubmitionList.Add(await Finalsubmition_1(paymentSubmissionRequest.PaxChargeDetails, paymentSubmissionRequest.RegisEmail));
            return Ok(new { d = FinalsubmitionList });
        }

    }

    private async Task<string> Finalsubmition_1(string PaxChargeDetails, string RegisEmail)
    {
        Int32 BookingRef = 0;
        Int32 PaymentID = 0;

        string CompanyID = string.Empty;
        string UpdatedBy = string.Empty;
        string UserType = string.Empty;
        string Error = string.Empty;
        string SearchID = string.Empty;
        string URL = string.Empty;

        Decimal Surcharge = 0;
        bool IsFixed = false;
        bool IsPercent = false;
        string Card_Type_Name = string.Empty;
        string MerchantID = string.Empty;
        string CardType = string.Empty;// CC//DC//NET by cardname
        string CardName = string.Empty;
        string verifyError = string.Empty;
        try
        {
            UpdatedBy = UserHelper.GetStaffID(User);
            CompanyID = UserHelper.GetCompanyID(User);

            if (HttpContext.Session.GetString("BOOK") == null && HttpContext.Session.GetString("FinalResult") != null && HttpContext.Session.GetString("SelectedFltOut") != null && HttpContext.Session.GetString("PaxXML") != null && (CompanyID.IndexOf("A-") != -1 || CompanyID.IndexOf("C-") != -1 || RegisEmail.IndexOf("@") != -1))
            {
                HttpContext.Session.SetString("BOOK", "BOOK");
                SearchID = HttpContext.Session.GetString("SearchID");
                UserType = UserHelper.GetUserType(User);

                string AdminID = string.Empty;
                if (CompanyID.Length.Equals(0) && RegisEmail.Length > 0)
                {
                    AdminID = await _gettWhitelabelAdminidFromHostQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Host.Host);
                    if (AdminID.IndexOf("AD-") != -1)
                    {
                        UserType = "B2C";
                    }
                    else if (AdminID.IndexOf("A-") != -1)
                    {
                        UserType = "B2B2C";
                    }
                }

                Hashtable htPayment = new Hashtable();
                htPayment = PaymentGatewayHelper.GetPaymentTypeAirline(PaxChargeDetails);
                if (htPayment.Count.Equals(6))
                {
                    IsFixed = Convert.ToBoolean(htPayment["FXD"].ToString().Trim());
                    IsPercent = Convert.ToBoolean(htPayment["PER"].ToString().Trim());
                    Surcharge = Convert.ToDecimal(htPayment["CHG"].ToString().Trim());
                    Card_Type_Name = htPayment["CTN"].ToString().Trim();
                    CardType = htPayment["CN"].ToString().Trim();
                    MerchantID = htPayment["MER"].ToString().Trim();

                    if (CardType.IndexOf("CC") != -1)
                    {
                        CardName = "Credit";
                    }
                    else if (CardType.IndexOf("DC") != -1)
                    {
                        CardName = "Debit";
                    }
                    else if (CardType.IndexOf("NET") != -1)
                    {
                        CardName = "NetBanking";
                    }
                    else
                    {
                        CardName = CardType;
                    }

                    string AvailabilityResponse = HttpContext.Session.GetString("FinalResult");
                    string PassengerResponse = HttpContext.Session.GetString("PaxXML");

                    bool IsCombi = false;
                    bool IsRTfare = false;
                    if (HttpContext.Session.GetString("SearchValue").Equals("INT"))
                    {
                        IsCombi = true;
                    }
                    else if (HttpContext.Session.GetString("SearchValue").Equals("RT"))
                    {
                        IsRTfare = true;
                    }

                    string RefID_O = HttpContext.Session.GetString("SelectedFltOut");
                    string RefID_I = string.Empty;
                    if (HttpContext.Session.TryGetValue("SelectedFltIn", out _))
                    {
                        RefID_I = HttpContext.Session.GetString("SelectedFltIn");
                    }


                    bool VerifyBalance = false;
                    if (CardName.ToUpper().Equals("PAYMENTHOLD") && UserType != "B2C" && UserType != "B2B2C")
                    {
                        VerifyBalance = true;  // if payment hold then no need to check the balances
                        XmlDocument _xml = new XmlDocument();
                        _xml.LoadXml(PassengerResponse);
                        XmlNode root = _xml.DocumentElement;
                        XmlNode childNode = _xml.CreateNode(XmlNodeType.Element, "PaymentTypeInfo", "");
                        XmlNode childNode1 = _xml.CreateNode(XmlNodeType.Element, "PaymentType", "");
                        childNode1.InnerText = "PaymentHold";
                        childNode.AppendChild(childNode1);
                        root.AppendChild(childNode);
                        PassengerResponse = _xml.OuterXml;

                    }
                    else if (CardName.ToUpper().Equals("PREPAID") && UserType != "B2C" && UserType != "B2B2C")
                    {
                       await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "VerifyBalance-Before", _addDbLogCommandHandler);

                        try
                        {
                            VerifyBalance = await VerifyAgencyBalanceforTicket(SearchID, CompanyID, AvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, RefID_O, RefID_I);
                        }
                        catch (Exception ex)
                        {
                            VerifyBalance = false;
                            verifyError = ex.Message;
                        }

                      await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "VerifyBalance is : " + VerifyBalance, _addDbLogCommandHandler);
                    }
                    else
                    {
                        if (UserType.Equals("B2B2C") || UserType.Equals("B2B2B"))
                        {
                            bool IsOwnPG = await _getPGOurOwnerStatusQueryHandler.HandleAsync(MerchantID); // for other PG(subagen//customer)
                            if (IsOwnPG.Equals(false))
                            {
                                if (CompanyID.Length > 1)
                                {
                                    try
                                    {
                                        VerifyBalance = await VerifyAgencyBalanceforTicket(SearchID, CommonFunction.getCompany_by_SubCompany_Customer(CompanyID), AvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, RefID_O, RefID_I);
                                    }
                                    catch (Exception ex)
                                    {
                                        verifyError = ex.Message;
                                        VerifyBalance = false;
                                    }
                                }
                                else if (AdminID.Length > 1)
                                {
                                    try
                                    {
                                        VerifyBalance = await VerifyAgencyBalanceforTicket(SearchID, AdminID, AvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, RefID_O, RefID_I);
                                    }
                                    catch (Exception ex)
                                    {
                                        verifyError = ex.Message;
                                        VerifyBalance = false;
                                    }
                                }
                                else
                                {
                                    VerifyBalance = true;
                                }
                            }
                            else
                            {
                                VerifyBalance = true;
                            }
                        }
                    }


                    if (VerifyBalance.Equals(false))
                    {
                        Error = verifyError + Environment.NewLine + " Contact to your Administrator";
                    }
                    else
                    {
                        string GstInfo = string.Empty;
                        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("GstInfo")))
                        {
                            GstInfo = HttpContext.Session.GetString("GstInfo");

                            var setGSTdetailCommand = new SetGSTdetailCommand()
                            { CompanyID = CompanyID, PassengerResponse = PassengerResponse, GSTInfo = GstInfo };
                            await _setGSTdetailCommandHandler.HandleAsync(setGSTdetailCommand);
                        }
                        if ((CardName.ToUpper().Equals("PREPAID") || CardName.ToUpper().Equals("PAYMENTHOLD")) && UserType != "B2C" && UserType != "B2B2C" && CompanyID.IndexOf("A-") != -1)
                        {

                            await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "BookingRef-Before", _addDbLogCommandHandler);
                            var SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);
                            var setbookingQuery = new SetBookingQuery()
                            {
                                SearchID = SearchID,
                                CompanyID = CompanyID,
                                UpdatedBy = UpdatedBy,
                                IsCombi = IsCombi,
                                IsRTfare = IsRTfare,
                                IsQueue = false,
                                IsOffline = false,
                                PaymentType = CardName,
                                PaymentID = string.Empty,
                                PassengerResponse = PassengerResponse,
                                AvailabilityResponse = SelectedAvailabilityResponse,
                                RefID_O = RefID_O,
                                RefID_I = RefID_I,
                                GstInfo = GstInfo
                            };

                            try
                            {
                                BookingRef = await _setBookingQueryHandler.HandleAsync(setbookingQuery);
                            }
                            catch (Exception ex)
                            {
                                Error = ex.Message;
                                _logger.LogError($"Error Genrating Booking Ref : {ex.Message}" , ex.StackTrace);
                            }

                            await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "BookingRef => " + BookingRef, _addDbLogCommandHandler);

                            if (BookingRef > 0)
                            {
                              await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "getPNRdetail-Before", _addDbLogCommandHandler);
                                try
                                {
                                    await GetPNRdetail(IsCombi, HttpContext.Session.GetString("SearchValue"), SearchID, CompanyID, BookingRef, AvailabilityResponse, PassengerResponse, RefID_O, RefID_I, GstInfo, CardName);
                                }
                                catch (Exception ex)
                                {
                                    Error = ex.Message;
                                    _logger.LogError($"Error Genrating PNR : {ex.Message}", ex.StackTrace);
                                }

                               await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, "getPNRdetail - After", _addDbLogCommandHandler);

                            }
                        }
                        else if (CardName.ToUpper() != "PREPAID")
                        {
                            bool tempRegisSave = false;
                            bool tempSave = false;
                            Decimal Amount = 0;
                            Decimal SurchargeAmount = 0;

                            string Mobile = UserHelper.GetCompanyMobile(User);
                            string Email = UserHelper.GetCompanyEmail(User);
                            string Address = UserHelper.GetCompanyAddress(User);
                            string Host = HttpContext.Request.Host.Value;
                            string IP = HttpContext.Connection.RemoteIpAddress.ToString();

                            if (RegisEmail.Length > 0 && RegisEmail.IndexOf("@") != -1)
                            {
                                Email = RegisEmail;
                                TravellerInformationHelper.GetCustomerDetail(out Mobile, out Address);
                            }

                            Amount = await BookingHelper.GetTransactionAmount(SearchID, CompanyID, AdminID, BookingRef, AvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, UserType, RefID_O, RefID_I, _getCFeeQueryHandler);
                            if (IsFixed.Equals(true))
                            {
                                SurchargeAmount = Surcharge;
                            }
                            else if (IsPercent.Equals(true))
                            {
                                SurchargeAmount = CommonFunction.getPercentValue(Amount, Surcharge);
                            }

                            var query = new SetPaymentGatewayLoggerQuery()
                            { MerchantCode = MerchantID, CompanyID = CompanyID, BookingRef = BookingRef, Amount = Amount, SurchargeAmount = SurchargeAmount, Surcharge = Surcharge, CardType = CardType, TransactionType = "AIR", RequestRemark = "BOOK-AIR", Mobile = Mobile, Email = Email, Host = Host, IP = IP, CardName = Card_Type_Name };


                            PaymentID = await _setPaymentGatewayLoggerQueryHandler.HandleAsync(query);

                            if (PaymentID > 0)
                            {
                                if ((UserType.Equals("B2C") || UserType.Equals("B2B2C")) && CompanyID.Length.Equals(0) && RegisEmail.Length > 0 && AdminID.Length > 0)
                                {
                                    // required for B2C and B2B2C - not implemented for now
                                    //tempRegisSave = objbcl.setCustomerRegistrationData(Email, Mobile, Address, AdminID, PaymentID, "AIR");
                                }
                                else
                                {
                                    tempRegisSave = true;
                                }

                                var isMC = false;
                                if (HttpContext.Session.GetString("SearchValue").Equals("MC"))
                                {
                                    isMC = true;
                                }

                                var SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);

                                string Currency = "INR";
                                if (HttpContext.Session.GetString("Curr") == null || HttpContext.Session.GetString("Curr").Equals("INR"))
                                {

                                }
                                else
                                {
                                    Currency = HttpContext.Session.GetString("Curr");
                                }

                                double dCurrency = 1;
                                if (HttpContext.Session.GetString("dCurrency") != null)
                                {
                                    dCurrency = Convert.ToDouble(HttpContext.Session.GetString("dCurrency"));
                                }

                                var setBookingAirlineLogForPGQuery = new SetBookingAirlineLogForPGQuery()
                                {
                                    IsCombi = IsCombi,
                                    IsRT = IsRTfare,
                                    IsMC = isMC,
                                    SearchID = SearchID,
                                    CompanyID = CompanyID,
                                    BookingRef = BookingRef,
                                    AvailabilityResponse = SelectedAvailabilityResponse,
                                    PassengerResponse = PassengerResponse,
                                    RefID_O = RefID_O,
                                    RefID_I = RefID_I,
                                    PaymentID = PaymentID,
                                    PaymentType = CardName,
                                    Currency = Currency,
                                    CurrencyValue = dCurrency
                                };
                                tempSave = await _setBookingAirlineLogForPGQueryHandler.HandleAsync(setBookingAirlineLogForPGQuery);
                            }

                            if (PaymentID > 0 && MerchantID.Equals("rzp_test_ijLW5upIxt81Pb") && tempSave && tempRegisSave)
                            {
                                URL = "//pg.monatours.in/defaultpg.aspx?referkey=" + PaymentID.ToString();
                            }
                            else if (PaymentID > 0 && MerchantID.Equals("rzp_live_4uZXcr9aIrB4PF") && tempSave && tempRegisSave)
                            {
                                URL = "//pg.monatours.in/defaultpg.aspx?referkey=" + PaymentID.ToString();
                            }
                            else
                            {
                                BookingRef = 0;
                            }
                        }
                    }
                }

                else
                {
                    Error = "Payment Method not verified";
                }
            }
            else
            {
                Error = "Double Click !!! Session not verified !!!";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error Booking Ticket - {BookingRef}: {ex.Message}");
            await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "Finalsubmition", "FlightController", Error, SearchID, ex.Message, _addDbLogCommandHandler);
        }

        if (BookingRef > 0)
        {
            return BookingRef.ToString() + "^" + CardType + "^" + URL + "!" + CardName + "!" + PaymentID;
        }
        else if (PaymentID > 0)
        {
            return BookingRef.ToString() + "^" + CardType + "^" + URL + "!" + CardName + "!" + PaymentID;
        }
        else
        {
            return BookingRef.ToString() + "^" + CardType + "^" + Error + "!" + CardName + "!" + PaymentID;
        }
    }

    private async Task<FlightPaymentInfoResponse> FlightPaymentInfoResponse()
    {
        var flightPaymentInfoResponse = new FlightPaymentInfoResponse();
        flightPaymentInfoResponse.HostName = _configuration["SiteURL:BasePath"];
        flightPaymentInfoResponse.LivePathImage = "/assets/img/CommonImages/";
        if (HttpContext.Session.GetString("Curr") != null)
        {
            flightPaymentInfoResponse.CurrencyType = HttpContext.Session.GetString("Curr");
        }
        else
        {
            flightPaymentInfoResponse.CurrencyType = "INR";
        }
        if (HttpContext.Session.GetString("regis") != null)
        {
            flightPaymentInfoResponse.RegisterEmail = HttpContext.Session.GetString("regis");
        }

        flightPaymentInfoResponse.RoundTrip = HttpContext.Session.GetString("SearchValue"); 


        //set compnay
        var cmpid = UserHelper.GetCompanyID(User);
        flightPaymentInfoResponse.CompanyId = cmpid;
        //set promo
        PromoDiscountHelper.AddPromo(_gettWhitelabelAdminidFromHostQueryHandler, _getPromoAmountQueryHandler);
        //set cfee
        string strcmpid = cmpid;
        if (string.IsNullOrEmpty(strcmpid) || string.IsNullOrEmpty(Request.Query["value"]))
        {
            strcmpid = await _gettWhitelabelAdminidFromHostQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Host.Host);
        }
        string Sector = "D";
        if (HttpContext.Session.GetString("SEARCH_TYPE") == "INT")
        {
            Sector = "I";
        }
        var isValidCfee = await _isWalletCfeeQueryHandler.HandleAsync(new IsWalletCfeeQuery { CompanyID = strcmpid, BookingType = "AIRLINE", Sector = Sector });
        if (isValidCfee)
        {
            flightPaymentInfoResponse.ValidCfee = "1";
        }
        else
        {
            flightPaymentInfoResponse.ValidCfee = "0";
        }


        var ssionResult4MCListData = HttpContext.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
        if (ssionResult4MCListData != null)

        {
            HttpContext.Session.SetString("SearchValue", "MC");
           
            StringBuilder OutbountItinary;
            StringBuilder Faredetaildiv;
            TravellerInformationHelper.GetFilghtShow(out OutbountItinary, out Faredetaildiv, true);

            flightPaymentInfoResponse.OutbountItinary = OutbountItinary.ToString();
            flightPaymentInfoResponse.Faredetaildiv = Faredetaildiv.ToString();
            flightPaymentInfoResponse.Pageredirect = TravellerInformationHelper.GetBackPageName(flightPaymentInfoResponse.CompanyId, true);
            HttpContext.Session.SetString("SearchValue", "OW");

        }
        else
        {

            StringBuilder OutbountItinary;
            StringBuilder Faredetaildiv;
            TravellerInformationHelper.GetFilghtShow(out OutbountItinary, out Faredetaildiv, true);

            flightPaymentInfoResponse.OutbountItinary = OutbountItinary.ToString();
            flightPaymentInfoResponse.Faredetaildiv = Faredetaildiv.ToString();
            flightPaymentInfoResponse.Pageredirect = TravellerInformationHelper.GetBackPageName(flightPaymentInfoResponse.CompanyId, false);
        }

        //Set payment options

        string strAdminID = string.Empty;
        if (cmpid.Equals(string.Empty) && (string.IsNullOrEmpty(Request.Query["regis"])))
        {
            strAdminID = await _gettWhitelabelAdminidFromHostQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Host.Host);
        }

        var paymentOptionsList = await _getPaymentGatewayCardChargesQueryHandler.HandleAsync(new GetPaymentGatewayCardChargesQuery { CompanyID = cmpid, AdminID = strAdminID});
        string DetailsPayType = XMLHelper.ConvertListToXml(paymentOptionsList);

        flightPaymentInfoResponse = SetPaymentOptionHTML(DetailsPayType, flightPaymentInfoResponse) ;

        // set js fare info

        int markup = 0;
        int tds = 0;
        int totalfare = 0;
        int discount = 0;
        int cfee = 0;

        
        TravellerInformationHelper.GetFilghtDisplayJsFareInformation(out markup, out tds, out totalfare, out discount, out cfee);

        flightPaymentInfoResponse.TotalMarkup = markup.ToString();
        flightPaymentInfoResponse.TotalTDS = tds.ToString();
        flightPaymentInfoResponse.TotalFare = totalfare.ToString();
        flightPaymentInfoResponse.Discount = discount.ToString();
        flightPaymentInfoResponse.TotalCfee = cfee.ToString();

      
        // set passengers info

        flightPaymentInfoResponse.PaxInformation = TravellerInformationHelper.GetTravellerInformation();

        return flightPaymentInfoResponse;
    }

    private async Task<bool> VerifyAgencyBalanceforTicket(string SearchID, string CompanyID, string AvailabilityResponse, string PassengerResponse, bool IsCombi, bool IsRTfare, string RefID_O, string RefID_I)
    {
        try
        {
            var SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);
            Decimal dTicketAmount = ZealTravel.Front.Web.Helper.Flight.BookingHelper.GETTransactionAmount(SearchID, CompanyID, 0, SelectedAvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, RefID_O, RefID_I);
            if (dTicketAmount > 200)
            {
                var _verifyTicketBalanceQuery = new VerifyTicketBalanceQuery { CompanyID = CompanyID, TicketAmount = dTicketAmount };
                return await _verifyTicketBalanceQueryHandler.HandleAsync(_verifyTicketBalanceQuery);
            }
           
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            //DBLoggerHelper.DBLogAsync(CompanyID, 0, "VerifyAgencyBalanceforTicket", "BookingSaveService", "VerifyBalance", SearchID, ex.Message);
            throw new Exception(ex.Message);
        }
    }

    public FlightPaymentInfoResponse SetPaymentOptionHTML(string detailsPayType, FlightPaymentInfoResponse flightPaymentInfoResponse)
    {
        try
        {
            bool IsPrepaid = false;
            bool IsCC = false;
            bool IsDC = false;
            bool IsNET = false;
            bool IsElse = false;

            DataTable dtPayment = CommonFunction.StringToDataSet(detailsPayType).Tables[0];
            if (dtPayment != null && dtPayment.Rows.Count > 0)
            {
                DataRow[] drSelect = dtPayment.Select("Card_Type='" + "Prepaid" + "'");
                if (drSelect.Length > 0)
                {
                    IsPrepaid = true;
                }
                else
                {
                    drSelect = dtPayment.Select("Card_Type='" + "Credit" + "'");
                    if (drSelect.Length > 0)
                    {
                        IsCC = true;
                    }
                    else
                    {
                        drSelect = dtPayment.Select("Card_Type='" + "Debit" + "'");
                        if (drSelect.Length > 0)
                        {
                            IsDC = true;
                        }
                        else
                        {
                            drSelect = dtPayment.Select("Card_Type='" + "NetBanking" + "'");
                            if (drSelect.Length > 0)
                            {
                                IsNET = true;
                            }
                            else
                            {
                                IsElse = true;
                            }
                        }
                    }
                }
            }

            StringBuilder PGResponse = new StringBuilder();
            StringBuilder PGtabdata = new StringBuilder();
            XmlDocument xd2 = new XmlDocument();
            xd2.LoadXml(detailsPayType);
            int credidstatus = 0;
            int Debitstatus = 0;
            int Netstatus = 0;
            int Prepaidstatus = 0;
            // int Otherstatus = 0;
            PGResponse.Append("<div class='list-group'>");
            XmlNodeList nodes = xd2.DocumentElement.SelectNodes("/ArrayOfPaymentGatewayDisplayOption/PaymentGatewayDisplayOption");
            for (int i = 0; i < nodes.Count; i++)
            {
                string Merchantcode = nodes[i].SelectSingleNode("Merchant_Code").InnerText;
                string Cardname = nodes[i].SelectSingleNode("Card_Name").InnerText;
                string cardtype = nodes[i].SelectSingleNode("Card_Type").InnerText;
                string pgname = nodes[i].SelectSingleNode("PG_Name").InnerText;
                string charhe = nodes[i].SelectSingleNode("Charges").InnerText;
                string cardtypename = nodes[i].SelectSingleNode("Card_Type_Name").InnerText;
                //string charhe = "2";
                string fix = nodes[i].SelectSingleNode("FIXED").InnerText;
                string percent = nodes[i].SelectSingleNode("PERCNT").InnerText;

                if (cardtype == "Credit")
                {
                    if (credidstatus == 0)
                    {
                        if (IsCC)
                        {
                            PGResponse.Append("<a href='#' class='list-group-item active text-center'>Credit</a>");
                        }
                        else
                        {
                            PGResponse.Append("<a href='#' class='list-group-item text-center'>Credit</a>");
                        }

                        XmlNodeList Creditnodes = xd2.SelectNodes("/PGCardChgDetails/Payment_Gateway_Card_Charges_Detail/Card_Type[.='" + cardtype + "']");

                        if (IsCC)
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content active' style='min-height:331px;'>");
                        }
                        else
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content' style='min-height:331px;'>");
                        }

                        PGtabdata.Append("<center>");
                        for (int j = 0; j < Creditnodes.Count; j++)
                        {

                            PGtabdata.Append("<div class='col-md-4 col-sm-4 col-xs-4 paymat' style='text-align:center; font-size:18px;'>");


                            PGtabdata.Append("<label class='radio-inline'>");
                            PGtabdata.Append("<input type='radio' onchange ='calulateCharge(" + nodes[j + i].SelectSingleNode("FIXED").InnerText + "," + nodes[j + i].SelectSingleNode("Charges").InnerText + "," + nodes[j + i].SelectSingleNode("PERCNT").InnerText + ")' name='optradio' value='Credit----" + nodes[j + i].SelectSingleNode("Card_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Card_Type_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Merchant_Code").InnerText + "'>");
                            PGtabdata.Append("<img src='/assets/img/BankLogo/" + nodes[j + i].SelectSingleNode("Card_Type_Name").InnerText.ToUpper() + ".png' style='width: 93px;  margin-left:0px;    height: 41px; ' alt=''/>");
                            PGtabdata.Append("</label>");
                            if (nodes[j + i].SelectSingleNode("FIXED").InnerText == "true")
                            {
                                PGtabdata.Append("<span class='paysebsic5'>Surcharge + " + nodes[j + i].SelectSingleNode("Charges").InnerText + "  </span>");
                            }
                            else
                            {
                                PGtabdata.Append("<span class='paysebsic' style='margin-left:25px;'>Surcharge  " + nodes[j + i].SelectSingleNode("Charges").InnerText + " %</span>");
                            }
                            PGtabdata.Append("</div>");
                        }

                        PGtabdata.Append("</center>");
                        PGtabdata.Append("</div>");
                    }
                    credidstatus = credidstatus + 1;
                }
                else if (cardtype == "Debit")
                {
                    if (Debitstatus == 0)
                    {
                        if (IsDC)
                        {
                            PGResponse.Append("<a href='#' class='list-group-item active  text-center'>Debit</a>");
                        }
                        else
                        {
                            PGResponse.Append("<a href='#' class='list-group-item  text-center'>Debit</a>");
                        }


                        XmlNodeList Debitnodes = xd2.SelectNodes("/PGCardChgDetails/Payment_Gateway_Card_Charges_Detail/Card_Type[.='" + cardtype + "']");

                        if (IsDC)
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content active' style='min-height:331px;'>");
                        }
                        else
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content' style='min-height:331px;'>");
                        }

                        PGtabdata.Append("<center>");
                        for (int j = 0; j < Debitnodes.Count; j++)
                        {
                            PGtabdata.Append("<div class='col-md-4 col-sm-4 col-xs-4 paymat' style='text-align:center; font-size:18px;'>");
                            PGtabdata.Append("<label class='radio-inline' >");
                            PGtabdata.Append("<input type='radio' onchange ='calulateCharge(" + nodes[j + i].SelectSingleNode("FIXED").InnerText + "," + nodes[j + i].SelectSingleNode("Charges").InnerText + "," + nodes[j + i].SelectSingleNode("PERCNT").InnerText + ")' name='optradio' value='Debit----" + nodes[j + i].SelectSingleNode("Card_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Card_Type_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Merchant_Code").InnerText + "'>");
                            PGtabdata.Append("<img src='/assets/img/BankLogo/" + nodes[j + i].SelectSingleNode("PG_Name").InnerText.ToUpper() + ".png' style='width: 93px;  margin-left:0px;    height: 41px; ' alt=''/>");
                            PGtabdata.Append("</label>");
                            if (nodes[j + i].SelectSingleNode("FIXED").InnerText == "true")
                            {
                                PGtabdata.Append("<span class='paysebsic6'>Surcharge  + " + nodes[j + i].SelectSingleNode("Charges").InnerText + "  </span>");
                            }
                            else
                            {
                                PGtabdata.Append("<span class='paysebsic1' style='margin-left:25px;'>Surcharge  " + nodes[j + i].SelectSingleNode("Charges").InnerText + " %</span>");
                            }
                            PGtabdata.Append("</div>");
                        }
                        PGtabdata.Append("</center>");
                        PGtabdata.Append("</div>");
                    }
                    Debitstatus = Debitstatus + 1;
                }
                else if (cardtype == "NetBanking")
                {
                    if (Netstatus == 0)
                    {
                        if (IsNET)
                        {
                            PGResponse.Append("<a href='#' class='list-group-item active  text-center'>NetBanking</a>");
                        }
                        else
                        {
                            PGResponse.Append("<a href='#' class='list-group-item  text-center'>NetBanking</a>");
                        }


                        XmlNodeList NetBanknodes = xd2.SelectNodes("/PGCardChgDetails/Payment_Gateway_Card_Charges_Detail/Card_Type[.='" + cardtype + "']");

                        if (IsNET)
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content active' style='min-height:331px;'>");
                        }
                        else
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content' style='min-height:331px;'>");
                        }

                        PGtabdata.Append("<center>");
                        for (int j = 0; j < NetBanknodes.Count; j++)
                        {
                            PGtabdata.Append("<div class='col-md-4 col-sm-4 col-xs-4 paymat' style='text-align:center; font-size:18px; '>");

                            PGtabdata.Append("<label class='radio-inline' >");
                            PGtabdata.Append("<input type='radio' onchange ='calulateCharge(" + nodes[j + i].SelectSingleNode("FIXED").InnerText + "," + nodes[j + i].SelectSingleNode("Charges").InnerText + "," + nodes[j + i].SelectSingleNode("PERCNT").InnerText + ")' name='optradio' value='NetBanking----" + nodes[j + i].SelectSingleNode("Card_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Card_Type_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Merchant_Code").InnerText + "'>");
                            PGtabdata.Append("<img src='/assets/img/BankLogo/" + nodes[j + i].SelectSingleNode("PG_Name").InnerText.ToUpper() + ".png' style='width: 93px;  margin-left:0px;    height: 41px; ' alt=''/>");
                            PGtabdata.Append("</label>");
                            if (nodes[j + i].SelectSingleNode("FIXED").InnerText == "true")
                            {
                                PGtabdata.Append("<span class='paysebsic2' >Surcharge + " + nodes[j + i].SelectSingleNode("Charges").InnerText + "  </span>");
                            }
                            else
                            {
                                PGtabdata.Append("<span class='paysebsic2' style='margin-left:25px;'>Surcharge  " + nodes[j + i].SelectSingleNode("Charges").InnerText + " %</span>");
                            }
                            PGtabdata.Append("</div>");
                        }
                        PGtabdata.Append("</center>");
                        PGtabdata.Append("</div>");
                    }
                    Netstatus = Netstatus + 1;
                }
                else if (cardtype == "Prepaid")
                {
                    string PG_logo = pgname.ToUpper();
                    if (Prepaidstatus == 0)
                    {
                        if (IsPrepaid)
                        {
                            PGResponse.Append("<a href='#' class='list-group-item active  text-center'>Wallet</a>");
                        }
                        else
                        {
                            PGResponse.Append("<a href='#' class='list-group-item  text-center'>Wallet</a>");
                        }

                        XmlNodeList Prepaidnodes = xd2.SelectNodes("/ArrayOfPaymentGatewayDisplayOption/PaymentGatewayDisplayOption/Card_Type[.='" + cardtype + "']");
                        if (IsPrepaid)
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content active' style='min-height:331px;'>");
                        }
                        else
                        {
                            PGtabdata.Append("<div class='bhoechie-tab-content' style='min-height:331px;'>");
                        }

                        PGtabdata.Append("<center>");
                        for (int j = 0; j < Prepaidnodes.Count; j++)
                        {
                            PGtabdata.Append("<div class='col-md-4 col-sm-4 col-xs-4 paymat' style='text-align:center; font-size:18px; '>");
                            PGtabdata.Append("<label class='radio-inline' >");
                            PGtabdata.Append("<input type='radio' onchange ='calulateCharge(" + nodes[j + i].SelectSingleNode("FIXED").InnerText + "," + nodes[j + i].SelectSingleNode("Charges").InnerText + "," + nodes[j + i].SelectSingleNode("PERCNT").InnerText + ")' name='optradio' value='Prepaid----" + nodes[j + i].SelectSingleNode("Card_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Card_Type_Name").InnerText + "|" + nodes[j + i].SelectSingleNode("Merchant_Code").InnerText + "'>");
                            PGtabdata.Append("<img src='/assets/img/BankLogo/" + nodes[j + i].SelectSingleNode("PG_Name").InnerText.ToUpper() + ".png' style='width: 93px;  margin-left:0px;    height: 41px; ' alt=''/>");
                            PGtabdata.Append("</label>");
                            PGtabdata.Append("</div>");
                        }
                        PGtabdata.Append("</center>");
                        PGtabdata.Append("</div>");
                    }
                    Prepaidstatus = Prepaidstatus + 1;
                }
                else
                {
                    if (IsElse)
                    {
                        PGResponse.Append("<a href='#' class='list-group-item active  text-center'>" + cardtype + "</a>");
                    }
                    else
                    {
                        PGResponse.Append("<a href='#' class='list-group-item   text-center'>" + cardtype + "</a>");
                    }


                    if (IsElse)
                    {
                        PGtabdata.Append("<div class='bhoechie-tab-content active' style='min-height:331px;'>");
                    }
                    else
                    {
                        PGtabdata.Append("<div class='bhoechie-tab-content ' style='min-height:331px;'>");
                    }
                    PGtabdata.Append("<center>");
                    PGtabdata.Append("<div class='col-md-4 col-sm-4 col-xs-4 paymat' style='text-align:center; font-size:18px; '>");
                    PGtabdata.Append("<label class='radio-inline' >");
                    PGtabdata.Append("<input type='radio' onchange ='calulateCharge(" + nodes[i].SelectSingleNode("FIXED").InnerText + "," + nodes[i].SelectSingleNode("Charges").InnerText + "," + nodes[i].SelectSingleNode("PERCNT").InnerText + ")' name='optradio' value='" + cardtype + "----" + nodes[i].SelectSingleNode("Card_Name").InnerText + "|" + nodes[i].SelectSingleNode("Card_Type_Name").InnerText + "|" + nodes[i].SelectSingleNode("Merchant_Code").InnerText + "'>");
                    PGtabdata.Append("<img src='/assets/img/BankLogo/" + nodes[i].SelectSingleNode("PG_Name").InnerText.ToUpper() + ".png' style='width: 93px;  margin-left:0px;    height: 41px; ' alt=''/>");
                    PGtabdata.Append("</label>");
                    if (nodes[i].SelectSingleNode("FIXED").InnerText == "true")
                    {
                        PGtabdata.Append("<span class='paysebsic4'>Surcharge + " + nodes[i].SelectSingleNode("Charges").InnerText + "  </span>");
                        //PGtabdata.Append("<span>Surcharge Rs. + 1</span>");
                    }
                    else
                    {
                        PGtabdata.Append("<span class='paysebsic3' style='margin-left:25px;'>Surcharge  " + nodes[i].SelectSingleNode("Charges").InnerText + " %</span>");
                        // PGtabdata.Append("<span style='margin-left:25px;'>Surcharge  1 %</span>");
                    }
                    PGtabdata.Append("</div>");
                    // }
                    PGtabdata.Append("</center>");
                    PGtabdata.Append("</div>");

                }
            }
            PGResponse.Append("</div>");
            flightPaymentInfoResponse.PaymentTabMenu = PGResponse.ToString();
            flightPaymentInfoResponse.PaymentTabContent = PGtabdata.ToString();

            if (!string.IsNullOrEmpty(Request.Query["regis"]))
            {
               flightPaymentInfoResponse.RegisterEmail = Request.Query["regis"].ToString().Trim();
            }

            
            // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "prepaid()", true);
        }
        catch (Exception ex)
        {
           DBLoggerHelper.DBLogAsync("", 0, "FlightController", "setpaymentdiv", "", detailsPayType, ex.Message, _addDbLogCommandHandler);
        }
        return flightPaymentInfoResponse;
    }

    private async Task<bool> GetPNRdetail(bool IsCombi, string resultType, string SearchID, string CompanyID, Int32 BookingRef, string AirRS, string PassengerRS, string RefID_O, string RefID_I, string GSTInfo,string PaymentType)
    {
        bool status = false;
        string SelectAirRS = string.Empty;

        try
        {

            
            SelectAirRS = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AirRS, IsCombi);


            var addDBLogger = new AddDBSearchLogCommand
            {
                CompanyID = CompanyID,
                BookingRef = BookingRef,
                StaffID = UserHelper.GetStaffID(User),
                SearchID = SearchID,
                Location = "PNR",
                Status = "BOOK",
                Place = "FO",
                Remark = SelectAirRS,
                Remark2 = PassengerRS,
                Host = HttpContextHelper.Current.Request.Host.Host + "," + HttpContextHelper.Current.Request.HttpContext.Connection.RemoteIpAddress.ToString()

            };

        
            await _addDbSearchLogCommandHandler.HandleAsync(addDBLogger);

            string JourneyType = "";
            if (IsCombi.Equals(true))
            {
                JourneyType = "RT";
            }
            else if (resultType.IndexOf("RT") != -1)
            {
                JourneyType = "RTLCC";
            }
            else if (resultType.IndexOf("MC") != -1)
            {
                JourneyType = "MC";
            }
            else
            {
                JourneyType = "OW";
            }

            var query = new GetAirCommitQuery()
            {
                JourneyType = JourneyType,
                SearchID = SearchID,
                CompanyID = CompanyID,
                BookingRef = BookingRef,
                AirRS = SelectAirRS,
                PassengerRS = PassengerRS,
                GstRS = GSTInfo,
                PaymentType = PaymentType
            };

            status = await _getAirCommitHandler.HandleAsync(query);
         
        }
        catch (Exception ex)
        {
           await DBLoggerHelper.DBLogAsync(CompanyID, BookingRef, "getPNRdetail", "clsPNR", SelectAirRS + Environment.NewLine + PassengerRS, SearchID, ex.Message, _addDbLogCommandHandler);
            throw new Exception("getPNRdetail - " + ex.Message);
        }

        return status;
    }
}

