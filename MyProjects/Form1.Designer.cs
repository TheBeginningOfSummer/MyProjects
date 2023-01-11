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
            this.RB_Line = new System.Windows.Forms.RadioButton();
            this.RB_Rectangle = new System.Windows.Forms.RadioButton();
            this.RB_Circle = new System.Windows.Forms.RadioButton();
            this.TC_Main = new System.Windows.Forms.TabControl();
            this.TB_CV = new System.Windows.Forms.TabPage();
            this.GB_Test = new System.Windows.Forms.GroupBox();
            this.GB_FigureSelect = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MyPicture)).BeginInit();
            this.GB_ConsoleTest.SuspendLayout();
            this.TC_Main.SuspendLayout();
            this.TB_CV.SuspendLayout();
            this.GB_Test.SuspendLayout();
            this.GB_FigureSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // PB_MyPicture
            // 
            this.PB_MyPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_MyPicture.Location = new System.Drawing.Point(6, 6);
            this.PB_MyPicture.Name = "PB_MyPicture";
            this.PB_MyPicture.Size = new System.Drawing.Size(624, 321);
            this.PB_MyPicture.TabIndex = 0;
            this.PB_MyPicture.TabStop = false;
            // 
            // BTN_Test
            // 
            this.BTN_Test.Location = new System.Drawing.Point(6, 20);
            this.BTN_Test.Name = "BTN_Test";
            this.BTN_Test.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test.TabIndex = 1;
            this.BTN_Test.Text = "测试";
            this.BTN_Test.UseVisualStyleBackColor = true;
            this.BTN_Test.Click += new System.EventHandler(this.BTN_Test_Click);
            // 
            // TB_Info
            // 
            this.TB_Info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Info.Location = new System.Drawing.Point(6, 331);
            this.TB_Info.Margin = new System.Windows.Forms.Padding(1);
            this.TB_Info.Multiline = true;
            this.TB_Info.Name = "TB_Info";
            this.TB_Info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Info.Size = new System.Drawing.Size(624, 114);
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
            this.GB_ConsoleTest.Location = new System.Drawing.Point(636, 6);
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
            this.BTN_ReadImage.Location = new System.Drawing.Point(634, 420);
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
            this.BTN_ReadVideo.Location = new System.Drawing.Point(715, 420);
            this.BTN_ReadVideo.Name = "BTN_ReadVideo";
            this.BTN_ReadVideo.Size = new System.Drawing.Size(75, 23);
            this.BTN_ReadVideo.TabIndex = 7;
            this.BTN_ReadVideo.Text = "读取视频";
            this.BTN_ReadVideo.UseVisualStyleBackColor = true;
            this.BTN_ReadVideo.Click += new System.EventHandler(this.BTN_ReadVideo_Click);
            // 
            // BTN_Test2
            // 
            this.BTN_Test2.Location = new System.Drawing.Point(6, 47);
            this.BTN_Test2.Name = "BTN_Test2";
            this.BTN_Test2.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test2.TabIndex = 8;
            this.BTN_Test2.Text = "测试2";
            this.BTN_Test2.UseVisualStyleBackColor = true;
            this.BTN_Test2.Click += new System.EventHandler(this.BTN_Test2_Click);
            // 
            // BTN_Test3
            // 
            this.BTN_Test3.Location = new System.Drawing.Point(6, 74);
            this.BTN_Test3.Name = "BTN_Test3";
            this.BTN_Test3.Size = new System.Drawing.Size(75, 23);
            this.BTN_Test3.TabIndex = 9;
            this.BTN_Test3.Text = "测试3";
            this.BTN_Test3.UseVisualStyleBackColor = true;
            this.BTN_Test3.Click += new System.EventHandler(this.BTN_Test3_Click);
            // 
            // RB_Line
            // 
            this.RB_Line.AutoSize = true;
            this.RB_Line.Checked = true;
            this.RB_Line.Location = new System.Drawing.Point(6, 22);
            this.RB_Line.Name = "RB_Line";
            this.RB_Line.Size = new System.Drawing.Size(50, 21);
            this.RB_Line.TabIndex = 10;
            this.RB_Line.TabStop = true;
            this.RB_Line.Text = "直线";
            this.RB_Line.UseVisualStyleBackColor = true;
            this.RB_Line.CheckedChanged += new System.EventHandler(this.RB_Line_CheckedChanged);
            // 
            // RB_Rectangle
            // 
            this.RB_Rectangle.AutoSize = true;
            this.RB_Rectangle.Location = new System.Drawing.Point(6, 49);
            this.RB_Rectangle.Name = "RB_Rectangle";
            this.RB_Rectangle.Size = new System.Drawing.Size(50, 21);
            this.RB_Rectangle.TabIndex = 11;
            this.RB_Rectangle.Text = "矩形";
            this.RB_Rectangle.UseVisualStyleBackColor = true;
            this.RB_Rectangle.CheckedChanged += new System.EventHandler(this.RB_Line_CheckedChanged);
            // 
            // RB_Circle
            // 
            this.RB_Circle.AutoSize = true;
            this.RB_Circle.Location = new System.Drawing.Point(6, 76);
            this.RB_Circle.Name = "RB_Circle";
            this.RB_Circle.Size = new System.Drawing.Size(50, 21);
            this.RB_Circle.TabIndex = 12;
            this.RB_Circle.Text = "圆形";
            this.RB_Circle.UseVisualStyleBackColor = true;
            this.RB_Circle.CheckedChanged += new System.EventHandler(this.RB_Line_CheckedChanged);
            // 
            // TC_Main
            // 
            this.TC_Main.Controls.Add(this.TB_CV);
            this.TC_Main.Controls.Add(this.tabPage2);
            this.TC_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TC_Main.Location = new System.Drawing.Point(0, 0);
            this.TC_Main.Name = "TC_Main";
            this.TC_Main.SelectedIndex = 0;
            this.TC_Main.Size = new System.Drawing.Size(884, 481);
            this.TC_Main.TabIndex = 13;
            // 
            // TB_CV
            // 
            this.TB_CV.Controls.Add(this.GB_Test);
            this.TB_CV.Controls.Add(this.GB_FigureSelect);
            this.TB_CV.Controls.Add(this.PB_MyPicture);
            this.TB_CV.Controls.Add(this.TB_Info);
            this.TB_CV.Controls.Add(this.BTN_ReadVideo);
            this.TB_CV.Controls.Add(this.GB_ConsoleTest);
            this.TB_CV.Controls.Add(this.BTN_ReadImage);
            this.TB_CV.Location = new System.Drawing.Point(4, 26);
            this.TB_CV.Name = "TB_CV";
            this.TB_CV.Padding = new System.Windows.Forms.Padding(3);
            this.TB_CV.Size = new System.Drawing.Size(876, 451);
            this.TB_CV.TabIndex = 0;
            this.TB_CV.Text = "CV";
            this.TB_CV.UseVisualStyleBackColor = true;
            // 
            // GB_Test
            // 
            this.GB_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Test.Controls.Add(this.BTN_Test);
            this.GB_Test.Controls.Add(this.BTN_Test2);
            this.GB_Test.Controls.Add(this.BTN_Test3);
            this.GB_Test.Location = new System.Drawing.Point(720, 86);
            this.GB_Test.Name = "GB_Test";
            this.GB_Test.Size = new System.Drawing.Size(98, 106);
            this.GB_Test.TabIndex = 14;
            this.GB_Test.TabStop = false;
            this.GB_Test.Text = "测试";
            // 
            // GB_FigureSelect
            // 
            this.GB_FigureSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_FigureSelect.Controls.Add(this.RB_Line);
            this.GB_FigureSelect.Controls.Add(this.RB_Rectangle);
            this.GB_FigureSelect.Controls.Add(this.RB_Circle);
            this.GB_FigureSelect.Location = new System.Drawing.Point(639, 86);
            this.GB_FigureSelect.Name = "GB_FigureSelect";
            this.GB_FigureSelect.Size = new System.Drawing.Size(75, 106);
            this.GB_FigureSelect.TabIndex = 14;
            this.GB_FigureSelect.TabStop = false;
            this.GB_FigureSelect.Text = "图形";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(876, 451);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 481);
            this.Controls.Add(this.TC_Main);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PB_MyPicture)).EndInit();
            this.GB_ConsoleTest.ResumeLayout(false);
            this.GB_ConsoleTest.PerformLayout();
            this.TC_Main.ResumeLayout(false);
            this.TB_CV.ResumeLayout(false);
            this.TB_CV.PerformLayout();
            this.GB_Test.ResumeLayout(false);
            this.GB_FigureSelect.ResumeLayout(false);
            this.GB_FigureSelect.PerformLayout();
            this.ResumeLayout(false);

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
        private TabPage TB_CV;
        private GroupBox GB_Test;
        private GroupBox GB_FigureSelect;
        private TabPage tabPage2;
    }
}