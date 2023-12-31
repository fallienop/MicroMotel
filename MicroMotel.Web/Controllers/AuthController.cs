﻿using MicroMotel.Shared.Services;
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
        private readonly IUserService _userService;

        public AuthController(IROPService ropservice, ISignUpService signupservice, IUserService userService)
        {
            _ropservice = ropservice;
            _signupservice = signupservice;
           _userService = userService;
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
            if (resp.Data is null)
            {
                foreach(var error in resp.Errors)
                {
                    ModelState.AddModelError("", error);
                }
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

        public async Task<IActionResult> getusersets()
        {
            var resp = await _userService.GetUser();
            return View(resp);
        }

        
        public async Task<IActionResult> UpdateUser()
        {
            var resp = await _userService.GetUser();
            return View(resp);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserUpdateModel uum)
        {

            var resp = await _userService.UpdateUser(uum);
            return RedirectToAction(nameof(getusersets));

        }
    }
}
