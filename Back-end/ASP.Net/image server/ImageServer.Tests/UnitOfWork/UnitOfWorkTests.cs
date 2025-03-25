using ImageServer.Api.Data;
using ImageServer.Api.Data.UnitOfWork;
using ImageServer.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace ImageServer.Tests.UnitOfWork;

public class UnitOfWorkTests
{
    private readonly ImageDbContext _context;
    private readonly Api.Data.UnitOfWork.UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<ImageDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ImageDbContext(options);
        _unitOfWork = new Api.Data.UnitOfWork.UnitOfWork(_context);
    }

    [Fact]
    public void Images_ReturnsImageRepository()
    {
        // Act
        var repository = _unitOfWork.Images;

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task SaveChangesAsync_SavesChangesToDatabase()
    {
        // Arrange
        var image = TestImageFixture.CreateTestImage();
        await _unitOfWork.Images.AddAsync(image);

        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.True(await _unitOfWork.Images.ExistsAsync(image.Id));
    }

    [Fact]
    public void Dispose_DisposesContext()
    {
        // Act
        _unitOfWork.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => _context.Database.EnsureCreated());
    }
}
