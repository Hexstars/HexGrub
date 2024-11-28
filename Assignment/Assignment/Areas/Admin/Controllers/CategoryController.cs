using Assignment.Helpers;
using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ICategorySvc _categorySvc;
        private IUploadHelper _uploadHelper;

        public CategoryController(IWebHostEnvironment webHostEnvironment, ICategorySvc categorySvc, IUploadHelper uploadHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _categorySvc = categorySvc;
            _uploadHelper = uploadHelper;
        }

        // GET: CategoryController
        public ActionResult Index()
        {
            return View(_categorySvc.GetAllCategory());
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var product = _categorySvc.GetCategory(id);
            return View(product);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                if (category.ImageFile != null)
                {
                    if (category.ImageFile.Length > 0)
                    {
                        string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        _uploadHelper.UploadImage(category.ImageFile, rootPath, "Categories");
                        category.Image = category.ImageFile.FileName;
                    }
                }
                _categorySvc.AddCategory(category);
                return RedirectToAction(nameof(Details), new { id = category.CategoryId });
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _categorySvc.GetCategory(id);
            return View(product);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (category.ImageFile != null)
                    {
                        if (category.ImageFile.Length > 0)
                        {
                            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                            //_uploadHelper.RemoveImage(rootPath + @"\monan\" + monAn.Hinh);
                            _uploadHelper.UploadImage(category.ImageFile, rootPath, "Categories");
                            category.Image = category.ImageFile.FileName;
                        }
                    }
                    _categorySvc.EditCategory(id, category);
                    return RedirectToAction(nameof(Details), new { id = category.CategoryId });
                }
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
