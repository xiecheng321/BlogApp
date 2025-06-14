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

            // 3. ����Ӧ��
            var app = builder.Build();

            // 4. ע�� Session �м��
            app.UseSession();

            // 5. ���� HTTP ����ܵ�
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

            // 6. ����Ĭ��·��
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            // �������ݳ�ʼ����ֻ���һ�Σ��ظ����������ظ��ӣ�
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BlogApp.Models.AppDbContext>();
                if (!context.Categories.Any())
                {

                        //��� Category �� Icon �ֶΣ�Ҳ����ͬʱ��ֵ��
                        //new Category { Name = "����", Icon = "???" }
                    context.Categories.AddRange(
                        new Category { Name = "����", Icon = "???" },
                        new Category { Name = "����", Icon = "???" },
                        new Category { Name = "�ƻ�", Icon = "??" },
                        new Category { Name = "����", Icon = "??" },
                        new Category { Name = "��ʷ", Icon = "??" },
                        new Category { Name = "��ʵ", Icon = "???" },
                        new Category { Name = "ʫ��", Icon = "??" }
                        // ...���Լ�����
                    );
                    context.SaveChanges();
                }
            }




            app.Run();
        }
    }
}
