using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace IPFS.ViewModels
{
    public class MainWindowVM : ObservableObject, IRecipient<string>
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

        public MainWindowVM()
        {
            //监听页面切换消息
            WeakReferenceMessenger.Default.Register(this);
            LoadPageCommand = new RelayCommand<object?>(ChangePage);
            MinimizeCommand = new RelayCommand<object?>(MinimizeWindow);
            MaximizeCommand = new RelayCommand<object?>(MaximizeWindow);
            CloseCommand = new RelayCommand<object?>(CloseWindow);
            ChangePage("DisplayPage.xaml");
        }

        public void Receive(string message)
        {
            if (!string.IsNullOrEmpty(message)) PageName = message;
        }

        public void ChangePage(object? page)
        {
            if (page is string pageName) PageName = pageName;
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
