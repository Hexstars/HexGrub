using Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Services
{
    public interface IOrderSvc
    {
        //List<OrderView> GetAllOrders();
        Task<bool> CreateOrder(Order order);
        Task<List<CartProduct>> GetAllProduct(int id);
        Task<bool> AddIntoDetail(List<CartProduct> products, int userId);

    }
    public class OrderSvc : IOrderSvc
    {
        protected DataContext _context;

        public OrderSvc(DataContext context)
        {
            _context = context;
        }
        //public async Task<List<OrderView> GetAllOrder()
        //{

        //} 
        public async Task<bool> CreateOrder(Order order)
        {
            try
            {
                _context.Add(order);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CartProduct>> GetAllProduct(int id)
        {
            // Lấy danh sách ProductId từ CartDetails và Products
            List<CartProduct> products = await (from cd in _context.CartDetails
                             join p in _context.Products on cd.ProductId equals p.ProductId
                             where cd.CartId == id
                             select new CartProduct
                             {
                                 ProductId = p.ProductId,
                                 Quantity = cd.Quantity,
                                 UnitPrice = p.UnitPrice,
                             }).ToListAsync(); // Chỉ lấy ProductId trực tiếp
            return products;
        }
        public async Task<bool> AddIntoDetail(List<CartProduct> products, int orderId)
        {
            try
            {
                foreach (var item in products)
                {
                    OrderDetail detail = new OrderDetail
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    };

                    _context.OrderDetails.Add(detail);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // Return false if an error occurs
            }
        }
    }
}
