using ImageServer.Api.Data.Repositories;

namespace ImageServer.Api.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IImageRepository Images { get; }
    Task<int> SaveChangesAsync();
}
