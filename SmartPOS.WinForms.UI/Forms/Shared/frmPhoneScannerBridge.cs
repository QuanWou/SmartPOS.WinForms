using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace SmartPOS.WinForms.UI.Forms.Shared
{
    public class frmPhoneScannerBridge : Form
    {
        private PhoneScanBridgeServer _server;
        private bool _scanCompleted;

        private Label lblTitle;
        private Label lblInstruction;
        private Label lblUrlCaption;
        private Label lblStatus;
        private TextBox txtUrl;
        private PictureBox picQr;
        private Button btnCopy;
        private Button btnClose;

        public string ScannedCode { get; private set; }

        public frmPhoneScannerBridge(string title, string instruction)
        {
            InitializeComponent(title, instruction);
        }

        private void InitializeComponent(string title, string instruction)
        {
            this.Text = string.IsNullOrWhiteSpace(title) ? "Qu\u00e9t b\u1eb1ng \u0111i\u1ec7n tho\u1ea1i" : title;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.ClientSize = new Size(760, 560);

            lblTitle = new Label
            {
                Text = this.Text,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblInstruction = new Label
            {
                Text = string.IsNullOrWhiteSpace(instruction)
                    ? "M\u1edf \u0111i\u1ec7n tho\u1ea1i c\u00f9ng Wi-Fi, qu\u00e9t QR ho\u1eb7c m\u1edf \u0111\u01b0\u1eddng d\u1eabn b\u00ean d\u01b0\u1edbi, r\u1ed3i ch\u1ee5p \u1ea3nh m\u00e3 v\u1ea1ch \u0111\u1ec3 g\u1eedi v\u1ec1 m\u00e1y t\u00ednh."
                    : instruction,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(100, 110, 138),
                AutoSize = false,
                Size = new Size(700, 46),
                Location = new Point(24, 54)
            };

            lblUrlCaption = new Label
            {
                Text = "\u0110\u01b0\u1eddng d\u1eabn m\u1edf tr\u00ean \u0111i\u1ec7n tho\u1ea1i",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 122)
            };

            txtUrl = new TextBox
            {
                Location = new Point(24, 148),
                Size = new Size(560, 30),
                ReadOnly = true
            };

            btnCopy = new Button
            {
                Text = "Sao ch\u00e9p",
                Size = new Size(100, 32),
                Location = new Point(596, 146),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += BtnCopy_Click;

            picQr = new PictureBox
            {
                Location = new Point(24, 202),
                Size = new Size(220, 220),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            lblStatus = new Label
            {
                Text = "\u0110ang ch\u1edd \u0111i\u1ec7n tho\u1ea1i g\u1eedi m\u00e3...",
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                Size = new Size(430, 52),
                Location = new Point(272, 214)
            };

            Label lblTips = new Label
            {
                Text = "1. \u0110i\u1ec7n tho\u1ea1i v\u00e0 m\u00e1y t\u00ednh ph\u1ea3i c\u00f9ng m\u1ea1ng.\r\n2. Tr\u00ean \u0111i\u1ec7n tho\u1ea1i, b\u1ea5m ch\u1ee5p \u1ea3nh m\u00e3 v\u1ea1ch.\r\n3. N\u1ebfu kh\u00f4ng m\u1edf \u0111\u01b0\u1ee3c trang, ki\u1ec3m tra Firewall c\u1ee7a Windows.",
                Font = new Font("Segoe UI", 9.25F),
                ForeColor = Color.FromArgb(106, 116, 138),
                AutoSize = false,
                Size = new Size(430, 100),
                Location = new Point(272, 280)
            };

            btnClose = new Button
            {
                Text = "\u0110\u00f3ng",
                Size = new Size(120, 36),
                Location = new Point(576, 492),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnClose.FlatAppearance.BorderSize = 0;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblInstruction);
            this.Controls.Add(lblUrlCaption);
            this.Controls.Add(txtUrl);
            this.Controls.Add(btnCopy);
            this.Controls.Add(picQr);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblTips);
            this.Controls.Add(btnClose);

            this.Load += FrmPhoneScannerBridge_Load;
            this.FormClosing += FrmPhoneScannerBridge_FormClosing;
            this.CancelButton = btnClose;
        }

        private void FrmPhoneScannerBridge_Load(object sender, EventArgs e)
        {
            string lanAddress = PhoneScanBridgeServer.GetBestLanAddress();
            if (string.IsNullOrWhiteSpace(lanAddress))
            {
                MessageBox.Show("Kh\u00f4ng t\u00ecm th\u1ea5y \u0111\u1ecba ch\u1ec9 m\u1ea1ng n\u1ed9i b\u1ed9. H\u00e3y k\u1ebft n\u1ed1i Wi-Fi/LAN r\u1ed3i m\u1edf l\u1ea1i t\u00ednh n\u0103ng n\u00e0y.", "Th\u00f4ng b\u00e1o");
                this.BeginInvoke(new Action(Close));
                return;
            }

            _server = new PhoneScanBridgeServer();
            _server.CodeReceived += Server_CodeReceived;
            _server.Start();

            string url = "http://" + lanAddress + ":" + _server.Port + "/";
            txtUrl.Text = url;
            picQr.Image = BuildQrCode(url);
            lblStatus.Text = "\u0110ang ch\u1edd \u0111i\u1ec7n tho\u1ea1i g\u1eedi m\u00e3 t\u1eeb: " + lanAddress;
        }

        private void FrmPhoneScannerBridge_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_server != null)
            {
                _server.CodeReceived -= Server_CodeReceived;
                _server.Dispose();
                _server = null;
            }

            if (picQr.Image != null)
            {
                picQr.Image.Dispose();
                picQr.Image = null;
            }
        }

        private void Server_CodeReceived(string code)
        {
            if (_scanCompleted || this.IsDisposed)
            {
                return;
            }

            if (!this.IsHandleCreated)
            {
                return;
            }

            this.BeginInvoke(new Action(() =>
            {
                if (_scanCompleted || this.IsDisposed)
                {
                    return;
                }

                _scanCompleted = true;
                ScannedCode = code;
                lblStatus.Text = "\u0110\u00e3 nh\u1eadn m\u00e3 t\u1eeb \u0111i\u1ec7n tho\u1ea1i: " + code;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }));
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                return;
            }

            Clipboard.SetText(txtUrl.Text);
            lblStatus.Text = "\u0110\u00e3 sao ch\u00e9p \u0111\u01b0\u1eddng d\u1eabn. M\u1edf tr\u00ean \u0111i\u1ec7n tho\u1ea1i r\u1ed3i g\u1eedi m\u00e3 v\u1ec1 m\u00e1y t\u00ednh.";
        }

        private static Bitmap BuildQrCode(string text)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 220,
                    Height = 220,
                    Margin = 1
                }
            };

            return writer.Write(text);
        }
    }
}
