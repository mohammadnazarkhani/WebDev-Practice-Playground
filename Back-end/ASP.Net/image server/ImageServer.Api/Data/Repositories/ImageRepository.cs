using ImageServer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageServer.Api.Data.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
    private ImageDbContext DbContext => (ImageDbContext)Context;

    public ImageRepository(ImageDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Image>> GetRecentImagesAsync(int count)
        => await DbContext.Images
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<bool> ExistsAsync(Guid id)
        => await DbContext.Images.AnyAsync(x => x.Id == id);
}
