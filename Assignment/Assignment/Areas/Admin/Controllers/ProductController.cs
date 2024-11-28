using Assignment.Helpers;
using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IProductSvc _productSvc;
        private ICategorySvc _categorySvc;
        private IUploadHelper _uploadHelper;

        public ProductController(IWebHostEnvironment webHostEnvironment, IProductSvc productSvc, ICategorySvc categorySvc, IUploadHelper uploadHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _productSvc = productSvc;
            _categorySvc = categorySvc;
            _uploadHelper = uploadHelper;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            return View(_productSvc.GetAllProduct());
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var product = _productSvc.GetProduct(id);
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            // Lấy danh sách category từ cơ sở dữ liệu
            var categories = _categorySvc.GetAllCategory();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (product.ImageFile != null)
                {
                    if (product.ImageFile.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(product.ImageFile, rootPath, "Products");
                        product.Image = product.ImageFile.FileName;
                    }
                }
                _productSvc.AddProduct(product);
                return RedirectToAction(nameof(Details), new { id = product.ProductId });
            }
            catch
            {
                return View();
            }

        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _productSvc.GetProduct(id);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (product.ImageFile != null)
                    {
                        if (product.ImageFile.Length > 0)
                        {
                            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                            //_uploadHelper.RemoveImage(rootPath + @"\monan\" + monAn.Hinh);
                            _uploadHelper.UploadImage(product.ImageFile, rootPath, "Products");
                            product.Image = product.ImageFile.FileName;
                        }
                    }
                    _productSvc.EditProduct(id, product);
                    return RedirectToAction(nameof(Details), new { id = product.ProductId });
                }
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));

        }
    }
}
