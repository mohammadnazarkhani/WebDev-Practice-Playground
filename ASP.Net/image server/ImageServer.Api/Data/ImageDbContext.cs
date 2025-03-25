using Microsoft.EntityFrameworkCore;
using ImageServer.Api.Models;

namespace ImageServer.Api.Data;

public class ImageDbContext : DbContext
{
    public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
    {
    }

    public DbSet<Image> Images { get; set; }
}
