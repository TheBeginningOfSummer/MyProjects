using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPFS.Models;
using IPFS.Services;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace IPFS.ViewModels;

public class RemoteVM : ObservableObject, IRecipient<RequestMessage<Album>>
{
    private string? _selectedIPNS;
    public string? SelectedIPNS
    {
        get => _selectedIPNS;
        set => SetProperty(ref _selectedIPNS, value);
    }

    private string? _ipnsText;
    public string? IPNSText
    {
        get => _ipnsText;
        set => SetProperty(ref _ipnsText, value);
    }

    private Album? _album;
    public Album? MyAlbum
    {
        get => _album;
        set => SetProperty(ref _album, value);
    }

    public ObservableCollection<string> IPNS { get; } = new ObservableCollection<string>();
    public ObservableCollection<Album> Albums { get; } = new ObservableCollection<Album>();
    public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }

    private RelayCommand? _refreshCommand;
	public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(async () =>
	{
        try
        {
            string name = await GetIPNSData();
            await _ipnsSqlite.SQLConnection.CloseAsync();
            _ipnsSqlite = new SQLiteService(name, "IPNSData");
        }
        catch (Exception e)
        {
            MessageBox.Show($"加载失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        await PageUpdateAsync(_ipnsSqlite);
    });

    private RelayCommand? _saveCommand;
    public RelayCommand SaveCommand => _saveCommand ??= new RelayCommand(() =>
    {
        try
        {
            if (string.IsNullOrEmpty(IPNSText)) return;
            if (IPNSText.Contains(':'))
            {
                string ipnsKey = IPNSText.Split(':')[0];
                string ipns = IPNSText.Split(":")[1];
                if (!IPNS.Contains(IPNSText))
                    IPNS.Add(IPNSText);
                _csl.RemoteIPNS.Change(ipnsKey, ipns);
                MessageBox.Show($"保存成功。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"保存失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    });

    private RelayCommand? _deleteCommand;
    public RelayCommand DeleteCommand => _deleteCommand ??= new RelayCommand(() =>
    {
        try
        {
            if (string.IsNullOrEmpty(IPNSText)) return;
            if (IPNSText.Contains(':'))
            {
                string ipnsKey = IPNSText.Split(':')[0];
                string ipns = IPNSText.Split(":")[1];
                if (!IPNS.Contains(IPNSText)) return;
                IPNS.Remove(IPNSText);
                _csl.RemoteIPNS.Remove(ipnsKey);
                MessageBox.Show($"删除成功。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"删除失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    });

    public void ItemDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (MyAlbum == null) return; MyAlbum.Page = "RemotePage.xaml";
            if (e.LeftButton == MouseButtonState.Pressed)
                NavigationService.Navigation("DetailPage.xaml", "DetailVM", MyAlbum);
        }
        catch (Exception)
        {

        }
    }

    #region 组件
    private CommonServiceLoader _csl = CommonServiceLoader.Instance;
    private SQLiteService _ipnsSqlite = new();
    #endregion

    private async Task<string> GetIPNSData()
    {
        if (string.IsNullOrEmpty(IPNSText)) return "";
        if (IPNSText.Contains(':'))
        {
            string ipnsKey = IPNSText.Split(':')[0];
            string ipns = IPNSText.Split(":")[1];
            string cid = await _csl.IPFSApi.ResolveIPNSAsync(ipns);
            await _csl.IPFSApi.DownloadFileAsync(cid, ipnsKey, "IPNSData");
            return ipnsKey;
        }
        return "";
    }

    public RemoteVM()
    {
        foreach (var item in _csl.RemoteIPNS.KeyValueList)
            IPNS.Add($"{item.Key}:{item.Value}");
        PageMessengerInitialize();
    }

    public void Receive(RequestMessage<Album> message)
    {
        if (MyAlbum != null) message.Reply(MyAlbum);
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
        //WeakReferenceMessenger.Default.Register<Album, string>(this, "RemoteVM", MessageUpdateAsync);
    }


}
