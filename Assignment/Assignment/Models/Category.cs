using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Assignment.Models
{
	public class Category
	{
		[Key]
		public int CategoryId { get; set; }


        [Display(Name = "Tên danh mục")]
        [StringLength(30)]
        [Required(ErrorMessage = "Bạn cần nhập tên danh mục.")]
        [Column(TypeName = "nvarchar(30)")]
        public string CategoryName { get; set; }


        [StringLength(250)]
        [Display(Name = "Hình")]
        public string? Image { get; set; }


        [NotMapped]
        [Display(Name = "Chọn hình")]
        public IFormFile ImageFile { get; set; }


        //Navigation property to link back to Products (one-to-many relationship)
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
