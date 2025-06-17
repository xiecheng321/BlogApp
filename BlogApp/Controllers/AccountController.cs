using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // 发送验证码
        [HttpPost]
        public IActionResult SendCode(string phoneNumber)
        {
            var code = new Random().Next(1000, 9999).ToString();
            _httpContextAccessor.HttpContext.Session.SetString("VerifyCode_" + phoneNumber, code);
            _httpContextAccessor.HttpContext.Session.SetString("CodeTime_" + phoneNumber, DateTime.Now.ToString());
            // 开发阶段直接返回验证码
            return Json(new { success = true, code = code });
        }

        // 登录注册 GET
        [HttpGet]
        public IActionResult LoginRegister()
        {
            return View();
        }

        // 登录注册 POST
        [HttpPost]
        public async Task<IActionResult> LoginRegister(string phoneNumber, string verifyCode)
        {
            var code = _httpContextAccessor.HttpContext.Session.GetString("VerifyCode_" + phoneNumber);
            var codeTimeStr = _httpContextAccessor.HttpContext.Session.GetString("CodeTime_" + phoneNumber);

            if (code == null || codeTimeStr == null)
            {
                ModelState.AddModelError("", "请先获取验证码");
                ViewData["PhoneNumber"] = phoneNumber;
                return View();
            }
            if (code != verifyCode)
            {
                ModelState.AddModelError("", "验证码不正确");
                ViewData["PhoneNumber"] = phoneNumber;
                return View();
            }

            // 查找或注册用户
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumber,
                    CreateTime = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            // 写Session（可选，过渡期保留）
            _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
            _httpContextAccessor.HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
            _httpContextAccessor.HttpContext.Session.SetString("UserName", user.UserName ?? "");

            // 写Cookie认证（用 "Cookies" 字符串，确保和Program.cs注册完全一致）
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(
                "Cookies",
                new ClaimsPrincipal(claimsIdentity)
            );

            // 登录成功后进入信息展示页
            return RedirectToAction("Profile");
        }

        // 注销
        public async Task<IActionResult> Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("UserId");
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }

        // 【1】只读信息页 GET
        [HttpGet]
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LoginRegister");
            var user = _context.Users.Find(userId);
            // TempData 用于显示保存成功等一次性消息
            ViewBag.Msg = TempData["Msg"];
            return View(user); // 视图只读模式（Profile.cshtml）
        }

        // 【2】进入编辑页 GET
        [HttpGet]
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LoginRegister");
            var user = _context.Users.Find(userId);
            return View(user); // 视图是编辑表单（EditProfile.cshtml）
        }

        // 【3】提交编辑 POST
        [HttpPost]
        public IActionResult EditProfile(User model, IFormFile avatarFile)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("LoginRegister");
            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("LoginRegister");

            // 修改基本信息
            user.UserName = model.UserName;
            user.Bio = model.Bio;
            user.Gender = model.Gender;
            user.Age = model.Age;

            // 头像上传
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var folder = Path.Combine("wwwroot", "avatars");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    avatarFile.CopyTo(stream);
                }
                user.AvatarUrl = "/avatars/" + fileName;
            }

            _context.SaveChanges();

            // Session同步
            HttpContext.Session.SetString("UserName", user.UserName ?? "");

            TempData["Msg"] = "保存成功！"; // 一次性显示消息

            // 保存后回到只读信息页
            return RedirectToAction("Profile");
        }
    }
}
