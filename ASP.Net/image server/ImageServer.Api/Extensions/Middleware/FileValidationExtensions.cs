using ImageServer.Api.Middleware;

namespace ImageServer.Api.Extensions.Middleware;

/// <summary>
/// Extension methods for configuring file validation middleware
/// </summary>
public static class FileValidationExtensions
{
    /// <summary>
    /// Adds file validation middleware to the application's request pipeline
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseFileValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FileValidationMiddleware>();
    }
}
