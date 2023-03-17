using MyToolkit;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using STTech.BytesIO.Tcp;
using System.Collections.Concurrent;
using System.Diagnostics;
using Tesseract;

namespace MyProjects
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit testProcess = new("C:\\Users\\1\\Desktop\\MyProjects\\OpencvTest\\dist\\CVTest\\CVTest.exe");
        //readonly ConnectionToolkit.SocketConnection server = new("127.0.0.1", 5000);
        readonly TcpServer bytesIOServer = new();
        readonly DataTransfer dataTransfer = new();
        readonly TesseractEngine tess;
        readonly OpenCVTool cvTool;

        public Form1()
        {
            InitializeComponent();
            tess = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default)
            {
                DefaultPageSegMode = PageSegMode.Auto
            };
            cvTool = new OpenCVTool();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                bytesIOServer.Host = "127.0.0.1";
                bytesIOServer.Port = 5000;
                bytesIOServer.Started += BytesIOServer_Started;
                bytesIOServer.Closed += BytesIOServer_Closed;
                bytesIOServer.ClientConnected += BytesIOServer_ClientConnected;
                bytesIOServer.ClientDisconnected += BytesIOServer_ClientDisconnected;
                bytesIOServer.StartAsync();
                dataTransfer.DataReceive();

                //OpenCVTool.VideoUpdateAction += UpdatePicture;
                //cvTool.MouseCallbackEvent += MouseEvent;

                //server.ReceiveFromClient += UpdateClientInfo;
                //server.ClientListUpdate += UpdateClientList;
                //server.StartListening();
                //TB_Info.AppendText($"服务端监听开始{Environment.NewLine}");

                //testProcess.TargetProcess.OutputDataReceived += TargetProcess_OutputDataReceived;
                //testProcess.TargetProcess.ErrorDataReceived += TargetProcess_ErrorDataReceived;
                //testProcess.StartProcessAsync();
                //ShowMessage($"控制台程序启动完成");
                //ShowMessage($"{AppDomain.CurrentDomain.BaseDirectory}tessdata\\");
                //ShowMessage(@"./tessdata");

            }
            catch (Exception ex)
            {
                TB_Info.AppendText($"初始化错误:{ex.Message}{Environment.NewLine}");
            }
        }

        private void MouseEvent(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            throw new NotImplementedException();
        }

        private void PB_MyPicture_Paint(object? sender, PaintEventArgs e)
        {
            Invalidate();
            PB_MyPicture.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            bytesIOServer.CloseAsync();
        }

        private void ShowMessage(string message)
        {
            TB_Info.Invoke(new Action(() =>
                TB_Info.AppendText($"[{DateTime.Now}]{message}\r\n")));
        }

        private void UpdatePicture(Mat picture)
        {
            //Invalidate();
            //PB_MyPicture.Invalidate();
            PB_MyPicture.Image = BitmapConverter.ToBitmap(picture);
            //Application.DoEvents();
        }

        #region TCP通信事件
        private void BytesIOServer_ClientDisconnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientDisconnectedEventArgs e)
        {
            ShowMessage($"客户端[{e.Client.Host}:{e.Client.Port}]断开成功");
        }

        private void BytesIOServer_ClientConnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientConnectedEventArgs e)
        {
            ShowMessage($"客户端[{e.Client.Host}:{e.Client.Port}]连接成功");
            e.Client.OnDataReceived += Client_OnDataReceived;
        }

        private void Client_OnDataReceived(object? sender, STTech.BytesIO.Core.DataReceivedEventArgs e)
        {
            try
            {
                dataTransfer.Cache.Enqueue(new UnpackagedMessage(e.Data));
                Thread.Sleep(10);
                dataTransfer.DataParseSwitch.Set();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void BytesIOServer_Closed(object? sender, EventArgs e)
        {
            ShowMessage("停止监听");
        }

        private void BytesIOServer_Started(object? sender, EventArgs e)
        {
            ShowMessage("开始监听");
        }

        private void UpdateClientInfo(System.Net.Sockets.Socket client, byte[] data)
        {
            PB_MyPicture.Invoke(new Action(() => PB_MyPicture.Image = Image.FromStream(new MemoryStream(data))));
            //PB_MyPicture.Image = Image.FromStream(new MemoryStream(data));
        }

        private void UpdateClientList()
        {

        }
        #endregion

        private void TargetProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                ShowMessage(e.Data);
        }

        private void TargetProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                ShowMessage(e.Data);
        }

        private void BTN_Test_Click(object sender, EventArgs e)
        {
            try
            {
                //if (PB_MyPicture.Image != null)
                //    PB_MyPicture.Image = null;
                //TB_Info.Clear();
                openFileDialog1.Filter = "png图片|*.png|jpg图片|*.jpg|MP4|*.MP4";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //MemoryStream imageStream = new(ImageToolkit.GetBinary(Path.GetFullPath(openFileDialog1.FileName)));
                    //PB_MyPicture.Image = (Bitmap)Image.FromStream(imageStream);
                    //imageStream.Close();
                    cvTool.GetImage(Path.GetFullPath(openFileDialog1.FileName), "video");
                }

                //MemoryStream imageStream = new(ImageToolkit.GetBinary("test1.png"));
                //Mat img = Mat.FromStream(imageStream, ImreadModes.Color);
                //Cv2.ImShow("Input Image", img);
                //Mat img2 = img.Threshold(29, 255, ThresholdTypes.Binary);
                //Cv2.ImShow("Threshold", img2);

                //var img = Pix.LoadFromMemory(ImageToolkit.GetBinary("test1.png"));
                //var img = Pix.LoadFromFile(openFileDialog1.FileName);
                //var page = tess.Process(img);
                //var text = page.GetText();
                //ShowMessage(text);
                //page.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BTN_Test2_Click(object sender, EventArgs e)
        {
            cvTool.ClearDraw();
        }

        private void BTN_Test3_Click(object sender, EventArgs e)
        {
            cvTool.IsStopRead = !cvTool.IsStopRead;
            //OpenCvSharp.Cv2.MoveWindow("video", 0, 0);
            if (cvTool.IsStopRead)
            {
                BTN_Test3.BackColor = Color.OrangeRed;
            }
            else
            {
                BTN_Test3.BackColor = Color.White;
            }
        }

        private void RB_Line_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Line.Checked)
            {
                cvTool.DrawShape = 0;
            }
            else if (RB_Rectangle.Checked)
            {
                cvTool.DrawShape = 1;
            }
            else if (RB_Circle.Checked)
            {
                cvTool.DrawShape = 2;
            }
        }

        private void BTN_ProcessInput_Click(object sender, EventArgs e)
        {
            try
            {
                if (TB_ConsoleInput.Text == "") return;
                testProcess.ProcessInput(TB_ConsoleInput.Text);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void BTN_ReadImage_Click(object sender, EventArgs e)
        {
            //cvTool.Clear();
            openFileDialog1.Filter = "png图片|*.png|jpg图片|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //cvTool.ReadImage(Path.GetFullPath(openFileDialog1.FileName));
                cvTool.Model1(Path.GetFullPath(openFileDialog1.FileName));
            }
        }

        private void BTN_ReadVideo_Click(object sender, EventArgs e)
        {
            //OpenCVTool.IsStopRead = true;
            openFileDialog1.Filter = "mp4视频|*.mp4";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Task.Run(() => { cvTool.IsStopRead = false; cvTool.ReadVideo(Path.GetFullPath(openFileDialog1.FileName), "video"); });
                //OpenCVTool.IsStopRead = false; 
                //OpenCVTool.ReadVideo(Path.GetFullPath(openFileDialog1.FileName), "video");
                //OpenCVTool.ReadVideo("http://192.168.1.6:4747/mjpegfeed?1920x1080", "video");
            }
            //OpenCVTool.ReadVideo(0, "video");
        }

        private void BTN_LocalCamera_Click(object sender, EventArgs e)
        {
            Task.Run(() => { cvTool.IsStopRead = false; cvTool.ReadCamera(0, "video"); });
        }

        private void BTN_CVStop_Click(object sender, EventArgs e)
        {
            cvTool.IsStopRead = true;
        }
    }
}