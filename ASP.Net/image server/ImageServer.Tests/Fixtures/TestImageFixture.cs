using ImageServer.Api.Models;
using Microsoft.AspNetCore.Http;

namespace ImageServer.Tests.Fixtures;

public class TestImageFixture
{
    public static Image CreateTestImage(bool withThumbnail = false)
    {
        return new Image
        {
            Id = Guid.NewGuid(),
            Name = "test-image",
            FilePath = "test/path/image.jpg",
            ThumbnailPath = withThumbnail ? "test/path/thumb_image.jpg" : null,
            ContentType = "image/jpeg",
            FileSize = 1024,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static IFormFile CreateTestFormFile(string filename = "test.jpg", string contentType = "image/jpeg", long length = 1024)
    {
        var stream = new MemoryStream(new byte[length]);
        return new FormFile(stream, 0, length, "file", filename)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}
