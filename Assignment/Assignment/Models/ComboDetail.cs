using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class ComboDetail
    {
        [Required(ErrorMessage = "Bạn cần chọn mã combo.")]
        [Display(Name = "Mã combo")]
        public int ComboId { get; set; }
        public virtual Combo? Combo { get; set; }  // Khóa ngoại tham chiếu đến bảng Combo


        [Required(ErrorMessage = "Bạn cần chọn mã sản phẩm.")]
        [Display(Name = "Mã sản phẩm")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }  // Khóa ngoại tham chiếu đến bảng Product

        [Display(Name = "Số lượng")]
        [Required(ErrorMessage = "Bạn cần nhập số lượng.")]
        [Range(0, 20, ErrorMessage = "Bạn cần nhập số lượng hợp lệ.")]
        public int Quantity { get; set; } //Quản lý số lượng sản phẩm trong combo
    }
}
