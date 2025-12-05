using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models.Flight;
using log4net.Core;

namespace ZealTravel.Front.Web.Controllers
{
    [Authorize]
    public class FlightDiscountedRoundController :  AgencyBaseController
    {
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private readonly IConfiguration _configuration;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly ILogger<FlightDiscountedRoundController> _logger;

        public FlightDiscountedRoundController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler, IConfiguration configuration, IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler, ILogger<FlightDiscountedRoundController> logger) : base(getAvailableBalanceQueryHandler)
        {
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _configuration = configuration;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _logger = logger;
        }


        [Route("/flight/int")]
        [HttpGet]
        public async Task<IActionResult> DiscountedRoundFlight()
        {
            // Check for searchId in query parameter (for progressive loading)
            var searchIdFromQuery = HttpContext.Request.Query["searchId"].ToString();
            if (!string.IsNullOrEmpty(searchIdFromQuery))
            {
                // Set SearchID in session if provided in query parameter
                HttpContext.Session.SetString("SearchID", searchIdFromQuery);
            }

            if (HttpContext.Session.GetString("SearchID") == null || (User == null && HttpContext.Session.GetString("accidC") == null))
            {
                return RedirectToAction("Index", "Flight");
            }
            var roundFlightResponse = await DiscounteRoundFlightGetPageValues();

            return View("~/Views/Flight/Int.cshtml", roundFlightResponse);
        }

        private async Task<DiscountedRoundFlightResponse> DiscounteRoundFlightGetPageValues()
        {
            var roundFlightResponse = new DiscountedRoundFlightResponse();
            var roundAccountId = string.Empty;
            var roundPSQXML = string.Empty;
            roundFlightResponse.HostName = _configuration["SiteURL:BasePath"];
            roundFlightResponse.URL = HttpContext.Request.Host.Value;
            roundFlightResponse.CompanyId = string.Empty;

            if (string.IsNullOrEmpty(HttpContext.Request.Query["value"]) && User != null && User.Identity.IsAuthenticated && (_configuration["Company:IsBO"].Equals("0")))
            {
                roundFlightResponse.CompanyId = UserHelper.GetCompanyID(User);
            }
            else
            {
                roundAccountId = EncodeDecodeHelper.DecodeFrom64(HttpContext.Request.Query["value"].ToString().Trim());
                if (!string.IsNullOrEmpty(roundAccountId))
                {
                    roundFlightResponse.AccountNumber = roundAccountId;
                    var companyIdByAccountIdQuery = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(roundAccountId) };
                    roundFlightResponse.CompanyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(companyIdByAccountIdQuery);
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
                        roundFlightResponse.AdultsCount = obj[0]._NoOfAdult;
                        roundFlightResponse.ChildrenCount = obj[0]._NoOfChild;
                        roundFlightResponse.InfantsCount = obj[0]._NoOfInfant;
                        roundFlightResponse.OriginCity = obj[0]._DepartureStation;
                        roundFlightResponse.ArrivalCity = obj[0]._ArrivalStation;
                        roundFlightResponse.DepartureDate = obj[0]._BeginDate;
                        roundFlightResponse.ArrivalDate = obj[0]._EndDate;
                        roundFlightResponse.DepartureStation = obj[0]._DepartureStation;
                        roundFlightResponse.ArrivalStation = obj[0]._ArrivalStation;

                        string OrgPSQ1 = "<AvailabilityRequest><DepartureStation>" + obj[0]._DepartureStation + "</DepartureStation><ArrivalStation>" + obj[0]._ArrivalStation + "</ArrivalStation><Cabin>Y</Cabin>";
                        string OrgPSQ2 = "<StartDate>" + obj[0]._BeginDate + "</StartDate><EndDate>" + obj[0]._EndDate + "</EndDate><Adult>" + obj[0]._NoOfAdult + "</Adult><Child>" + obj[0]._NoOfChild + "</Child><Infant>" + obj[0]._NoOfInfant + "</Infant></AvailabilityRequest>";
                        string FinalPsq = OrgPSQ1 + OrgPSQ2;
                        roundFlightResponse.PSQXMl = FinalPsq;


                        roundFlightResponse.Cabin = obj[0]._TravelClass.Equals("C") ? "Business" : "Economy";
                        roundFlightResponse.CityDeparture = arrdep[1];
                        roundFlightResponse.CityArrival = arrArr[1];
                        roundFlightResponse.LblModsrcdes = $"<span class='depboxfi'>{DepartureStation}</span><span class='mydkduq'> <i class='fa fa-plane planeki3'></i>  </span><span class='depboxfi yte'>{ArrivalStation}</span>";
                        roundFlightResponse.LblTravlDate = $"<span class='kdgrw'><i class='fa fa-calendar' style='font-size: 14px;'></i> <span>Onward</span></span> {obj[0]._BeginDate}";
                        var searchdate = DateHelper.EightDIgit2DateFormat(obj[0]._BeginDate);
                        roundFlightResponse.CalenderVisibleDate = Convert.ToDateTime(searchdate);
                        roundFlightResponse.CalenderSelectedDate = Convert.ToDateTime(searchdate);

                        var sb = new StringBuilder();
                        sb.Append("<table>");
                        sb.Append("<tr><td style='padding-left:20px;font-size: 14px;'> Adult </td><td style='padding-left:20px;font-size: 14px;'> Child </td><td style='padding-left:20px;font-size: 14px;'> Infant </td></tr>");
                        sb.Append("<tr><td style='padding-left:30px'>" + obj[0]._NoOfAdult + "</td>");
                        sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfChild) > 0 ? obj[0]._NoOfChild : "---") + "</td>");
                        sb.Append("<td style='padding-left:30px'>" + (int.Parse(obj[0]._NoOfInfant) > 0 ? obj[0]._NoOfInfant : "---") + "</td></tr>");
                        sb.Append("</table>");
                        roundFlightResponse.LblNoofPax = sb.ToString();
                    }
                }

                roundPSQXML = HttpContext.Session.GetString("PSQ");
                roundFlightResponse.PreferredAirline = roundPSQXML;
                roundFlightResponse.CurrencyType = HttpContext.Session.GetString("Curr") ?? "INR";
            }
            return roundFlightResponse;
        }
    }
}
