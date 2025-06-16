using System;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Novel> Novels { get; set; }
        public DbSet<Volume> Volumes { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 用户与作者一对一
            modelBuilder.Entity<User>()
                .HasOne(u => u.Author)
                .WithOne(a => a.User)
                .HasForeignKey<Author>(a => a.UserId);

            // 重点：Volume和Chapter之间外键删除行为用Restrict
            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Volume)
                .WithMany(v => v.Chapters)
                .HasForeignKey(c => c.VolumeId)
                .OnDelete(DeleteBehavior.Restrict); // 或 .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
