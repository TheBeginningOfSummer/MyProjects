using IPFSVideo.Models;
using MyToolkit;
using WinFormsLibrary;

namespace IPFSVideo.Service
{
    public class VideoInfoPageService
    {
        public int MaxAmount { get; set; }
        public List<VideoAlbum> VideoAlbums = new();
        public List<PictureBox> Covers = new();

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
                    Image = Image.FromStream(_defaultImage),
                    Location = _InfoLocation[i],
                    BackColor = Color.Silver
                };
                picture.MouseClick += Picture_MouseClick;
                Covers.Add(picture);
            }
            for (int i = 0; i < Covers.Count; i++)
            {
                _canvas.Controls.Add(Covers[i]);
            }
        }

        private void Picture_MouseClick(object? sender, MouseEventArgs e)
        {
            
        }

        public void UpdatePage()
        {
            
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
