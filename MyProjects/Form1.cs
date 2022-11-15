using MyToolkit;

namespace MyProjects
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit testProcess = new();
        readonly ConnectionToolkit.SocketConnection server = new("127.0.0.1", 5000);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                testProcess.TargetProcess.OutputDataReceived += TargetProcess_OutputDataReceived;
                testProcess.StartProcess("E:\\PythonProjects\\OpenCVTest\\dist\\main\\main.exe");
                TB_Info.AppendText($"控制台程序启动完成{Environment.NewLine}");
                server.ReceiveFromClient += UpdateClientInfo;
                server.ClientListUpdate += UpdateClientList;
                server.StartListening();
                TB_Info.AppendText($"服务端监听开始{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                TB_Info.AppendText($"初始化错误:{ex.Message}{Environment.NewLine}");
            }
        }

        private void TargetProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                TB_Info.Invoke(new Action(() =>
                TB_Info.AppendText(e.Data + Environment.NewLine)));
        }

        private void UpdateClientInfo(System.Net.Sockets.Socket client, byte[] data)
        {

        }

        private void UpdateClientList()
        {

        }

        private void BTN_Test_Click(object sender, EventArgs e)
        {
            PB_MyPicture.Image = Image.FromStream
                (new MemoryStream(ImageToolkit.GetBinary("C:\\Users\\1\\Desktop\\autumn.jpg")));
        }

        private void BTN_ProcessInput_Click(object sender, EventArgs e)
        {
            if (TB_ConsoleInput.Text == "") return;
            testProcess.ProcessInput(TB_ConsoleInput.Text);
        }
    }
}