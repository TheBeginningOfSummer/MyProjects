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

namespace IPFS.ViewModels;

public class RemoteVM : ObservableObject
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

    #region 组件
    private CommonServiceLoader _csl = CommonServiceLoader.Instance;
    private SQLiteService _ipnsSqlite = new();
    #endregion

    public RemoteVM()
    {
        foreach (var item in _csl.RemoteIPNS.KeyValueList)
            IPNS.Add($"{item.Key}:{item.Value}");
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

    private async Task<string> GetIPNSData()
    {
        if (string.IsNullOrEmpty(IPNSText)) return"";
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
}
