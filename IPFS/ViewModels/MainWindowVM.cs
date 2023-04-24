using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Services;
using System.Windows;

namespace IPFS.ViewModels
{
    public class MainWindowVM : ObservableObject
    {
        private string? _pageName;
        public string? PageName
        {
            get => _pageName;
            set => SetProperty(ref _pageName, value);
        }

        public RelayCommand<object?> LoadPageCommand { get; }
        public RelayCommand<object?> MinimizeCommand { get; }
        public RelayCommand<object?> MaximizeCommand { get; }
        public RelayCommand<object?> CloseCommand { get; }

        private readonly SQLiteService _sqlite = new();
        private readonly HttpClientAPI _ipfsApi = new();

        public MainWindowVM()
        {
            LoadPageCommand = new RelayCommand<object?>(GetPageName);
            MinimizeCommand = new RelayCommand<object?>(MinimizeWindow);
            MaximizeCommand = new RelayCommand<object?>(MaximizeWindow);
            CloseCommand = new RelayCommand<object?>(CloseWindow);
            GetPageName("DisplayPage.xaml");
        }

        public void GetPageName(object? page)
        {
            if (page != null) PageName = page.ToString();
        }

        public static void MinimizeWindow(object? window)
        {
            SystemCommands.MinimizeWindow(window as Window);
        }

        public static void MaximizeWindow(object? window)
        {
            if (window is not Window mainWindow) return;
            switch (mainWindow.WindowState)
            {
                case WindowState.Normal:
                    SystemCommands.MaximizeWindow(mainWindow);
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    SystemCommands.RestoreWindow(mainWindow);
                    break;
                default:
                    break;
            }
        }

        public static void CloseWindow(object? window)
        {
            SystemCommands.CloseWindow(window as Window);
        }
    }
}
