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
            VV_Screen = new LibVLCSharp.WinForms.VideoView();
            TM_Play = new System.Windows.Forms.Timer(components);
            CTB_PlayerTrack = new WinFormsLibrary.CustomTrackBar();
            LB_Duration = new Label();
            toolStrip1 = new ToolStrip();
            TSB_UploadSet = new ToolStripButton();
            TSB_Display = new ToolStripButton();
            TC_Main = new TabControl();
            TP_Video = new TabPage();
            PB_Screen = new PictureBox();
            TP_Data = new TabPage();
            PN_VideoInfo = new Panel();
            ((System.ComponentModel.ISupportInitialize)VV_Screen).BeginInit();
            toolStrip1.SuspendLayout();
            TC_Main.SuspendLayout();
            TP_Video.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PB_Screen).BeginInit();
            TP_Data.SuspendLayout();
            SuspendLayout();
            // 
            // TB_Info
            // 
            resources.ApplyResources(TB_Info, "TB_Info");
            TB_Info.Name = "TB_Info";
            // 
            // BTN_Test
            // 
            resources.ApplyResources(BTN_Test, "BTN_Test");
            BTN_Test.Name = "BTN_Test";
            BTN_Test.UseVisualStyleBackColor = true;
            BTN_Test.Click += BTN_Test_ClickAsync;
            // 
            // TB_CID
            // 
            resources.ApplyResources(TB_CID, "TB_CID");
            TB_CID.Name = "TB_CID";
            // 
            // BTN_Download
            // 
            resources.ApplyResources(BTN_Download, "BTN_Download");
            BTN_Download.Name = "BTN_Download";
            BTN_Download.UseVisualStyleBackColor = true;
            BTN_Download.Click += BTN_Download_ClickAsync;
            // 
            // BTN_Upload
            // 
            resources.ApplyResources(BTN_Upload, "BTN_Upload");
            BTN_Upload.Name = "BTN_Upload";
            BTN_Upload.UseVisualStyleBackColor = true;
            BTN_Upload.Click += BTN_Upload_ClickAsync;
            // 
            // BTN_Play
            // 
            resources.ApplyResources(BTN_Play, "BTN_Play");
            BTN_Play.Name = "BTN_Play";
            BTN_Play.UseVisualStyleBackColor = true;
            BTN_Play.Click += BTN_Play_ClickAsync;
            // 
            // LB_CID
            // 
            resources.ApplyResources(LB_CID, "LB_CID");
            LB_CID.Name = "LB_CID";
            // 
            // TB_Command
            // 
            resources.ApplyResources(TB_Command, "TB_Command");
            TB_Command.Name = "TB_Command";
            // 
            // LB_Command
            // 
            resources.ApplyResources(LB_Command, "LB_Command");
            LB_Command.Name = "LB_Command";
            // 
            // VV_Screen
            // 
            resources.ApplyResources(VV_Screen, "VV_Screen");
            VV_Screen.BackColor = Color.Black;
            VV_Screen.MediaPlayer = null;
            VV_Screen.Name = "VV_Screen";
            // 
            // TM_Play
            // 
            TM_Play.Interval = 1000;
            TM_Play.Tick += TM_Play_Tick;
            // 
            // CTB_PlayerTrack
            // 
            resources.ApplyResources(CTB_PlayerTrack, "CTB_PlayerTrack");
            CTB_PlayerTrack.L_BarColor = Color.FromArgb(224, 224, 224);
            CTB_PlayerTrack.L_BarSize = 10;
            CTB_PlayerTrack.L_IsRound = true;
            CTB_PlayerTrack.L_Maximum = 100;
            CTB_PlayerTrack.L_Minimum = 0;
            CTB_PlayerTrack.L_Orientation = WinFormsLibrary.Orientation.Horizontal_LR;
            CTB_PlayerTrack.L_SliderColor = SystemColors.Highlight;
            CTB_PlayerTrack.L_Value = 0;
            CTB_PlayerTrack.Name = "CTB_PlayerTrack";
            // 
            // LB_Duration
            // 
            resources.ApplyResources(LB_Duration, "LB_Duration");
            LB_Duration.Name = "LB_Duration";
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { TSB_UploadSet, TSB_Display });
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.Name = "toolStrip1";
            // 
            // TSB_UploadSet
            // 
            TSB_UploadSet.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(TSB_UploadSet, "TSB_UploadSet");
            TSB_UploadSet.Name = "TSB_UploadSet";
            TSB_UploadSet.Click += TSB_UploadSet_Click;
            // 
            // TSB_Display
            // 
            TSB_Display.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(TSB_Display, "TSB_Display");
            TSB_Display.Name = "TSB_Display";
            TSB_Display.Click += TSB_Display_Click;
            // 
            // TC_Main
            // 
            resources.ApplyResources(TC_Main, "TC_Main");
            TC_Main.Controls.Add(TP_Video);
            TC_Main.Controls.Add(TP_Data);
            TC_Main.Name = "TC_Main";
            TC_Main.SelectedIndex = 0;
            // 
            // TP_Video
            // 
            TP_Video.Controls.Add(PB_Screen);
            TP_Video.Controls.Add(LB_Command);
            TP_Video.Controls.Add(LB_Duration);
            TP_Video.Controls.Add(TB_Command);
            TP_Video.Controls.Add(VV_Screen);
            TP_Video.Controls.Add(LB_CID);
            TP_Video.Controls.Add(TB_CID);
            TP_Video.Controls.Add(CTB_PlayerTrack);
            TP_Video.Controls.Add(BTN_Play);
            TP_Video.Controls.Add(TB_Info);
            TP_Video.Controls.Add(BTN_Upload);
            TP_Video.Controls.Add(BTN_Download);
            TP_Video.Controls.Add(BTN_Test);
            resources.ApplyResources(TP_Video, "TP_Video");
            TP_Video.Name = "TP_Video";
            TP_Video.UseVisualStyleBackColor = true;
            // 
            // PB_Screen
            // 
            resources.ApplyResources(PB_Screen, "PB_Screen");
            PB_Screen.BackColor = Color.Silver;
            PB_Screen.Name = "PB_Screen";
            PB_Screen.TabStop = false;
            // 
            // TP_Data
            // 
            TP_Data.Controls.Add(PN_VideoInfo);
            resources.ApplyResources(TP_Data, "TP_Data");
            TP_Data.Name = "TP_Data";
            TP_Data.UseVisualStyleBackColor = true;
            // 
            // PN_VideoInfo
            // 
            resources.ApplyResources(PN_VideoInfo, "PN_VideoInfo");
            PN_VideoInfo.Name = "PN_VideoInfo";
            PN_VideoInfo.SizeChanged += PN_VideoInfo_SizeChanged;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(toolStrip1);
            Controls.Add(TC_Main);
            Name = "Form1";
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)VV_Screen).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            TC_Main.ResumeLayout(false);
            TP_Video.ResumeLayout(false);
            TP_Video.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PB_Screen).EndInit();
            TP_Data.ResumeLayout(false);
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
        private LibVLCSharp.WinForms.VideoView VV_Screen;
        private System.Windows.Forms.Timer TM_Play;
        private WinFormsLibrary.CustomTrackBar CTB_PlayerTrack;
        private Label LB_Duration;
        private ToolStrip toolStrip1;
        private ToolStripButton TSB_UploadSet;
        private TabControl TC_Main;
        private TabPage TP_Video;
        private TabPage TP_Data;
        private ToolStripButton TSB_Display;
        private Panel PN_VideoInfo;
        private PictureBox PB_Screen;
    }
}