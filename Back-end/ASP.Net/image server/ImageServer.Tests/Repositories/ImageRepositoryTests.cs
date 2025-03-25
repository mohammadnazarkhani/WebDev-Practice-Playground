using ImageServer.Api.Data;
using ImageServer.Api.Data.Repositories;
using ImageServer.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace ImageServer.Tests.Repositories;

public class ImageRepositoryTests
{
    private readonly ImageDbContext _context;
    private readonly ImageRepository _repository;

    public ImageRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ImageDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ImageDbContext(options);
        _repository = new ImageRepository(_context);
    }

    [Fact]
    public async Task GetRecentImagesAsync_ReturnsOrderedImages()
    {
        // Arrange
        var images = new[]
        {
            TestImageFixture.CreateTestImage(),
            TestImageFixture.CreateTestImage(),
            TestImageFixture.CreateTestImage()
        };

        images[1].CreatedAt = DateTime.UtcNow.AddDays(-1);
        images[2].CreatedAt = DateTime.UtcNow.AddDays(-2);

        await _context.Images.AddRangeAsync(images);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRecentImagesAsync(2);

        // Assert
        var imagesList = result.ToList();
        Assert.Equal(2, imagesList.Count);
        Assert.Equal(images[0].Id, imagesList[0].Id);
        Assert.Equal(images[1].Id, imagesList[1].Id);
    }

    [Fact]
    public async Task ExistsAsync_ExistingImage_ReturnsTrue()
    {
        // Arrange
        var image = TestImageFixture.CreateTestImage();
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(image.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_NonExistingImage_ReturnsFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}
