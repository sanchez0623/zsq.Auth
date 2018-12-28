using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zsq.MvcCookieAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using zsq.MvcCookieAuth.ViewModels;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Test;

namespace zsq.MvcCookieAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;

        public AccountController(TestUserStore users)
        {
            _users = users;
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            // if (ModelState.IsValid)
            // {
            //     ViewData["ReturnUrl"] = returnUrl;
            //     var identityUser = new ApplicationUser
            //     {
            //         Email = registerViewModel.Email,
            //         UserName = registerViewModel.Email,
            //         NormalizedUserName = registerViewModel.Email,
            //     };

            //     var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
            //     if (identityResult.Succeeded)
            //     {
            //         await _signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
            //         return RedirectToLoacl(returnUrl);
            //     }
            //     else
            //     {
            //         AddErrors(identityResult);
            //     }
            // }

            return View();
        }

        private IActionResult RedirectToLoacl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var user = _users.FindByUsername(loginViewModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.UserName), "Email not exists");
                }
                else
                {
                    if (_users.ValidateCredentials(loginViewModel.UserName, loginViewModel.Password))
                    {
                        AuthenticationProperties props = null;
                        if (loginViewModel.RememberMe)
                        {
                            props = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                            };
                        }

                        await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(
                            HttpContext, user.SubjectId, user.Username, props);
                        return RedirectToLoacl(returnUrl);
                    }

                    ModelState.AddModelError(nameof(loginViewModel.Password), "Wrong Password");
                }
            }

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}