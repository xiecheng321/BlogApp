using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

public class NovelDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Novel> Novels { get; set; }
    public DbSet<Chapter> Chapters { get; set; }

    public NovelDbContext(DbContextOptions<NovelDbContext> options)
        : base(options)
    {
    }
}
