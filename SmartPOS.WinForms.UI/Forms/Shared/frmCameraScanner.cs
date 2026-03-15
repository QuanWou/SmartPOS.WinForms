using AForge.Video;
using AForge.Video.DirectShow;
using SmartPOS.WinForms.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace SmartPOS.WinForms.UI.Forms.Shared
{
    public class frmCameraScanner : Form
    {
        private readonly string _instructionText;
        private readonly BarcodeReader _barcodeReader;

        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoSource;
        private DateTime _lastDecodeAtUtc;
        private bool _scanCompleted;
        private bool _isClosing;

        private Label lblTitle;
        private Label lblInstruction;
        private Label lblCamera;
        private Label lblStatus;
        private Label lblNote;
        private ComboBox cboCamera;
        private Button btnDong;
        private PictureBox picPreview;

        public string ScannedCode { get; private set; }

        public frmCameraScanner(string title, string instructionText)
        {
            _instructionText = string.IsNullOrWhiteSpace(instructionText)
                ? "\u0110\u01b0a m\u00e3 v\u1ea1ch v\u00e0o gi\u1eefa khung h\u00ecnh. H\u1ec7 th\u1ed1ng s\u1ebd t\u1ef1 nh\u1eadn khi camera b\u1eaft \u0111\u01b0\u1ee3c m\u00e3."
                : instructionText.Trim();

            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.CODE_93,
                        BarcodeFormat.ITF,
                        BarcodeFormat.CODABAR,
                        BarcodeFormat.UPC_A,
                        BarcodeFormat.UPC_E,
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.DATA_MATRIX,
                        BarcodeFormat.PDF_417
                    }
                }
            };

            InitializeComponent(title);
        }

        private void InitializeComponent(string title)
        {
            this.Text = string.IsNullOrWhiteSpace(title) ? "Qu\u00e9t m\u00e3 v\u1ea1ch" : title;
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
                Text = _instructionText,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(105, 112, 130),
                AutoSize = false,
                Size = new Size(700, 42),
                Location = new Point(24, 52)
            };

            lblCamera = new Label
            {
                Text = "Camera",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(70, 78, 96),
                AutoSize = true,
                Location = new Point(24, 110)
            };

            cboCamera = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(24, 134),
                Size = new Size(500, 30)
            };
            cboCamera.SelectedIndexChanged += CboCamera_SelectedIndexChanged;

            btnDong = new Button
            {
                Text = "\u0110\u00f3ng",
                Size = new Size(110, 34),
                Location = new Point(614, 130),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnDong.FlatAppearance.BorderSize = 0;

            lblStatus = new Label
            {
                Text = "\u0110ang chu\u1ea9n b\u1ecb camera...",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(90, 110, 200),
                AutoSize = true,
                Location = new Point(24, 176)
            };

            picPreview = new PictureBox
            {
                Location = new Point(24, 206),
                Size = new Size(706, 286),
                BackColor = Color.FromArgb(18, 23, 37),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            lblNote = new Label
            {
                Text = "H\u1ed7 tr\u1ee3 webcam laptop ho\u1eb7c \u0111i\u1ec7n tho\u1ea1i n\u1ebfu Windows nh\u1eadn thi\u1ebft b\u1ecb \u0111\u00f3 l\u00e0 webcam. V\u1edbi m\u00e3 th\u01b0\u1eddng, h\u1ec7 th\u1ed1ng ch\u1ec9 l\u1ea5y ra m\u00e3 s\u1ea3n ph\u1ea9m.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(110, 118, 136),
                AutoSize = false,
                Size = new Size(706, 38),
                Location = new Point(24, 506)
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblInstruction);
            this.Controls.Add(lblCamera);
            this.Controls.Add(cboCamera);
            this.Controls.Add(btnDong);
            this.Controls.Add(lblStatus);
            this.Controls.Add(picPreview);
            this.Controls.Add(lblNote);

            this.Load += FrmCameraScanner_Load;
            this.FormClosing += FrmCameraScanner_FormClosing;
            this.CancelButton = btnDong;
        }

        private void FrmCameraScanner_Load(object sender, EventArgs e)
        {
            LoadCameras();
        }

        private void FrmCameraScanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isClosing = true;
            StopCamera();
            DisposePreviewImage();
        }

        private void LoadCameras()
        {
            cboCamera.Items.Clear();

            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Kh\u00f4ng \u0111\u1ecdc \u0111\u01b0\u1ee3c danh s\u00e1ch camera: " + ex.Message;
                cboCamera.Enabled = false;
                return;
            }

            if (_videoDevices == null || _videoDevices.Count == 0)
            {
                lblStatus.Text = "Kh\u00f4ng t\u00ecm th\u1ea5y camera. H\u00e3y d\u00f9ng webcam laptop ho\u1eb7c \u0111i\u1ec7n tho\u1ea1i \u0111ang \u0111\u01b0\u1ee3c Windows nh\u1eadn l\u00e0 webcam.";
                cboCamera.Enabled = false;
                return;
            }

            foreach (FilterInfo device in _videoDevices)
            {
                cboCamera.Items.Add(device.Name);
            }

            cboCamera.SelectedIndex = 0;
        }

        private void CboCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            RestartSelectedCamera();
        }

        private void RestartSelectedCamera()
        {
            StopCamera();

            if (_videoDevices == null || cboCamera.SelectedIndex < 0 || cboCamera.SelectedIndex >= _videoDevices.Count)
            {
                return;
            }

            try
            {
                FilterInfo selectedDevice = _videoDevices[cboCamera.SelectedIndex];
                _videoSource = new VideoCaptureDevice(selectedDevice.MonikerString);
                _videoSource.NewFrame += VideoSource_NewFrame;
                _videoSource.Start();

                lblStatus.Text = "\u0110ang qu\u00e9t b\u1eb1ng camera: " + selectedDevice.Name;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Kh\u00f4ng m\u1edf \u0111\u01b0\u1ee3c camera: " + ex.Message;
            }
        }

        private void StopCamera()
        {
            VideoCaptureDevice currentSource = _videoSource;
            _videoSource = null;

            if (currentSource == null)
            {
                return;
            }

            try
            {
                currentSource.NewFrame -= VideoSource_NewFrame;

                if (currentSource.IsRunning)
                {
                    currentSource.SignalToStop();
                    currentSource.WaitForStop();
                }
            }
            catch
            {
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (_isClosing || _scanCompleted)
            {
                return;
            }

            Bitmap previewFrame = null;
            string decodedText = null;

            try
            {
                previewFrame = (Bitmap)eventArgs.Frame.Clone();

                if ((DateTime.UtcNow - _lastDecodeAtUtc).TotalMilliseconds >= 350)
                {
                    _lastDecodeAtUtc = DateTime.UtcNow;

                    Result result = _barcodeReader.Decode(previewFrame);
                    if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                    {
                        decodedText = BarcodeHelper.Normalize(result.Text);
                    }
                }

                if (!this.IsHandleCreated)
                {
                    previewFrame.Dispose();
                    return;
                }

                Bitmap frameForUi = previewFrame;
                string codeForUi = decodedText;

                this.BeginInvoke(new Action(() => UpdatePreview(frameForUi, codeForUi)));
                previewFrame = null;
            }
            catch
            {
                if (previewFrame != null)
                {
                    previewFrame.Dispose();
                }
            }
        }

        private void UpdatePreview(Bitmap previewFrame, string decodedText)
        {
            if (_isClosing || this.IsDisposed)
            {
                if (previewFrame != null)
                {
                    previewFrame.Dispose();
                }

                return;
            }

            Image oldImage = picPreview.Image;
            picPreview.Image = previewFrame;
            if (oldImage != null)
            {
                oldImage.Dispose();
            }

            if (!string.IsNullOrWhiteSpace(decodedText))
            {
                CompleteScan(decodedText);
            }
        }

        private void CompleteScan(string decodedText)
        {
            if (_scanCompleted)
            {
                return;
            }

            _scanCompleted = true;
            ScannedCode = BarcodeHelper.Normalize(decodedText);
            lblStatus.Text = "\u0110\u00e3 nh\u1eadn m\u00e3: " + ScannedCode;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DisposePreviewImage()
        {
            Image oldImage = picPreview.Image;
            picPreview.Image = null;

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }
    }
}
