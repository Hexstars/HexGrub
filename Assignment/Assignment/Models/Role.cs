using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }

		[Display(Name = "Tên vai trò")]
		[StringLength(30)]
		[Required(ErrorMessage = "Bạn cần nhập tên vai trò.")]
        [Column(TypeName = "nvarchar(30)")]
        public string RoleName { get; set; }

		//Navigation property to link back to Accounts (one-to-many relationship)
		//Khi lấy Role thì lấy hết Accounts
		public ICollection<Account> Accounts { get; set; } = new List<Account>();
	}
}
