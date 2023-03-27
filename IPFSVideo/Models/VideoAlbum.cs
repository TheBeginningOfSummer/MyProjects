using SQLite;
using System.Text.Json;
using System.Windows.Forms;

namespace IPFSVideo.Models
{
    public class VideoAlbum
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? AlbumName { get; set; }
        public string? PublishDate { get; set; }
        public string? CoverHash { get; set; }
        public string? VideoData { get; set; }

        public VideoAlbum(string albumName, string publishDate, string coverHash, params string[] videoInfo)
        {
            AlbumName = albumName;
            PublishDate = publishDate;
            CoverHash = coverHash;
            VideoData += "{";
            foreach (var info in videoInfo)
                VideoData += info + ",";
            VideoData = VideoData[..^1];
            VideoData += "}";
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
    }

    public class Animation : VideoAlbum
    {
        public Dictionary<string, string>? VideoInfo;

        public Animation(string albumName, string publishDate, string coverHash, params string[] videoInfo)
            : base(albumName, publishDate, coverHash, videoInfo)
        {
            VideoInfo = GetObject(VideoData!);
        }

        public Animation(VideoAlbum videoAlbum)
        {
            Id = videoAlbum.Id;
            AlbumName = videoAlbum.AlbumName;
            PublishDate = videoAlbum.PublishDate;
            CoverHash = videoAlbum.CoverHash;
            VideoData = videoAlbum.VideoData;
            VideoInfo = GetObject(VideoData!);
        }

        public Animation()
        {

        }
    }

    
}
