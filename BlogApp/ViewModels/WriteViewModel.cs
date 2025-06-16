using System.Collections.Generic;
using BlogApp.Models;


// 建议放在 BlogApp/ViewModels/WriteViewModel.cs
namespace BlogApp.ViewModels
{
    public class WriteViewModel
    {
        public Models.Novel Novel { get; set; }
        public List<Models.Chapter> Chapters { get; set; }
        public List<Models.Chapter> DraftChapters { get; set; }
        public List<Models.Chapter> PublishedChapters { get; set; }
        public List<Models.Chapter> RecycledChapters { get; set; }
        public Models.Chapter CurrentChapter { get; set; }
    }
}
