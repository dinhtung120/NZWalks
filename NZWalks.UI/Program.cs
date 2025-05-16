using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using NZWalks.UI.Services;

// Khởi tạo ứng dụng web
var builder = WebApplication.CreateBuilder(args);

// Đăng ký các service cho DI container
// Thêm hỗ trợ cho MVC với Views
builder.Services.AddControllersWithViews();

// Thêm HttpClient để gọi API
// Sử dụng cho việc giao tiếp với API backend
builder.Services.AddHttpClient();

// Thêm các service
builder.Services.AddScoped<IImageService, ImageService>();

// Cấu hình xác thực với Cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

var app = builder.Build();

// Cấu hình middleware pipeline
if (!app.Environment.IsDevelopment())
{
    // Xử lý lỗi trong môi trường production
    // Chuyển hướng đến trang lỗi khi có exception
    app.UseExceptionHandler("/Home/Error");
    
    // Cấu hình HSTS (HTTP Strict Transport Security)
    // Bảo mật bằng cách yêu cầu kết nối HTTPS
    // Giá trị mặc định là 30 ngày
    app.UseHsts();
}

// Cấu hình HTTPS và static files
// Chuyển hướng HTTP sang HTTPS
app.UseHttpsRedirection();
// Cho phép truy cập các file tĩnh (CSS, JS, images)
app.UseStaticFiles();

// Cấu hình routing
// Cho phép định tuyến các request
app.UseRouting();

// Cấu hình xác thực và phân quyền
// Kiểm tra quyền truy cập của người dùng
app.UseAuthentication();
app.UseAuthorization();

// Cấu hình route mặc định
// Pattern: {controller}/{action}/{id?}
// Mặc định: Home/Index
// Ví dụ: /Home/Index, /Regions/Details/1
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Chạy ứng dụng
app.Run();