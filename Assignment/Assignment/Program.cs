using Assignment.Helpers;
using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"))
	);

builder.Services.AddTransient<IAccountSvc, AccountSvc>();

builder.Services.AddTransient<IProductSvc, ProductSvc>();

builder.Services.AddTransient<ICategorySvc, CategorySvc>();

builder.Services.AddTransient<ICartSvc, CartSvc>();

builder.Services.AddTransient<IOrderSvc, OrderSvc>();

builder.Services.AddTransient<ICartDetailSvc, CartDetailSvc>();

builder.Services.AddTransient<IRoleSvc, RoleSvc>();

builder.Services.AddTransient<IUploadHelper, UploadHelper>();

builder.Services.AddTransient<IEncryptionHelper, EncryptionHelper>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(x =>
	{
		x.LoginPath = "/Admin/Login/Login";
		x.AccessDeniedPath = "/Admin/Login/AccessDenied"; // trỏ đến action accessdenied
		x.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		x.SlidingExpiration = true; //Làm mới cookie mỗi request
		x.Cookie.HttpOnly = true; // Cookie chỉ tương tác được ở server
		x.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; //Dùng Https
	});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(20);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "MyArea",
	pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
