using Blog.Context;
using BlogV1.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogDbContext>();

//identity db context  baglantýsý.
builder.Services.AddDbContext<BlogIdentityDbContext>(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

//kullanýcný admin giriþi yapmadýgýnda default sayfa yönlendirme iþlemleri..
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Blog/Login";
});

builder.Services.AddIdentity<BlogIdentityUser, BlogIdentityRole>().AddEntityFrameworkStores<BlogIdentityDbContext>().AddDefaultTokenProviders();

var app = builder.Build();



// default ayarlar.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");

app.Run();
