using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace IPFS.Models
{
    public class FileData
    {
        public string? Name { get; set; }
        public string? Cid { get; set; }
        public long? Size { get; set; }

        public FileData(string name, string cid, long size)
        {
            Name = name;
            Cid = cid;
            Size = size;
        }

        public FileData()
        {

        }

        public string GetInfo()
        {
            return $"{Name} {Cid} {Size}";
        }

    }

    public class VideoAlbum
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
            VideosJson = GetVideosDataJson(videosData!);
            VideosData = videosData;
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
            VideosData = GetVideosData(VideosJson!);
        }

        public Animation()
        {

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
        /// 数据流转为图片
        /// </summary>
        /// <param name="imageStream">图片数据流</param>
        public void GetImage(Stream imageStream)
        {
            CoverImage.StreamSource = imageStream;
        }
        /// <summary>
        /// Json转为字典数据
        /// </summary>
        public void GetVideosData()
        {
            VideosData = GetVideosData(VideosJson!);
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

}
