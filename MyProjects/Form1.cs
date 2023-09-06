using MyMouseAndKeyboard;
using MyToolkit;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using STTech.BytesIO.Tcp;
using System.Diagnostics;
using Tesseract;

namespace MyProjects;

public partial class Form1 : Form
{
    #region 对象
    readonly ProcessToolkit testProcess = new("C:\\Users\\1\\Desktop\\MyProjects\\OpencvTest\\dist\\CVTest\\CVTest.exe");
    //readonly ConnectionToolkit.SocketConnection server = new("127.0.0.1", 5000);
    readonly TcpServer bytesIOServer = new();
    readonly DataTransfer dataTransfer = new();
    readonly TesseractEngine tess;
    readonly OpenCVTool cvTool;
    #endregion

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

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        bytesIOServer.CloseAsync();
    }

    private void ShowMessage(string message)
    {
        TB_Info.Invoke(new Action(() =>
            TB_Info.AppendText($"[{DateTime.Now}]{message}\r\n")));
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

    #region 控制台程序
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
    #endregion

    #region 图片视频读取
    private void BTN_ReadImage_Click(object sender, EventArgs e)
    {
        //cvTool.Clear();
        openFileDialog1.Filter = "png图片|*.png|jpg图片|*.jpg";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            cvTool.ReadImage(Path.GetFullPath(openFileDialog1.FileName));
            //cvTool.Model1(Path.GetFullPath(openFileDialog1.FileName));
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
    #endregion

    #region 相机
    private void BTN_LocalCamera_Click(object sender, EventArgs e)
    {
        Task.Run(() => { cvTool.IsStopRead = false; cvTool.ReadCamera(0, "video"); });
    }

    private void BTN_CVStop_Click(object sender, EventArgs e)
    {
        cvTool.IsStopRead = true;
    }
    #endregion

    #region cvTool测试
    private void BTN_Test1_Click(object sender, EventArgs e)
    {
        try
        {
            openFileDialog1.Filter = "png图片|*.png|jpg图片|*.jpg|MP4|*.MP4";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //MemoryStream imageStream = new(ImageToolkit.GetBinary(Path.GetFullPath(openFileDialog1.FileName)));
                //PB_MyPicture.Image = (Bitmap)Image.FromStream(imageStream);
                //imageStream.Close();

                //cvTool.GetImage(Path.GetFullPath(openFileDialog1.FileName), "video");
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
    ImageProcessing processing = new ImageProcessing();
    private void BTN_Test2_Click(object sender, EventArgs e)
    {
        Mat sourceImage = Cv2.ImRead("有问题点我！安装教程！.png");
        //Cv2.ImShow("原始图", sourceImage);
        //ImageProcessing.Positioning(sourceImage);
        processing.DrawShape = 1;
        processing.ReadImage(sourceImage);
        //ImageProcessing.Cut(sourceImage);
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
    #endregion

    #region 测试
    private void BTN_Test_Click(object sender, EventArgs e)
    {

        int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
        int workHeight = Screen.PrimaryScreen.WorkingArea.Height;
        int width = Screen.PrimaryScreen.Bounds.Width;
        int height = Screen.PrimaryScreen.Bounds.Height;
        //ShowMessage($"{workWidth}:{workHeight},{width}:{height}");
        //MouseAndKeyboard.mouse_event((int)(MouseOperate.Absolute | MouseOperate.Move),
        //    500 * 65535 / width, 500 * 65535 / height, 0, 0);

        //MouseAndKeyboard.PostMessage(hWnd, 256, Keys.A, 2);
        MouseAndKeyboard.PostMessage(hWnd, 0x100, Keys.I, 0);
        //MouseAndKeyboard.PostMessage(hWnd, 0x101, Keys.I, 0);
        //MouseAndKeyboard.PostMessage(hWnd, 0x0201, IntPtr.Zero, new IntPtr(5 + (5 << 16)));
        //MouseAndKeyboard.PostMessage(hWnd, 0x0202, IntPtr.Zero, new IntPtr(5 + (5 << 16)));
        //MouseAndKeyboard.PostMessage(this.Handle, 0x0202, 1, 200 * 65535 / width + 200 * 65535 / height * 65536);
        //ShowMessage($"{workWidth}:{workHeight},{width}:{height}");
        TB_Test.AppendText($"X-{point.X}:Y-{point.Y}\r\n");

        //inputs[0].type = (int)InputType.Keyboard;
        //inputs[0].ki.wVk = 0;
        //inputs[0].ki.wScan = 0x11;
        //inputs[0].ki.dwFlags = (int)(KeyCode.KeyDown | KeyCode.Scancode);
        //inputs[0].ki.dwExtraInfo = MouseAndKeyboard.GetMessageExtraInfo();
        //inputs[1].type = (int)InputType.Keyboard;
        //inputs[1].ki.wVk = 0;
        //inputs[1].ki.wScan = 0x11;
        //inputs[1].ki.dwFlags = (int)(KeyCode.KeyDown | KeyCode.Scancode);
        //inputs[1].ki.dwExtraInfo = MouseAndKeyboard.GetMessageExtraInfo();
        //MouseAndKeyboard.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
    }
    IntPtr hWnd;
    System.Drawing.Point point = new();
    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt && e.KeyCode == Keys.P)
        {
            MouseAndKeyboard.GetCursorPos(ref point);
            TB_Test.AppendText($"X={point.X},Y={point.Y};\r\n");

            hWnd = MouseAndKeyboard.WindowFromPoint(point);
            TB_Test.AppendText($"{hWnd}\r\n");

            //MouseAndKeyboard.SendMessage(hWnd, 0x0086, 1, 0);
        }
    }
    Input[] inputs = new Input[2];
    #endregion
}