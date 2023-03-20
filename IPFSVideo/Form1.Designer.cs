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
            PB_Image = new PictureBox();
            BTN_Download = new Button();
            BTN_Upload = new Button();
            ((System.ComponentModel.ISupportInitialize)PB_Image).BeginInit();
            SuspendLayout();
            // 
            // TB_Info
            // 
            TB_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Info.Location = new Point(12, 327);
            TB_Info.Multiline = true;
            TB_Info.Name = "TB_Info";
            TB_Info.ScrollBars = ScrollBars.Vertical;
            TB_Info.Size = new Size(695, 86);
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
            TB_Test.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Test.Location = new Point(12, 419);
            TB_Test.Name = "TB_Test";
            TB_Test.Size = new Size(695, 23);
            TB_Test.TabIndex = 2;
            // 
            // PB_Image
            // 
            PB_Image.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PB_Image.Location = new Point(12, 12);
            PB_Image.Name = "PB_Image";
            PB_Image.Size = new Size(695, 309);
            PB_Image.SizeMode = PictureBoxSizeMode.Zoom;
            PB_Image.TabIndex = 3;
            PB_Image.TabStop = false;
            // 
            // BTN_Download
            // 
            BTN_Download.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Download.Location = new Point(713, 390);
            BTN_Download.Name = "BTN_Download";
            BTN_Download.Size = new Size(75, 23);
            BTN_Download.TabIndex = 4;
            BTN_Download.Text = "下载";
            BTN_Download.UseVisualStyleBackColor = true;
            BTN_Download.Click += BTN_Download_ClickAsync;
            // 
            // BTN_Upload
            // 
            BTN_Upload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Upload.Location = new Point(713, 361);
            BTN_Upload.Name = "BTN_Upload";
            BTN_Upload.Size = new Size(75, 23);
            BTN_Upload.TabIndex = 5;
            BTN_Upload.Text = "上传";
            BTN_Upload.UseVisualStyleBackColor = true;
            BTN_Upload.Click += BTN_Upload_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BTN_Upload);
            Controls.Add(BTN_Download);
            Controls.Add(PB_Image);
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
        private PictureBox PB_Image;
        private Button BTN_Download;
        private Button BTN_Upload;
    }
}