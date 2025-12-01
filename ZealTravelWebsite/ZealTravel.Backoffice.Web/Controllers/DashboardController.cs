using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Application.BackofficeManagement.Queries.Dashboard;
using ZealTravel.Application.BackofficeManagement.Handlers.Dashboard;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Common.Helpers;
using ZealTravel.Backoffice.Web.Models.Dashboard;
using ZealTravel.Application.AgencyManagement.Queries;
using log4net.Core;
namespace ZealTravel.Backoffice.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;
        private readonly IHandlesQueryAsync<List<DashboardAirlinePendingBookingData>> _dashboardAirlinePendingBookingHandler;
        private readonly IHandlesQueryAsync<DashboardAirlinetopTenBookingQuery, List<DashboardAirlineTopTenBookingData>> _dashboardAirlinetopTenBookingHandler;
        private readonly IHandlesQueryAsync<GetCompanyPopupDetailsQuery, CompanyPopupDetails> _getCompanyPopupDetailsQueryHandler;

        public DashboardController(IConfiguration configuration, IMapper mapper, IHandlesQueryAsync<List<DashboardAirlinePendingBookingData>> dashboardAirlinePendingBookingHandler, IHandlesQueryAsync<DashboardAirlinetopTenBookingQuery, List<DashboardAirlineTopTenBookingData>> dashboardAirlinetopTenBookingHandler, IHandlesQueryAsync<GetCompanyPopupDetailsQuery, CompanyPopupDetails> getCompanyPopupDetailsQueryHandler, ILogger<DashboardController> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _dashboardAirlinePendingBookingHandler = dashboardAirlinePendingBookingHandler;
            _dashboardAirlinetopTenBookingHandler = dashboardAirlinetopTenBookingHandler;
            _getCompanyPopupDetailsQueryHandler = getCompanyPopupDetailsQueryHandler;
            _logger = logger;
        }

        [Route("/Dashboard")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {

            var companyId = UserHelper.GetCompanyID(User);
            var airlinePendingBookings = await BindAirlinePendingBookings();
            var airlineTopTenBookings = await BindAirlineTopTenBooking(companyId);
            var dashboardData = new DashboardResponse
            {
                DashboardAirlinePendingBookings = airlinePendingBookings,
                DashboardAirlinetopTenBookings = airlineTopTenBookings
            };
            return View("~/Views/Dashboard/Dashboard.cshtml", dashboardData);
        }

        public async Task<string> Showpopup(string srchtxt)
        {
            var result = string.Empty;
            var companyID = srchtxt.Split('_')[0];
            var query = new GetCompanyPopupDetailsQuery
            {
                CompanyID = companyID
            };
            var companyPopupDetails = await _getCompanyPopupDetailsQueryHandler.HandleAsync(query);

            if (companyPopupDetails != null)
            {

                result = companyPopupDetails.CompanyName + "," + companyPopupDetails.FirstName + "," + companyPopupDetails.LastName + "," + companyPopupDetails.Mobile + "," + companyPopupDetails.Email + "," + companyPopupDetails.Pan_Name + "," + companyPopupDetails.Pan_No + "," + companyPopupDetails.UserType + "," + companyPopupDetails.AccountID + "," + companyPopupDetails.Gst;

            }
            return result;
        }


        private async Task<List<DashboardAirlinePendingBookingData>> BindAirlinePendingBookings()
        {
            return await _dashboardAirlinePendingBookingHandler.HandleAsync();
        }
        private async Task<List<DashboardAirlineTopTenBookingData>> BindAirlineTopTenBooking(string companyID)
        {
            var query = new DashboardAirlinetopTenBookingQuery
            {
                CompanyId = companyID
            };

            return await _dashboardAirlinetopTenBookingHandler.HandleAsync(query);
        }
    }
}
