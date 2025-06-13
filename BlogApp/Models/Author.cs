using BlogApp.Models;

namespace BlogApp.Models
{
    public class Author
    {
        public int Id { get; set; }           // 主键
        public int UserId { get; set; }       // 外键，指向User
        public string PenName { get; set; }   // 笔名
        public string Bio { get; set; }       // 作家简介
        public DateTime ApplyTime { get; set; }// 申请时间
        public int Status { get; set; }       // 认证/审核等状态
                                              // 导航属性
        public User User { get; set; }        // 导航属性
    }
}
