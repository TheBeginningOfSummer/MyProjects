using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using IPFS.Models;
using IPFS.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace IPFS.ViewModels;

public class DisplayVM : BaseVM, IRecipient<RequestMessage<Album>>
{
    #region 绑定的属性
    private Album? _selectedAlbum;
    public Album? SelectedAlbum
    {
        get => _selectedAlbum;
        set => SetProperty(ref _selectedAlbum, value);
    }

    public ObservableCollection<Album> Albums { get; } = new ObservableCollection<Album>();
    public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }
    #endregion

    #region 绑定的命令
    private RelayCommand? _refreshCommand;
    public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(async () =>
    {
        await UpdateAlbumsAsync(Albums, LocalDatabase);
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
                    PinFile? pinFile = await HttpClientAPI.RemovePinAsync(item.Cid);
                }
                Albums.Remove(message);
                await CSL.Databases["Local"].SQLConnection.DeleteAsync(message);
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
            if (SelectedAlbum == null) return;
            var targetAlbum = SelectedAlbum;
            targetAlbum.Status = "下载中……";
            foreach (var item in targetAlbum.FilesData!.Values)
                await HttpClientAPI.DownloadFileAsync(item, $"{CSL.Configs["Config"].Load("DownloadPath")}\\{targetAlbum.Name}");
            targetAlbum.Status = "下载完成";
        }
        catch (Exception e)
        {
            MessageBox.Show($"下载出错。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    });

    public void ItemDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (SelectedAlbum == null) return; SelectedAlbum.Page = "DisplayPage.xaml";
            if (e.LeftButton == MouseButtonState.Pressed)
                NavigationService.Navigation("DetailPage.xaml", "DetailVM", SelectedAlbum);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    public DisplayVM()
    {
        Initialize();
    }

    public async void Initialize()
    {
        //监听数据请求
        WeakReferenceMessenger.Default.Register(this, "InitializeDetailVM");
        //上传数据更改时，刷新界面
        WeakReferenceMessenger.Default.Register<Album, string>(this, "DisplayVM", PageUpdate);
        //刷新数据列表
        await UpdateAlbumsAsync(Albums, LocalDatabase);
    }

    public void Receive(RequestMessage<Album> message)
    {
        if (SelectedAlbum != null) message.Reply(SelectedAlbum);
    }

    private async void PageUpdate(object recipient, Album message)
    {
        await UpdateAlbumsAsync(Albums, LocalDatabase);
    }
}
