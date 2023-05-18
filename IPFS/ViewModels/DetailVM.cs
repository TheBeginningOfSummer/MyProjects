using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace IPFS.ViewModels
{
    public class DetailVM : ObservableObject
    {
        #region 绑定的属性
        private Album? _albumInfo;
        public Album? AlbumInfo
        {
            get => _albumInfo;
            set => SetProperty(ref _albumInfo, value);
        }

        private FileData? _selectedFileData;
        public FileData? SelectedFileData
        {
            get => _selectedFileData;
            set => SetProperty(ref _selectedFileData, value);
        }

        public ObservableCollection<FileData> FileListInfo { get; } = new ObservableCollection<FileData>();
        #endregion

        #region 绑定的命令
        private RelayCommand? _returnCommand;
        public RelayCommand ReturnCommand => _returnCommand ??= new RelayCommand(() =>
        {
            WeakReferenceMessenger.Default.Send("DisplayPage.xaml");
        });

        private RelayCommand<string>? _openInBrowser;
        public RelayCommand<string> OpenInBrowser => _openInBrowser ??= new RelayCommand<string>((cid) =>
        {
            try
            {
                System.Diagnostics.Process.Start(_csl.Config.Load("BrowserPath"), $"http://localhost:8080/ipfs/{cid}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"打开失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private RelayCommand<FileData>? _copyCommand;
        public RelayCommand<FileData> CopyCommand => _copyCommand ??= new RelayCommand<FileData>((message) =>
        {
            if (message != null) Clipboard.SetText(message.Cid);
        });

        private RelayCommand<FileData>? _deleteCommand;
        public RelayCommand<FileData> DeleteCommand => _deleteCommand ??= new RelayCommand<FileData>(async (message) =>
        {
            try
            {
                var userResult = MessageBox.Show("是否删除？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (userResult == MessageBoxResult.No) return;
                if (AlbumInfo!.FilesData != null && message != null)
                {
                    PinFile? pinFile = await _csl.IPFSApi.RemovePinAsync(message.Cid);

                    AlbumInfo.FilesData.Remove(message.Name!);
                    AlbumInfo.GetFilesDataJson();
                    await AlbumInfo.DatabaseUpdateAsync(_csl.SQLite);
                    FileListInfo.Remove(message);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show($"删除失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private RelayCommand? _downloadCommand;
        public RelayCommand DownloadCommand => _downloadCommand ??= new RelayCommand(async () =>
        {
            if (SelectedFileData == null) return;
            try
            {
                await _csl.IPFSApi.DownloadFileAsync(SelectedFileData, _csl.Config.Load("DownloadPath"));
            }
            catch (Exception e)
            {
                MessageBox.Show($"下载失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
        #endregion

        #region 组件
        private readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;
        #endregion

        public DetailVM()
        {
            PageMessengerInitialize();
        }

        private void MessageUpdate(object recipient, Album message)
        {
            if (message != null) AlbumInfo = message;
            PageUpdate(AlbumInfo);
        }

        private void PageUpdate(Album? albumInfo)
        {
            if (albumInfo == null) return; FileListInfo.Clear();
            foreach (var item in albumInfo!.FilesData!.Values)
                FileListInfo.Add(item);
        }

        private void PageMessengerInitialize()
        {
            //从其他页面请求初始数据
            var result = WeakReferenceMessenger.Default.Send(new RequestMessage<Album>(), "InitializeDetailVM");
            //请求到的数据
            AlbumInfo = result.Response; PageUpdate(AlbumInfo);
            //注册数据接收
            WeakReferenceMessenger.Default.Register<Album, string>(this, "DetailVM", MessageUpdate);
        }


    }
}
