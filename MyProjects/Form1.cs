using MyToolkit;
using OpenCvSharp;
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
        #region 网络传输数据拼接、缓存与接收开关
        readonly Dictionary<string, MemoryStream> memoryStreamDic = new();
        readonly ConcurrentQueue<UnpackagedMessage> cache = new();
        readonly ManualResetEvent dataParseSwitch = new(false);
        #endregion
        TesseractEngine tess;

        public Form1()
        {
            InitializeComponent();
            tess = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default)
            {
                DefaultPageSegMode = PageSegMode.Auto
            };
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
                ParseData();

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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            bytesIOServer.CloseAsync();
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
                cache.Enqueue(new UnpackagedMessage(e.Data));
                Thread.Sleep(10);
                dataParseSwitch.Set();
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

        private void ShowMessage(string message)
        {
            TB_Info.Invoke(new Action(() =>
                TB_Info.AppendText($"[{DateTime.Now}]{message}\r\n")));
        }

        private void ParseData()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    dataParseSwitch.WaitOne();
                    Thread.Sleep(10);
                    while (!cache.IsEmpty)
                    {
                        cache.TryDequeue(out var message);
                        if (message != null)
                            switch (message.Type)
                            {
                                case TransferType.Text:
                                    ShowMessage(message.Data.EncodeToString());
                                    break;
                                case TransferType.FileInfo:
                                    memoryStreamDic[message.Args.EncodeToString()] = new MemoryStream();
                                    break;
                                case TransferType.FileContent:
                                    lock (memoryStreamDic[message.Args.EncodeToString()])
                                        memoryStreamDic[message.Args.EncodeToString()].Write(message.Data);
                                    break;
                                case TransferType.FileEnd:
                                    PB_MyPicture.Invoke(new Action(() => PB_MyPicture.Image = Image.FromStream(memoryStreamDic[message.Args.EncodeToString()])));
                                    memoryStreamDic[message.Args.EncodeToString()].Close();
                                    memoryStreamDic[message.Args.EncodeToString()].Dispose();
                                    memoryStreamDic.Remove(message.Args.EncodeToString());
                                    dataParseSwitch.Reset();
                                    break;
                            }
                    }
                }
            });
        }

        private void BTN_Test_Click(object sender, EventArgs e)
        {
            try
            {
                //if (PB_MyPicture.Image != null)
                //    PB_MyPicture.Image = null;
                //TB_Info.Clear();
                MemoryStream imageStream = new(ImageToolkit.GetBinary("test1.png"));
                //Mat img = Mat.FromStream(imageStream, ImreadModes.Color);
                //Cv2.ImShow("Input Image", img);
                //Mat img2 = img.Threshold(29, 255, ThresholdTypes.Binary);
                //Cv2.ImShow("Threshold", img2);

                //Bitmap image = (Bitmap)Image.FromStream(imageStream);
                //Bitmap image = (Bitmap)Image.FromFile("C:\\Users\\1\\Desktop\\MyProjects\\OpencvTest\\Image\\test1.png");
                //PB_MyPicture.Image = image;

                var img = Pix.LoadFromMemory(ImageToolkit.GetBinary("test1.png"));
                
                imageStream.Close();
                var page = tess.Process(img);
                var text = page.GetText();
                ShowMessage(text);
                page.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        
    }
}