using MyToolkit;
using System.IO;

namespace IPFSVideo
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        readonly HttpClientAPI apiInstance = new();

        public Form1()
        {
            InitializeComponent();
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
                TB_Info.AppendText($"[{DateTime.Now}]{message}\r\n")));
        }

        private async void BTN_Test_Click(object sender, EventArgs e)
        {
            try
            {
                string result = await apiInstance.DoCommandAsync(HttpClientAPI.BuildCommand(TB_Test.Text));
                ShowMessage(result.ToString());
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
                string result = await apiInstance.AddAsync(FileManager.GetFileStream());
                ShowMessage(result.ToString());
            }
            catch (Exception)
            {

            }
        }

        private async void BTN_Download_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                Stream stream = await apiInstance.DownloadAsync(HttpClientAPI.BuildCommand("cat", TB_CID.Text));
                PB_Image.Image = Image.FromStream(stream);
            }
            catch (Exception)
            {

            }
        }
    }
}