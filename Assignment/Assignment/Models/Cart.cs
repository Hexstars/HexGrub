﻿using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required(ErrorMessage = "Bạn cần chọn mã người dùng.")]
        [Display(Name = "Mã người dùng")]
        public int AccountId { get; set; }  // Foreign Key
        public Account Account { get; set; } //Dùng để đi vào bảng Account để truy vấn các trường khác

        public ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>(); // Danh sách các CartDetail
    }
}
