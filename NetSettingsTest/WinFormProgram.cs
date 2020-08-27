using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using NetSettingsTest.Forms;

namespace NetSettingsTest
{
    static class WinFormProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
