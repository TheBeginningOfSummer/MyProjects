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

namespace IPFS.ViewModels
{
    public class UploadVM : ObservableObject
    {
        private string? _pageName;
        public string? PageName
        {
            get => _pageName;
            set => SetProperty(ref _pageName, value);
        }

        #region 组件
        private readonly VideoAlbum _albumInfo = new();
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
                    //PB_AlbumCover.Image = Image.FromStream(stream);
                    //ShowMessage("上传完成");
                    if (result != null)
                    {
                        _albumInfo.VideosJson = "";
                        _albumInfo.CoverHash = result.Cid;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        });

        private RelayCommand? _uploadFileCommand;
        public RelayCommand UploadFileCommand => _uploadFileCommand ??= new RelayCommand(async () =>
        {
            try
            {
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
                    Animation animation = new(_albumInfo, _fileDic);
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
