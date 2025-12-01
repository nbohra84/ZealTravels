using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Controllers
{
    public class TextPagesController : AgencyBaseController
    {
        public TextPagesController(IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler) : base(getAvailableBalanceQueryHandler)
        {

        }
            [Route("/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("/bank-details")]
        public IActionResult BankDetails()
        {
            return View();
        }

        [Route("/privacy-policy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/terms-conditions")]
        public IActionResult TermsConditions()
        {
            return View();
        }

        [Route("/disclaimer")]
        public IActionResult Disclaimer()
        {
            return View();
        }
    }
}
