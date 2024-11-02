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


        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                UserName = "admin",
                Role = UserRole.Admin,
                Password = "hashed_password_here", // Ensure to hash passwords in a real application
                Email = "admin@example.com"
            },
            new User
            {
                Id = 2,
                UserName = "creator",
                Role = UserRole.ContentCreator,
                Password = "hashed_password_here",
                Email = "creator@example.com"
            },
            new User
            {
                Id = 3,
                UserName = "user",
                Role = UserRole.User,
                Password = "hashed_password_here",
                Email = "user@example.com"
            }
        );

        modelBuilder.Entity<BlogPost>().HasData(
            new BlogPost
            {
                Id = 1,
                Title = "First Blog Post",
                Content = "This is the content of the first blog post.",
                AuthorId = 1, // Refers to the admin user
            },
            new BlogPost
            {
                Id = 2,
                Title = "Second Blog Post",
                Content = "This is the content of the second blog post.",
                AuthorId = 2, // Refers to the content creator user
            }
        );

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                UserId = 3, // Refers to the user
                BlogPostId = 1,
            },
            new Comment
            {
                Id = 2,
                UserId = 3, // Refers to the user
                BlogPostId = 2,
            }
        );
    }
}