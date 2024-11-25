using Assignment.Models;

namespace Assignment.Services
{
    public interface ICartSvc
    {
        List<Cart> GetAllCart();

        Cart GetCart(int id);

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
    }
}
