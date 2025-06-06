using Microsoft.AspNetCore.Mvc;
using BlogApp.Models; // 引入你刚才的模型命名空间
using System.Collections.Generic;
using System.IO; 
using System.Text.Json;


namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        // 模拟内存数据
        /*private static List<BlogPost> posts = new List<BlogPost>
        {
            new BlogPost { Id = 1, Title = "第一篇文章", Content = "欢迎来到我的博客", CreatedAt = DateTime.Now },
            new BlogPost { Id = 2, Title = "第二篇文章", Content = "这是第二篇文章的内容", CreatedAt = DateTime.Now }
        };*/

        private static readonly string DataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blogposts.json");

        public IActionResult Index()
        {
            var posts = LoadPosts();
            return View(posts);  // 把文章列表传递给视图
        }

        // GET: 显示发布文章页面
        public IActionResult Create()
        {
            return View();
        }

        // POST: 提交文章数据
        [HttpPost]
        public IActionResult Create(BlogPost newPost)
        {
            var posts = LoadPosts();
            newPost.Id = posts.Count + 1;
            newPost.CreatedAt = DateTime.Now;
            posts.Add(newPost);
            SavePosts(posts);
            return RedirectToAction("Index");
        }

        private static List<BlogPost> LoadPosts()
        {
            if (System.IO.File.Exists(DataFile))
            {
                var json = System.IO.File.ReadAllText(DataFile);
                return JsonSerializer.Deserialize<List<BlogPost>>(json) ?? new List<BlogPost>();
            }
            return new List<BlogPost>();
        }

        private static void SavePosts(List<BlogPost> posts)
        {
            var json = JsonSerializer.Serialize(posts);
            System.IO.File.WriteAllText(DataFile, json);
        }

        // GET: 删除文章
        public IActionResult Delete(int id)
        {
            var posts = LoadPosts();
            var postToRemove = posts.Find(p => p.Id == id);
            if (postToRemove != null)
            {
                posts.Remove(postToRemove);
                SavePosts(posts);
            }
            return RedirectToAction("Index");
        }

        // GET: 显示编辑页面
        public IActionResult Edit(int id)
        {
            var posts = LoadPosts();
            var post = posts.Find(p => p.Id == id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        // POST: 提交编辑后的数据
        [HttpPost]
        public IActionResult Edit(BlogPost editedPost)
        {
            var posts = LoadPosts();
            var post = posts.Find(p => p.Id == editedPost.Id);
            if (post != null)
            {
                post.Title = editedPost.Title;
                post.Content = editedPost.Content;
                // 不修改 CreatedAt
                SavePosts(posts);
            }
            return RedirectToAction("Index");
        }




    }
}

