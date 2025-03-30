using System.ComponentModel.DataAnnotations;
using EFCoreShowcase.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace EFCoreShowcase.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
