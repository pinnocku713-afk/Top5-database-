using Microsoft.AspNetCore.Mvc;

namespace Top5.Controllers
{
    public class VersusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Winner()
        {
            return View();
        }
    }
}
