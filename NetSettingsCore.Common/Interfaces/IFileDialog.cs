using System;

namespace NetSettingsCore.Common
{
    public class IFileDialog
    {
        private IFileDialog dialog;
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public string FileName { get; set; }


        public DialogResult ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}