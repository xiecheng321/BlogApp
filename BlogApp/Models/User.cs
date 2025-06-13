using System;

namespace BlogApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreateTime { get; set; }

        // 可选：导航属性（允许一个用户拥有一个作者身份）
        public Author? Author { get; set; }

        public int? AuthorId { get; set; }     // 外键（可空）
    }
}

