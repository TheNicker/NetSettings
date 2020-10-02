using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace NetSettings.Avalonia
{
    public static class ControlHelper
    {
        #region Redraw Suspend/Resume
        [DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        private const int WM_SETREDRAW = 0xB;

        public static void SuspendDrawing(this Control target)
        {
            //SendMessage(target.Handle, WM_SETREDRAW, 0, 0);
            throw new NotImplementedException();
        }

        public static void ResumeDrawing(this Control target) { ResumeDrawing(target, true); }
        public static void ResumeDrawing(this Control target, bool redraw)
        {
            //SendMessage(target.Handle, WM_SETREDRAW, 1, 0);

            //if (redraw)
            //{
            //    target.Refresh();
            //}
            throw new NotImplementedException();
        }
        #endregion
    }
}
