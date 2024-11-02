using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Api.Entities;

public enum UserRole
{
    User,
    ContentCreator,
    Admin
}

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public UserRole Role { get; set; }
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
}