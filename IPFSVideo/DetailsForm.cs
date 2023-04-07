using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPFSVideo
{
    public partial class DetailsForm : Form
    {
        public List<LinkLabel> Links = new();

        public DetailsForm()
        {
            InitializeComponent();
        }

        public void AddLinks(string text, Point point)
        {
            var link = new LinkLabel()
            {
                AutoSize = true,
                Text = text,
                Location = point,
                ContextMenuStrip = CMS_RightClick
            };
            link.LinkClicked += LinkLabel_LinkClicked;
            PN_DetailsPanel.Controls.Add(link);
            Links.Add(link);
        }

        public void UpdateControls()
        {
            foreach (Control link in Links)
            {
                PN_DetailsPanel.Controls.Add(link);
            }
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LinkLabel? link = sender as LinkLabel;
                System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", $"http://localhost:8080/ipfs/{link!.Text}");
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cid = ((LinkLabel)CMS_RightClick.SourceControl).Text;
            Clipboard.SetText(cid);
            //Clipboard.SetDataObject(cid, true);
        }

        private void DetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PN_DetailsPanel.Controls.Clear();
        }
    }
}
