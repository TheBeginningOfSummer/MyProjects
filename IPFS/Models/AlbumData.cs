using IPFS.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IPFS.Models;

[Table("Animation")]
public class AlbumData : ISQLData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string? Name { get; set; }
    public string? Information { get; set; }
    public string? Date { get; set; }
    public string? CoverHash { get; set; }
    private string? filesJson;
    public string? FilesJson
    {
        get
        {
            //更新FilesJson
            if (FilesData == null || FilesData.Count == 0)
                filesJson = "";
            else
                filesJson = JsonSerializer.Serialize(FilesData);
            return filesJson;
        }
        set
        {
            filesJson = value;
            //更新FilesData
            if (string.IsNullOrEmpty(filesJson))
                FilesData = new Dictionary<string, FileData>();
            else
                FilesData = JsonSerializer.Deserialize<Dictionary<string, FileData>>(filesJson!);
        }
    }
    [Ignore]
    public ImageSource? CoverImage { get; set; }//"/Resources/Image/Autumn.jpg"
    [Ignore]
    public Dictionary<string, FileData>? FilesData { get; set; }

    public AlbumData(string name, string information, string date, string coverHash, string videoJson = "")
    {
        Name = name;
        Information = information;
        Date = date;
        CoverHash = coverHash;
        FilesJson = videoJson;
    }

    public AlbumData()
    {

    }

    /// <summary>
    /// 得到图片
    /// </summary>
    /// <param name="imageStream">图片流</param>
    public async Task GetImageAsync()
    {
        if (string.IsNullOrEmpty(CoverHash)) return;
        BitmapImage image = new();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = await HttpClientAPI.DownloadAsync(HttpClientAPI.BuildCommand("cat", CoverHash));
        image.EndInit();
        CoverImage = image;
    }
    /// <summary>
    /// 用字符串显示类信息
    /// </summary>
    /// <returns></returns>
    public string GetInfo()
    {
        string info = $"专辑名：{Name} 描述：{Information} 封面：{CoverHash}{Environment.NewLine}";
        if (FilesData != null)
        {
            foreach (var video in FilesData)
                info += video.Value.GetInfo() + Environment.NewLine;
        }
        return info;
    }
}

public class Album : AlbumData, INotifyPropertyChanged
{
    public string Page = "";

    private string _status = "";
    [Ignore]
    public string Status
    {
        get { return _status; }
        set
        {
            _status = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 用于存储，得到可存储的json数据
    /// </summary>
    /// <param name="albumData">专辑数据</param>
    /// <param name="filesData">要更新的存储的数据</param>
    public Album(AlbumData albumData, Dictionary<string, FileData> filesData)
    {
        Name = albumData.Name;
        Information = albumData.Information;
        Date = albumData.Date;
        CoverHash = albumData.CoverHash;
        FilesData = filesData;
    }
    /// <summary>
    /// 用于读取，得到FileData数据
    /// </summary>
    /// <param name="albumData">读出的专辑数据</param>
    public Album(AlbumData albumData)
    {
        Id = albumData.Id;
        Name = albumData.Name;
        Information = albumData.Information;
        Date = albumData.Date;
        CoverHash = albumData.CoverHash;
        FilesJson = albumData.FilesJson;
    }
    /// <summary>
    /// 用于数据库加载
    /// </summary>
    public Album()
    {

    }

    /// <summary>
    /// 更新或插入类数据
    /// </summary>
    /// <param name="sqlite">要更新的数据库</param>
    /// <param name="flag">更新数据库的方式，0为同步FilesData列表，其他为增加列表元素</param>
    /// <returns></returns>
    public async Task DataUpdateAsync(SQLiteService sqlite, int flag = 0)
    {
        //数据插入（先读取是否存在，使用专辑名称查找）
        var data = await sqlite.SQLConnection.FindAsync<Album>((arg) => arg.Name == Name);
        if (data != null)
        {
            //增加缺少的数据条目
            foreach (string item in FilesData!.Keys)
            {
                if (!data.FilesData!.ContainsKey(item))
                    data.FilesData.Add(item, FilesData![item]);
            }
            if (flag == 0)
                //减少删除的条目
                foreach (string item in data.FilesData!.Keys)
                {
                    if (!FilesData!.ContainsKey(item))
                        data.FilesData.Remove(item);
                }
            //信息修改
            if (!string.IsNullOrEmpty(Information))
                data.Information = Information;
            //封面修改
            if (!string.IsNullOrEmpty(CoverHash))
                data.CoverHash = CoverHash;
            //日期修改
            if (!string.IsNullOrEmpty(Date))
                data.Date = Date;
            //更新数据库中的数据
            await sqlite.SQLConnection.UpdateAsync(data);
        }
        else
        {
            await sqlite.SQLConnection.InsertAsync(this);
        }
    }
    
}
