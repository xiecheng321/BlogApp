using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        // С˵�����ļ�·��
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "novels.json");

        private List<Novel> LoadNovels()
        {
            if (!System.IO.File.Exists(DataFile))
                return new List<Novel>();
            var json = System.IO.File.ReadAllText(DataFile);
            return JsonSerializer.Deserialize<List<Novel>>(json) ?? new List<Novel>();
        }

        // ��ҳ - �Ƽ��鼮
        public IActionResult Index()
        {
            var novels = LoadNovels();
            // ����ֻ��ȡǰ6��С˵��Ϊ���Ƽ���
            var recommended = novels.Take(6).ToList();
            return View(recommended);  // ��Ӧ Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
