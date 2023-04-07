using SQLite;
using System.Text.Json;
using System.Windows.Forms;

namespace IPFSVideo.Models
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
        public string? AlbumName { get; set; }
        public string? PublishDate { get; set; }
        public string? CoverHash { get; set; }
        public string? VideosJson { get; set; }

        public VideoAlbum(string albumName, string publishDate, string coverHash, string videoJson)
        {
            AlbumName = albumName;
            PublishDate = publishDate;
            CoverHash = coverHash;
            VideosJson = videoJson;
        }

        public VideoAlbum()
        {

        }

        public static string GetJson(string key, string value)
        {
            return $"\"{key}\":\"{value}\"";
        }

        public static string[] GetJson(Dictionary<string, string> keyValueInfo)
        {
            List<string> infolist = new();
            foreach (var info in keyValueInfo)
                infolist.Add($"\"{info.Key}\":\"{info.Value}\"");
            return infolist.ToArray();
        }

        public static Dictionary<string, string>? GetObject(string jsonString)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }

        public static async Task<Dictionary<string, string>?> GetObjectAsync(Stream jsonStream)
        {
            return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(jsonStream);
        }
    }

    public class Animation : VideoAlbum
    {
        public Dictionary<string, FileData>? VideosData;

        public Animation(string albumName, string publishDate, string coverHash, string videoJson)
            : base(albumName, publishDate, coverHash, videoJson)
        {
            VideosData = GetVideosData(VideosJson!);
        }

        public Animation(VideoAlbum videoAlbum, Dictionary<string, FileData>? videosData)
        {
            AlbumName = videoAlbum.AlbumName;
            PublishDate = videoAlbum.PublishDate;
            CoverHash = videoAlbum.CoverHash;
            VideosJson = GetVideosDataJson(videosData!);
            VideosData = videosData;
        }

        public Animation(VideoAlbum videoAlbum)
        {
            Id = videoAlbum.Id;
            AlbumName = videoAlbum.AlbumName;
            PublishDate = videoAlbum.PublishDate;
            CoverHash = videoAlbum.CoverHash;
            VideosJson = videoAlbum.VideosJson;
            VideosData = GetVideosData(VideosJson!);
        }

        public Animation()
        {

        }

        public void GetVideosData()
        {
            VideosData = GetVideosData(VideosJson!);
        }

        public static string GetVideosDataJson(Dictionary<string, FileData> videosData)
        {
            return JsonSerializer.Serialize(videosData);
        }

        public static Dictionary<string, FileData>? GetVideosData(string videosJson)
        {
            return JsonSerializer.Deserialize<Dictionary<string, FileData>>(videosJson);
        }

        public string GetInfo()
        {
            string info = $"专辑名：{AlbumName} 发布日期：{PublishDate} 封面：{CoverHash}{Environment.NewLine}";
            if (VideosData != null)
            {
                foreach (var video in VideosData)
                {
                    info += video.Value.GetInfo() + Environment.NewLine;
                }
            }
            return info;
        }
    }

    

    
}
