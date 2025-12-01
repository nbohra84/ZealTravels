using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Backoffice.Web.Controllers
{
    public class TextPagesController : Controller
    {
        public TextPagesController()
        {

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
