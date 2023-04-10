using IPFSVideo.Models;
using MyToolkit;
using WinFormsLibrary;

namespace IPFSVideo.Service
{
    public class VideoInfoPageService
    {
        readonly HttpClientAPI ipfsApi = new();
        public int MaxAmount { get; set; }
        //public List<VideoAlbum> VideoAlbums = new();
        public List<PictureBox> Covers = new();
        public List<Label> Labels = new();

        private readonly Control _canvas;
        private List<Point> _InfoLocation;
        private MemoryStream _defaultImage;

        public VideoInfoPageService(Control canvas, int maxAmount, int pictureWidth, int pictureHeight)
        {
            MaxAmount = maxAmount;
            _canvas = canvas;
            _defaultImage = new MemoryStream(FileManager.GetFileBinary("autumn.jpg"));
            _InfoLocation = ControlLayout.SetLocation(10, 10, MaxAmount, 7, 125, 180);
            for (int i = 0; i < MaxAmount; i++)
            {
                PictureBox picture = new()
                {
                    Name = i.ToString(),
                    Width = pictureWidth,
                    Height = pictureHeight,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = null,
                    Location = _InfoLocation[i],
                    BackColor = Color.Silver
                };
                Label label = new()
                {
                    Name = i.ToString(),
                    //AutoSize = true,
                    Width = pictureWidth,
                    Height = 36,
                    Location = new Point(_InfoLocation[i].X + 1, _InfoLocation[i].Y + 140),
                    //BackColor = Color.Silver
                };
                picture.MouseClick += Picture_MouseClick;
                Covers.Add(picture);
                Labels.Add(label);
            }
            for (int i = 0; i < MaxAmount; i++)
            {
                _canvas.Controls.Add(Covers[i]);
                _canvas.Controls.Add(Labels[i]);
            }
        }

        private void Picture_MouseClick(object? sender, MouseEventArgs e)
        {
            
        }

        public async Task UpdatePageAsync(List<Animation> animation)
        {
            for (int i = 0; i < 15; i++)
            {
                Covers[i].Tag = null;
                if (animation.Count <= i) continue;
                Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", animation[i].CoverHash));
                Covers[i].Image = Image.FromStream(stream);
                animation[i].GetVideosData();
                Covers[i].Tag = animation[i];
                Labels[i].Text = $"{animation[i].AlbumName}\n{animation[i].PublishDate}";
            }
        }

        public void PageSizeChanged()
        {
            int length = _canvas.Width / 123;
            _InfoLocation = ControlLayout.SetLocation(10, 10, MaxAmount, length, 120, 180);
            for (int i = 0; i < Covers.Count; i++)
            {
                Covers[i].Location = _InfoLocation[i];
            }
        }
    }
}
