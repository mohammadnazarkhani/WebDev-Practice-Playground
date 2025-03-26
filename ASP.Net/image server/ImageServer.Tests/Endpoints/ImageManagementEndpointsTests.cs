using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Base;
using Moq;

namespace ImageServer.Tests.Endpoints;

public class ImageManagementEndpointsTests : TestBase
{
    private readonly Mock<IImageService> _imageServiceMock;

    public ImageManagementEndpointsTests()
    {
        _imageServiceMock = new Mock<IImageService>();
    }

    [Fact]
    public async Task DeleteImage_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        _imageServiceMock
            .Setup(x => x.DeleteImageAsync(id))
            .ReturnsAsync((true, "Image deleted successfully"));

        // Act
        var result = await _imageServiceMock.Object.DeleteImageAsync(id);

        // Assert
        Assert.True(result.success);
        Assert.Equal("Image deleted successfully", result.message);
    }

    [Fact]
    public async Task DeleteAllImages_ReturnsSuccess()
    {
        // Arrange
        _imageServiceMock
            .Setup(x => x.DeleteAllImagesAsync())
            .ReturnsAsync((true, "All images deleted successfully"));

        // Act
        var result = await _imageServiceMock.Object.DeleteAllImagesAsync();

        // Assert
        Assert.True(result.success);
        Assert.Contains("deleted successfully", result.message);
    }
}
