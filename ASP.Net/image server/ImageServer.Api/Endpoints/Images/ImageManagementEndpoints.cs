using ImageServer.Api.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace ImageServer.Api.Endpoints.Images;

public static class ImageManagementEndpoints
{
    private static readonly List<OpenApiTag> ImageTags = new()
    {
        new OpenApiTag { Name = "Images" }
    };

    public static IEndpointRouteBuilder MapImageManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/images/{id}", async (string id, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (success, message) = await imageService.DeleteImageAsync(guidId);
            return success ? Results.Ok(new { message }) : Results.NotFound(new { error = message });
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete image";
            operation.Description = "Delete an image by its ID";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image to delete";
            return operation;
        })
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .DisableAntiforgery();

        app.MapDelete("/images/all", async (IImageService imageService) =>
        {
            var (success, message) = await imageService.DeleteAllImagesAsync();
            return success ? Results.Ok(new { message }) : Results.BadRequest(new { error = message });
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete all images";
            operation.Description = "Delete all stored images from the server";
            operation.Tags = ImageTags;
            return operation;
        })
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .DisableAntiforgery();

        return app;
    }
}
