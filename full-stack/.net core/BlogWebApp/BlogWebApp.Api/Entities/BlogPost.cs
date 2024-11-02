namespace BlogWebApp.Api.Entities;

public class BlogPost
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Content { get; set; } = "No Content";
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}