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
            PB_MyPicture = new PictureBox();
            BTN_Test = new Button();
            TB_Info = new TextBox();
            BTN_ProcessInput = new Button();
            TB_ConsoleInput = new TextBox();
            GB_ConsoleTest = new GroupBox();
            openFileDialog1 = new OpenFileDialog();
            BTN_ReadImage = new Button();
            BTN_ReadVideo = new Button();
            BTN_Test2 = new Button();
            BTN_Test3 = new Button();
            RB_Line = new RadioButton();
            RB_Rectangle = new RadioButton();
            RB_Circle = new RadioButton();
            TC_Main = new TabControl();
            TP_CV = new TabPage();
            BTN_CVStop = new Button();
            BTN_LocalCamera = new Button();
            GB_Test = new GroupBox();
            GB_FigureSelect = new GroupBox();
            TP_设置 = new TabPage();
            ((System.ComponentModel.ISupportInitialize)PB_MyPicture).BeginInit();
            GB_ConsoleTest.SuspendLayout();
            TC_Main.SuspendLayout();
            TP_CV.SuspendLayout();
            GB_Test.SuspendLayout();
            GB_FigureSelect.SuspendLayout();
            SuspendLayout();
            // 
            // PB_MyPicture
            // 
            PB_MyPicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PB_MyPicture.Location = new Point(6, 6);
            PB_MyPicture.Name = "PB_MyPicture";
            PB_MyPicture.Size = new Size(624, 321);
            PB_MyPicture.TabIndex = 0;
            PB_MyPicture.TabStop = false;
            // 
            // BTN_Test
            // 
            BTN_Test.Location = new Point(6, 20);
            BTN_Test.Name = "BTN_Test";
            BTN_Test.Size = new Size(75, 23);
            BTN_Test.TabIndex = 1;
            BTN_Test.Text = "测试";
            BTN_Test.UseVisualStyleBackColor = true;
            BTN_Test.Click += BTN_Test_Click;
            // 
            // TB_Info
            // 
            TB_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Info.Location = new Point(6, 331);
            TB_Info.Margin = new Padding(1);
            TB_Info.Multiline = true;
            TB_Info.Name = "TB_Info";
            TB_Info.ScrollBars = ScrollBars.Vertical;
            TB_Info.Size = new Size(624, 114);
            TB_Info.TabIndex = 2;
            // 
            // BTN_ProcessInput
            // 
            BTN_ProcessInput.Dock = DockStyle.Bottom;
            BTN_ProcessInput.Location = new Point(3, 48);
            BTN_ProcessInput.Name = "BTN_ProcessInput";
            BTN_ProcessInput.Size = new Size(106, 23);
            BTN_ProcessInput.TabIndex = 3;
            BTN_ProcessInput.Text = "程序输入写入";
            BTN_ProcessInput.UseVisualStyleBackColor = true;
            BTN_ProcessInput.Click += BTN_ProcessInput_Click;
            // 
            // TB_ConsoleInput
            // 
            TB_ConsoleInput.Dock = DockStyle.Top;
            TB_ConsoleInput.Location = new Point(3, 19);
            TB_ConsoleInput.Name = "TB_ConsoleInput";
            TB_ConsoleInput.Size = new Size(106, 23);
            TB_ConsoleInput.TabIndex = 4;
            // 
            // GB_ConsoleTest
            // 
            GB_ConsoleTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GB_ConsoleTest.Controls.Add(TB_ConsoleInput);
            GB_ConsoleTest.Controls.Add(BTN_ProcessInput);
            GB_ConsoleTest.Location = new Point(636, 6);
            GB_ConsoleTest.Name = "GB_ConsoleTest";
            GB_ConsoleTest.Size = new Size(112, 74);
            GB_ConsoleTest.TabIndex = 5;
            GB_ConsoleTest.TabStop = false;
            GB_ConsoleTest.Text = "控制台程序测试";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // BTN_ReadImage
            // 
            BTN_ReadImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_ReadImage.Location = new Point(634, 420);
            BTN_ReadImage.Name = "BTN_ReadImage";
            BTN_ReadImage.Size = new Size(75, 23);
            BTN_ReadImage.TabIndex = 6;
            BTN_ReadImage.Text = "读取图片";
            BTN_ReadImage.UseVisualStyleBackColor = true;
            BTN_ReadImage.Click += BTN_ReadImage_Click;
            // 
            // BTN_ReadVideo
            // 
            BTN_ReadVideo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_ReadVideo.Location = new Point(715, 420);
            BTN_ReadVideo.Name = "BTN_ReadVideo";
            BTN_ReadVideo.Size = new Size(75, 23);
            BTN_ReadVideo.TabIndex = 7;
            BTN_ReadVideo.Text = "读取视频";
            BTN_ReadVideo.UseVisualStyleBackColor = true;
            BTN_ReadVideo.Click += BTN_ReadVideo_Click;
            // 
            // BTN_Test2
            // 
            BTN_Test2.Location = new Point(6, 47);
            BTN_Test2.Name = "BTN_Test2";
            BTN_Test2.Size = new Size(75, 23);
            BTN_Test2.TabIndex = 8;
            BTN_Test2.Text = "测试2";
            BTN_Test2.UseVisualStyleBackColor = true;
            BTN_Test2.Click += BTN_Test2_Click;
            // 
            // BTN_Test3
            // 
            BTN_Test3.Location = new Point(6, 74);
            BTN_Test3.Name = "BTN_Test3";
            BTN_Test3.Size = new Size(75, 23);
            BTN_Test3.TabIndex = 9;
            BTN_Test3.Text = "测试3";
            BTN_Test3.UseVisualStyleBackColor = true;
            BTN_Test3.Click += BTN_Test3_Click;
            // 
            // RB_Line
            // 
            RB_Line.AutoSize = true;
            RB_Line.Checked = true;
            RB_Line.Location = new Point(6, 22);
            RB_Line.Name = "RB_Line";
            RB_Line.Size = new Size(50, 21);
            RB_Line.TabIndex = 10;
            RB_Line.TabStop = true;
            RB_Line.Text = "直线";
            RB_Line.UseVisualStyleBackColor = true;
            RB_Line.CheckedChanged += RB_Line_CheckedChanged;
            // 
            // RB_Rectangle
            // 
            RB_Rectangle.AutoSize = true;
            RB_Rectangle.Location = new Point(6, 49);
            RB_Rectangle.Name = "RB_Rectangle";
            RB_Rectangle.Size = new Size(50, 21);
            RB_Rectangle.TabIndex = 11;
            RB_Rectangle.Text = "矩形";
            RB_Rectangle.UseVisualStyleBackColor = true;
            RB_Rectangle.CheckedChanged += RB_Line_CheckedChanged;
            // 
            // RB_Circle
            // 
            RB_Circle.AutoSize = true;
            RB_Circle.Location = new Point(6, 76);
            RB_Circle.Name = "RB_Circle";
            RB_Circle.Size = new Size(50, 21);
            RB_Circle.TabIndex = 12;
            RB_Circle.Text = "圆形";
            RB_Circle.UseVisualStyleBackColor = true;
            RB_Circle.CheckedChanged += RB_Line_CheckedChanged;
            // 
            // TC_Main
            // 
            TC_Main.Controls.Add(TP_CV);
            TC_Main.Controls.Add(TP_设置);
            TC_Main.Dock = DockStyle.Fill;
            TC_Main.Location = new Point(0, 0);
            TC_Main.Name = "TC_Main";
            TC_Main.SelectedIndex = 0;
            TC_Main.Size = new Size(884, 481);
            TC_Main.TabIndex = 13;
            // 
            // TP_CV
            // 
            TP_CV.Controls.Add(BTN_CVStop);
            TP_CV.Controls.Add(BTN_LocalCamera);
            TP_CV.Controls.Add(GB_Test);
            TP_CV.Controls.Add(GB_FigureSelect);
            TP_CV.Controls.Add(PB_MyPicture);
            TP_CV.Controls.Add(TB_Info);
            TP_CV.Controls.Add(BTN_ReadVideo);
            TP_CV.Controls.Add(GB_ConsoleTest);
            TP_CV.Controls.Add(BTN_ReadImage);
            TP_CV.Location = new Point(4, 26);
            TP_CV.Name = "TP_CV";
            TP_CV.Padding = new Padding(3);
            TP_CV.Size = new Size(876, 451);
            TP_CV.TabIndex = 0;
            TP_CV.Text = "CV";
            TP_CV.UseVisualStyleBackColor = true;
            // 
            // BTN_CVStop
            // 
            BTN_CVStop.Location = new Point(795, 391);
            BTN_CVStop.Name = "BTN_CVStop";
            BTN_CVStop.Size = new Size(75, 23);
            BTN_CVStop.TabIndex = 16;
            BTN_CVStop.Text = "停止";
            BTN_CVStop.UseVisualStyleBackColor = true;
            BTN_CVStop.Click += BTN_CVStop_Click;
            // 
            // BTN_LocalCamera
            // 
            BTN_LocalCamera.Location = new Point(715, 391);
            BTN_LocalCamera.Name = "BTN_LocalCamera";
            BTN_LocalCamera.Size = new Size(75, 23);
            BTN_LocalCamera.TabIndex = 15;
            BTN_LocalCamera.Text = "本地相机";
            BTN_LocalCamera.UseVisualStyleBackColor = true;
            BTN_LocalCamera.Click += BTN_LocalCamera_Click;
            // 
            // GB_Test
            // 
            GB_Test.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GB_Test.Controls.Add(BTN_Test);
            GB_Test.Controls.Add(BTN_Test2);
            GB_Test.Controls.Add(BTN_Test3);
            GB_Test.Location = new Point(720, 86);
            GB_Test.Name = "GB_Test";
            GB_Test.Size = new Size(98, 106);
            GB_Test.TabIndex = 14;
            GB_Test.TabStop = false;
            GB_Test.Text = "测试";
            // 
            // GB_FigureSelect
            // 
            GB_FigureSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GB_FigureSelect.Controls.Add(RB_Line);
            GB_FigureSelect.Controls.Add(RB_Rectangle);
            GB_FigureSelect.Controls.Add(RB_Circle);
            GB_FigureSelect.Location = new Point(639, 86);
            GB_FigureSelect.Name = "GB_FigureSelect";
            GB_FigureSelect.Size = new Size(75, 106);
            GB_FigureSelect.TabIndex = 14;
            GB_FigureSelect.TabStop = false;
            GB_FigureSelect.Text = "图形";
            // 
            // TP_设置
            // 
            TP_设置.Location = new Point(4, 26);
            TP_设置.Name = "TP_设置";
            TP_设置.Padding = new Padding(3);
            TP_设置.Size = new Size(876, 451);
            TP_设置.TabIndex = 1;
            TP_设置.Text = "设置";
            TP_设置.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 481);
            Controls.Add(TC_Main);
            Name = "Form1";
            Text = "Form1";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)PB_MyPicture).EndInit();
            GB_ConsoleTest.ResumeLayout(false);
            GB_ConsoleTest.PerformLayout();
            TC_Main.ResumeLayout(false);
            TP_CV.ResumeLayout(false);
            TP_CV.PerformLayout();
            GB_Test.ResumeLayout(false);
            GB_FigureSelect.ResumeLayout(false);
            GB_FigureSelect.PerformLayout();
            ResumeLayout(false);
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
        private RadioButton RB_Line;
        private RadioButton RB_Rectangle;
        private RadioButton RB_Circle;
        private TabControl TC_Main;
        private TabPage TP_CV;
        private GroupBox GB_Test;
        private GroupBox GB_FigureSelect;
        private TabPage TP_设置;
        private Button BTN_LocalCamera;
        private Button BTN_CVStop;
    }
}