using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ImageServer.WPFClient.Models;

namespace ImageServer.WPFClient.Services;

public class ImageService : IImageService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5000";

    public ImageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task<List<ImageModel>> GetImagesAsync()
    {
        var response = await _httpClient.GetAsync("/images");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ImageModel>>() ?? new();
    }

    public async Task<ImageModel> UploadImageAsync(string name, Stream imageStream, string fileName)
    {
        using var content = new MultipartFormDataContent();

        // Create file content and set content type based on file extension
        using var fileContent = new StreamContent(imageStream);
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        // Add the file and name to the form data
        content.Add(fileContent, "file", Path.GetFileName(fileName));
        content.Add(new StringContent(name), "name");

        try
        {
            var response = await _httpClient.PostAsync("/upload", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed: {error}");
            }

            return await response.Content.ReadFromJsonAsync<ImageModel>()
                ?? throw new Exception("Failed to parse response");
        }
        catch (Exception ex)
        {
            throw new Exception($"Upload failed: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteImageAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/images/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAllImagesAsync()
    {
        var response = await _httpClient.DeleteAsync("/images/all");
        return response.IsSuccessStatusCode;
    }

    public async Task<ImageModel> UpdateImageAsync(Guid id, string name, Stream? imageStream = null)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(name), "name");

        if (imageStream != null)
        {
            using var streamContent = new StreamContent(imageStream);
            content.Add(streamContent, "file", "image.jpg");
        }

        var response = await _httpClient.PutAsync($"/images/{id}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ImageModel>() ?? throw new Exception("Failed to parse response");
    }
}
