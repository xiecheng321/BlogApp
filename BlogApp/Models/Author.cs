using BlogApp.Models;

public class Author
{
    public int Id { get; set; }               // 作者唯一Id
    public string Name { get; set; }          // 作者名（可重名）
    public string Bio { get; set; }           // 简介（可选）
    public string AvatarUrl { get; set; }     // 头像（可选）
    // 可继续扩展：认证标记、社交账号、联系邮箱等
    public List<Novel> Novels { get; set; }   // 作者所有作品
}
