using System.Collections.Generic;


namespace BlogApp.Models
{
    public class Novel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public List<Chapter> Chapters { get; set; } = new(); 
    }
}
