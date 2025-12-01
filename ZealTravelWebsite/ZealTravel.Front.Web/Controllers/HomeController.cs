using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZealTravel.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route ("/flight")]
        public IActionResult Index()
        {
            return View("~/Views/Flight/Flight.cshtml");
        }

    }
}
