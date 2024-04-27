using Microsoft.AspNetCore.Mvc;

namespace Demo.PLL.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
