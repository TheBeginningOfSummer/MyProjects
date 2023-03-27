namespace IPFSVideo
{
    partial class UploadForm
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
            BTN_Upload = new Button();
            SuspendLayout();
            // 
            // BTN_Upload
            // 
            BTN_Upload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BTN_Upload.Location = new Point(713, 415);
            BTN_Upload.Name = "BTN_Upload";
            BTN_Upload.Size = new Size(75, 23);
            BTN_Upload.TabIndex = 0;
            BTN_Upload.Text = "上传";
            BTN_Upload.UseVisualStyleBackColor = true;
            // 
            // UploadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BTN_Upload);
            Name = "UploadForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UploadForm";
            ResumeLayout(false);
        }

        #endregion

        private Button BTN_Upload;
    }
}