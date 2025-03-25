using ImageServer.Api.Services;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Fixtures;
using ImageServer.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ImageServer.Api.DTOs.Images.Requests;
using ImageServer.Api.Data.UnitOfWork;
using ImageServer.Api.Data.Repositories;

namespace ImageServer.Tests.Services;

public class ImageServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IFileSettings> _fileSettingsMock;
    private readonly Mock<IImageProcessor> _imageProcessorMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<ILogger<ImageService>> _loggerMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly ImageService _imageService;

    public ImageServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _fileServiceMock = new Mock<IFileService>();
        _fileSettingsMock = new Mock<IFileSettings>();
        _imageProcessorMock = new Mock<IImageProcessor>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<ImageService>>();
        _imageRepositoryMock = new Mock<IImageRepository>();

        _unitOfWorkMock.Setup(x => x.Images).Returns(_imageRepositoryMock.Object);

        var persistenceService = new ImagePersistenceService(_unitOfWorkMock.Object);

        _imageService = new ImageService(
            persistenceService,
            _fileServiceMock.Object,
            _fileSettingsMock.Object,
            _imageProcessorMock.Object,
            _httpContextAccessorMock.Object,
            _loggerMock.Object
        );

        SetupHttpContext();
    }

    [Fact]
    public async Task UploadImageAsync_ValidImage_ReturnsSuccessResult()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        var dto = new ImageUploadDto { Name = "test", File = file };
        var filePath = "test/path/image.jpg";
        var thumbnailPath = "test/path/thumb_image.jpg";

        _fileServiceMock.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
            .ReturnsAsync((true, null));
        _fileServiceMock.Setup(x => x.GetFilePath(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(filePath);
        _fileServiceMock.Setup(x => x.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _imageRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask)
            .Callback<Image>(img =>
            {
                img.FilePath = filePath;
                img.ThumbnailPath = thumbnailPath;
            });

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _imageProcessorMock.Setup(x => x.CreateThumbnailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()))
            .ReturnsAsync(thumbnailPath);

        _fileSettingsMock.Setup(x => x.UploadPath)
            .Returns("uploads");

        SetupHttpContext();

        // Act
        var result = await _imageService.UploadImageAsync(dto);

        // Assert
        Assert.True(result.success);
        _imageRepositoryMock.Verify(x => x.AddAsync(It.Is<Image>(i =>
            i.Name == dto.Name &&
            i.ContentType == file.ContentType &&
            i.FileSize == file.Length)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _fileServiceMock.Verify(x => x.SaveFileAsync(file, filePath), Times.Once);
        _imageProcessorMock.Verify(x => x.CreateThumbnailAsync(
            filePath,
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UploadImageAsync_InvalidFile_ReturnsFalse()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        var dto = new ImageUploadDto { Name = "test", File = file };

        _fileServiceMock.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
            .ReturnsAsync((false, "Invalid file"));

        // Act
        var result = await _imageService.UploadImageAsync(dto);

        // Assert
        Assert.False(result.success);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    private void SetupHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost:7000");

        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(context);
    }

    [Fact]
    public async Task GetImagesAsync_ReturnsAllImages()
    {
        // Arrange
        var images = new List<Image> { TestImageFixture.CreateTestImage() };
        _imageRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(images);

        SetupHttpContext();

        // Act
        var result = await _imageService.GetImagesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _imageRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteImageAsync_ExistingImage_ReturnsSuccess()
    {
        // Arrange
        var image = TestImageFixture.CreateTestImage();
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(image.Id))
            .ReturnsAsync(image);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _fileServiceMock.Setup(x => x.DeleteFile(It.IsAny<string>()))
            .Verifiable();

        // Act
        var result = await _imageService.DeleteImageAsync(image.Id);

        // Assert
        Assert.True(result.success);
        _imageRepositoryMock.Verify(x => x.Remove(image), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _fileServiceMock.Verify(x => x.DeleteFile(image.FilePath), Times.Once);
        if (!string.IsNullOrEmpty(image.ThumbnailPath))
        {
            _fileServiceMock.Verify(x => x.DeleteFile(image.ThumbnailPath), Times.Once);
        }
    }

    [Fact]
    public async Task DeleteImageAsync_NonExistingImage_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Image?)null);

        // Act
        var result = await _imageService.DeleteImageAsync(id);

        // Assert
        Assert.False(result.success);
        _imageRepositoryMock.Verify(x => x.Remove(It.IsAny<Image>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
