using ImageServer.Api.Services.Interfaces;

namespace ImageServer.Api.Middleware;

public class FileValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<FileValidationMiddleware> _logger;

    public FileValidationMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<FileValidationMiddleware> logger)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (!context.Request.HasFormContentType || context.Request.Form.Files.Count == 0)
            {
                await _next(context);
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var validationService = scope.ServiceProvider.GetRequiredService<IFileValidationService>();

            var (isValid, errors) = validationService.ValidateFiles(context.Request.Form.Files);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { errors });
                return;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in file validation middleware");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "An error occurred while processing the request" });
        }
    }
}
