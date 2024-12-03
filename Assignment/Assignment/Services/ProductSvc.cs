using Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Services
{
    public interface IProductSvc
    {
        List<Product> GetProducts();
        List<Product> Search(string Name);
        (List<Product> Products, int TotalCount) GetAllProduct(int currentPage, int pageSize);

        Product GetProduct(int id);

        int AddProduct(Product product);

        int EditProduct(int id, Product product);
    }
    public class ProductSvc : IProductSvc
    {
        protected DataContext _context;

        public ProductSvc(DataContext context)
        {
            _context = context;
        }
        public List<Product> GetProducts()
        {
            var list = _context.Products.Include(p=>p.Category).ToList();
            return list;
        }
        public List<Product> Search(string Name)
        {
            var products = _context.Products
        .Where(p => p.ProductName.ToLower().Contains(Name.ToLower())) // Convert both to lowercase
        .ToList();

            return products;
        }
        public (List<Product> Products, int TotalCount) GetAllProduct(int currentPage, int pageSize)
        {
            int pageIndex = currentPage - 1;

            // Đếm tổng số sản phẩm để tính số trang
            int totalCount = _context.Products.Count();

            // Lấy dữ liệu cho trang hiện tại
            List<Product> list = _context.Products
                                        .Include(p => p.Category)
                                        .Skip(pageIndex * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            return (list, totalCount);
        }

        public Product GetProduct(int id) 
        {
            Product product = null;
            product = _context.Products.Find(id); //Chỉ tìm bằng khóa chính

            //product = _context.Products.Where(p => p.id == id).FirstOrDefault();
            return product;
        }
        public int AddProduct(Product product)
        {
            int ret = 0;
            try
            {
                _context.Add(product);
                _context.SaveChanges();
                ret = product.ProductId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public int EditProduct(int id, Product product)
        {
            int ret = 0;
            try
            {
                Product _product = null;
                _product = _context.Products.Find(id); //cách này chỉ dùng cho Khóa chính

                _product.ProductName = product.ProductName;
                _product.UnitPrice = product.UnitPrice;
                _product.Quantity = product.Quantity;
                _product.Image = product.Image;
                _product.CategoryId = product.CategoryId;

                _context.Update(_product);
                _context.SaveChanges();
                ret = product.ProductId;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

    }
}
