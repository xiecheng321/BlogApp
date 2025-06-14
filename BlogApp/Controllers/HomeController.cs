

using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context; // 声明字段

        public HomeController(AppDbContext context)   // 构造函数注入
        {
            _context = context;
        }


        // 网站首页
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();   //在每个需要用到分类菜单的 Controller 里传递分类数据。例如在 HomeController 的每个 Action 里加.
            return View();
        }




        /*
        // 小说数据文件路径
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "novels.json");

        private List<Novel> LoadNovels()
        {
            if (!System.IO.File.Exists(DataFile))
                return new List<Novel>();
            var json = System.IO.File.ReadAllText(DataFile);
            return JsonSerializer.Deserialize<List<Novel>>(json) ?? new List<Novel>();
        }

        // 首页 - 推荐书籍
        public IActionResult Index()
        {
            var novels = LoadNovels();
            // 这里只简单取前6本小说作为“推荐”
            var recommended = novels.Take(6).ToList();
            return View(recommended);  // 对应 Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}

