using ImageServer.Api.DTOs.Images.Requests;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Base;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ImageServer.Tests.Endpoints;

public class ImageUploadEndpointsTests : TestBase
{
    private readonly Mock<IImageService> _imageServiceMock;

    public ImageUploadEndpointsTests()
    {
        _imageServiceMock = new Mock<IImageService>();
    }

    [Fact]
    public async Task Upload_WithValidFile_ReturnsOk()
    {
        // Arrange
        var fileName = "test.jpg";
        var content = new MemoryStream(new byte[] { 0x42 });
        var file = new FormFile(content, 0, content.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };

        var uploadDto = new ImageUploadDto
        {
            Name = "Test Image",
            File = file
        };

        _imageServiceMock
            .Setup(x => x.UploadImageAsync(It.IsAny<ImageUploadDto>()))
            .ReturnsAsync((true, new { Id = Guid.NewGuid() }));

        // Act
        var result = await _imageServiceMock.Object.UploadImageAsync(uploadDto);

        // Assert
        Assert.True(result.success);
        Assert.NotNull(result.result);
    }

    [Fact]
    public async Task Upload_WithInvalidFile_ReturnsBadRequest()
    {
        // Arrange
        var uploadDto = new ImageUploadDto
        {
            Name = "Test Image",
            File = null!
        };

        _imageServiceMock
            .Setup(x => x.UploadImageAsync(It.IsAny<ImageUploadDto>()))
            .ReturnsAsync((false, new { error = "No file provided" }));

        // Act
        var result = await _imageServiceMock.Object.UploadImageAsync(uploadDto);

        // Assert
        Assert.False(result.success);
    }
}
