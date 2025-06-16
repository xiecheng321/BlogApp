using System;

namespace BlogApp.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public int NovelId { get; set; }         // 外键
        public Novel Novel { get; set; }         // 导航属性
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; } // 0=草稿，1=已发布，2=回收站
        public int VolumeId { get; set; }    // 外键，属于哪一卷
        public Volume Volume { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }


}

