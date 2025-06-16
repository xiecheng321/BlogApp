using System.Collections.Generic;


namespace BlogApp.Models
{
    public enum NovelStatus
    {
        Created,      // 已创建
        Serializing,  // 连载中
        Finished      // 已完结
    }
    public class Novel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }         // 外键，关联 Author
        public Author Author { get; set; }        // 导航属性
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public double Score { get; set; }
        public List<Volume> Volumes { get; set; }   // 新增：分卷集合
        public List<Chapter> Chapters { get; set; }


        // 新增扩展字段
        //public string Category { get; set; }         // 分类
        //public string Status { get; set; }           // 状态（如“连载中”/“已完结”）
        public NovelStatus Status { get; set; } = NovelStatus.Created;          // 状态（如“连载中”/“已完结”）枚举
        public DateTime CreateTime { get; set; }     // 创建时间
        public DateTime UpdateTime { get; set; }     // 最后更新时间
        public int WordCount { get; set; }           // 总字数

        public int CategoryId { get; set; }        // 外键
        public Category Category { get; set; }     // 导航属性


        public Novel()
        {
            Chapters = new List<Chapter>();
        }
    }



}

