using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BMFE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Thêm dịch vụ Razor Pages
            builder.Services.AddRazorPages();

            // Cấu hình HttpClient cho API
            builder.Services.AddHttpClient("AuthApi", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/"); // Địa chỉ API
            });

            // Cấu hình Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Xử lý lỗi
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Sử dụng Session và Authorization
            app.UseSession();
            app.UseAuthorization();

            // Định nghĩa route cho trang chính
            app.MapGet("/", async context =>
            {
                context.Response.Redirect("/BlogManage/HomePage");
            });

            // Định nghĩa các Razor Pages
            app.MapRazorPages();

            // Chạy ứng dụng
            app.Run();
        }
    }
}