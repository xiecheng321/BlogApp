using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ChapterController : Controller
{
    private readonly AppDbContext _context;

    public ChapterController(AppDbContext context)
    {
        _context = context;
    }

    // ========== 1. 写作主页面：左侧分组/右侧写作 ==========
    public IActionResult Write(int novelId, int? chapterId)
    {
        var novel = _context.Novels
            .Include(n => n.Chapters)
            .FirstOrDefault(n => n.Id == novelId);

        if (novel == null) return NotFound();

        var draftChapters = novel.Chapters.Where(c => c.Status == 0).OrderBy(c => c.Id).ToList();
        var publishedChapters = novel.Chapters.Where(c => c.Status == 1).OrderBy(c => c.Id).ToList();
        var recycledChapters = novel.Chapters.Where(c => c.Status == 2).OrderBy(c => c.Id).ToList();

        Chapter current = null;
        if (chapterId != null && chapterId != 0)
            current = novel.Chapters.FirstOrDefault(c => c.Id == chapterId);

        var viewModel = new WriteViewModel
        {
            Novel = novel,
            Chapters = novel.Chapters.ToList(),
            DraftChapters = draftChapters,
            PublishedChapters = publishedChapters,
            RecycledChapters = recycledChapters,
            CurrentChapter = current
        };

        return View(viewModel); // Views/Chapter/Write.cshtml
    }

    // ========== 2. 保存/发布/删除章节 ==========
    [HttpPost]
    public IActionResult Save(int NovelId, int? Id, string Title, string Content, string action)
    {
        Chapter chapter;
        if (Id != null && Id != 0)
        {
            chapter = _context.Chapters.FirstOrDefault(c => c.Id == Id);
            if (chapter == null) return NotFound();
            chapter.Title = Title;
            chapter.Content = Content;
        }
        else
        {
            chapter = new Chapter
            {
                NovelId = NovelId,
                Title = Title,
                Content = Content,
                CreateTime = DateTime.Now
            };
            _context.Chapters.Add(chapter);
        }

        // 控制状态
        if (action == "draft")
            chapter.Status = 0;
        else if (action == "publish")
            chapter.Status = 1;
        else if (action == "delete")
            chapter.Status = 2;

        _context.SaveChanges();
        // 保存后回到写作区并高亮当前章节
        return RedirectToAction("Write", new { novelId = NovelId, chapterId = chapter.Id });
    }

    // ========== 3. 章节列表页（老版，可留作入口） ==========
    public IActionResult List(int novelId)
    {
        var novel = _context.Novels
            .Include(n => n.Chapters)
            .FirstOrDefault(n => n.Id == novelId);

        if (novel == null)
            return NotFound();

        return View(novel); // List.cshtml
    }

    // ========== 4. 老版新建章节（可选保留） ==========
    [HttpGet]
    public IActionResult Create(int? novelId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users
            .Include(u => u.Author)
            .ThenInclude(a => a.Novels)
            .FirstOrDefault(u => u.Id == userId);

        if (user?.Author == null)
            return RedirectToAction("Apply", "Author");

        ViewBag.Novels = user.Author.Novels;
        ViewBag.DefaultNovelId = novelId;

        return View();
    }

    [HttpPost]
    public IActionResult Create(int NovelId, string Title, string Content)
    {
        var chapter = new Chapter
        {
            NovelId = NovelId,
            Title = Title,
            Content = Content,
            CreateTime = DateTime.Now,
            Status = 0 // 新建默认草稿
        };

        _context.Chapters.Add(chapter);
        _context.SaveChanges();

        return RedirectToAction("Write", new { novelId = NovelId, chapterId = chapter.Id });
    }

    // ========== 5. 老版章节编辑 ==========
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var chapter = _context.Chapters.Include(c => c.Novel).FirstOrDefault(c => c.Id == id);
        if (chapter == null) return NotFound();
        return View(chapter); // Edit.cshtml
    }

    [HttpPost]
    public IActionResult Edit(int id, string Title, string Content)
    {
        var chapter = _context.Chapters.FirstOrDefault(c => c.Id == id);
        if (chapter == null) return NotFound();
        chapter.Title = Title;
        chapter.Content = Content;
        _context.SaveChanges();
        return RedirectToAction("Write", new { novelId = chapter.NovelId, chapterId = chapter.Id });
    }

    // ========== 6. 真正物理删除章节 ==========
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var chapter = _context.Chapters.FirstOrDefault(c => c.Id == id);
        if (chapter == null) return NotFound();
        int novelId = chapter.NovelId;
        _context.Chapters.Remove(chapter);
        _context.SaveChanges();
        return RedirectToAction("Write", new { novelId });
    }
}
