using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace BlogApp.Controllers
{
    public class NovelController : Controller
    {
        // JSON 数据文件路径（保存所有小说）
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "novels.json");

        // 读取小说数据
        private List<Novel> LoadNovels()
        {
            if (!System.IO.File.Exists(DataFile))
                return new List<Novel>();

            var json = System.IO.File.ReadAllText(DataFile);
            return JsonSerializer.Deserialize<List<Novel>>(json) ?? new List<Novel>();
        }

        // 保存小说数据
        private void SaveNovels(List<Novel> novels)
        {
            var json = JsonSerializer.Serialize(novels);
            System.IO.File.WriteAllText(DataFile, json);
        }

        // 小说列表页（可选）
        public IActionResult Index()
        {
            var novels = LoadNovels();
            return View(novels); // 对应 Views/Novel/Index.cshtml
        }

        // 创建小说页面（GET）
        public IActionResult Create()
        {
            return View(); // 显示表单
        }

        // 创建小说页面（POST）
        [HttpPost]
        public IActionResult Create(Novel novel)
        {
            var novels = LoadNovels();

            // 设置 ID：列表最大 ID + 1
            novel.Id = novels.Any() ? novels.Max(n => n.Id) + 1 : 1;
            novels.Add(novel);

            SaveNovels(novels);

            // 创建成功后跳转到小说详情页
            return RedirectToAction("Details", new { id = novel.Id });
        }

        // 小说详情页（展示简介 + 添加章节按钮）
        public IActionResult Details(int id)
        {
            var novels = LoadNovels();
            var chapters = LoadChapters();

            var novel = novels.FirstOrDefault(n => n.Id == id);
            if (novel == null) return NotFound();

            // 手动查找所有属于该小说的章节
            novel.Chapters = chapters.Where(c => c.NovelId == id).ToList();

            return View(novel); // 对应 Views/Novel/Details.cshtml
        }

        // 读取章节数据（用于小说详情页中展示该小说的所有章节）
        private List<Chapter> LoadChapters()
        {
            var chapterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chapters.json");
            if (!System.IO.File.Exists(chapterFile))
                return new List<Chapter>();

            var json = System.IO.File.ReadAllText(chapterFile);
            return JsonSerializer.Deserialize<List<Chapter>>(json) ?? new List<Chapter>();
        }

        /*public IActionResult Read(int id) 
        {
            var novels = LoadNovels();
            var novel = novels.FirstOrDefault(n => n.Id == id);
            if (novel == null) return NotFound();

            //加载章节
            var chapters = LoadChapters();
            novel.Chapters = chapters.Where(c => c.NovelId == id).ToList();

            return View(novel); //对应 Views/Novel/Read.cshtml
        }*/
    }
}
