using IPFSVideo.Models;
using WinFormsLibrary;

namespace IPFSVideo.Service
{
    public class AlbumInfoPageService
    {
        readonly HttpClientAPI ipfsApi = new();
        public int MaxAmount { get; set; }
        public Image DefaultImage;
        public List<Label> Labels = new();
        public List<PictureBox> Covers = new();

        private readonly Control _canvas;
        private List<Point> _InfoLocation;

        public AlbumInfoPageService(Control canvas, int maxAmount, int pictureWidth, int pictureHeight)
        {
            MaxAmount = maxAmount;
            DefaultImage = Image.FromFile("autumn.jpg");
            _canvas = canvas;
            _InfoLocation = ControlLayout.SetLocation(10, 10, MaxAmount, 7, 125, 180);
            for (int i = 0; i < MaxAmount; i++)
            {
                Label label = new()
                {
                    Name = i.ToString(),
                    //AutoSize = true,
                    Width = pictureWidth,
                    Height = 36,
                    Location = new Point(_InfoLocation[i].X + 1, _InfoLocation[i].Y + 140),
                    //BackColor = Color.Silver
                };
                PictureBox picture = new()
                {
                    Name = i.ToString(),
                    Width = pictureWidth,
                    Height = pictureHeight,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = DefaultImage,
                    Location = _InfoLocation[i],
                    BackColor = Color.Silver
                };
                Labels.Add(label);
                Covers.Add(picture);
            }
            for (int i = 0; i < MaxAmount; i++)
            {
                _canvas.Controls.Add(Labels[i]);
                _canvas.Controls.Add(Covers[i]);
            }
        }

        public async Task UpdatePageAsync(List<Animation> animation)
        {
            for (int i = 0; i < MaxAmount; i++)
            {
                //清空tag
                Covers[i].Tag = null;
                if (animation.Count <= i) continue;
                Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", animation[i].CoverHash));
                Covers[i].Image = Image.FromStream(stream);
                //Json=>Dic
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
