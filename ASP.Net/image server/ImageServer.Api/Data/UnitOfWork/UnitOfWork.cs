using ImageServer.Api.Data.Repositories;

namespace ImageServer.Api.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ImageDbContext _context;
    private IImageRepository? _imageRepository;

    public UnitOfWork(ImageDbContext context)
    {
        _context = context;
    }

    public IImageRepository Images => _imageRepository ??= new ImageRepository(_context);

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
    }
}
