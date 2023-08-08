using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
