using System;
using System.Windows.Forms;
using SmartPOS.WinForms.UI.Forms.Aulh;
using SmartPOS.WinForms.UI.Forms.Shared;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            EnvFileLoader.Load();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new frmLogin());
            }
            finally
            {
                PhoneScanBridgeHub.Stop();
            }
        }
    }
}
