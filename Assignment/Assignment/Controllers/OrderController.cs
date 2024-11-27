using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Controllers
{
    public class OrderController : Controller
    {
        public IOrderSvc _orderSvc;
        public IAccountSvc _accountSvc;
        public ICartSvc _cartSvc;

        public OrderController(DataContext context, IOrderSvc orderSvc, IAccountSvc accountSvc, ICartSvc cartSvc)
        {
            _orderSvc = orderSvc;
            _accountSvc = accountSvc;
            _cartSvc = cartSvc;
        }
        public ActionResult Index()
        {
            var orders = _orderSvc.GetAllOrders(Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value));
            return View(orders);
        }
        public async Task<ActionResult> CreateOrder()
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
                int userId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);


                var user = _accountSvc.GetAccount(userId);
                Order order = new Order
                {
                    AccountId = user.AccountId,
                    OrderDate = DateTime.Now,
                    Phone = user.Phone,
                    Address = user.Address,
                    Status = 0,
                };
                
                //add order
                bool result = await _orderSvc.CreateOrder(order);
                if (result)
                {
                    Cart cart = _cartSvc.GetUserCart(userId);

                    var products = await _orderSvc.GetAllProduct(cart.CartId);

                    if (products.Any())
                    {
                        // Add products to the order details
                        await _orderSvc.AddIntoDetail(products, order.OrderId);

                        // Clear the cart after adding items to the order
                        await _cartSvc.RemoveAll(cart.CartId);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Cart");

                }
            }
            return RedirectToAction("Index", "Cart");
        }
    }
}
