using Microsoft.AspNetCore.Mvc;

namespace ArmyBuilder.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
