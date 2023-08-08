using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignUp()
        {
            return View();
        }
        
    }
}
