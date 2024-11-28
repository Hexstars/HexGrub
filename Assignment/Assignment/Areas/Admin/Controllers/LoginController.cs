using Assignment.Models;
using Assignment.Services;
using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Assignment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        public const string CookieEmail = "EmailLogin";
        public IAccountSvc _accountSvc;
        public LoginController(DataContext context, IAccountSvc accountSvc)
        {
            _context = context;
            _accountSvc = accountSvc;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, int Remember)
        {
            //Check the user name and password
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            var user = _accountSvc.Login(email, password);

            if (user != null)
            {
                HttpContext.Session.SetString("email", user.Email);
                HttpContext.Session.SetString("userId", user.AccountId.ToString());
                HttpContext.Session.SetString("role", user.Role.RoleName);

                // Create the identity for the user
                identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }
            else
            {
                ViewBag.Info = "Đăng nhập thất bại!";
                return View();
            }

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Remember == 1, // If Remember is 1, make the login persistent
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)  // Optional: Set the cookie expiration
                };

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                if (!User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    return RedirectToAction("AccessDenied");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            //Xóa cookie
            if (Request.Cookies.ContainsKey(CookieEmail))
            {
                Response.Cookies.Delete(CookieEmail);
            }
            //Xóa session
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("role");

            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
