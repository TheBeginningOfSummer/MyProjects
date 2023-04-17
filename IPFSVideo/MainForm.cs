using MyToolkit;
using LibVLCSharp.Shared;
using Microsoft.IO;
using SQLite;
using IPFSVideo.Models;
using IPFSVideo.Service;
using Newtonsoft.Json.Linq;

namespace IPFSVideo
{
    public partial class MainForm : Form, IProgress<TransferProgress>
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        readonly HttpClientAPI ipfsApi = new();
        readonly LibVLC libVLC;
        readonly MediaPlayer mediaPlayer;
        Media? video;

        #region 内存缓存
        readonly byte[] buffer = new byte[1024 * 1024];
        static readonly RecyclableMemoryStreamManager streamManager = new();
        MemoryStream? cache;
        StreamMediaInput? streamMedia;
        #endregion

        #region 数据
        private static readonly string databasePath = (ConfigurationService.Instance.Config.Load("DatabaseName") == "")
            ? "data.db" : ConfigurationService.Instance.Config.Load("DatabaseName");
        private SQLiteAsyncConnection? sqlconnection;
        public SQLiteAsyncConnection SQLConnection => sqlconnection ??= new SQLiteAsyncConnection(databasePath);
        public List<VideoAlbum>? DataSource;
        public List<Animation>? AnimationSource;
        #endregion

        /// <summary>
        /// 上传页面
        /// </summary>
        readonly UploadForm uploadForm = new();
        /// <summary>
        /// 专辑信息页管理
        /// </summary>
        readonly AlbumInfoPageService albumInfoPage;
        /// <summary>
        /// 详情页面
        /// </summary>
        private DetailsForm detailsForm = new();

        public MainForm()
        {
            InitializeComponent();
            Core.Initialize();
            libVLC = new LibVLC();
            //mediaPlayer = new MediaPlayer(libVLC);
            //mediaPlayer.Hwnd = PB_Screen.Handle;
            mediaPlayer = VV_Screen.MediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.Stopped += MediaPlayer_Stopped;
            mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            mediaPlayer.Playing += MediaPlayer_Playing;

            albumInfoPage = new AlbumInfoPageService(PN_VideoInfo, 20, 100, 140);
            Task.Run(async () =>
            {
                await InitializeDatabase();
                await albumInfoPage.UpdatePageAsync(AnimationSource!);
            });
            foreach (var cover in albumInfoPage.Covers)
            {
                cover.MouseClick += Cover_MouseClick;
            }
            uploadForm.Uploaded += UploadedAsync;
        }

        private void TargetProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                ShowMessage(e.Data);
        }

        #region 方法
        public async Task InitializeDatabase()
        {
            try
            {
                DataSource = await SQLConnection.Table<VideoAlbum>().ToListAsync();
                AnimationSource = await SQLConnection.Table<Animation>().ToListAsync();
                foreach (var dataSource in AnimationSource)
                    dataSource.GetVideosData();
            }
            catch (Exception)
            {
                //if (e.Message == "no such table: VideoAlbum")
                await SQLConnection.CreateTableAsync<VideoAlbum>();
                await SQLConnection.CreateTableAsync<Animation>();
            }
        }

        private static string GetTimeString(int val)
        {
            int hour = val / 3600;
            val %= 3600;
            int minute = val / 60;
            int second = val % 60;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
        }

        private void ShowMessage(string? message)
        {
            if (message != null)
                TB_Info.Invoke(new Action(() =>
                    TB_Info.AppendText($"[{DateTime.Now}] {message}\r\n")));
        }

        public void Report(TransferProgress value)
        {
            TB_Info.Invoke(new Action(() =>
                TB_Info.AppendText($"[{DateTime.Now}] {value.Name} {(float)value.Bytes / value.AllLength * 100}%\r\n")));
        }

        private void Play()
        {
            streamMedia = new(cache!);
            video = new(libVLC, streamMedia);
            mediaPlayer.Play(video);
            video.Dispose();
        }
        #endregion

        #region 控件事件
        private void Form1_Load(object sender, EventArgs e)
        {
            ipfsProcess.TargetProcess.OutputDataReceived += TargetProcess_OutputDataReceived;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void TM_Play_Tick(object sender, EventArgs e)
        {

        }

        private void PN_VideoInfo_SizeChanged(object sender, EventArgs e)
        {
            albumInfoPage.PageSizeChanged();
        }
        //图片点击
        private void Cover_MouseClick(object? sender, MouseEventArgs e)
        {
            PictureBox? picture = sender as PictureBox;
            //如果PictureBox的Tag为Animation且不为空
            if (picture!.Tag is Animation animation)
            {
                detailsForm = new();
                int i = 0;
                foreach (var item in animation.VideosData!)
                {
                    detailsForm.AddLabels(item.Value.Name!, new Point(0, i * 20));
                    detailsForm.AddLinks(item.Value.Cid!, new Point(200, i * 20));
                    i++;
                }
                detailsForm.ShowDialog();
            }
        }
        //上传完成
        private async void UploadedAsync()
        {
            AnimationSource = await SQLConnection.Table<Animation>().ToListAsync();
            await albumInfoPage.UpdatePageAsync(AnimationSource!);
        }
        #endregion

        #region VLC事件
        private void MediaPlayer_Playing(object? sender, EventArgs e)
        {
            CTB_PlayerTrack.L_Minimum = 0;
            CTB_PlayerTrack.L_Maximum = (int)(mediaPlayer.Length / 1000);
        }

        private void MediaPlayer_TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
        {
            CTB_PlayerTrack.L_Value = (int)mediaPlayer.Time / 1000;
            Invoke(new Action(() =>
            {
                LB_Duration.Text = $"{GetTimeString(CTB_PlayerTrack.L_Value)}/{GetTimeString((int)mediaPlayer.Length / 1000)}";
            }));

            if (mediaPlayer.Time / 1000 == mediaPlayer.Length / 1000)
            {
                Task.Run(mediaPlayer.Stop);
                CTB_PlayerTrack.L_Value = 0;
            }
        }

        private void MediaPlayer_Stopped(object? sender, EventArgs e)
        {
            cache?.Dispose();
        }
        #endregion

        #region 按钮
        private async void BTN_Test_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                //video.AddOption($":sout=#rtp{{sdp = rtsp://127.0.0.1:8554/video}} :sout-all :sout-keep");

                //string result = await ipfsApi.DoCommandAsync(HttpClientAPI.BuildCommand(TB_Command.Text, TB_CID.Text));
                //ShowMessage(result.ToString());

                //System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", $"http://localhost:8080/ipfs/{TB_CID.Text}");
                //var result = await ipfsApi.DoCommandAsync
                //    (HttpClientAPI.BuildCommand("name/publish", "QmUat6n7w6nXBs2fC7jpubGg1Rgid83z5iXpknL37GcQ85", "key=self"));
                var ipnsList = await ipfsApi.GetIPNSAsync();
                var result = await ipfsApi.DoCommandAsync
                    (HttpClientAPI.BuildCommand("name/resolve", ipnsList["self"]));
                JObject resultObject = JObject.Parse(result);
                var flie = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", resultObject["Path"]!.ToString()));
                await FileManager.WriteStreamAsync("Test", "self.db", flie);
                ShowMessage(result.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BTN_Upload_ClickAsync(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void BTN_Download_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                //Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text));
                //PB_Screen.Image = Image.FromStream(stream);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private async void BTN_Play_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                mediaPlayer.Stop();
                using Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text));
                cache = streamManager.GetStream();
                int length;
                while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //var pos = cache.Position;
                    //cache.Position = cache.Length;
                    await cache.WriteAsync(buffer.AsMemory(0, length));
                    //cache.Position = pos;
                }
                await Task.Run(Play);

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void TSB_UploadSet_Click(object sender, EventArgs e)
        {
            uploadForm.ShowDialog();
        }

        private async void TSB_AlbumUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                await albumInfoPage.UpdatePageAsync(AnimationSource!);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }
        #endregion




    }
}