using Assignment.Models;
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
            return View();
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
                Cart cart = _cartSvc.GetCart(Convert.ToInt32(@User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)));
                bool added = _cartDetailSvc.AddToCart(cart, product, Quantity);

                if (added)
                {
                    TempData["Info"] = "ĐÃ THÊM SẢN PHẨM VÀO GIỎ HÀNG";
                }
                else
                {
                    TempData["Info"] = "THÊM VÀO GIỎ HÀNG THẤT BẠI";
                }
                return RedirectToAction("ProductDetail", new { id = product.ProductId });//Giống bên Index
            }
        }
    }
}
