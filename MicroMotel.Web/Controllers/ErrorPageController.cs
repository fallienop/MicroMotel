using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult unauthorized()
        {
            return View();
        }
    }
}
