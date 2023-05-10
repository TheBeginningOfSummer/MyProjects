using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;

namespace IPFS.ViewModels
{
    public class DetailVM : ObservableObject
    {
        #region 绑定的属性
        private Animation? _albumInfo;
        public Animation? AlbumInfo
        {
            get => _albumInfo;
            set => SetProperty(ref _albumInfo, value);
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
            System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", $"http://localhost:8080/ipfs/{cid}");
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
                if (AlbumInfo!.VideosData != null && message != null)
                {
                    string result = await _csl.IPFSApi.DoCommandAsync(HttpClientAPI.BuildCommand("pin/rm", message.Cid));
                    PinFile pinFile = JsonSerializer.Deserialize<PinFile>(result)!;

                    AlbumInfo.VideosData.Remove(message.Name!);
                    AlbumInfo.GetVideosDataJson();
                    await AlbumInfo.DatabaseUpdateAsync(_csl.SQLite);
                    FileListInfo.Remove(message);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show($"删除失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void PageUpdate(Animation? albumInfo)
        {
            if (albumInfo == null) return; FileListInfo.Clear();
            foreach (var item in albumInfo!.VideosData!.Values)
                FileListInfo.Add(item);
        }

        private void MessageUpdate(object recipient, Animation message)
        {
            if (message != null) AlbumInfo = message;
            PageUpdate(AlbumInfo);
        }

        private void PageMessengerInitialize()
        {
            //请求初始数据
            var result = WeakReferenceMessenger.Default.Send(new RequestMessage<Animation>(), "InitializeDetailVM");
            AlbumInfo = result.Response; PageUpdate(AlbumInfo);
            //注册数据接收
            WeakReferenceMessenger.Default.Register<Animation, string>(this, "DetailVM", MessageUpdate);
        }

    }
}
