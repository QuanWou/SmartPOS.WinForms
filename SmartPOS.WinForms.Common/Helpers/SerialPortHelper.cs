using System;
using System.IO.Ports;

namespace SmartPOS.WinForms.Common.Helpers
{
    public class SerialPortHelper : IDisposable
    {
        private SerialPort _serialPort;

        public bool IsOpen
        {
            get { return _serialPort != null && _serialPort.IsOpen; }
        }

        public void Open(string portName, int baudRate)
        {
            if (IsOpen)
            {
                return;
            }

            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
        }

        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void Send(string command)
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Cổng Serial chưa được mở.");
            }

            _serialPort.WriteLine(command);
        }

        public void Dispose()
        {
            Close();

            if (_serialPort != null)
            {
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
    }
}