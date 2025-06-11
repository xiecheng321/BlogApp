using System;

namespace BlogApp.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NovelId { get; set; }
        public Novel Novel { get; set; }
    }

}
