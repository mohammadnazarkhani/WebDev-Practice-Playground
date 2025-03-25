using ImageServer.Api.Services;
using ImageServer.Api.Services.Interfaces;
using ImageServer.Tests.Fixtures;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ImageServer.Tests.Services;

public class FileServiceTests
{
    private readonly FileService _fileService;
    private readonly Mock<IFileSettings> _fileSettingsMock;
    private readonly Mock<IValidator<IFormFile>> _validatorMock;

    public FileServiceTests()
    {
        _fileSettingsMock = new Mock<IFileSettings>();
        _validatorMock = new Mock<IValidator<IFormFile>>();

        _fileService = new FileService(_fileSettingsMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task ValidateFile_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<IFormFile>(), default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _fileService.ValidateFile(file);

        // Assert
        Assert.True(result.isValid);
        Assert.Null(result.error);
    }

    [Fact]
    public async Task ValidateFile_InvalidFile_ReturnsError()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        var validationFailure = new ValidationFailure("File", "Invalid file");
        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<IFormFile>(), default))
            .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

        // Act
        var result = await _fileService.ValidateFile(file);

        // Assert
        Assert.False(result.isValid);
        Assert.NotNull(result.error);
    }

    [Fact]
    public async Task SaveFileAsync_ValidFile_SavesSuccessfully()
    {
        // Arrange
        var file = TestImageFixture.CreateTestFormFile();
        var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            // Act
            await _fileService.SaveFileAsync(file, tempPath);

            // Assert
            Assert.True(File.Exists(tempPath));
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Fact]
    public void DeleteFile_NonExistentFile_DoesNotThrow()
    {
        // Arrange
        var filePath = "non/existent/path.jpg";

        // Act & Assert
        var exception = Record.Exception(() => _fileService.DeleteFile(filePath));
        Assert.Null(exception);
    }

    [Fact]
    public void GetFilePath_ReturnsCorrectPath()
    {
        // Arrange
        var id = Guid.NewGuid();
        var fileName = "test.jpg";
        _fileSettingsMock.Setup(x => x.UploadPath).Returns("uploads");

        // Act
        var result = _fileService.GetFilePath(id, fileName);

        // Assert
        Assert.Contains(id.ToString(), result);
        Assert.EndsWith(".jpg", result);
    }
}
