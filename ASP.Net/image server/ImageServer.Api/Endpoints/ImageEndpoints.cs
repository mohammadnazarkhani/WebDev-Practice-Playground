using ImageServer.Api.Endpoints.Images;

namespace ImageServer.Api.Endpoints;

/// <summary>
/// Extension methods for configuring image-related endpoints
/// </summary>
public static class ImageEndpoints
{
    /// <summary>
    /// Maps all image-related endpoints to the application
    /// </summary>
    /// <param name="app">The endpoint route builder</param>
    /// <returns>The endpoint route builder for chaining</returns>
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapImageUploadEndpoints()
           .MapImageQueryEndpoints()
           .MapImageManagementEndpoints();
    }
}
