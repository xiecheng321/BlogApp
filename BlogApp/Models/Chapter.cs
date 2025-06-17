// ✅ Chapter.cs - 数据模型
using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public enum ChapterStatus
    {
        Draft,
        Published
    }

    public class Chapter
    {
        public int Id { get; set; }

        [Required]
        public int NovelId { get; set; }
        public Novel Novel { get; set; } = null!;

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Content { get; set; } = "";

        public string? AuthorNote { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? LastEditTime { get; set; }
        public ChapterStatus Status { get; set; } = ChapterStatus.Draft;
        public int WordCount => string.IsNullOrWhiteSpace(Content) ? 0 : Content.Length;
    }
}
