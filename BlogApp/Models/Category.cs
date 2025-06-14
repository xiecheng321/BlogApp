namespace BlogApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }    // 分类名，如“玄幻”
        public List<Novel> Novels { get; set; }  // 导航属性
        public string Icon { get; set; }
    }
}
