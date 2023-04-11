using IPFSVideo.Models;
using MyToolkit;
using SQLite;

namespace IPFSVideo
{
    public partial class UploadForm : Form
    {
        readonly HttpClientAPI ipfsApi = new();
        readonly VideoAlbum album = new();
        readonly Dictionary<string, FileData> videoDic = new();

        public Action? Uploaded;

        #region 数据
        private static readonly string databasePath = "data.db";
        private SQLiteAsyncConnection? sqlconnection;
        public SQLiteAsyncConnection SQLConnection => sqlconnection ??= new SQLiteAsyncConnection(databasePath);
        public List<VideoAlbum>? DataSource;
        public List<Animation>? AnimationSource;
        #endregion

        public UploadForm()
        {
            InitializeComponent();
        }

        public async void InitializeDatabase()
        {
            try
            {
                //DataSource = await SQLConnection.Table<VideoAlbum>().ToListAsync();
                AnimationSource = await SQLConnection.Table<Animation>().ToListAsync();
                foreach (var dataSource in AnimationSource)
                    dataSource.GetVideosData();
            }
            catch (Exception e)
            {
                if (e.Message == "no such table: VideoAlbum")
                    await SQLConnection.CreateTableAsync<Animation>();
            }
        }

        private void ShowMessage(string? message)
        {
            if (message != null)
                TB_Test.Invoke(new Action(() =>
                    TB_Test.AppendText($"[{DateTime.Now}] {message}\r\n")));
        }

        private async void BTN_UploadPicture_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                OFD_OpenFile.Filter = "图片|*.png|图片|*.jpg";
                if (OFD_OpenFile.ShowDialog() == DialogResult.OK)
                {
                    long fileLength = new FileInfo(OFD_OpenFile.FileName).Length;
                    Stream stream = FileManager.GetFileStream(OFD_OpenFile.FileName);
                    ShowMessage("上传中……");
                    FileData? result = await ipfsApi.
                        AddAsync(stream, OFD_OpenFile.FileName.Split('\\').LastOrDefault("nofile"),
                        null, fileLength);
                    PB_AlbumCover.Image = Image.FromStream(stream);
                    ShowMessage("上传完成");
                    if (result != null)
                    {
                        album.VideosJson = "";
                        album.CoverHash = result.Cid;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private async void BTN_Upload_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (TB_AlbumName.Text == "") return;
                if (TB_PublishDate.Text == "") return;
                album.AlbumName = TB_AlbumName.Text;
                album.PublishDate = TB_PublishDate.Text;
                OFD_OpenFile.Filter = "mp4视频|*.mp4";
                videoDic.Clear();

                if (OFD_OpenFile.ShowDialog() == DialogResult.OK)
                {
                    foreach (var path in OFD_OpenFile.FileNames)
                    {
                        //文件名
                        string fileName = path.Split('\\').LastOrDefault("nofile");
                        //文件长度
                        long fileLength = new FileInfo(path).Length;
                        ShowMessage($"{fileName}上传中……");
                        var result = await ipfsApi.
                            AddAsync(FileManager.GetFileStream(path), fileName, null, fileLength);
                        if (result != null)
                        {
                            if (!videoDic.ContainsKey(fileName))
                                videoDic.Add(fileName, result);
                        }
                    }
                    ShowMessage("上传完成");
                    Animation animation = new(album, videoDic);
                    await SQLConnection.InsertAsync(animation);
                }
                //重读数据库并刷新界面
                Uploaded?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        

    }
}
