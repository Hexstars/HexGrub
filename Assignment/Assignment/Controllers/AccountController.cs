﻿using Assignment.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using Assignment.Services;

namespace Assignment.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        public const string CookieEmail = "EmailLogin";
        public IAccountSvc _accountSvc;
        public AccountController(DataContext context, IAccountSvc accountSvc)
        {
            _context = context;
            _accountSvc = accountSvc;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            string session = HttpContext.Session.GetString("email");
            if (session != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account user)
        {
            user.RoleId = 2;
            if (ModelState.IsValid)
            {
                // Using LINQ with Entity Framework to check if the email exists
                bool emailExists = _context.Accounts.Any(u => u.Email == user.Email);

                if (emailExists)
                {
                    ViewBag.Info = "Email đã tồn tại";
                    return View();
                }
                else
                {
                    // Add account
                    _accountSvc.AddAccount(user);

                    // tạo cart
                    var cart = new Cart
                    {
                        AccountId = user.AccountId  // set Id
                    };

                    // Add cart
                    _context.Carts.Add(cart);
                    _context.SaveChanges();  //lưu
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return View();
            }
        }
        public IActionResult Login()
        {

            // Kiểm tra cookie tồn tại hay không?
            string? emailLogined = Request.Cookies.ContainsKey(CookieEmail) ? Request.Cookies[CookieEmail] : null;
            string session = HttpContext.Session.GetString("email");
            //Nếu session hết hạn thì xóa cookie?



            // Kiểm tra nếu session đã hết hạn
            if (emailLogined != null/*session != null*/)
            {
                var user = _context.Accounts
                    .Include(u => u.Role)  // Ensure Role is loaded along with the User
                    .FirstOrDefault(u => u.Email == emailLogined);

                if (user != null)
                {
                    // Thiết lập session mới
                    HttpContext.Session.SetString("email", user.Email);

                    // Đăng nhập người dùng bằng CookieAuthentication
                    var identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.RoleName)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(); // Trả về trang đăng nhập nếu không có session và cookie
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, int Remember)
        {

            //Check the user name and password
            //Here can be implemented checking logic from the database
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            var user = _accountSvc.Login(email, password);
            if (user != null)
            {

                HttpContext.Session.SetString("email", user.Email);
                HttpContext.Session.SetString("userId", user.AccountId.ToString());
                HttpContext.Session.SetString("role", user.Role.RoleName);

                //Create the identity for the user
                identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, user.FullName),
                            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                            new Claim(ClaimTypes.Role, user.Role.RoleName)
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;

                if (Remember == 1)
                {
                    CookieOptions options = new CookieOptions()
                    {
                        Domain = "localhost",
                        Path = "/",
                        Expires = DateTime.Now.AddDays(1),
                        Secure = false,
                        HttpOnly = true,
                        IsEssential = true,
                    };

                    Response.Cookies.Append(CookieEmail, user.Email, options);
                }

                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Info = "Đăng nhập thất bại!";
                return View();
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

        public IActionResult Info()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Save the current URL to redirect back after login
                string returnUrl = Request.GetEncodedUrl(); // Correct way to get the full URL in ASP.NET Core

                // Redirect to the login page, passing the return URL
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            var account = _accountSvc.GetAccount(Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value));
            return View(account);
        }
    }
}
