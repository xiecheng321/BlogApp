using System;

namespace BlogApp.Models
{
    public class BlogPost
    {
        public int Id { get; set; }                  // 每篇文章的唯一编号
        public string Title { get; set; } = "";      // 文章标题
        public string Content { get; set; } = "";    // 正文内容
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // 创建时间
    }
}
