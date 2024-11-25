using Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Services
{
    public interface ICartDetailSvc
    {
        bool AddToCart(Cart cart, Product product, int quantity);
    }
    public class CartDetailSvc : ICartDetailSvc
    {
        protected DataContext _context;

        public CartDetailSvc(DataContext context)
        {
            _context = context;
        }
        public bool AddToCart(Cart cart, Product product, int quantity)
        {
            // Kiểm tra nếu giỏ hàng đã có sản phẩm này chưa
            var existingCartDetail = _context.CartDetails
                .FirstOrDefault(cd => cd.CartId == cart.CartId && cd.ProductId == product.ProductId);

            if (existingCartDetail != null)
            {
                // Nếu có rồi, tăng số lượng của sản phẩm trong giỏ hàng
                existingCartDetail.Quantity += 1;
                _context.CartDetails.Update(existingCartDetail);
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới vào giỏ hàng
                var cartDetail = new CartDetail
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = quantity,
                };
                _context.CartDetails.Add(cartDetail);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
            return true;
        }
    }
}
