// Models/WriteViewModel.cs
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class WriteViewModel
    {
        public Novel Novel { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<Chapter> DraftChapters { get; set; }      // 草稿
        public List<Chapter> PublishedChapters { get; set; }   // 已发布
        public List<Chapter> RecycledChapters { get; set; }    // 回收站
        public Chapter CurrentChapter { get; set; }
    }

}
