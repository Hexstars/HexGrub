using Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Services
{
    public interface IOrderSvc
    {
        //List<OrderView> GetAllOrders();
        Task<bool> CreateOrder(Order order);
        bool UpdateOrder(int orderId, int userId, int status);
        Task<List<CartProduct>> GetAllProduct(int id);
        Task<bool> AddIntoDetail(List<CartProduct> products, int userId);
        List<OrderView> GetAllOrders(int userId);

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
        public async Task<List<Order>> GetUserOrders(int userId)
        {
            return await _context.Orders
                         .Where(o => o.AccountId == userId) //
                         .ToListAsync(); // Trả query
        }
        public bool UpdateOrder(int orderId, int userId, int status)
        {
            try
            {
                // Tìm đơn hàng cần cập nhật
                var order = _context.Orders
                    .FirstOrDefault(o => o.OrderId == orderId && o.AccountId == userId);

                // Nếu không tìm thấy đơn hàng
                if (order == null)
                {
                    return false;
                }

                // Cập nhật trạng thái đơn hàng
                order.Status = (OrderStatus)status;

                // Cập nhật vào cơ sở dữ liệu
                _context.Orders.Update(order); // Gọi Update để thông báo EF thay đổi
                _context.SaveChanges(); // Lưu thay đổi

                return true; // Thành công
            }
            catch
            {
                return false;
            }
        }
        public List<OrderView> GetAllOrders(int userId)
        {
            // Lấy danh sách tất cả các đơn hàng của user
            var orders = (from o in _context.Orders
                          where o.AccountId == userId
                          select new OrderView
                          {
                              OrderId = o.OrderId,
                              OrderDate = o.OrderDate,
                              Phone = o.Phone,
                              Address = o.Address,
                              Status = o.Status,
                              Products = (from od in _context.OrderDetails //Query tất cả sản phẩm ở chi tiết đơn hàng và gán vào list OrderProductView
                                          join p in _context.Products on od.ProductId equals p.ProductId
                                          where od.OrderId == o.OrderId
                                          select new OrderProductView
                                          {
                                              ProductName = p.ProductName,
                                              Image = p.Image,
                                              UnitPrice = od.UnitPrice,
                                              Quantity = od.Quantity
                                          }).ToList()
                          }).ToList();

            return orders;
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
