using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPFSVideo.Models
{
    public class VideoAlbum
    {
        public string? AlbumName { get; set; }
        public string? Date { get; set; }
        public string? CoverHash { get; set; }

        //public Dictionary<string, string> VideoInfo { get; set; } = new();

        public VideoAlbum(string albumName, string date, string coverHash)
        {
            AlbumName = albumName;
            Date = date;
            CoverHash = coverHash;
        }

        public VideoAlbum()
        {

        }
    }

    public class VideoInfo
    {

    }
}
