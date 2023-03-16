using MyToolkit;

namespace IPFSVideo
{
    public partial class Form1 : Form
    {
        readonly ProcessToolkit ipfsProcess = new("ipfs");
        HttpClientAPI clientAPI = new HttpClientAPI();

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
                //string result = clientAPI.DoCommandAsync(HttpClientAPI.BuildCommand(TB_Test.Text)).Result;
                string result = await clientAPI.UploadAsync(HttpClientAPI.BuildCommand("add"),
                    new StreamContent(FileManager.GetFileStream()));
                //Stream stream = await clientAPI.DownloadAsync(HttpClientAPI.BuildCommand("cat", "Qmb3Ln3gkSthhbNGmSPArwn83wLAfnEQ6pf1JLAq7mv6KJ"));
                ShowMessage(result.ToString());
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }
    }
}