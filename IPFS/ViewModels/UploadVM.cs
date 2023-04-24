using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Models;
using IPFS.Services;
using Microsoft.Win32;
using MyToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace IPFS.ViewModels
{
    public class UploadVM : ObservableObject
    {
        #region 属性
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
        #endregion

        #region 组件
        private readonly VideoAlbum _album = new();
        private readonly Dictionary<string, FileData> _fileDic = new();
        private readonly OpenFileDialog _openFileDialog = new();
        private readonly SQLiteService _sqlite = new();
        private readonly HttpClientAPI _ipfsApi = new();
        #endregion

        public UploadVM()
        {

        }

        private RelayCommand? _uploadFileInfoCommand;
        public RelayCommand UploadFileInfoCommand => _uploadFileInfoCommand ??= new RelayCommand(async () =>
        {
            try
            {
                _openFileDialog.Filter = "图片|*.png|图片|*.jpg";
                if (_openFileDialog.ShowDialog() == true)
                {
                    long fileLength = new FileInfo(_openFileDialog.FileName).Length;
                    Stream stream = FileManager.GetFileStream(_openFileDialog.FileName);
                    //ShowMessage("上传中……");
                    FileData? result = await _ipfsApi.AddAsync
                    (stream, _openFileDialog.FileName.Split('\\').LastOrDefault("nofile"), null, fileLength);
                    CoverImage = new BitmapImage(new Uri(_openFileDialog.FileName));
                    //ShowMessage("上传完成");
                    if (result != null)
                    {
                        _album.VideosJson = "";
                        _album.CoverHash = result.Cid;
                    }
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
                if (string.IsNullOrEmpty(Year) || string.IsNullOrEmpty(Month) || string.IsNullOrEmpty(Day) || string.IsNullOrEmpty(Description)) return;
                string albumInfo = $"{Year}-{Month}-{Day}|{Description}";
                _album.Name = AlbumName;
                _album.Information = albumInfo;

                _fileDic.Clear();
                _openFileDialog.Title = "上传文件";
                _openFileDialog.Filter = "mp4视频|*.mp4|其他文件|*.*";
                if (_openFileDialog.ShowDialog() == true)
                {
                    foreach (var path in _openFileDialog.FileNames)
                    {
                        //文件名
                        string fileName = path.Split('\\').LastOrDefault("nofile");
                        //文件长度
                        long fileLength = new FileInfo(path).Length;
                        //ShowMessage($"{fileName}上传中……");
                        var result = await _ipfsApi.AddAsync
                        (FileManager.GetFileStream(path), fileName, null, fileLength);
                        if (result != null)
                        {
                            if (!_fileDic.ContainsKey(fileName))
                                _fileDic.Add(fileName, result);
                        }
                    }
                    //文件上传完成

                    //数据存储表
                    Animation animation = new(_album, _fileDic);
                    //数据插入
                    await _sqlite.SQLConnection.InsertAsync(animation);
                    //上传数据库到IPFS
                    var databaseInfo = await _ipfsApi.AddAsync
                    (FileManager.GetFileStream(_sqlite.DatabasePath), _sqlite.DatabaseName, null, new FileInfo(_sqlite.DatabasePath).Length);
                    //发布到IPNS
                    if (databaseInfo != null)
                    {
                        var resultPub = await _ipfsApi.DoCommandAsync
                        (HttpClientAPI.BuildCommand("name/publish", databaseInfo.Cid, "key=self"));
                    }
                }
            }
            catch (Exception)
            {

            }
        });


    }
}
