using Microsoft.EntityFrameworkCore;
using BlogApp.Models;

namespace BlogApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. ע�����ݿ�������
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // 2. ע�� MVC ��������Razor ҳ��Session��HttpContext
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            // 3. ע��Cookie��֤ ���������֡�
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Account/LoginRegister";
                    options.LogoutPath = "/Account/Logout";
                });

            // 4. ����Ӧ��
            var app = builder.Build();

            // 5. ע�� Session �м��
            app.UseSession();

            // 6. ���� HTTP ����ܵ�
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // 7. ע��Cookie��֤�м�� ���������֡�
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            // 8. ����Ĭ��·��
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // �������ݳ�ʼ��...
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BlogApp.Models.AppDbContext>();
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "����" },
                        new Category { Name = "����" },
                        new Category { Name = "�ƻ�" },
                        new Category { Name = "����" },
                        new Category { Name = "��ʷ" },
                        new Category { Name = "��ʵ" },
                        new Category { Name = "ʫ��" }
                    );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
