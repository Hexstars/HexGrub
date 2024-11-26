using Assignment.Helpers;
using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private IProductSvc _productSvc;
        private ICategorySvc _categorySvc;

        public HomeController(IProductSvc productSvc, ICategorySvc categorySvc)
        {
            _productSvc = productSvc;
            _categorySvc = categorySvc;
        }

        public async Task<IActionResult> Index()
        {
            var categories = _categorySvc.GetAllCategory();
            var products = _productSvc.GetAllProduct();

            ViewData["Categories"] = categories;
            ViewData["Products"] = products;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
