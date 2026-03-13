using System;
using System.Windows.Forms;
using SmartPOS.WinForms.UI.Forms.Aulh;

namespace SmartPOS.WinForms.UI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}