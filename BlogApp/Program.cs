using Microsoft.EntityFrameworkCore;
using BlogApp.Models;

namespace BlogApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. 注册数据库上下文
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 2. 注册 MVC 控制器、Razor 页、Session、HttpContext
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            // 3. 注册Cookie认证 【新增部分】
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Account/LoginRegister";
                    options.LogoutPath = "/Account/Logout";
                });

            // 4. 构建应用
            var app = builder.Build();

            // 5. 注册 Session 中间件
            app.UseSession();

            // 6. 配置 HTTP 请求管道
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // 7. 注册Cookie认证中间件 【新增部分】
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            // 8. 配置默认路由
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // 分类数据初始化...
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BlogApp.Models.AppDbContext>();
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "玄幻" },
                        new Category { Name = "都市" },
                        new Category { Name = "科幻" },
                        new Category { Name = "悬疑" },
                        new Category { Name = "历史" },
                        new Category { Name = "现实" },
                        new Category { Name = "诗文" }
                    );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
