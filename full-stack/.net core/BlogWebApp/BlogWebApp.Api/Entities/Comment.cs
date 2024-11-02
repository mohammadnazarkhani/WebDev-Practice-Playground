namespace BlogWebApp.Api.Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int BlogPostId { get; set; }
    public BlogPost? BlogPost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}