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

    // 显示作家专区首页
    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users.Include(u => u.Author).FirstOrDefault(u => u.Id == userId);
        if (user == null) return RedirectToAction("LoginRegister", "Account");

        if (user.Author == null)
        {
            // 还不是作家，跳转到申请页
            return RedirectToAction("Apply");
        }
        // 已是作家，显示作家中心
        return RedirectToAction("Me");
    }

    // 申请成为作者
    [HttpGet]
    public IActionResult Apply()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Apply(string penName, string intro)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users.Include(u => u.Author).FirstOrDefault(u => u.Id == userId);
        if (user == null) return RedirectToAction("LoginRegister", "Account");

        if (user.Author != null)
        {
            // 已是作者
            return RedirectToAction("Me");
        }
        var author = new Author
        {
            PenName = penName,
            Bio = intro,
            // 可以添加其他字段如：申请时间
        };
        _context.Authors.Add(author);
        _context.SaveChanges();

        // 关联
        user.AuthorId = author.Id;
        user.Author = author;
        _context.SaveChanges();

        return RedirectToAction("Me");
    }

    // 显示作家中心
    public IActionResult Me()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("LoginRegister", "Account");

        var user = _context.Users.Include(u => u.Author).FirstOrDefault(u => u.Id == userId);
        if (user == null || user.Author == null) return RedirectToAction("Apply");
        return View(user.Author);
    }
}
