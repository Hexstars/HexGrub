using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    public class Combo
    {
        [Key]
        public int ComboId { get; set; }

        [Display(Name = "Tên combo")]
        [StringLength(30)]
        [Required(ErrorMessage = "Bạn cần nhập tên combo.")]
        [Column(TypeName = "nvarchar(30)")]
        public string ComboName { get; set; }


        [StringLength(250)]
        [Display(Name = "Hình")]
        public string? Image { get; set; }

        // Tạo mối quan hệ nhiều-nhiều
        public ICollection<ComboDetail> ComboDetails { get; set; } = new List<ComboDetail>();
    }
}
