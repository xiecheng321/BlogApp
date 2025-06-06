using System;

namespace BlogApp.Models
{
    public class Chapter
    {
        public int Id { get; set; }                  // 每篇文章的唯一编号
        public string Title { get; set; } = "";      // 文章标题
        public string Content { get; set; } = "";    // 正文内容
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // 创建时间
        public string Author { get; set; } = "";    //文章作者
        
        // 新增：外键与导航属性
        public int NovelId { get; set; }
        public Novel? Novel { get; set; }
    }
}
