using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using NetSettingsCore.Common;

namespace NetSettings.Forms
{
    public partial class PreviewForm : Form, IPreviewForm
    {
        private string fImageName;

        public string ImageName
        {
            get => fImageName;
            set
            {
                if (value != null)
                {
                    if (value != fImageName || !this.Visible)
                    {
                        //PreviewTimer.Enabled = true;
                        fImageName = value;
                        Image img;
                        try
                        {
                            img = Image.FromFile(fImageName);
                        }
                        catch
                        {
                            throw new NotImplementedException("Implement: Add an image or text that says \"Preview not available\"");
                        }
                        //(this.Controls[0] as PictureBox).Image = img;
                        this.pbPreviewImage.Image = img;
                    }
                }
            }
        }

        public PreviewForm()
        {
            InitializeComponent();
        }

        //TODO: Delete this method
        private void ShowImage()
        {
            if (System.IO.File.Exists(fImageName))
            {
                Image img;
                try
                {
                    img = Image.FromFile(fImageName);
                }
                catch
                {
                    throw new NotImplementedException("Implement: Add an image or text that says \"Preview not available\"");
                }
                this.pbPreviewImage.Image = img;

                const int maxWidth = 400;
                const int maxHeight = 400;

                var h = (float)maxHeight / img.Height;
                var v = (float)maxWidth / img.Width;
                var ratio = Math.Min(h, v);
                var size = new SizeF(img.Width * ratio, img.Height * ratio);
                this.Width = (int)size.Width;
                this.Height = (int)size.Height;
                this.TopMost = true;
                this.Show();
            }
        }

        private void p_MouseLeave(object sender, EventArgs e)
        {
            //HideImage();
        }

        private void pbPreviewImage_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            const int maxWidth = 400;
            const int maxHeight = 400;
            //TODO: Open ratio calcs
            //var h = (float)maxHeight / img.Height;
            //var v = (float)maxWidth / img.Width;
            //var ratio = Math.Min(h, v);
            //var size = new SizeF(img.Width * ratio, img.Height * ratio);
            //this.Width = (int)size.Width;
            //this.Height = (int)size.Height;
            this.TopMost = true;
            //this.Show();
        }

        private void PreviewForm_Load(object sender, EventArgs e)
        {
            this.Width = 800;
            this.Height = 600;
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
