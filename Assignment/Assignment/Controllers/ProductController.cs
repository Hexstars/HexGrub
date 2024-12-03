using Assignment.Helpers;
using Assignment.Models;
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
        public IActionResult Search(string Name)
        {
            var products = _productSvc.Search(Name);
            //if (products.Count == 0)
            //{
            //	ViewBag.Info = "Không tìm thấy sản phẩm nào";
            //}

            return View(products);
        }
    }
}
