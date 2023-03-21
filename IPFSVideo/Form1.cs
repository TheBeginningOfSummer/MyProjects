using MyToolkit;
using LibVLCSharp.Shared;
using System.Runtime.Intrinsics.Arm;

namespace IPFSVideo
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        readonly HttpClientAPI ipfsApi = new();
        readonly LibVLC libVLC;
        readonly MediaPlayer mediaPlayer;
        MemoryStream cache = new MemoryStream();
        StreamMediaInput? streamMedia;

        readonly OpenFileDialog fileDialog = new();

        public Form1()
        {
            InitializeComponent();
            Core.Initialize();
            libVLC = new LibVLC();
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.Hwnd = PB_Image.Handle;
            mediaPlayer.Stopped += MediaPlayer_Stopped;
        }

        private void MediaPlayer_Stopped(object? sender, EventArgs e)
        {
            cache?.Dispose();
            streamMedia?.Dispose();
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

        private void ShowMessage(string message)
        {
            TB_Info.Invoke(new Action(() =>
                TB_Info.AppendText($"[{DateTime.Now}] {message}\r\n")));
        }

        private async void BTN_Test_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                //Media video = new(libVLC, "QmZNA91PGEA2HyqsdNBjmQqKvVkvD97We613apZrWoLvRx.mp4", FromType.FromPath);
                //video.AddOption($":sout=#rtp{{sdp = rtsp://127.0.0.1:8554/video}} :sout-all :sout-keep");
                string result = await ipfsApi.DoCommandAsync(HttpClientAPI.BuildCommand(TB_Command.Text));
                ShowMessage(result.ToString());
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
                    string result = await ipfsApi.
                        AddAsync(FileManager.GetFileStream(fileDialog.FileName), fileDialog.FileName.Split('\\').LastOrDefault("nofile"));
                    ShowMessage(result.ToString());
                }
                //string result = await ipfsApi.UploadAsync(HttpClientAPI.BuildCommand("add"),
                //    new StreamContent(FileManager.GetFileStream(uploadPath)));
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
                Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_Test.Text));
                PB_Image.Image = Image.FromStream(stream);
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
                //Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", "QmZNA91PGEA2HyqsdNBjmQqKvVkvD97We613apZrWoLvRx"));
                //TB_Info.Clear();
                //byte[] data = new byte[10240];
                //int length;
                //using (FileStream file = new FileStream("QmZNA91PGEA2HyqsdNBjmQqKvVkvD97We613apZrWoLvRx.mp4", FileMode.OpenOrCreate))
                //{
                //    while ((length = await stream.ReadAsync(data)) != 0)
                //    {
                //        await file.WriteAsync(data, 0, length);
                //    }
                //}
                mediaPlayer.Stop();
                cache?.Close();
                streamMedia?.Close();
                
                using (Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", "QmZNA91PGEA2HyqsdNBjmQqKvVkvD97We613apZrWoLvRx")))
                {
                    cache = new MemoryStream();
                    byte[] buffer = new byte[10240]; int length;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        var pos = cache.Position;
                        cache.Position = cache.Length;
                        cache.Write(buffer, 0, length);
                        cache.Position = pos;
                    }
                    streamMedia = new(cache);
                    Media video = new(libVLC, streamMedia);
                    mediaPlayer.Play(video);
                    video.Dispose();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }
    }
}