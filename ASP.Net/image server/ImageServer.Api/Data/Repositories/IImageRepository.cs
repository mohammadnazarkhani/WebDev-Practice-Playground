using ImageServer.Api.Models;

namespace ImageServer.Api.Data.Repositories;

public interface IImageRepository : IRepository<Image>
{
    Task<IEnumerable<Image>> GetRecentImagesAsync(int count);
    Task<bool> ExistsAsync(Guid id);
}
