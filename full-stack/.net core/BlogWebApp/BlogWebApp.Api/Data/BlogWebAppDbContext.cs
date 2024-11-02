using BlogWebApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Api.Data;

public class BlogWebAppDbContext(DbContextOptions<BlogWebAppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>()
            .HasOne(p => p.Author)
            .WithMany() // This assumes one author can have many blog posts
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade); // Optional

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany() // Assuming one user can have many comments
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction); // Optional

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.BlogPost)
            .WithMany() // Assuming one blog post can have many comments
            .HasForeignKey(c => c.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade); // Optional
    }

}