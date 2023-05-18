using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Services;
using Microsoft.Win32;
using MyToolkit;
using System;
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

    private string? _browserPath;
    public string? BrowserPath
    {
        get => _browserPath;
        set => SetProperty(ref _browserPath, value);
    }

    private RelayCommand? _saveCommand;
    public RelayCommand SaveCommand => _saveCommand ??= new RelayCommand(() =>
    {
        try
        {
            _config.Change("DownloadPath", string.IsNullOrEmpty(DownloadPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : DownloadPath);
            _config.Change("BrowserPath", string.IsNullOrEmpty(BrowserPath) ? "" : BrowserPath);
            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (System.Exception)
        {

        }
    });

    private RelayCommand<string>? _getFolderPathCommand;
    public RelayCommand<string> GetFolderPathCommand => _getFolderPathCommand ??= new RelayCommand<string>((message) =>
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

    private RelayCommand<string>? _getFilePathCommand;
    public RelayCommand<string> GetFilePathCommand => _getFilePathCommand ??= new RelayCommand<string>((message) =>
    {
        try
        {
            if (_openFileDialog.ShowDialog() == true)
                BrowserPath = _openFileDialog.FileName;
        }
        catch (System.Exception)
        {

        }
    });

    readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
    readonly OpenFileDialog _openFileDialog = new();
    readonly KeyValueLoader _config = CommonServiceLoader.Instance.Config;

    public SetVM()
    {
        DownloadPath = _config.Load("DownloadPath");
        BrowserPath = _config.Load("BrowserPath");
    }
}
