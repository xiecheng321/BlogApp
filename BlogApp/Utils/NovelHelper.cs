// 文件路径：BlogApp/Utils/NovelHelper.cs
using BlogApp.Models;

namespace BlogApp.Utils
{
    public static class NovelHelper
    {
        public static string GetStatusDisplay(NovelStatus status)
        {
            switch (status)
            {
                case NovelStatus.Created: return "已创建";
                case NovelStatus.Serializing: return "连载中";
                case NovelStatus.Finished: return "已完结";
                default: return "未知";
            }
        }
    }
}
