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

public class RemoteVM : BaseVM, IRecipient<RequestMessage<Album>>
{
    #region 属性绑定
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

    private Album? _selectedAlbum;
    public Album? SelectedAlbum
    {
        get => _selectedAlbum;
        set => SetProperty(ref _selectedAlbum, value);
    }

    public ObservableCollection<string> IPNS { get; } = new ObservableCollection<string>();
    public ObservableCollection<Album> Albums { get; } = new ObservableCollection<Album>();
    public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }
    #endregion

    #region 命令绑定
    private RelayCommand? _refreshCommand;
    public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(async () =>
    {
        try
        {
            string name = await CSL.DownloadIPNSDatabaseAsync(IPNSText);
            await _currentIPNSSqlite.SQLConnection.CloseAsync();
            _currentIPNSSqlite = new SQLiteService(name, "IPNSData");
        }
        catch (Exception e)
        {
            MessageBox.Show($"加载失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        await UpdateAlbumsAsync(Albums, _currentIPNSSqlite);
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
                CSL.Configs["RemoteIPNS"].Change(ipnsKey, ipns);
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
                CSL.Configs["RemoteIPNS"].Remove(ipnsKey);
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
            if (SelectedAlbum == null) return; SelectedAlbum.Page = "RemotePage.xaml";
            if (e.LeftButton == MouseButtonState.Pressed)
                NavigationService.Navigation("DetailPage.xaml", "DetailVM", SelectedAlbum);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region 组件
    private SQLiteService _currentIPNSSqlite = new();
    #endregion

    public RemoteVM()
    {
        Initialize();
    }

    public async void Initialize()
    {
        LoadConfig(IPNS);
        //监听数据请求
        WeakReferenceMessenger.Default.Register(this, "InitializeDetailVM");
        //刷新数据列表
        await UpdateAlbumsAsync(Albums, LocalDatabase);
    }

    public void Receive(RequestMessage<Album> message)
    {
        if (SelectedAlbum != null) message.Reply(SelectedAlbum);
    }

}
