

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


    }
}

