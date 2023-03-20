using MyToolkit;
using LibVLCSharp.Shared;
using System.IO;
using System;

namespace IPFSVideo
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        readonly HttpClientAPI ipfsApi = new();
        private readonly LibVLC libVLC;
        private readonly MediaPlayer mediaPlayer;

        private string uploadPath = "C:\\Users\\Summer\\Desktop\\20230312155239.png";

        public Form1()
        {
            InitializeComponent();
            Core.Initialize();
            libVLC = new LibVLC();
            mediaPlayer = new MediaPlayer(libVLC);
            mediaPlayer.Hwnd = PB_Image.Handle;

            uploadPath = "E:\\สำฦต\\MV\\AMV.mp4";
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

                Stream stream = await ipfsApi.DownloadAsync(HttpClientAPI.BuildCommand("cat", "Qmd63D8VL9DkR7DvPaUvZNfrnVgoWACczFBHXxiWt2YNDx"));
                //TB_Info.Clear();
                byte[] data = new byte[10240];
                int length;
                using (FileStream file = new FileStream("Qmd63D8VL9DkR7DvPaUvZNfrnVgoWACczFBHXxiWt2YNDx.mp4", FileMode.OpenOrCreate))
                {
                    while ((length = await stream.ReadAsync(data)) != 0)
                    {
                        await file.WriteAsync(data, 0, length);
                    }
                }
                
                Media video = new Media(libVLC, "Qmd63D8VL9DkR7DvPaUvZNfrnVgoWACczFBHXxiWt2YNDx.mp4");
                mediaPlayer.Play(video);
                video.Dispose();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private async void BTN_Upload_Click(object sender, EventArgs e)
        {
            try
            {
                string result = await ipfsApi.UploadAsync(HttpClientAPI.BuildCommand("add"),
                    new StreamContent(FileManager.GetFileStream(uploadPath)));
                ShowMessage(result.ToString());
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

        

    }
}