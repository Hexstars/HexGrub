using Assignment.Models;

namespace Assignment.Services
{
    public interface ICategorySvc
    {
        List<Category> GetAllCategory();

        Category GetCategory(int id);

        int AddCategory(Category category);

        int EditCategory(int id, Category category);
    }
    public class CategorySvc : ICategorySvc
    {
        protected DataContext _context;

        public CategorySvc(DataContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategory()
        {
            List<Category> list = new List<Category>();
            list = _context.Categories.ToList();
            return list;
        }

        public Category GetCategory(int id)
        {
            Category category = null;
            category = _context.Categories.Find(id); //Chỉ tìm bằng khóa chính

            //product = _context.Products.Where(p => p.id == id).FirstOrDefault();
            return category;
        }
        public int AddCategory(Category product)
        {
            int ret = 0;
            try
            {
                _context.Add(product);
                _context.SaveChanges();
                ret = product.CategoryId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public int EditCategory(int id, Category category)
        {
            int ret = 0;
            try
            {
                Category _category = null;
                _category = _context.Categories.Find(id); //cách này chỉ dùng cho Khóa chính

                _category.CategoryName = category.CategoryName;
                _category.Image = category.Image;

                _context.Update(_category);
                _context.SaveChanges();
                ret = category.CategoryId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

    }
}
