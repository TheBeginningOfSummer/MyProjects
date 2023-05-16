using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyToolkit;
using System.Windows;

namespace IPFS.ViewModels;

public class SetVM : ObservableObject
{
    private string? _downloadPath;
    public string? DownloadPath
    {
        get => _downloadPath;
        set => SetProperty(ref _downloadPath, value);
    }

    readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
    readonly KeyValueLoader _config = new("Configuration.json", "Config");

    private RelayCommand? _saveCommand;
    public RelayCommand SaveCommand => _saveCommand ??= new RelayCommand(() =>
    {
        try
        {
            _config.Change("DownloadPath", string.IsNullOrEmpty(DownloadPath) ? "" : DownloadPath);
            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (System.Exception)
        {

        }
    });

    private RelayCommand? _getPathCommand;
    public RelayCommand GetPathCommand => _getPathCommand ??= new RelayCommand(() =>
    {
        try
        {
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                DownloadPath = _folderBrowserDialog.SelectedPath;
        }
        catch (System.Exception)
        {

        }
    });

    public SetVM()
    {
        DownloadPath = _config.Load("DownloadPath");
    }
}
