using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AuthorController : Controller
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // ========= 主入口页 =========
    public IActionResult Home()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users
            .Include(u => u.Author)
                .ThenInclude(a => a.Novels)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null)
            return RedirectToAction("LoginRegister", "Account");
        if (user.Author == null)
            return RedirectToAction("Apply");

        return View(user.Author); // Home.cshtml
    }

    // ========= 默认页跳主入口 =========
    public IActionResult Index()
    {
        return RedirectToAction("Home");
    }

    // ========= 申请成为作者 =========
    [HttpGet]
    public IActionResult Apply()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Apply(object? notUsed = null)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users.Include(u => u.Author).FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return RedirectToAction("LoginRegister", "Account");

        if (user.Author != null)
            return RedirectToAction("Me");

        var author = new Author
        {
            UserId = user.Id,
            ApplyTime = DateTime.Now,
            Status = 0 // 0待审核，1已通过
        };

        _context.Authors.Add(author);
        _context.SaveChanges();

        user.AuthorId = author.Id;
        user.Author = author;
        _context.SaveChanges();

        return RedirectToAction("Me");
    }

    // ========= 工作台/后台 =========
    public IActionResult Me()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users
            .Include(u => u.Author)
                .ThenInclude(a => a.Novels)
            .FirstOrDefault(u => u.Id == userId);

        if (user == null || user.Author == null)
            return RedirectToAction("Apply");

        return View(user.Author); // Me.cshtml
    }

    // ========= 作品管理 =========
    public IActionResult Works()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users
            .Include(u => u.Author)
                .ThenInclude(a => a.Novels)
            .FirstOrDefault(u => u.Id == userId);

        if (user?.Author == null)
            return RedirectToAction("Apply");

        return View(user.Author); // Works.cshtml
    }

    // ========= 不再负责写作/章节页面 =========
    // 所有写作入口全部指向 ChapterController 的 Write
}
