using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Models;
using IPFS.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
            _csl.Configs["Config"].Change("DownloadPath", string.IsNullOrEmpty(DownloadPath) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : DownloadPath);
            _csl.Configs["Config"].Change("BrowserPath", string.IsNullOrEmpty(BrowserPath) ? "" : BrowserPath);
            _csl.Configs["Config"].Change("IPNSName", string.IsNullOrEmpty(SelectedIPNS.Key) ? "self" : SelectedIPNS.Key);
            _csl.Configs["Config"].Change("IPNSId", string.IsNullOrEmpty(SelectedIPNS.Value) ? IPNS["self"]! : SelectedIPNS.Value);
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
        //var ipnsSource = _csl.IPFSApi.GetIPNSAsync().Result;
        //if (ipnsSource != null)
        //{
        //    foreach (var ipns in ipnsSource)
        //        IPNS.Add(ipns.Key, ipns.Value);
        //}
    });

    readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
    readonly OpenFileDialog _openFileDialog = new();
    readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;

    public SetVM()
    {
        DownloadPath = _csl.Configs["Config"].Load("DownloadPath");
        BrowserPath = _csl.Configs["Config"].Load("BrowserPath");
        SelectedIPNS = new(_csl.Configs["Config"].Load("IPNSName"), _csl.Configs["Config"].Load("IPNSId"));
        try
        {
            var ipnsSource = _csl.IPFSApi.GetIPNSAsync().Result;
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
