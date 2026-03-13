using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPOS.WinForms.Common.Config
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }

        public string SerialPortName { get; set; }

        public int SerialBaudRate { get; set; }
        //change
        public int CameraIndex { get; set; }
    }
}
