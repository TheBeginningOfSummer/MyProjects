using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IPFS.Models;
using IPFS.Services;
using Microsoft.Win32;
using MyToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IPFS.ViewModels;

public class UploadVM : ObservableObject
{
    #region 绑定的属性
    private BitmapImage? _coverImage;
    public BitmapImage? CoverImage
    {
        get => _coverImage;
        set => SetProperty(ref _coverImage, value);
    }

    private string? _albumName;
    public string? AlbumName
    {
        get => _albumName;
        set => SetProperty(ref _albumName, value);
    }

    private string? _year;
    public string? Year
    {
        get => _year;
        set => SetProperty(ref _year, value);
    }

    private string? _month;
    public string? Month
    {
        get => _month;
        set => SetProperty(ref _month, value);
    }

    private string? _day;
    public string? Day
    {
        get => _day;
        set => SetProperty(ref _day, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string? _uploadStatus;
    public string? UploadStatus
    {
        get => _uploadStatus;
        set => SetProperty(ref _uploadStatus, value);
    }

    #endregion

    #region 绑定的命令
    private RelayCommand? _uploadFileInfoCommand;
    public RelayCommand UploadFileInfoCommand => _uploadFileInfoCommand ??= new RelayCommand(async () =>
    {
        try
        {
            _openFileDialog.Filter = "图片|*.png|图片|*.jpg";
            if (_openFileDialog.ShowDialog() == true)
            {
                FileData? result = await _csl.LoadAndUploadFileAsync(_openFileDialog.FileName);
                CoverImage = new BitmapImage(new Uri(_openFileDialog.FileName));
                if (result != null) _album.CoverHash = result.Cid;
            }
        }
        catch (Exception)
        {

        }
    });

    private RelayCommand? _uploadFileCommand;
    public RelayCommand UploadFileCommand => _uploadFileCommand ??= new RelayCommand(async () =>
    {
        try
        {
            if (string.IsNullOrEmpty(AlbumName)) return;
            InitializeAlbum();
            _openFileDialog.Title = "上传文件";
            _openFileDialog.Filter = "mp4视频|*.mp4|其他文件|*.*";
            if (_openFileDialog.ShowDialog() == true)
            {
                UploadStatus = "上传中……";
                foreach (string? path in _openFileDialog.FileNames)
                {
                    FileData? result = await _csl.LoadAndUploadFileAsync(path);
                    AddAlbumData(result);
                }
                //数据存储表
                Animation animation = new(_album, _fileDic);
                await _csl.PublishDatabaseAsync(animation);
                //展示页面数据刷新
                WeakReferenceMessenger.Default.Send(animation, "DisplayVM");
            }
            UploadStatus = "上传完成";
        }
        catch (Exception)
        {

        }
    });
    #endregion

    #region 组件
    private readonly VideoAlbum _album = new();//上传专辑信息
    private readonly Dictionary<string, FileData> _fileDic = new();//专辑数据信息
    private readonly OpenFileDialog _openFileDialog = new();
    private readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;
    #endregion

    public UploadVM()
    {

    }

    private void InitializeAlbum()
    {
        string albumInfo = "";
        if (!(string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Day) && string.IsNullOrEmpty(Description)))
            albumInfo = $"{Year}-{Month}-{Day}{Environment.NewLine}{Description}";
        _album.Name = AlbumName;
        _album.Information = albumInfo;
        _fileDic.Clear();
    }

    private void AddAlbumData(FileData? result)
    {
        if (result != null)
        {
            if (!_fileDic.ContainsKey(result.Name!))
                _fileDic.Add(result.Name!, result);
        }
    }

}
