using Microsoft.AspNetCore.Mvc;

namespace Demo.PLL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
