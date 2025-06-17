using System;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        public int UserId { get; set; }             // 一对一外键
        public DateTime ApplyTime { get; set; }     // 创建时间
        public int Status { get; set; } = 1;        // 默认已通过

        public User User { get; set; } = null!;
        public List<Novel> Novels { get; set; } = new();
    }
}
