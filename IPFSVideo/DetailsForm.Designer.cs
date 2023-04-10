namespace IPFSVideo
{
    partial class DetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            CMS_RightClick = new ContextMenuStrip(components);
            复制ToolStripMenuItem = new ToolStripMenuItem();
            PN_DetailsPanel = new Panel();
            CMS_RightClick.SuspendLayout();
            SuspendLayout();
            // 
            // CMS_RightClick
            // 
            CMS_RightClick.Items.AddRange(new ToolStripItem[] { 复制ToolStripMenuItem });
            CMS_RightClick.Name = "CMS_RightClick";
            CMS_RightClick.Size = new Size(101, 26);
            // 
            // 复制ToolStripMenuItem
            // 
            复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            复制ToolStripMenuItem.Size = new Size(100, 22);
            复制ToolStripMenuItem.Text = "复制";
            复制ToolStripMenuItem.Click += 复制ToolStripMenuItem_Click;
            // 
            // PN_DetailsPanel
            // 
            PN_DetailsPanel.AutoScroll = true;
            PN_DetailsPanel.Location = new Point(12, 12);
            PN_DetailsPanel.Name = "PN_DetailsPanel";
            PN_DetailsPanel.Size = new Size(776, 426);
            PN_DetailsPanel.TabIndex = 2;
            // 
            // DetailsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(PN_DetailsPanel);
            Name = "DetailsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DetailsForm";
            FormClosing += DetailsForm_FormClosing;
            CMS_RightClick.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip CMS_RightClick;
        private ToolStripMenuItem 复制ToolStripMenuItem;
        private Panel PN_DetailsPanel;
    }
}