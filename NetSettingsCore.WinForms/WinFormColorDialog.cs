using System;
using System.Windows.Forms;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;
//using WinFormDialogResult = System.Windows.Forms.DialogResult;
using DialogResult = NetSettings.Common.Classes.DialogResult;
using WinFormColor = System.Drawing.Color;

namespace NetSettings.WinForms
{
    public class WinFormColorDialog : WinFormComponent, IColorDialog
    {
        private readonly ColorDialog _colorDialog = new ColorDialog();

        public Color Color
        {
            get
            {
                var color = _colorDialog.Color;
                return Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            set => _colorDialog.Color = WinFormColor.FromArgb(value.A, value.R, value.G, value.B);
        }

        public bool FullOpen { get => _colorDialog.FullOpen; set => _colorDialog.FullOpen = value; }
        public DialogResult ShowDialog()
        {
            return Enum.Parse<DialogResult>(_colorDialog.ShowDialog().ToString());
        }

        public WinFormColorDialog()
        {
            _component = _colorDialog;
        }
    }
}