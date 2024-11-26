using Assignment.Models;

namespace Assignment.Services
{
    public interface ICartSvc
    {
        List<Cart> GetAllCart();

        Cart GetCart(int id); //CRUD

        Cart GetUserCart(int id); //Thêm/Xóa sản phẩm trong giỏ

        List<CartProduct> GetAllProduct(int id); //Hiển thị giỏ hàng

        Task<bool> RemoveAll(int id);

    }
    public class CartSvc : ICartSvc
    {
        protected DataContext _context;

        public CartSvc(DataContext context)
        {
            _context = context;
        }

        public List<Cart> GetAllCart()
        {
            List<Cart> list = new List<Cart>();
            list = _context.Carts.ToList();
            return list;
        }
        public Cart GetCart(int id)
        {
            Cart cart = new Cart();
            cart = _context.Carts.Find(id);
            return cart;
        }
        public Cart GetUserCart(int id)
        {
            Cart cart = new Cart();
            cart = _context.Carts.FirstOrDefault(c => c.AccountId == id);
            return cart;
        }

        public List<CartProduct> GetAllProduct(int id) 
        {
            Cart cart = GetUserCart(id);

            if (cart == null)
            {
                return new List<CartProduct>(); // Trả về rỗng nếu thiếu
            }

            //Query và tạo list chứa sản phẩm
            List<CartProduct> cartProducts = (from cd in _context.CartDetails
                                join p in _context.Products on cd.ProductId equals p.ProductId
                                where cd.CartId == cart.CartId
                                select new CartProduct
                                {
                                    ProductId = p.ProductId,
                                    Image = p.Image,
                                    ProductName = p.ProductName,
                                    UnitPrice = p.UnitPrice,
                                    Quantity = cd.Quantity
                                }).ToList();
            return cartProducts;
        }

        public async Task<bool> RemoveAll(int CartId)
        {
            try
            {
                // Find all items in the cart
                var cartDetails = _context.CartDetails.Where(cd => cd.CartId == CartId).ToList();

                // Remove all items
                _context.CartDetails.RemoveRange(cartDetails);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
