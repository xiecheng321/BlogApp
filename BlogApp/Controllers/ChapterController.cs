using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace BlogApp.Controllers
{
    public class ChapterController : Controller
    {
        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chapters.json");

        // 读取章节列表
        private List<Chapter> LoadChapters()
        {
            if (!System.IO.File.Exists(DataFile))
                return new List<Chapter>();

            var json = System.IO.File.ReadAllText(DataFile);
            return JsonSerializer.Deserialize<List<Chapter>>(json) ?? new List<Chapter>();
        }

        // 保存章节列表
        private void SaveChapters(List<Chapter> chapters)
        {
            var json = JsonSerializer.Serialize(chapters);
            System.IO.File.WriteAllText(DataFile, json);
        }

        // 显示“添加章节”页面，接收 novelId
        [HttpGet]
        public IActionResult Create(int novelId)
        {
            var chapter = new Chapter { NovelId = novelId };
            return View(chapter);
        }

        // 处理章节提交
        [HttpPost]
        public IActionResult Create(Chapter chapter)
        {
            var chapters = LoadChapters();

            // 分配唯一ID
            chapter.Id = chapters.Any() ? chapters.Max(c => c.Id) + 1 : 1;
            chapters.Add(chapter);
            SaveChapters(chapters);

            // 保存后跳回小说详情页
            return RedirectToAction("Details", "Novel", new { id = chapter.NovelId });
        }

        // 展示章节内容
        public IActionResult Details(int id)
        {
            var chapters = LoadChapters();
            var chapter = chapters.FirstOrDefault(c => c.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

    }
}
