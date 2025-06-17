using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BlogApp.Models
{
    public enum NovelStatus
    {
        Created,
        Serializing,
        Finished
    }

    public class Novel
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        //[Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; } = "";
        public string? Description { get; set; }

        //[Required(ErrorMessage = "请选择分类")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public NovelStatus Status { get; set; } = NovelStatus.Serializing;
        public string? LatestChapter { get; set; }
        public int FavoritesCount { get; set; } = 0;

        public List<Chapter> Chapters { get; set; } = new();
    }
}
