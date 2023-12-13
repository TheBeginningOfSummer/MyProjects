using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IPFS.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IPFS.ViewModels;

public class UploadVM : BaseVM
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
                FileData? result = await CSL.LoadThenUploadFileAsync(_openFileDialog.FileName);
                CoverImage = new BitmapImage(new Uri(_openFileDialog.FileName));
                if (result != null) coverHash = result.Cid!;
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
            await UploadInfo();
        }
        catch (Exception)
        {

        }
    });
    #endregion

    #region 组件
    private readonly OpenFileDialog _openFileDialog = new();
    #endregion

    string coverHash = "";

    public UploadVM()
    {
        _openFileDialog.Multiselect = true;
    }

    private async Task UploadInfo()
    {
        if (string.IsNullOrEmpty(AlbumName)) return;
        if (string.IsNullOrEmpty(Description)) return;
        if (string.IsNullOrEmpty(Year) || string.IsNullOrEmpty(Month) || string.IsNullOrEmpty(Day)) return;
        _openFileDialog.Title = "上传文件";
        _openFileDialog.Filter = "mp4视频|*.mp4|其他文件|*.*";
        AlbumData _album = new(AlbumName, Description, $"{Year}-{Month}-{Day}", coverHash, "");
        Dictionary<string, FileData> _fileDic = new();
        if (_openFileDialog.ShowDialog() == true)
        {
            UploadStatus = "上传中……";
            foreach (string? path in _openFileDialog.FileNames)
            {
                FileData? result = await CSL.LoadThenUploadFileAsync(path);
                if (result == null) continue;
                if (!_fileDic.ContainsKey(result.Name!))
                    _fileDic.Add(result.Name!, result);
            }
            //数据存储表
            Album animation = new(_album, _fileDic);
            //更新数据并上传
            await CSL.PublishIPNSDatabaseAsync(animation, CSL.Configs["Config"].Load("IPNSName"));
            //展示页面数据刷新
            WeakReferenceMessenger.Default.Send(animation, "DisplayVM");
        }
        UploadStatus = "上传完成";
    }


}
