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
using System.Linq;

namespace IPFS.ViewModels;

public class RemoteVM : ObservableObject
{
    private string? _selectedIPNS;
    public string? SelectedIPNS
    {
        get => _selectedIPNS;
        set => SetProperty(ref _selectedIPNS, value);
    }

    public List<string> IPNS { get; set; }

    public ObservableCollection<Album> Albums { get; } = new ObservableCollection<Album>();
    public ICollectionView AlbumsView { get { return CollectionViewSource.GetDefaultView(Albums); } }

    private RelayCommand? _refreshCommand;
	public RelayCommand RefreshCommand => _refreshCommand ??= new RelayCommand(() =>
	{

	});

    #region 组件
    private readonly CommonServiceLoader _csl = CommonServiceLoader.Instance;
    SQLiteService sqlite = new SQLiteService();
    #endregion

    public RemoteVM()
    {
        IPNS = _csl.LocalIPNSDic.Values.ToList();
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
}
