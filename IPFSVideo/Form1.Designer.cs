namespace IPFSVideo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TB_Info = new TextBox();
            BTN_Test = new Button();
            TB_Test = new TextBox();
            BTN_Upload = new Button();
            BTN_Download = new Button();
            PB_Image = new PictureBox();
            TB_CID = new TextBox();
            LB_CID = new Label();
            ((System.ComponentModel.ISupportInitialize)PB_Image).BeginInit();
            SuspendLayout();
            // 
            // TB_Info
            // 
            TB_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Info.Location = new Point(12, 313);
            TB_Info.Multiline = true;
            TB_Info.Name = "TB_Info";
            TB_Info.ScrollBars = ScrollBars.Vertical;
            TB_Info.Size = new Size(695, 100);
            TB_Info.TabIndex = 0;
            // 
            // BTN_Test
            // 
            BTN_Test.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Test.Location = new Point(713, 419);
            BTN_Test.Name = "BTN_Test";
            BTN_Test.Size = new Size(75, 23);
            BTN_Test.TabIndex = 1;
            BTN_Test.Text = "测试";
            BTN_Test.UseVisualStyleBackColor = true;
            BTN_Test.Click += BTN_Test_Click;
            // 
            // TB_Test
            // 
            TB_Test.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            TB_Test.Location = new Point(556, 419);
            TB_Test.Name = "TB_Test";
            TB_Test.Size = new Size(151, 23);
            TB_Test.TabIndex = 2;
            // 
            // BTN_Upload
            // 
            BTN_Upload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BTN_Upload.Location = new Point(713, 12);
            BTN_Upload.Name = "BTN_Upload";
            BTN_Upload.Size = new Size(75, 23);
            BTN_Upload.TabIndex = 3;
            BTN_Upload.Text = "上传";
            BTN_Upload.UseVisualStyleBackColor = true;
            BTN_Upload.Click += BTN_Upload_ClickAsync;
            // 
            // BTN_Download
            // 
            BTN_Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BTN_Download.Location = new Point(713, 41);
            BTN_Download.Name = "BTN_Download";
            BTN_Download.Size = new Size(75, 23);
            BTN_Download.TabIndex = 4;
            BTN_Download.Text = "下载";
            BTN_Download.UseVisualStyleBackColor = true;
            BTN_Download.Click += BTN_Download_ClickAsync;
            // 
            // PB_Image
            // 
            PB_Image.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PB_Image.Location = new Point(12, 12);
            PB_Image.Name = "PB_Image";
            PB_Image.Size = new Size(695, 295);
            PB_Image.SizeMode = PictureBoxSizeMode.Zoom;
            PB_Image.TabIndex = 5;
            PB_Image.TabStop = false;
            // 
            // TB_CID
            // 
            TB_CID.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_CID.Location = new Point(50, 419);
            TB_CID.Name = "TB_CID";
            TB_CID.Size = new Size(500, 23);
            TB_CID.TabIndex = 6;
            // 
            // LB_CID
            // 
            LB_CID.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LB_CID.AutoSize = true;
            LB_CID.Location = new Point(12, 422);
            LB_CID.Name = "LB_CID";
            LB_CID.Size = new Size(32, 17);
            LB_CID.TabIndex = 7;
            LB_CID.Text = "CID:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LB_CID);
            Controls.Add(TB_CID);
            Controls.Add(PB_Image);
            Controls.Add(BTN_Download);
            Controls.Add(BTN_Upload);
            Controls.Add(TB_Test);
            Controls.Add(BTN_Test);
            Controls.Add(TB_Info);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)PB_Image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TB_Info;
        private Button BTN_Test;
        private TextBox TB_Test;
        private Button BTN_Upload;
        private Button BTN_Download;
        private PictureBox PB_Image;
        private TextBox TB_CID;
        private Label LB_CID;
    }
}