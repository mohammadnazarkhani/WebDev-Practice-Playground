using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ImageServer.Api.Swagger;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var uploadAttributes = context.MethodInfo?.GetCustomAttributes(true)
            .OfType<ConsumesAttribute>()
            .SelectMany(attr => attr.ContentTypes)
            .Where(t => t.Equals("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            .Any();

        if (uploadAttributes ?? false)
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["name"] = new OpenApiSchema { Type = "string" },
                                ["file"] = new OpenApiSchema { Type = "string", Format = "binary" }
                            },
                            Required = new HashSet<string> { "name", "file" }
                        }
                    }
                }
            };
        }
    }
}
