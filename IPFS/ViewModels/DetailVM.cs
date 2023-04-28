using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace IPFS.ViewModels
{
    public class DetailVM : ObservableObject
    {
        #region 绑定的值
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
        #endregion

        public DetailVM()
        {
            //请求初始数据
            var result = WeakReferenceMessenger.Default.Send(new RequestMessage<Animation>());
            AlbumInfo = result.Response;
            UpdateFileList(AlbumInfo);
            //注册数据接收
            WeakReferenceMessenger.Default.Register<Animation>(this, LoadInfo);
        }

        private void UpdateFileList(Animation? AlbumInfo)
        {
            if (AlbumInfo == null) return;
            FileListInfo.Clear();
            foreach (var item in AlbumInfo!.VideosData!.Values)
                FileListInfo.Add(item);
        }

        private void LoadInfo(object recipient, Animation message)
        {
            if (message != null) AlbumInfo = message;
            UpdateFileList(AlbumInfo);
        }

    }
}
