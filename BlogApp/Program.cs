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

            // 3. 构建应用
            var app = builder.Build();

            // 4. 注册 Session 中间件
            app.UseSession();

            // 5. 配置 HTTP 请求管道
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();

            // 6. 配置默认路由
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            // 分类数据初始化（只会加一次，重复启动不会重复加）
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BlogApp.Models.AppDbContext>();
                if (!context.Categories.Any())
                {

                        //如果 Category 有 Icon 字段，也可以同时赋值：
                        //new Category { Name = "玄幻", Icon = "???" }
                    context.Categories.AddRange(
                        new Category { Name = "玄幻", Icon = "???" },
                        new Category { Name = "都市", Icon = "???" },
                        new Category { Name = "科幻", Icon = "??" },
                        new Category { Name = "悬疑", Icon = "??" },
                        new Category { Name = "历史", Icon = "??" },
                        new Category { Name = "现实", Icon = "???" },
                        new Category { Name = "诗文", Icon = "??" }
                        // ...可以继续加
                    );
                    context.SaveChanges();
                }
            }




            app.Run();
        }
    }
}
