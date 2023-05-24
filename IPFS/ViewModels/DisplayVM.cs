using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IPFS.ViewModels
{
    public class DisplayVM : ObservableObject, IRecipient<RequestMessage<Album>>
    {
        #region 绑定的属性
        private Album? _selectedItem;
        public Album? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<Album> Albums { get; } = new ObservableCollection<Album>();
        public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }
        #endregion

        #region 绑定的命令
        private RelayCommand? _refreshCommand;
        public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(async () =>
        {
            await PageUpdateAsync(_csl.SQLite);
        });

        private RelayCommand<Album>? _copyCommand;
        public RelayCommand<Album> CopyCommand => _copyCommand ??= new RelayCommand<Album>((message) =>
        {
            if (message != null) Clipboard.SetText(message.Name);
        });

        private RelayCommand<Album>? _DeleteCommand;
        public RelayCommand<Album> DeleteCommand => _DeleteCommand ??= new RelayCommand<Album>(async (message) =>
        {
            try
            {
                var userResult = MessageBox.Show("是否删除？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (userResult == MessageBoxResult.No) return;
                if (message != null)
                {
                    foreach (var item in message.FilesData!.Values)
                    {
                        PinFile? pinFile = await _csl.IPFSApi.RemovePinAsync(item.Cid);
                    }
                    Albums.Remove(message);
                    await _csl.SQLite.SQLConnection.DeleteAsync(message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"删除失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private RelayCommand? _downloadCommand;
        public RelayCommand DownloadCommand => _downloadCommand ??= new RelayCommand(async () =>
        {
            try
            {
                if (SelectedItem == null) return;
                var targetAlbum = SelectedItem;
                targetAlbum.Status = "下载中……";
                foreach (var item in targetAlbum.FilesData!.Values)
                    await _csl.IPFSApi.DownloadFileAsync(item, $"{_csl.Config.Load("DownloadPath")}\\{targetAlbum.Name}");
                targetAlbum.Status = "下载完成";
            }
            catch (Exception e)
            {
                MessageBox.Show($"下载出错。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private RelayCommand? _itemPaddingCommand;
        public RelayCommand ItemPaddingCommand => _itemPaddingCommand ??= new RelayCommand(() =>
        {

        });

        public void ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectedItem == null) return; SelectedItem.Page = "DisplayPage.xaml";
                if (e.LeftButton == MouseButtonState.Pressed)
                    NavigationService.Navigation("DetailPage.xaml", "DetailVM", SelectedItem);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 组件
        private readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;
        #endregion

        public DisplayVM()
        {
            _csl.SQLite.InitializeTableAsync<Album?>();
            PageMessengerInitialize();
        }

        public void Receive(RequestMessage<Album> message)
        {
            if (SelectedItem != null) message.Reply(SelectedItem);
        }

        private async void MessageUpdateAsync(object recipient, Album message)
        {
            await PageUpdateAsync(_csl.SQLite);
        }

        private async Task PageUpdateAsync(SQLiteService sqlite)
        {
            try
            {
                Albums.Clear();
                List<Album>? AlbumsSource = await sqlite.SQLConnection.Table<Album>().ToListAsync();
                if (AlbumsSource != null)
                    foreach (var animation in AlbumsSource)
                    {
                        Stream stream = await _csl.IPFSApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", animation.CoverHash));
                        //加载图片
                        animation.GetImage(stream);
                        //读出的json数据解析
                        animation.GetFilesData();

                        Albums.Add(animation);
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show($"加载失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void PageMessengerInitialize()
        {
            await PageUpdateAsync(_csl.SQLite);
            //初始化详情页面时，监听数据请求
            WeakReferenceMessenger.Default.Register(this, "InitializeDetailVM");
            //上传数据更改时，刷新界面
            WeakReferenceMessenger.Default.Register<Album, string>(this, "DisplayVM", MessageUpdateAsync);
        }

    }
}
