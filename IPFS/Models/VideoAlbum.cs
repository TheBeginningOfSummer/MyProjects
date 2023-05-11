using IPFS.Services;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IPFS.Models;

public class VideoAlbum : ISQLData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string? Name { get; set; }
    public string? Information { get; set; }
    public string? CoverHash { get; set; }
    public string? VideosJson { get; set; }

    public VideoAlbum(string name, string information, string coverHash, string videoJson)
    {
        Name = name;
        Information = information;
        CoverHash = coverHash;
        VideosJson = videoJson;
    }

    public VideoAlbum()
    {

    }
}

public class Animation : VideoAlbum
{
    public BitmapImage CoverImage = new();
    public Dictionary<string, FileData>? VideosData;

    public Animation(string name, string information, string coverHash, string videosJson)
        : base(name, information, coverHash, videosJson)
    {
        VideosData = GetVideosData(VideosJson!);
    }
    /// <summary>
    /// 用于存储，得到可存储的VideoAlbum数据
    /// </summary>
    /// <param name="videoAlbum">专辑数据</param>
    /// <param name="videosData">视频链接数据</param>
    public Animation(VideoAlbum videoAlbum, Dictionary<string, FileData>? videosData)
    {
        Name = videoAlbum.Name;
        Information = videoAlbum.Information;
        CoverHash = videoAlbum.CoverHash;
        VideosData = videosData;
        GetVideosDataJson();
    }
    /// <summary>
    /// 用于读取，得到Animation数据
    /// </summary>
    /// <param name="videoAlbum">专辑数据</param>
    public Animation(VideoAlbum videoAlbum)
    {
        Id = videoAlbum.Id;
        Name = videoAlbum.Name;
        Information = videoAlbum.Information;
        CoverHash = videoAlbum.CoverHash;
        VideosJson = videoAlbum.VideosJson;
        GetVideosData();
    }

    public Animation()
    {

    }

    /// <summary>
    /// 数据流转为图片
    /// </summary>
    /// <param name="imageStream">图片数据流</param>
    public void GetImage(Stream imageStream)
    {
        CoverImage.StreamSource = imageStream;
    }
    /// <summary>
    /// 字典转为Json数据数据
    /// </summary>
    /// <param name="videosData">视频字典列表</param>
    /// <returns></returns>
    public static string GetVideosDataJson(Dictionary<string, FileData> videosData)
    {
        return JsonSerializer.Serialize(videosData);
    }
    /// <summary>
    /// Json转为字典数据
    /// </summary>
    /// <param name="videosJson">Json数据</param>
    /// <returns></returns>
    public static Dictionary<string, FileData>? GetVideosData(string videosJson)
    {
        return JsonSerializer.Deserialize<Dictionary<string, FileData>>(videosJson);
    }

    /// <summary>
    /// 字典转为Json数据数据
    /// </summary>
    public void GetVideosDataJson()
    {
        if (VideosData == null || VideosData.Count == 0)
            VideosJson = "";
        else
            VideosJson = JsonSerializer.Serialize(VideosData);
    }
    /// <summary>
    /// Json转为字典数据
    /// </summary>
    public void GetVideosData()
    {
        if (string.IsNullOrEmpty(VideosJson))
            VideosData = new Dictionary<string, FileData>();
        else
            VideosData = JsonSerializer.Deserialize<Dictionary<string, FileData>>(VideosJson!);
    }
    /// <summary>
    /// 更新或插入类数据
    /// </summary>
    /// <param name="sqlite">要更新的数据库</param>
    /// <returns></returns>
    public async Task DatabaseUpdateAsync(SQLiteService sqlite)
    {
        //数据插入（先读取是否存在）
        var data = await sqlite.SQLConnection.FindAsync<Animation>((arg) => arg.Name == Name);
        if (data != null)
        {
            //读取到的Json数据转换
            data.GetVideosData();
            //增加缺少的数据条目
            foreach (string item in VideosData!.Keys)
            {
                if (!data.VideosData!.ContainsKey(item))
                    data.VideosData.Add(item, VideosData![item]);
            }
            //减少删除的条目
            foreach (string item in data.VideosData!.Keys)
            {
                if (!VideosData!.ContainsKey(item))
                    data.VideosData.Remove(item);
            }
            //信息修改
            if (!string.IsNullOrEmpty(Information))
                data.Information = Information;
            //封面修改
            if (!string.IsNullOrEmpty(CoverHash))
                data.CoverHash = CoverHash;
            //修改的信息改回Json
            data.GetVideosDataJson();
            //更新数据库中的数据
            await sqlite.SQLConnection.UpdateAsync(data);
        }
        else
        {
            await sqlite.SQLConnection.InsertAsync(this);
        }
    }

    public string GetInfo()
    {
        string info = $"专辑名：{Name} 描述：{Information} 封面：{CoverHash}{Environment.NewLine}";
        if (VideosData != null)
        {
            foreach (var video in VideosData)
                info += video.Value.GetInfo() + Environment.NewLine;
        }
        return info;
    }
}
