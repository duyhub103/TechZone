using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyWeb.Data;


var builder = WebApplication.CreateBuilder(args);


// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Đăng ký DbContext vào hệ thống
builder.Services.AddDbContext<TechZoneDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<TechZoneDbContext>()
    .AddDefaultTokenProviders();

// Đăng ký MVC Controllers
builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TechZoneDbContext>();
        // Tự động chạy lệnh Update-Database nếu chưa có db
        context.Database.Migrate();

        // Gọi hàm tạo dữ liệu mẫu
        DbSeeder.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();