using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    [NotMapped]
    public class AccountEditModel
    {
        public int AccountId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@[a-zA-Z0-9-]+(\\.[a-zA-Z]{2,})+$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }


        [Display(Name = "Mật khẩu")]
        [StringLength(50)]
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        [Required(ErrorMessage = "Bạn cần nhập họ và tên.")]
        public string FullName { get; set; }


        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
        public string Phone { get; set; }


        [Display(Name = "Địa chỉ")]
        [StringLength(100)]
        [Required(ErrorMessage = "Bạn cần nhập họ địa chỉ.")]
        public string Address { get; set; }


        [Display(Name = "Vai trò")]
        //[ForeignKey("RoleId")] dùng nếu khác tên với trường của bảng Role
        public int RoleId { get; set; }  // Foreign Key
        public Role? Role { get; set; } //Dùng để đi vào bảng Role để truy vấn các trường khác
    }
}
