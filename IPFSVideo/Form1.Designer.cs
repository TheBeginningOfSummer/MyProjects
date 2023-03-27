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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TB_Info = new TextBox();
            BTN_Test = new Button();
            TB_CID = new TextBox();
            BTN_Download = new Button();
            BTN_Upload = new Button();
            BTN_Play = new Button();
            LB_CID = new Label();
            TB_Command = new TextBox();
            LB_Command = new Label();
            PB_Screen = new PictureBox();
            VV_Screen = new LibVLCSharp.WinForms.VideoView();
            TM_Play = new System.Windows.Forms.Timer(components);
            CTB_PlayerTrack = new WinFormsLibrary.CustomTrackBar();
            LB_Duration = new Label();
            toolStrip1 = new ToolStrip();
            TSB_UploadSet = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)PB_Screen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VV_Screen).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // TB_Info
            // 
            TB_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Info.Location = new Point(12, 271);
            TB_Info.Multiline = true;
            TB_Info.Name = "TB_Info";
            TB_Info.ScrollBars = ScrollBars.Vertical;
            TB_Info.Size = new Size(695, 142);
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
            BTN_Test.Click += BTN_Test_ClickAsync;
            // 
            // TB_CID
            // 
            TB_CID.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            TB_CID.Location = new Point(48, 419);
            TB_CID.Name = "TB_CID";
            TB_CID.Size = new Size(365, 23);
            TB_CID.TabIndex = 2;
            TB_CID.Text = "QmQYpBaAdjmkqAHsXbfawF7PHUKzNf6o5LVYJUVGfaXyJG";
            // 
            // BTN_Download
            // 
            BTN_Download.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Download.Location = new Point(713, 361);
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
            BTN_Upload.Location = new Point(713, 332);
            BTN_Upload.Name = "BTN_Upload";
            BTN_Upload.Size = new Size(75, 23);
            BTN_Upload.TabIndex = 5;
            BTN_Upload.Text = "上传";
            BTN_Upload.UseVisualStyleBackColor = true;
            BTN_Upload.Click += BTN_Upload_ClickAsync;
            // 
            // BTN_Play
            // 
            BTN_Play.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Play.Location = new Point(713, 390);
            BTN_Play.Name = "BTN_Play";
            BTN_Play.Size = new Size(75, 23);
            BTN_Play.TabIndex = 6;
            BTN_Play.Text = "播放";
            BTN_Play.UseVisualStyleBackColor = true;
            BTN_Play.Click += BTN_Play_ClickAsync;
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
            // TB_Command
            // 
            TB_Command.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Command.Location = new Point(496, 419);
            TB_Command.Name = "TB_Command";
            TB_Command.Size = new Size(211, 23);
            TB_Command.TabIndex = 8;
            // 
            // LB_Command
            // 
            LB_Command.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LB_Command.AutoSize = true;
            LB_Command.Location = new Point(419, 422);
            LB_Command.Name = "LB_Command";
            LB_Command.Size = new Size(71, 17);
            LB_Command.TabIndex = 9;
            LB_Command.Text = "Command:";
            // 
            // PB_Screen
            // 
            PB_Screen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            PB_Screen.Location = new Point(549, 28);
            PB_Screen.Name = "PB_Screen";
            PB_Screen.Size = new Size(158, 221);
            PB_Screen.SizeMode = PictureBoxSizeMode.Zoom;
            PB_Screen.TabIndex = 10;
            PB_Screen.TabStop = false;
            // 
            // VV_Screen
            // 
            VV_Screen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            VV_Screen.BackColor = Color.Black;
            VV_Screen.Location = new Point(12, 28);
            VV_Screen.MediaPlayer = null;
            VV_Screen.Name = "VV_Screen";
            VV_Screen.Size = new Size(531, 221);
            VV_Screen.TabIndex = 11;
            VV_Screen.Text = "videoView1";
            // 
            // TM_Play
            // 
            TM_Play.Interval = 1000;
            TM_Play.Tick += TM_Play_Tick;
            // 
            // CTB_PlayerTrack
            // 
            CTB_PlayerTrack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CTB_PlayerTrack.L_BarColor = Color.FromArgb(224, 224, 224);
            CTB_PlayerTrack.L_BarSize = 10;
            CTB_PlayerTrack.L_IsRound = true;
            CTB_PlayerTrack.L_Maximum = 100;
            CTB_PlayerTrack.L_Minimum = 0;
            CTB_PlayerTrack.L_Orientation = WinFormsLibrary.Orientation.Horizontal_LR;
            CTB_PlayerTrack.L_SliderColor = SystemColors.Highlight;
            CTB_PlayerTrack.L_Value = 0;
            CTB_PlayerTrack.Location = new Point(121, 255);
            CTB_PlayerTrack.Name = "CTB_PlayerTrack";
            CTB_PlayerTrack.Size = new Size(424, 10);
            CTB_PlayerTrack.TabIndex = 12;
            CTB_PlayerTrack.Text = "customTrackBar1";
            // 
            // LB_Duration
            // 
            LB_Duration.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LB_Duration.AutoSize = true;
            LB_Duration.Location = new Point(12, 252);
            LB_Duration.Name = "LB_Duration";
            LB_Duration.Size = new Size(109, 17);
            LB_Duration.TabIndex = 13;
            LB_Duration.Text = "00:00:00/00:00:00";
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { TSB_UploadSet });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 14;
            toolStrip1.Text = "toolStrip1";
            // 
            // TSB_UploadSet
            // 
            TSB_UploadSet.DisplayStyle = ToolStripItemDisplayStyle.Text;
            TSB_UploadSet.Image = (Image)resources.GetObject("TSB_UploadSet.Image");
            TSB_UploadSet.ImageTransparentColor = Color.Magenta;
            TSB_UploadSet.Name = "TSB_UploadSet";
            TSB_UploadSet.Size = new Size(60, 22);
            TSB_UploadSet.Text = "上传设置";
            TSB_UploadSet.Click += TSB_UploadSet_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip1);
            Controls.Add(LB_Duration);
            Controls.Add(CTB_PlayerTrack);
            Controls.Add(VV_Screen);
            Controls.Add(PB_Screen);
            Controls.Add(LB_Command);
            Controls.Add(TB_Command);
            Controls.Add(LB_CID);
            Controls.Add(BTN_Play);
            Controls.Add(BTN_Upload);
            Controls.Add(BTN_Download);
            Controls.Add(TB_CID);
            Controls.Add(BTN_Test);
            Controls.Add(TB_Info);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IPFS";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)PB_Screen).EndInit();
            ((System.ComponentModel.ISupportInitialize)VV_Screen).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TB_Info;
        private Button BTN_Test;
        private TextBox TB_CID;
        private Button BTN_Download;
        private Button BTN_Upload;
        private Button BTN_Play;
        private Label LB_CID;
        private TextBox TB_Command;
        private Label LB_Command;
        private PictureBox PB_Screen;
        private LibVLCSharp.WinForms.VideoView VV_Screen;
        private System.Windows.Forms.Timer TM_Play;
        private WinFormsLibrary.CustomTrackBar CTB_PlayerTrack;
        private Label LB_Duration;
        private ToolStrip toolStrip1;
        private ToolStripButton TSB_UploadSet;
    }
}