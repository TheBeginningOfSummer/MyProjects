using IPFSVideo.Models;
using MyToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public VideoInfoPageService(Control canvas, int maxAmount, int pictureWidth, int pictureHeight)
        {
            _canvas = canvas;
            MaxAmount = maxAmount;
            _InfoLocation = ControlLayout.SetLocation(10, 10, MaxAmount, 7, 123, 200);
            for (int i = 0; i < MaxAmount; i++)
            {
                PictureBox picture = new()
                {
                    Name = i.ToString(),
                    Width = pictureWidth,
                    Height = pictureHeight,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Image.FromStream(new MemoryStream(FileManager.GetFileBinary("autumn.jpg"))),
                    Location = _InfoLocation[i],
                    BackColor = Color.Silver
                };
                picture.MouseClick += Picture_MouseClick;
                Covers.Add(picture);
            }
        }

        private void Picture_MouseClick(object? sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void UpdatePage()
        {
            for (int i = 0; i < Covers.Count; i++)
            {
                _canvas.Controls.Add(Covers[i]);
            }
        }

        public void PageSizeChanged()
        {
            int length = _canvas.Width / 123;
            _InfoLocation = ControlLayout.SetLocation(10, 10, MaxAmount, length, 123, 200);
            for (int i = 0; i < Covers.Count; i++)
            {
                Covers[i].Location = _InfoLocation[i];
            }
        }
    }
}
