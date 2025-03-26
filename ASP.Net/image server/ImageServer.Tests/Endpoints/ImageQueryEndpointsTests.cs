using ImageServer.Api.DTOs.Images.Responses;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Base;
using Moq;

namespace ImageServer.Tests.Endpoints;

public class ImageQueryEndpointsTests : TestBase
{
    private readonly Mock<IImageService> _imageServiceMock;

    public ImageQueryEndpointsTests()
    {
        _imageServiceMock = new Mock<IImageService>();
    }

    [Fact]
    public async Task GetImages_ReturnsListOfImages()
    {
        // Arrange
        var images = new List<ImageResponseDto>
        {
            new(
                Guid.NewGuid(),
                "Test1",
                "image/jpeg",
                1024,
                DateTime.UtcNow,
                null,
                "http://test/1",
                "http://test/1/thumb"
            ),
            new(
                Guid.NewGuid(),
                "Test2",
                "image/png",
                2048,
                DateTime.UtcNow,
                null,
                "http://test/2",
                "http://test/2/thumb"
            )
        };

        _imageServiceMock
            .Setup(x => x.GetImagesAsync())
            .ReturnsAsync(images);

        // Act
        var result = await _imageServiceMock.Object.GetImagesAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetImage_WithValidId_ReturnsImage()
    {
        // Arrange
        var id = Guid.NewGuid();
        var filePath = "test.jpg";

        _imageServiceMock
            .Setup(x => x.GetImageAsync(id))
            .ReturnsAsync((true, filePath, null));

        // Act
        var result = await _imageServiceMock.Object.GetImageAsync(id);

        // Assert
        Assert.True(result.found);
        Assert.Equal(filePath, result.filePath);
    }
}
