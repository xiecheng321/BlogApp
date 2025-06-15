using BlogApp.Models;

namespace BlogApp.Models
{
    public class Author
    {
        public int Id { get; set; }            // 主键
        public int UserId { get; set; }        // 外键，指向 User
        public DateTime ApplyTime { get; set; }// 申请时间
        public int Status { get; set; }        // 认证/审核等状态（0待审核，1已通过）

        // 导航属性
        public User User { get; set; }         // 导航属性

        public List<Novel> Novels { get; set; }  // 一个作者拥有多本小说
    }
}

