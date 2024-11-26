﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
	public class Account
	{
		[Key]
		public int AccountId {  get; set; }

		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@[a-zA-Z0-9-]+(\\.[a-zA-Z]{2,})+$", ErrorMessage = "Email không hợp lệ")]
		//[Remote("")]
		public string Email { get; set; }

		[Display(Name = "Mật khẩu")]
		[StringLength(50)]
		[Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
		[Column(TypeName = "varchar(50)")]
		[DataType(DataType.Password)]
        public string Password { get; set; }

		
		[NotMapped]
		[Display(Name = "Nhập lại mật khẩu")]
		[StringLength(50)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Họ và tên")]
		[StringLength(100)]
		[Required(ErrorMessage = "Bạn cần nhập họ và tên.")]
		[Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

		[Display(Name = "Số điện thoại")]
		[StringLength(15)]
		[Column(TypeName = "varchar(15)")]
		[Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
		public string Phone { get; set; }


		[Display(Name = "Địa chỉ")]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Bạn cần nhập họ địa chỉ.")]
        public string Address { get; set; }


		[Display(Name = "Vai trò")]
        //[ForeignKey("RoleId")] dùng nếu khác tên với trường của bảng Role
        public int RoleId { get; set; }  // Foreign Key
		public Role? Role { get; set; } //Dùng để đi vào bảng Role để truy vấn các trường khác

        // Thêm thuộc tính Orders để biểu diễn mối quan hệ với đơn hàng
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // Mối quan hệ một-nhiều
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    }
}
