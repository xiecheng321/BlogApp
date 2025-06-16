

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
        private readonly AppDbContext _context; // �����ֶ�

        public HomeController(AppDbContext context)   // ���캯��ע��
        {
            _context = context;
        }


        // ��վ��ҳ
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();   //��ÿ����Ҫ�õ�����˵��� Controller �ﴫ�ݷ������ݡ������� HomeController ��ÿ�� Action ���.
            return View();
        }


    }
}

