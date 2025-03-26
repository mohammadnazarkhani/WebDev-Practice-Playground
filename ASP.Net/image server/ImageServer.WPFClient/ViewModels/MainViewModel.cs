using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageServer.WPFClient.Models;
using ImageServer.WPFClient.Services;
using Microsoft.Win32;

namespace ImageServer.WPFClient.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IImageService _imageService;

    [ObservableProperty]
    private ObservableCollection<ImageModel> _images = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ImageModel? _selectedImage;

    public MainViewModel(IImageService imageService)
    {
        _imageService = imageService;
    }

    [RelayCommand]
    public async Task LoadImages()
    {
        try
        {
            IsLoading = true;
            var images = await _imageService.GetImagesAsync();
            Images = new ObservableCollection<ImageModel>(images);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load images: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task UploadImage()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif",
            Title = "Select an image file"
        };

        if (dialog.ShowDialog() == true)
        {
            IsLoading = true;
            try
            {
                using var stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);
                var name = Path.GetFileNameWithoutExtension(dialog.FileName);
                var result = await _imageService.UploadImageAsync(name, stream, dialog.FileName);
                await LoadImages();
                MessageBox.Show("Image uploaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    [RelayCommand]
    private async Task DeleteImage(ImageModel image)
    {
        if (await _imageService.DeleteImageAsync(image.Id))
        {
            Images.Remove(image);
        }
    }
}
