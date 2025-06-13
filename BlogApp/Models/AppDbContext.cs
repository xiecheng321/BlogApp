using System;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        //public DbSet<Novel> Novels { get; set; }
        //public DbSet<Chapter> Chapters { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Author)
                .WithOne(a => a.User)
                .HasForeignKey<Author>(a => a.UserId);

            base.OnModelCreating(modelBuilder);
        }


    }

}
