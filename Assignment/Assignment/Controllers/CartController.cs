﻿using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class CartController : Controller
    {
        private ICartSvc _cartSvc;
        public ICartDetailSvc _cartDetailSvc;
        public CartController(IProductSvc productSvc, ICartSvc cartSvc, ICartDetailSvc cartDetailSvc)
        {
            _cartSvc = cartSvc;
            _cartDetailSvc = cartDetailSvc;
        }
        public ActionResult Index()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Save the current URL to redirect back after login
                string returnUrl = Request.GetEncodedUrl(); // Correct way to get the full URL in ASP.NET Core

                // Redirect to the login page, passing the return URL
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            return View(_cartSvc.GetAllProduct(Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(Product product, int Quantity)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Save the current URL to redirect back after login
                string returnUrl = Request.GetEncodedUrl(); // Correct way to get the full URL in ASP.NET Core

                // Redirect to the login page, passing the return URL
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            else
            {
                Cart cart = _cartSvc.GetUserCart(Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value));
                bool added = _cartDetailSvc.AddToCart(cart, product, Quantity);

                if (added)
                {
                    TempData["Info"] = "ĐÃ THÊM SẢN PHẨM VÀO GIỎ HÀNG";
                }
                else
                {
                    TempData["Info"] = "THÊM VÀO GIỎ HÀNG THẤT BẠI";
                }
                return RedirectToAction("ProductDetail", "Product", new { id = product.ProductId });//Giống bên Index
            }
        }
        public ActionResult UpdateQuantity(int productId, int newQuantity)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Save the current URL to redirect back after login
                string returnUrl = Request.GetEncodedUrl(); // Correct way to get the full URL in ASP.NET Core

                // Redirect to the login page, passing the return URL
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            else
            {
                var userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                _cartSvc.UpdateQuantity(userId, productId, newQuantity);

                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteFromCart(int productId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Save the current URL to redirect back after login
                string returnUrl = Request.GetEncodedUrl(); // Correct way to get the full URL in ASP.NET Core

                // Redirect to the login page, passing the return URL
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            else
            {
                var userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                _cartSvc.DeleteFromCart(userId, productId);

                return RedirectToAction("Index");
            }
        }
    }
}
