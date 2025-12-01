using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Controllers
{
    public class AgencyBaseController : Controller
    {
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        public AgencyBaseController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler)
        {
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var companyId = User.FindFirst("CompanyId")?.Value;
            if (companyId != null)
            {
                var balanceQuery = new GetAvailableBalanceQuery(companyId);
                var availableBalance = _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);
                ViewBag.AvailableBalance = availableBalance.Result;
            }
        }
    }
}
