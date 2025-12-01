using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ZealTravel.Application.AgencyManagement.Handlers.AgencyDashboard;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Front.Web.Models.Agency.Dashboard;
using AutoMapper;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using log4net.Core;

namespace ZealTravel.Front.Web.Controllers
{
    [Authorize]
    public class AgencyDashboardController : AgencyBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IHandlesQueryAsync<DashboardLedgerQuery, List<DashboardLedgerData>> _ledgerQueryHandler;
        private readonly IHandlesQueryAsync<DashboardNotificationQuery, List<DashboardNotificationData>> _notificationQueryHandler;
        private readonly IHandlesQueryAsync<DashboardChartQuery, List<DashboardChartData>> _dashboardChartQueryHandler;
        private readonly IHandlesQueryAsync<DashboardCorporateQuery, List<DashboardCorporateData>> _dashboardCorporateQueryHandler;
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<AgencyDashboardController> _logger;

        public AgencyDashboardController(
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler,
            IHandlesQueryAsync<DashboardLedgerQuery, List<DashboardLedgerData>> ledgerQueryHandler,
            IHandlesQueryAsync<DashboardNotificationQuery, List<DashboardNotificationData>> notificationQueryHandler,
            IHandlesQueryAsync<DashboardChartQuery, List<DashboardChartData>> dashboardChartQueryHandler,
            IHandlesQueryAsync<DashboardCorporateQuery, List<DashboardCorporateData>> dashboardCorporateQueryHandler,
            IConfiguration configuration,
            ILogger<AgencyDashboardController> logger,
            IMapper mapper
            ) : base(getAvailableBalanceQueryHandler)
        {
            _ledgerQueryHandler = ledgerQueryHandler;
            _notificationQueryHandler = notificationQueryHandler;
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _dashboardChartQueryHandler = dashboardChartQueryHandler;
            _dashboardCorporateQueryHandler = dashboardCorporateQueryHandler;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        [Route("agency/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var companyId = UserHelper.GetCompanyID(User);

            var ledgerResponse = await BindLedgerData(companyId);
            var notificationResponse = await BindNotificationData(companyId);
            var availableBalance = await BindAvailableBalance(companyId);
            var corporateResponse = await BindCorporateData(companyId);
            var chartXmlString = await BindChartData(companyId);
            var agencyRepresentative = BindAgencyRepresentative();

            var dashboardResponse = new AgencyDashboardResponse
            {
                LedgerStatement = ledgerResponse,
                Notification = notificationResponse,
                DashboardChartXML = chartXmlString,
                DashboardCorporate = corporateResponse,
                AvailableBalance = availableBalance,
                Agency = new List<AgencyRepresentative> { agencyRepresentative }
            };

            return View("~/Views/Agency/Dashboard.cshtml", dashboardResponse);
        }

        private async Task<List<DashboardLedgerResponse>> BindLedgerData(string companyId)
        {
            var query = new DashboardLedgerQuery
            {
                CompanyId = companyId,
                FromDate = DateTime.Today.AddMonths(-1),
                ToDate = DateTime.Today,
                SearchBy = "",
                TicketSearchType = "Last",
                SearchByValue = ""
            };
            var ledgerData = await _ledgerQueryHandler.HandleAsync(query);
            return _mapper.Map<List<DashboardLedgerResponse>>(ledgerData);
        }

        private async Task<List<DashboardNotificationResponse>> BindNotificationData(string companyId)
        {
            var query = new DashboardNotificationQuery { CompanyId = companyId };
            var notificationData = await _notificationQueryHandler.HandleAsync(query);
            return _mapper.Map<List<DashboardNotificationResponse>>(notificationData);
        }

        private async Task<decimal> BindAvailableBalance(string companyId)
        {
            var query = new GetAvailableBalanceQuery(companyId);
            return await _getAvailableBalanceQueryHandler.HandleAsync(query);
        }

        private async Task<List<DashboardCorporateResponse>> BindCorporateData(string companyId)
        {
            var query = new DashboardCorporateQuery { CompanyId = companyId };
            var corporateData = await _dashboardCorporateQueryHandler.HandleAsync(query);
            return _mapper.Map<List<DashboardCorporateResponse>>(corporateData);
        }

        private async Task<string> BindChartData(string companyId)
        {
            var query = new DashboardChartQuery { CompanyId = companyId };
            var chartData = await _dashboardChartQueryHandler.HandleAsync(query);
            var chartResponse = _mapper.Map<List<DashboardChartResponse>>(chartData);

            var groupedchartResponse = chartResponse
               .GroupBy(row => row.CarrierCode)
               .Select(g => new DashboardChartResponse
               {
                   CarrierCode = g.Key,
                   CarrierName = g.FirstOrDefault()?.CarrierName,
                   TotalBasic = g.Sum(x => x.TotalBasic),
                   TotalYQ = g.Sum(x => x.TotalYQ),
                   TotalFare = g.Sum(x => x.TotalFare),
                   TotalCommission = g.Sum(x => x.TotalCommission),
                   NoOfPassenger = g.Sum(x => x.NoOfPassenger),
                   NoOfBookings = g.Count()
               })
               .ToList();

            using var memStream = new MemoryStream();
            var serializer = new XmlSerializer(groupedchartResponse.GetType());
            serializer.Serialize(memStream, groupedchartResponse);
            memStream.Position = 0;

            return new StreamReader(memStream).ReadToEnd();
        }

        private AgencyRepresentative BindAgencyRepresentative()
        {
            return new AgencyRepresentative
            {
                CompanyFullName = "Support Team",
                CompanySupportEmail = _configuration["Company:Email"],
                CompanyMobile = _configuration["Company:Mobile"],
                CompanyPhone = _configuration["Company:Phone"],
                CompanyPackage = _configuration["Company:companyPackage"],
                CompanyVisa = _configuration["Company:companyVisa"],
                companyOther = _configuration["Company:companyOther"]
            };
        }
    }
}
