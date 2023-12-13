using CommunityToolkit.Mvvm.ComponentModel;
using IPFS.Models;
using IPFS.Services;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IPFS.ViewModels;

public partial class BaseVM : ObservableObject
{
    public readonly CommonServiceLoader CSL = CommonServiceLoader.Instance;
    public readonly SQLiteService LocalDatabase;

    public BaseVM()
    {
        LocalDatabase = CSL.Databases["Local"];
        LocalDatabase.InitializeTableAsync<Album?>();
    }

    public virtual async Task UpdateAlbumsAsync(ObservableCollection<Album> albums, SQLiteService sqlite)
    {
        try
        {
            albums.Clear();
            List<Album>? SourceList = await sqlite.SQLConnection.Table<Album>().ToListAsync();
            if (SourceList == null) return;
            foreach (Album albumInfo in SourceList)
            {
                //加载图片
                await albumInfo.GetImageAsync();
                //增加显示的数据
                albums.Add(albumInfo);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"加载失败。{e.Message}", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadConfig(ObservableCollection<string> list, string key = "RemoteIPNS")
    {
        foreach (var item in CSL.Configs[key].KeyValueList)
            list.Add($"{item.Key}:{item.Value}");
    }

}
