using CommunityToolkit.Mvvm.Input;
using IPFS.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IPFS.ViewModels;

public class SetVM : BaseVM
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

    private KeyValuePair<string,string> _selectedIPNS;
    public KeyValuePair<string,string> SelectedIPNS
    {
        get => _selectedIPNS;
        set => SetProperty(ref _selectedIPNS, value);
    }

    public ObservableDictionary<string, string> IPNS { get; } = new();
    
    private RelayCommand? _saveCommand;
    public RelayCommand SaveCommand => _saveCommand ??= new RelayCommand(() =>
    {
        try
        {
            CSL.Configs["Config"].Change("DownloadPath", string.IsNullOrEmpty(DownloadPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : DownloadPath);
            CSL.Configs["Config"].Change("BrowserPath", string.IsNullOrEmpty(BrowserPath) ? "" : BrowserPath);
            CSL.Configs["Config"].Change("IPNSName", string.IsNullOrEmpty(SelectedIPNS.Key) ? "self" : SelectedIPNS.Key);
            CSL.Configs["Config"].Change("IPNSId", string.IsNullOrEmpty(SelectedIPNS.Value) ? IPNS["self"]! : SelectedIPNS.Value);
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

    private RelayCommand? _ipnsCopyCommand;
    public RelayCommand IPNSCopyCommand => _ipnsCopyCommand ??= new RelayCommand(() =>
    {
        Clipboard.SetText($"{SelectedIPNS.Key}:{SelectedIPNS.Value}");
    });

    readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
    readonly OpenFileDialog _openFileDialog = new();

    public SetVM()
    {
        DownloadPath = CSL.Configs["Config"].Load("DownloadPath");
        BrowserPath = CSL.Configs["Config"].Load("BrowserPath");
        SelectedIPNS = new(CSL.Configs["Config"].Load("IPNSName"), CSL.Configs["Config"].Load("IPNSId"));
        try
        {
            var ipnsSource = HttpClientAPI.GetIPNSAsync().Result;
            if (ipnsSource != null)
            {
                foreach (var ipns in ipnsSource)
                    IPNS.Add(ipns.Key, ipns.Value);
            }
        }
        catch (Exception)
        {

        }
    }
}
