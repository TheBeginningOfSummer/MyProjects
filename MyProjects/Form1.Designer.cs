namespace MyProjects
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
            this.PB_MyPicture = new System.Windows.Forms.PictureBox();
            this.BTN_Test = new System.Windows.Forms.Button();
            this.TB_Info = new System.Windows.Forms.TextBox();
            this.BTN_ProcessInput = new System.Windows.Forms.Button();
            this.TB_ConsoleInput = new System.Windows.Forms.TextBox();
            this.GB_ConsoleTest = new System.Windows.Forms.GroupBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.BTN_ReadImage = new System.Windows.Forms.Button();
            this.BTN_ReadVideo = new System.Windows.Forms.Button();
            this.BTN_Test2 = new System.Windows.Forms.Button();
            this.BTN_Test3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MyPicture)).BeginInit();
            this.GB_ConsoleTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // PB_MyPicture
            // 
            this.PB_MyPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_MyPicture.Location = new System.Drawing.Point(12, 12);
            this.PB_MyPicture.Name = "PB_MyPicture";
            this.PB_MyPicture.Size = new System.Drawing.Size(592, 457);
            this.PB_MyPicture.TabIndex = 0;
            this.PB_MyPicture.TabStop = false;
            // 
            // BTN_Test
            // 
            this.BTN_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Test.Location = new System.Drawing.Point(799, 366);
            this.BTN_Test.Name = "BTN_Test";
            this.BTN_Test.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test.TabIndex = 1;
            this.BTN_Test.Text = "测试";
            this.BTN_Test.UseVisualStyleBackColor = true;
            this.BTN_Test.Click += new System.EventHandler(this.BTN_Test_Click);
            // 
            // TB_Info
            // 
            this.TB_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Info.Location = new System.Drawing.Point(608, 12);
            this.TB_Info.Margin = new System.Windows.Forms.Padding(1);
            this.TB_Info.Multiline = true;
            this.TB_Info.Name = "TB_Info";
            this.TB_Info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Info.Size = new System.Drawing.Size(266, 350);
            this.TB_Info.TabIndex = 2;
            // 
            // BTN_ProcessInput
            // 
            this.BTN_ProcessInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_ProcessInput.Location = new System.Drawing.Point(3, 48);
            this.BTN_ProcessInput.Name = "BTN_ProcessInput";
            this.BTN_ProcessInput.Size = new System.Drawing.Size(106, 23);
            this.BTN_ProcessInput.TabIndex = 3;
            this.BTN_ProcessInput.Text = "程序输入写入";
            this.BTN_ProcessInput.UseVisualStyleBackColor = true;
            this.BTN_ProcessInput.Click += new System.EventHandler(this.BTN_ProcessInput_Click);
            // 
            // TB_ConsoleInput
            // 
            this.TB_ConsoleInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.TB_ConsoleInput.Location = new System.Drawing.Point(3, 19);
            this.TB_ConsoleInput.Name = "TB_ConsoleInput";
            this.TB_ConsoleInput.Size = new System.Drawing.Size(106, 23);
            this.TB_ConsoleInput.TabIndex = 4;
            // 
            // GB_ConsoleTest
            // 
            this.GB_ConsoleTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_ConsoleTest.Controls.Add(this.TB_ConsoleInput);
            this.GB_ConsoleTest.Controls.Add(this.BTN_ProcessInput);
            this.GB_ConsoleTest.Location = new System.Drawing.Point(610, 366);
            this.GB_ConsoleTest.Name = "GB_ConsoleTest";
            this.GB_ConsoleTest.Size = new System.Drawing.Size(112, 74);
            this.GB_ConsoleTest.TabIndex = 5;
            this.GB_ConsoleTest.TabStop = false;
            this.GB_ConsoleTest.Text = "控制台程序测试";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BTN_ReadImage
            // 
            this.BTN_ReadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_ReadImage.Location = new System.Drawing.Point(610, 446);
            this.BTN_ReadImage.Name = "BTN_ReadImage";
            this.BTN_ReadImage.Size = new System.Drawing.Size(75, 23);
            this.BTN_ReadImage.TabIndex = 6;
            this.BTN_ReadImage.Text = "读取图片";
            this.BTN_ReadImage.UseVisualStyleBackColor = true;
            this.BTN_ReadImage.Click += new System.EventHandler(this.BTN_ReadImage_Click);
            // 
            // BTN_ReadVideo
            // 
            this.BTN_ReadVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_ReadVideo.Location = new System.Drawing.Point(691, 446);
            this.BTN_ReadVideo.Name = "BTN_ReadVideo";
            this.BTN_ReadVideo.Size = new System.Drawing.Size(75, 23);
            this.BTN_ReadVideo.TabIndex = 7;
            this.BTN_ReadVideo.Text = "读取视频";
            this.BTN_ReadVideo.UseVisualStyleBackColor = true;
            this.BTN_ReadVideo.Click += new System.EventHandler(this.BTN_ReadVideo_Click);
            // 
            // BTN_Test2
            // 
            this.BTN_Test2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Test2.Location = new System.Drawing.Point(799, 395);
            this.BTN_Test2.Name = "BTN_Test2";
            this.BTN_Test2.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test2.TabIndex = 8;
            this.BTN_Test2.Text = "测试2";
            this.BTN_Test2.UseVisualStyleBackColor = true;
            this.BTN_Test2.Click += new System.EventHandler(this.BTN_Test2_Click);
            // 
            // BTN_Test3
            // 
            this.BTN_Test3.Location = new System.Drawing.Point(799, 424);
            this.BTN_Test3.Name = "BTN_Test3";
            this.BTN_Test3.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test3.TabIndex = 9;
            this.BTN_Test3.Text = "测试3";
            this.BTN_Test3.UseVisualStyleBackColor = true;
            this.BTN_Test3.Click += new System.EventHandler(this.BTN_Test3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 481);
            this.Controls.Add(this.BTN_Test3);
            this.Controls.Add(this.BTN_Test2);
            this.Controls.Add(this.BTN_ReadVideo);
            this.Controls.Add(this.BTN_ReadImage);
            this.Controls.Add(this.GB_ConsoleTest);
            this.Controls.Add(this.TB_Info);
            this.Controls.Add(this.BTN_Test);
            this.Controls.Add(this.PB_MyPicture);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PB_MyPicture)).EndInit();
            this.GB_ConsoleTest.ResumeLayout(false);
            this.GB_ConsoleTest.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox PB_MyPicture;
        private Button BTN_Test;
        private TextBox TB_Info;
        private Button BTN_ProcessInput;
        private TextBox TB_ConsoleInput;
        private GroupBox GB_ConsoleTest;
        private OpenFileDialog openFileDialog1;
        private Button BTN_ReadImage;
        private Button BTN_ReadVideo;
        private Button BTN_Test2;
        private Button BTN_Test3;
    }
}