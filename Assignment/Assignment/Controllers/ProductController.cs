using Assignment.Helpers;
using Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class ProductController : Controller
    {
        private IProductSvc _productSvc;

        public ProductController(IProductSvc productSvc)
        {
            _productSvc = productSvc;
        }
        public IActionResult ProductDetail(int id)
        {
            return View(_productSvc.GetProduct(id));
        }
    }
}
