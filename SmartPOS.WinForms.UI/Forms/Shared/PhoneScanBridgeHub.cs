using System;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Shared
{
    internal static class PhoneScanBridgeHub
    {
        private static readonly object SyncRoot = new object();
        private static PhoneScanBridgeServer _server;

        public static event Action<string> CodeReceived;
        public static event Action<int> InvoiceCreated;

        public static string Url
        {
            get
            {
                EnsureStarted();
                string lanAddress = PhoneScanBridgeServer.GetBestLanAddress();
                return string.IsNullOrWhiteSpace(lanAddress)
                    ? string.Empty
                    : "http://" + lanAddress + ":" + _server.Port + "/";
            }
        }

        public static void EnsureStarted()
        {
            PhoneBridgeFirewallHelper.EnsureWhenBridgeStarts();

            lock (SyncRoot)
            {
                if (_server != null)
                {
                    return;
                }

                _server = new PhoneScanBridgeServer(5055);
                _server.CodeReceived += code => CodeReceived?.Invoke(code);
                _server.InvoiceCreated += invoiceId => InvoiceCreated?.Invoke(invoiceId);
                _server.Start();
            }
        }

        public static void Stop()
        {
            lock (SyncRoot)
            {
                if (_server == null)
                {
                    return;
                }

                _server.Dispose();
                _server = null;
            }
        }
    }
}
