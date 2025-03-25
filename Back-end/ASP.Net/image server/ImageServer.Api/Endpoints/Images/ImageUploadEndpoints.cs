using ImageServer.Api.DTOs;
using ImageServer.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ImageServer.Api.DTOs.Images.Requests;
using ImageServer.Api.DTOs.Images.Responses;

namespace ImageServer.Api.Endpoints.Images;

public static class ImageUploadEndpoints
{
    private static readonly List<OpenApiTag> ImageTags = new()
    {
        new OpenApiTag { Name = "Images" }
    };

    public static IEndpointRouteBuilder MapImageUploadEndpoints(this IEndpointRouteBuilder app)
    {
        var uploadGroup = app.MapGroup("/").DisableAntiforgery();

        uploadGroup.MapPost("/upload", async ([FromForm] ImageUploadDto uploadDto, IImageService imageService) =>
        {
            if (uploadDto.File == null)
                return Results.BadRequest(new { error = "No file uploaded" });

            var (success, result) = await imageService.UploadImageAsync(uploadDto);
            return success ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Upload a new image";
            operation.Description = "Upload a new image with a name and file";
            operation.Tags = ImageTags;
            operation.Responses[StatusCodes.Status200OK.ToString()].Description = "Image uploaded successfully";
            operation.Responses[StatusCodes.Status400BadRequest.ToString()].Description = "Invalid file or request";
            return operation;
        })
        .Accepts<ImageUploadDto>("multipart/form-data")
        .Produces<ImageResponseDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        uploadGroup.MapPut("/images/{id}", async ([FromRoute] string id, [FromForm] ImageFileUpdateDto updateDto, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (success, result) = await imageService.UpdateImageAsync(guidId, updateDto.Name, updateDto.File);
            return success ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Update image";
            operation.Description = "Update an existing image. Either name or file must be provided.";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image to update";
            return operation;
        })
        .Accepts<ImageFileUpdateDto>("multipart/form-data")
        .Produces<ImageResponseDto>()
        .ProducesProblem(StatusCodes.Status400BadRequest);

        uploadGroup.MapPatch("/images/{id}", async ([FromRoute] string id, [FromForm] ImageFileUpdateDto updateDto, IImageService imageService) =>
        {
            if (!Guid.TryParseExact(id, "D", out Guid guidId))
                return Results.BadRequest(new { error = "Invalid GUID format" });

            var (success, result) = await imageService.PatchImageAsync(guidId, updateDto.Name, updateDto.File);
            return success ? Results.Ok(result) : Results.NotFound(result);
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Patch image";
            operation.Description = "Partially update an existing image";
            operation.Tags = ImageTags;
            operation.Parameters[0].Description = "The ID of the image to patch";
            return operation;
        })
        .Accepts<ImageFileUpdateDto>("multipart/form-data")
        .Produces<ImageResponseDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
