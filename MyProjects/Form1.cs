using MyToolkit;
using STTech.BytesIO.Tcp;
using System.Collections.Concurrent;

namespace MyProjects
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit testProcess = new("ipfs");
        //readonly ConnectionToolkit.SocketConnection server = new("127.0.0.1", 5000);
        readonly TcpServer bytesIOServer = new();
        #region ���紫������ƴ�ӡ���������տ���
        readonly Dictionary<string, MemoryStream> memoryStreamDic = new();
        readonly ConcurrentQueue<UnpackagedMessage> cache = new();
        readonly ManualResetEvent dataParseSwitch = new(false);
        #endregion
        public Form1()
        {
            InitializeComponent();
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
                //TB_Info.AppendText($"����˼�����ʼ{Environment.NewLine}");

                //testProcess.TargetProcess.OutputDataReceived += TargetProcess_OutputDataReceived;
                //testProcess.StartProcess("E:\\PythonProjects\\OpenCVTest\\dist\\main\\main.exe");
                //ShowMessage($"����̨�����������");
            }
            catch (Exception ex)
            {
                TB_Info.AppendText($"��ʼ������:{ex.Message}{Environment.NewLine}");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            bytesIOServer.CloseAsync();
        }

        #region TCPͨ���¼�
        private void BytesIOServer_ClientDisconnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientDisconnectedEventArgs e)
        {
            ShowMessage($"�ͻ���[{e.Client.Host}:{e.Client.Port}]�Ͽ��ɹ�");
        }

        private void BytesIOServer_ClientConnected(object? sender, STTech.BytesIO.Tcp.Entity.ClientConnectedEventArgs e)
        {
            ShowMessage($"�ͻ���[{e.Client.Host}:{e.Client.Port}]���ӳɹ�");
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
            ShowMessage("ֹͣ����");
        }

        private void BytesIOServer_Started(object? sender, EventArgs e)
        {
            ShowMessage("��ʼ����");
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
            //PB_MyPicture.Image = Image.FromStream
            //    (new MemoryStream(ImageToolkit.GetBinary("C:\\Users\\1\\Desktop\\autumn.jpg")));
            if (PB_MyPicture.Image != null)
                PB_MyPicture.Image = null;
            TB_Info.Clear();
        }

        private void BTN_ProcessInput_Click(object sender, EventArgs e)
        {
            if (TB_ConsoleInput.Text == "") return;
            testProcess.ProcessInput(TB_ConsoleInput.Text);
        }

        
    }
}