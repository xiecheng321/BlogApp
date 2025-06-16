using System.Collections.Generic;

namespace BlogApp.Models
{
    public class Volume
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int NovelId { get; set; }         // 外键，属于哪本小说
        public Novel Novel { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
