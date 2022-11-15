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
            this.PB_MyPicture.Size = new System.Drawing.Size(602, 426);
            this.PB_MyPicture.TabIndex = 0;
            this.PB_MyPicture.TabStop = false;
            // 
            // BTN_Test
            // 
            this.BTN_Test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Test.Location = new System.Drawing.Point(713, 415);
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
            this.TB_Info.Location = new System.Drawing.Point(620, 12);
            this.TB_Info.Margin = new System.Windows.Forms.Padding(1);
            this.TB_Info.Multiline = true;
            this.TB_Info.Name = "TB_Info";
            this.TB_Info.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Info.Size = new System.Drawing.Size(170, 213);
            this.TB_Info.TabIndex = 2;
            // 
            // BTN_ProcessInput
            // 
            this.BTN_ProcessInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_ProcessInput.Location = new System.Drawing.Point(3, 48);
            this.BTN_ProcessInput.Name = "BTN_ProcessInput";
            this.BTN_ProcessInput.Size = new System.Drawing.Size(164, 23);
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
            this.TB_ConsoleInput.Size = new System.Drawing.Size(164, 23);
            this.TB_ConsoleInput.TabIndex = 4;
            // 
            // GB_ConsoleTest
            // 
            this.GB_ConsoleTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_ConsoleTest.Controls.Add(this.TB_ConsoleInput);
            this.GB_ConsoleTest.Controls.Add(this.BTN_ProcessInput);
            this.GB_ConsoleTest.Location = new System.Drawing.Point(620, 229);
            this.GB_ConsoleTest.Name = "GB_ConsoleTest";
            this.GB_ConsoleTest.Size = new System.Drawing.Size(170, 74);
            this.GB_ConsoleTest.TabIndex = 5;
            this.GB_ConsoleTest.TabStop = false;
            this.GB_ConsoleTest.Text = "控制台测试";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GB_ConsoleTest);
            this.Controls.Add(this.TB_Info);
            this.Controls.Add(this.BTN_Test);
            this.Controls.Add(this.PB_MyPicture);
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}