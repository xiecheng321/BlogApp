using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class NovelController : Controller
    {
        private readonly AppDbContext _context;

        public NovelController(AppDbContext context)
        {
            _context = context;
        }




        // GET: Novel/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public IActionResult Create(string Title, int CategoryId, string Description, IFormFile CoverImage)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LoginRegister", "Account");

            var user = _context.Users.Include(u => u.Author).FirstOrDefault(u => u.Id == userId);
            if (user == null || user.Author == null)
            {
                return RedirectToAction("Apply", "Author"); // 用户不是作者，不能发小说
            }

            var novel = new Novel
            {
                Title = Title,
                CategoryId = CategoryId,
                Description = Description,
                CreateTime = DateTime.Now,
                Status = NovelStatus.Created,
                AuthorId = user.Author.Id
            };

            // ====== 处理封面 ======
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var ext = Path.GetExtension(CoverImage.FileName);
                var fileName = Guid.NewGuid() + ext;
                var saveDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/covers");
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                var savePath = Path.Combine(saveDir, fileName);

                using (var fs = new FileStream(savePath, FileMode.Create))
                {
                    CoverImage.CopyTo(fs);
                }
                novel.CoverUrl = "/images/covers/" + fileName;
            }
            else
            {
                string saveDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/covers");
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                string fileName = Guid.NewGuid() + ".png";
                string savePath = Path.Combine(saveDir, fileName);

                BlogApp.Utils.CoverHelper.GenerateCover(novel.Title, "作者：" + user.UserName, savePath);
                novel.CoverUrl = "/images/covers/" + fileName;
            }

            _context.Novels.Add(novel);
            _context.SaveChanges();

            return RedirectToAction("Index", "Author");
        }





    }

}

