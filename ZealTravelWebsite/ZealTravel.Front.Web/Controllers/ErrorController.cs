using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Controllers
{
    public class ErrorController : AgencyBaseController
    {
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        public ErrorController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler) : base(getAvailableBalanceQueryHandler)
        {
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
        }
        [Route("/errorpage")]
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
