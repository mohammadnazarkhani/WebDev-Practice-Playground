using ImageServer.WPFClient.ViewModels;
using System.Windows;

namespace ImageServer.WPFClient;

public partial class MainWindow
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += async (s, e) => await viewModel.LoadImages();
    }

    private void MenuItem_Exit(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
