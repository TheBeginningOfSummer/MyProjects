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
            TB_Test = new TextBox();
            OFD_OpenFile = new OpenFileDialog();
            GB_AlbumInfo = new GroupBox();
            BTN_UploadPicture = new Button();
            PB_AlbumCover = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            TB_AlbumName = new TextBox();
            TB_PublishDate = new TextBox();
            GB_AlbumInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PB_AlbumCover).BeginInit();
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
            BTN_Upload.Click += BTN_Upload_ClickAsync;
            // 
            // TB_Test
            // 
            TB_Test.Location = new Point(12, 304);
            TB_Test.Multiline = true;
            TB_Test.Name = "TB_Test";
            TB_Test.Size = new Size(539, 134);
            TB_Test.TabIndex = 1;
            // 
            // OFD_OpenFile
            // 
            OFD_OpenFile.FileName = "openFile";
            OFD_OpenFile.Multiselect = true;
            // 
            // GB_AlbumInfo
            // 
            GB_AlbumInfo.Controls.Add(TB_PublishDate);
            GB_AlbumInfo.Controls.Add(TB_AlbumName);
            GB_AlbumInfo.Controls.Add(label2);
            GB_AlbumInfo.Controls.Add(label1);
            GB_AlbumInfo.Controls.Add(BTN_UploadPicture);
            GB_AlbumInfo.Controls.Add(PB_AlbumCover);
            GB_AlbumInfo.Location = new Point(12, 12);
            GB_AlbumInfo.Name = "GB_AlbumInfo";
            GB_AlbumInfo.Size = new Size(269, 202);
            GB_AlbumInfo.TabIndex = 2;
            GB_AlbumInfo.TabStop = false;
            GB_AlbumInfo.Text = "专辑信息";
            // 
            // BTN_UploadPicture
            // 
            BTN_UploadPicture.Location = new Point(20, 168);
            BTN_UploadPicture.Name = "BTN_UploadPicture";
            BTN_UploadPicture.Size = new Size(75, 23);
            BTN_UploadPicture.TabIndex = 1;
            BTN_UploadPicture.Text = "上传图片";
            BTN_UploadPicture.UseVisualStyleBackColor = true;
            BTN_UploadPicture.Click += BTN_UploadPicture_ClickAsync;
            // 
            // PB_AlbumCover
            // 
            PB_AlbumCover.Location = new Point(6, 22);
            PB_AlbumCover.Name = "PB_AlbumCover";
            PB_AlbumCover.Size = new Size(100, 140);
            PB_AlbumCover.SizeMode = PictureBoxSizeMode.Zoom;
            PB_AlbumCover.TabIndex = 0;
            PB_AlbumCover.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(112, 22);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 2;
            label1.Text = "名称：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(112, 50);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 3;
            label2.Text = "日期：";
            // 
            // TB_AlbumName
            // 
            TB_AlbumName.Location = new Point(162, 19);
            TB_AlbumName.Name = "TB_AlbumName";
            TB_AlbumName.Size = new Size(100, 23);
            TB_AlbumName.TabIndex = 4;
            // 
            // TB_PublishDate
            // 
            TB_PublishDate.Location = new Point(162, 47);
            TB_PublishDate.Name = "TB_PublishDate";
            TB_PublishDate.Size = new Size(100, 23);
            TB_PublishDate.TabIndex = 5;
            // 
            // UploadForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(GB_AlbumInfo);
            Controls.Add(TB_Test);
            Controls.Add(BTN_Upload);
            Name = "UploadForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UploadForm";
            GB_AlbumInfo.ResumeLayout(false);
            GB_AlbumInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PB_AlbumCover).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BTN_Upload;
        private TextBox TB_Test;
        private OpenFileDialog OFD_OpenFile;
        private GroupBox GB_AlbumInfo;
        private PictureBox PB_AlbumCover;
        private Button BTN_UploadPicture;
        private Label label1;
        private Label label2;
        private TextBox TB_PublishDate;
        private TextBox TB_AlbumName;
    }
}