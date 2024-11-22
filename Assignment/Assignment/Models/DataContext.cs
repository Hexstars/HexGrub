using Microsoft.EntityFrameworkCore;
using Assignment.Models;

namespace Assignment.Models
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.Property(p => p.UnitPrice)
				.HasColumnType("decimal(18,2)"); // 18 total digits, 2 decimal places

            modelBuilder.Entity<OrderDetail>()
                .Property(o => o.UnitPrice)
                .HasColumnType("decimal(18,2)");


            // Thiết lập khóa tổng hợp cho ComboDetail
            modelBuilder.Entity<ComboDetail>()
                .HasKey(cd => new { cd.ComboId, cd.ProductId }); // Đặt ComboId và ProductId là khóa chính tổng hợp
                    

            modelBuilder.Entity<ComboDetail>()
                .HasOne(cd => cd.Combo)
                .WithMany(c => c.ComboDetails)
                .HasForeignKey(cd => cd.ComboId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete để xóa chi tiết khi xóa combo

            // Thiết lập mối quan hệ giữa ComboDetail và Product
            modelBuilder.Entity<ComboDetail>()
                .HasOne(cd => cd.Product)
                .WithMany(p => p.ComboDetails)
                .HasForeignKey(cd => cd.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete để xóa chi tiết khi xóa sản phẩm


            // Thiết lập mối quan hệ giữa ComboDetail và Combo

            // Thiết lập khóa tổng hợp cho CartDetail
            modelBuilder.Entity<CartDetail>()
                .HasKey(cd => new { cd.CartId, cd.ProductId }); // Đặt CartId và ProductId là khóa chính tổng hợp
                                                                // Thiết lập mối quan hệ giữa CartDetail và Cart

            // Thiết lập quan hệ giữa CartDetail và Cart
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(cd => cd.CartId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete khi xóa giỏ hàng

            // Thiết lập quan hệ giữa CartDetail và Product
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Product)
                .WithMany(p => p.CartDetails)
                .HasForeignKey(cd => cd.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete khi xóa sản phẩm




            // Thiết lập khóa tổng hợp cho OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(cd => new { cd.OrderId, cd.ProductId }); // Đặt OrderId và ProductId là khóa chính tổng hợp
                                                                 // Thiết lập mối quan hệ giữa CartDetail và Cart

            // Thiết lập quan hệ giữa OrderDetail và Cart
            modelBuilder.Entity<OrderDetail>()
                .HasOne(cd => cd.Order)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(cd => cd.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete khi xóa giỏ hàng

            // Thiết lập quan hệ giữa OrderDetail và Product
            modelBuilder.Entity<OrderDetail>()
                .HasOne(cd => cd.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(cd => cd.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete khi xóa sản phẩm


        }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Combo> Combos { get; set; }
		public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
	    public DbSet<Assignment.Models.Order> Order { get; set; } = default!;

    }
}
