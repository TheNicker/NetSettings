using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace NetSettings
{
    class PreviewForm
    {
        Form fPreviewForm;
        string fImageName;
        Timer fPreviewImageTimer;
        

        public string ImageName 
        {
            get
            { return fImageName; }
            set
            {
                if (value != null)
                {
                    if (value != fImageName || !fPreviewForm.Visible)
                    {
                        fPreviewImageTimer.Enabled = true;
                        fImageName = value;
                    }
                }
            }
        
        }

        public Timer PreviewTimer { get { return fPreviewImageTimer; } }

        public PreviewForm()
        {
            fPreviewImageTimer = new Timer();
            fPreviewImageTimer.Tick += fPreviewImageTimer_Tick;
            fPreviewImageTimer.Interval = 500;
        }

        public void Show()
        {
            VerifiyForm();
            ShowImage();
        }

        private void ShowImage()
        {
            if (System.IO.File.Exists(fImageName))
            {
                Image img ;
                try
                {
                    img = Image.FromFile(fImageName);
                }
                catch
                {
                    return;
                }
                (fPreviewForm.Controls[0] as PictureBox).Image = img;

                const int MaxWidth = 400;
                const int MaxHeight = 400;

                float h = (float)MaxHeight / img.Height;
                float v = (float)MaxWidth / img.Width;
                float ratio = Math.Min(h, v);
                SizeF size = new SizeF(img.Width * ratio, img.Height * ratio);
                fPreviewForm.Width = (int)size.Width;
                fPreviewForm.Height = (int)size.Height;
                fPreviewForm.TopMost = true;
                fPreviewForm.Show();
            }
            
        }

        private void VerifiyForm()
        {
            if (fPreviewForm == null)
            {

               

                fPreviewForm = new Form();
                PictureBox p = new PictureBox();
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.Dock = DockStyle.Fill;
                fPreviewForm.Controls.Add(p);
                p.MouseLeave += p_MouseLeave;

                fPreviewForm.Width = 800;
                fPreviewForm.Height = 600;
                fPreviewForm.FormBorderStyle = FormBorderStyle.None;
            }
        }

        void p_MouseLeave(object sender, EventArgs e)
        {
            Hide();
        }

        void fPreviewImageTimer_Tick(object sender, EventArgs e)
        {
            Show();
        }

        public void Hide()
        {
            VerifiyForm();
            fPreviewImageTimer.Enabled = false;
            fPreviewForm.Hide();
        }
    }
}
