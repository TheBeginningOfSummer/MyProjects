using MyToolkit;
using LibVLCSharp.Shared;
using Microsoft.IO;
using System;
using SQLite;
using IPFSVideo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Forms;

namespace IPFSVideo
{
    public partial class Form1 : Form, IProgress<TransferProgress>
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        readonly HttpClientAPI ipfsApi = new();
        readonly LibVLC libVLC;
        readonly MediaPlayer mediaPlayer;
        Media? video;

        #region ÄÚ´æ»º´æ
        readonly byte[] buffer = new byte[1024 * 1024];
        static readonly RecyclableMemoryStreamManager streamManager = new();
        MemoryStream? cache;
        StreamMediaInput? streamMedia;
        #endregion

        private static readonly string databasePath = "data.db";
        private SQLiteAsyncConnection? sqlconnection;
        public SQLiteAsyncConnection SQLConnection => sqlconnection ??= new SQLiteAsyncConnection(databasePath);
        public List<VideoAlbum>? DataSource;
        public List<Animation>? AnimationSource;

        readonly OpenFileDialog fileDialog = new();
        readonly UploadForm uploadForm = new();

        public Form1()
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

            InitializeDatabase();
            //var data = new VideoAlbum("name", "2022-03-22", "112233", "\"vidoe3\":\"hash3\"", "\"vidoe4\":\"hash4\"");
            var data = new Animation("animation", "2022-03-22", "112233",
                VideoAlbum.GetJson("video5", "value5"),
                "\"vidoe4\":\"hash4\"");
            SQLConnection.InsertAsync(data);
        }

        public async void InitializeDatabase()
        {
            try
            {
                DataSource = await SQLConnection.Table<VideoAlbum>().ToListAsync();
                AnimationSource = await SQLConnection.Table<Animation>().ToListAsync();
                var dd = new Animation(DataSource[0]);
            }
            catch (Exception e)
            {
                if (e.Message == "no such table: VideoAlbum")
                    await SQLConnection.CreateTableAsync<VideoAlbum>();
                await SQLConnection.CreateTableAsync<Animation>();
            }
        }

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

        private void TM_Play_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ipfsProcess.TargetProcess.OutputDataReceived += TargetProcess_OutputDataReceived;
        }

        private void TargetProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                ShowMessage(e.Data);
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

        private static string GetTimeString(int val)
        {
            int hour = val / 3600;
            val %= 3600;
            int minute = val / 60;
            int second = val % 60;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
        }

        private void Play()
        {
            streamMedia = new(cache!);
            video = new(libVLC, streamMedia);
            mediaPlayer.Play(video);
            video.Dispose();
        }

        #region °´Å¥
        private async void BTN_Test_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                //Media video = new(libVLC, "QmZNA91PGEA2HyqsdNBjmQqKvVkvD97We613apZrWoLvRx.mp4", FromType.FromPath);
                //video.AddOption($":sout=#rtp{{sdp = rtsp://127.0.0.1:8554/video}} :sout-all :sout-keep");
                string result = await ipfsApi.DoCommandAsync(HttpClientAPI.BuildCommand(TB_Command.Text, TB_CID.Text));
                ShowMessage(result.ToString());

                //Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text));
                //TB_Info.Clear();
                //byte[] data = new byte[10240];
                //int length;
                //using (FileStream file = new FileStream("test.mp4", FileMode.OpenOrCreate))
                //{
                //    while ((length = await stream.ReadAsync(data)) != 0)
                //    {
                //        await file.WriteAsync(data, 0, length);
                //    }
                //}

                //System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", $"http://localhost:8080/ipfs/{TB_CID.Text}");

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void BTN_Upload_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    AddFileOptions options = new(this);
                    long fileLength = new FileInfo(fileDialog.FileName).Length;
                    await Task.Run(async () =>
                    {
                        var result = await ipfsApi.
                        AddAsync(FileManager.GetFileStream(fileDialog.FileName),
                        fileDialog.FileName.Split('\\').LastOrDefault("nofile"), options, fileLength);
                        //var resultDic = VideoAlbum.GetObject(result);
                        //var data = new Animation("animation", "2022-03-22", "112233",
                        //VideoAlbum.GetJson("video5", "value5"),
                        //"\"vidoe4\":\"hash4\"");
                        //await SQLConnection.InsertAsync(data);
                        ShowMessage(result!.Name);
                        ShowMessage(result!.Cid);
                        ShowMessage(result!.Size.ToString());
                    });

                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private async void BTN_Download_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text));
                PB_Screen.Image = Image.FromStream(stream);
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
                using (Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text)))
                {
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

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }
        #endregion

        private void TSB_UploadSet_Click(object sender, EventArgs e)
        {
            uploadForm.ShowDialog();
        }

        
    }
}