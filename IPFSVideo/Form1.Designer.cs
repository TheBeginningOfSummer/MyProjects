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
            SuspendLayout();
            // 
            // TB_Info
            // 
            TB_Info.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TB_Info.Location = new Point(12, 12);
            TB_Info.Multiline = true;
            TB_Info.Name = "TB_Info";
            TB_Info.ScrollBars = ScrollBars.Vertical;
            TB_Info.Size = new Size(695, 401);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TB_Test);
            Controls.Add(BTN_Test);
            Controls.Add(TB_Info);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TB_Info;
        private Button BTN_Test;
        private TextBox TB_Test;
    }
}