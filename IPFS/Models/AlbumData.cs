using IPFS.Services;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    public string? CoverHash { get; set; }
    public string? FilesJson { get; set; }

    public AlbumData(string name, string information, string coverHash, string videoJson)
    {
        Name = name;
        Information = information;
        CoverHash = coverHash;
        FilesJson = videoJson;
    }

    public AlbumData()
    {

    }
}

public class Album : AlbumData, INotifyPropertyChanged
{
    [Ignore]
    public ImageSource? CoverImage { get; set; }//"/Resources/Image/Autumn.jpg"
    public Dictionary<string, FileData>? FilesData;
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

    public string Page = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public Album(string name, string information, string coverHash, string filesJson)
        : base(name, information, coverHash, filesJson)
    {
        GetFilesData();
    }
    /// <summary>
    /// 用于存储，得到可存储的VideoAlbum数据
    /// </summary>
    /// <param name="albumData">专辑数据</param>
    /// <param name="filesData">视频链接数据</param>
    public Album(AlbumData albumData, Dictionary<string, FileData> filesData)
    {
        Name = albumData.Name;
        Information = albumData.Information;
        CoverHash = albumData.CoverHash;
        FilesData = filesData;
    }
    /// <summary>
    /// 用于读取，得到Animation数据
    /// </summary>
    /// <param name="albumData">专辑数据</param>
    public Album(AlbumData albumData)
    {
        Id = albumData.Id;
        Name = albumData.Name;
        Information = albumData.Information;
        CoverHash = albumData.CoverHash;
        FilesJson = albumData.FilesJson;
        GetFilesData();
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
    public async Task DatabaseUpdateAsync(SQLiteService sqlite, int flag = 0)
    {
        //数据插入（先读取是否存在）
        var data = await sqlite.SQLConnection.FindAsync<Album>((arg) => arg.Name == Name);
        if (data != null)
        {
            //读取到的Json数据转换
            data.GetFilesData();
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
            //修改的信息改回Json
            data.GetFilesDataJson();
            //更新数据库中的数据
            await sqlite.SQLConnection.UpdateAsync(data);
        }
        else
        {
            await sqlite.SQLConnection.InsertAsync(this);
        }
    }
    /// <summary>
    /// 字典转为Json数据数据
    /// </summary>
    public void GetFilesDataJson()
    {
        if (FilesData == null || FilesData.Count == 0)
            FilesJson = "";
        else
            FilesJson = JsonSerializer.Serialize(FilesData);
    }
    /// <summary>
    /// Json转为字典数据
    /// </summary>
    public void GetFilesData()
    {
        if (string.IsNullOrEmpty(FilesJson))
            FilesData = new Dictionary<string, FileData>();
        else
            FilesData = JsonSerializer.Deserialize<Dictionary<string, FileData>>(FilesJson!);
    }
    /// <summary>
    /// 得到图片
    /// </summary>
    /// <param name="imageStream">图片流</param>
    public void GetImage(Stream imageStream)
    {
        BitmapImage image = new();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = imageStream;
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
