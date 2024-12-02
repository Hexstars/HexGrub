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

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            int pageSize = 5; // Số sản phẩm trên mỗi trang

            var (products, totalCount) = _productSvc.GetAllProduct(currentPage, pageSize);

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Truyền dữ liệu qua ViewData
            ViewData["CurrentPage"] = currentPage;
            ViewData["TotalPages"] = totalPages;

            var categories = _categorySvc.GetAllCategory();

            ViewData["Products"] = products;

            return View(categories);
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
