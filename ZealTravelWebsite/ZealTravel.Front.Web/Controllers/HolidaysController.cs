using Microsoft.AspNetCore.Mvc;

namespace ZealTravel.Front.Web.Controllers
{
    public class HolidaysController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
