using ImageServer.Api.Services;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImageServer.Tests.Services;

public class FileValidationServiceTests
{
    private readonly FileValidationService _validationService;
    private readonly Mock<IFileSettings> _fileSettingsMock;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<ILogger<FileValidationService>> _loggerMock;

    public FileValidationServiceTests()
    {
        _fileSettingsMock = new Mock<IFileSettings>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _loggerMock = new Mock<ILogger<FileValidationService>>();

        _fileSettingsMock.Setup(x => x.MaxFileSize).Returns(1024 * 1024); // 1MB
        _fileSettingsMock.Setup(x => x.AllowedExtensions).Returns(new[] { ".jpg", ".jpeg", ".png" });
        _fileSettingsMock.Setup(x => x.FileType).Returns("image");

        _validationService = new FileValidationService(
            new[] { _fileSettingsMock.Object },
            _memoryCache,
            _loggerMock.Object
        );
    }

    [Fact]
    public void ValidateFiles_ValidImage_ReturnsTrue()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        var files = new FormFileCollection { file };

        // Act
        var result = _validationService.ValidateFiles(files);

        // Assert
        Assert.True(result.isValid);
        Assert.Empty(result.errors);
    }

    [Fact]
    public void ValidateFiles_InvalidExtension_ReturnsFalse()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile("test.txt", "text/plain");
        var files = new FormFileCollection { file };

        // Act
        var result = _validationService.ValidateFiles(files);

        // Assert
        Assert.False(result.isValid);
        Assert.NotEmpty(result.errors);
    }

    [Fact]
    public void IsFileValid_ValidImage_ReturnsTrue()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();

        // Act
        var result = _validationService.IsFileValid(file, _fileSettingsMock.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateFiles_FileSizeExceedsLimit_ReturnsFalse()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile(length: 2 * 1024 * 1024); // 2MB
        var files = new FormFileCollection { file };

        // Act
        var result = _validationService.ValidateFiles(files);

        // Assert
        Assert.False(result.isValid);
        Assert.Contains(result.errors, e => e.Contains("does not meet the configured requirements"));
    }

    [Fact]
    public void IsContentTypeValid_ValidImageType_ReturnsTrue()
    {
        // Arrange
        _fileSettingsMock.Setup(x => x.FileType).Returns("image");

        // Act
        var result = _validationService.IsContentTypeValid("image/jpeg", _fileSettingsMock.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsContentTypeValid_InvalidType_ReturnsFalse()
    {
        // Arrange
        _fileSettingsMock.Setup(x => x.FileType).Returns("image");

        // Act
        var result = _validationService.IsContentTypeValid("application/pdf", _fileSettingsMock.Object);

        // Assert
        Assert.False(result);
    }
}
