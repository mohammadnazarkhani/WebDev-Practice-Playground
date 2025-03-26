using ImageServer.Api.DTOs;
using ImageServer.Api.DTOs.Images.Responses;
using ImageServer.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace ImageServer.Api.Endpoints.Images;

public static class ImageQueryEndpoints
{
    private static readonly List<OpenApiTag> ImageTags = new()
    {
        new OpenApiTag { Name = "Images" }
    };

    public static IEndpointRouteBuilder MapImageQueryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/images", async (IImageService imageService) =>
            Results.Ok(await imageService.GetImagesAsync()))
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all images";
            operation.Description = "Retrieve a list of all uploaded images with their details";
            operation.Tags = ImageTags;
            return operation;
        })
        .Produces<List<ImageResponseDto>>();

        app.MapGet("/images/{id}", async (string id, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (found, filePath, error) = await imageService.GetImageAsync(guidId);
            if (!found) return Results.NotFound(error);

            var contentType = Path.GetExtension(filePath!).ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return Results.File(filePath!, contentType, enableRangeProcessing: true);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get image by ID";
            operation.Description = "Retrieve a specific image file by its ID";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image";
            return operation;
        })
        .Produces(200)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .DisableAntiforgery()
        .CacheOutput(x => x.Expire(TimeSpan.FromHours(1)));


        app.MapGet("/images/{id}/details", async (string id, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (found, details, error) = await imageService.GetImageDetailsAsync(guidId);
            if (!found) return Results.NotFound(new { error });

            return Results.Ok(details);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get detailed image information";
            operation.Description = "Retrieve detailed information about a specific image by its ID, including file paths and metadata";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image";
            return operation;
        })
        .Produces<ImageDetailsResponseDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithName("GetImageDetails");

        app.MapGet("/images/{id}/thumbnail", async (string id, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (found, filePath, error) = await imageService.GetThumbnailAsync(guidId);
            if (!found) return Results.NotFound(error);

            var contentType = Path.GetExtension(filePath!).ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return Results.File(filePath!, contentType);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get image thumbnail";
            operation.Description = "Retrieve a thumbnail version of an image by its ID";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image";
            return operation;
        })
        .Produces(200)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .DisableAntiforgery()
        .CacheOutput(x => x.Expire(TimeSpan.FromDays(30)));

        app.MapGet("/thumbnails", async (
            IImageService imageService,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20) =>
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var images = await imageService.GetImagesAsync();
            var pagedImages = images
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Results.Ok(new
            {
                page,
                pageSize,
                total = images.Count,
                items = pagedImages
            });
        })
        .WithName("GetThumbnails")
        .WithDescription("Get paginated list of images with their thumbnails");

        return app;
    }
}
