using MicroMotel.Shared.Services;
using MicroMotel.Web.Models;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IROPService _ropservice;
        private readonly ISignUpService _signupservice;

        public AuthController(IROPService ropservice, ISignUpService signupservice)
        {
            _ropservice = ropservice;
            _signupservice = signupservice;
        }

        public IActionResult Signin()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signin(SigninInput sii)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _ropservice.SignIn(sii);
            if (!response.IsSuccesful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(string.Empty, x);
                });
                return View();
            }
            return RedirectToAction("Index", "Home");

        }


        public IActionResult SignUp()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpViewModel usuvm)
        {
            var resp = await _signupservice.SignUpAsync(usuvm);
            if (!resp)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _ropservice.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
