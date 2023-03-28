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
    }

    public class VideoAlbum
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? AlbumName { get; set; }
        public string? PublishDate { get; set; }
        public string? CoverHash { get; set; }
        public string? VideosJson { get; set; }

        public VideoAlbum(string albumName, string publishDate, string coverHash, params string[] videoInfo)
        {
            AlbumName = albumName;
            PublishDate = publishDate;
            CoverHash = coverHash;
            VideosJson += "{";
            foreach (var info in videoInfo)
                VideosJson += info + ",";
            VideosJson = VideosJson[..^1];
            VideosJson += "}";
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
        public Dictionary<string, string>? VideoInfo;
        public Dictionary<string, FileData>? VideosData;

        public Animation(string albumName, string publishDate, string coverHash, params string[] videoInfo)
            : base(albumName, publishDate, coverHash, videoInfo)
        {
            VideoInfo = GetObject(VideosJson!);
        }

        public Animation(VideoAlbum videoAlbum)
        {
            Id = videoAlbum.Id;
            AlbumName = videoAlbum.AlbumName;
            PublishDate = videoAlbum.PublishDate;
            CoverHash = videoAlbum.CoverHash;
            VideosJson = videoAlbum.VideosJson;

            VideoInfo = GetObject(VideosJson!);
            VideosData = GetVideosData(VideosJson!);
        }

        public Animation()
        {

        }

        public static string GetVideosDataJson(Dictionary<string, FileData> videosData)
        {
            return JsonSerializer.Serialize(videosData);
        }

        public static Dictionary<string, FileData>? GetVideosData(string videosJson)
        {
            return JsonSerializer.Deserialize<Dictionary<string, FileData>>(videosJson);
        }
    }

    
}
