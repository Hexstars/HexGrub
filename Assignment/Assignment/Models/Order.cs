using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Chờ xử lý")]
        Pending = 0, // Default state

        [Display(Name = "Đang giao hàng")]
        InProgress = 1,

        [Display(Name = "Đã giao hàng")]
        Completed = 2,

        [Display(Name = "Đã hủy")]
        Cancelled = 3
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Display(Name = "Mã người dùng")]
        [Required(ErrorMessage = "Bạn cần chọn mã người dùng.")]
        
        public int AccountId { get; set; }// Foreign Key đến Account
        public Account Account { get; set; }

        [Display(Name = "Ngày đặt")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [Required(ErrorMessage = "Bạn cần chọn ngày.")]
        public DateTime OrderDate { get; set; }


        [Display(Name = "Trạng thái")]
        public OrderStatus Status { get; set; }  // Trạng thái đơn hàng (enum)

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Danh sách các OrderDetail
    }
}
