using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
    [NotMapped]
    public class ComboViewModel
    {
        public int ComboId { get; set; }

        [Display(Name = "Tên combo")]
        public string ComboName { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        public List<string> ProductName { get; set; }
    }
}
