using System.Drawing;

namespace NetSettings.Forms
{
    public partial class PreviewForm
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
            this.pbPreviewImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreviewImage
            // 
            this.pbPreviewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPreviewImage.Location = new Point(0, 0);
            this.pbPreviewImage.Name = "pbPreviewImage";
            this.pbPreviewImage.Size = new System.Drawing.Size(284, 261);
            this.pbPreviewImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreviewImage.TabIndex = 0;
            this.pbPreviewImage.TabStop = false;
            this.pbPreviewImage.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.pbPreviewImage_LoadCompleted);
            // 
            // PreviewForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pbPreviewImage);
            this.Name = "PreviewForm";
            this.Load += new System.EventHandler(this.PreviewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreviewImage;
    }
}