using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.POS
{
    public class frmCashPayment : Form
    {
        // Update these constants when the restaurant changes its receiving account.
        private const string TransferBankCode = "TechCombank";
        private const string TransferBankName = "TechCombank";
        private const string TransferAccountNumber = "2005111818";
        private const string TransferAccountName = "NHA HANG SMARTPOS";
        private const string TransferContent = "Thanh toan hoa don POS";
        private const string TransferTemplate = "compact2";
        private Label lblMenhGiaTitle;
        private Panel pnlMenhGia;

        private static readonly int[] MenhGia = {
    500000, 200000, 100000, 50000, 20000, 10000, 5000, 2000, 1000
};
        private readonly decimal _tongTien;

        public bool IsConfirmed { get; private set; }
        public decimal TienKhachDua { get; private set; }
        public decimal TienThoi { get; private set; }
        public string PaymentMethodLabel { get; private set; }

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTongTien;
        private Label lblTongTienValue;
        private Label lblPhuongThuc;
        private RadioButton rdoTienMat;
        private RadioButton rdoChuyenKhoan;
        private Panel pnlTienMat;
        private Label lblTienKhachDua;
        private TextBox txtTienKhachDua;
        private Label lblTienThoi;
        private Label lblTienThoiValue;
        private Panel pnlChuyenKhoan;
        private Label lblQrHint;
        private PictureBox picQrCode;
        private Label lblQrStatus;
        private Label lblBankValue;
        private Label lblAccountValue;
        private Label lblAccountNameValue;
        private Label lblAmountValue;
        private Button btnTaiLaiQr;
        private Button btnTinhTien;
        private Button btnXacNhan;
        private Button btnDong;

        public frmCashPayment(decimal tongTien)
        {
            _tongTien = tongTien;
            PaymentMethodLabel = "Tiền mặt";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Thanh toán";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(760, 680);
            this.MinimumSize = new Size(760, 680);
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "Thanh toán hóa đơn",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 22)
            };

            lblSubtitle = new Label
            {
                Text = "Chọn phương thức và xác nhận thanh toán cho hóa đơn",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(24, 52)
            };

            lblTongTien = new Label
            {
                Text = "Tổng tiền",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(24, 95)
            };

            lblTongTienValue = new Label
            {
                Text = _tongTien.ToString("N0") + " đ",
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 118)
            };

            lblPhuongThuc = new Label
            {
                Text = "Phương thức thanh toán",
                AutoSize = true,
                Location = new Point(24, 170)
            };

            rdoTienMat = new RadioButton
            {
                Text = "Tiền mặt",
                AutoSize = true,
                Checked = true,
                Location = new Point(28, 196)
            };
            rdoTienMat.CheckedChanged += PaymentMethod_CheckedChanged;

            rdoChuyenKhoan = new RadioButton
            {
                Text = "Chuyển khoản",
                AutoSize = true,
                Location = new Point(138, 196)
            };
            rdoChuyenKhoan.CheckedChanged += PaymentMethod_CheckedChanged;

            BuildCashPanel();
            BuildTransferPanel();

            btnTinhTien = new Button
            {
                Text = "Tính tiền thối",
                Location = new Point(24, 590),
                Size = new Size(120, 34),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTinhTien.FlatAppearance.BorderSize = 0;
            btnTinhTien.Click += BtnTinhTien_Click;

            btnXacNhan = new Button
            {
                Text = "Xác nhận",
                Location = new Point(486, 590),
                Size = new Size(120, 34),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.Click += BtnXacNhan_Click;

            btnDong = new Button
            {
                Text = "Đóng",
                Location = new Point(616, 590),
                Size = new Size(120, 34),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.Click += BtnDong_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblTongTien);
            this.Controls.Add(lblTongTienValue);
            this.Controls.Add(lblPhuongThuc);
            this.Controls.Add(rdoTienMat);
            this.Controls.Add(rdoChuyenKhoan);
            this.Controls.Add(pnlTienMat);
            this.Controls.Add(pnlChuyenKhoan);
            this.Controls.Add(btnTinhTien);
            this.Controls.Add(btnXacNhan);
            this.Controls.Add(btnDong);

            this.Load += FrmCashPayment_Load;
        }

        private void BuildCashPanel()
        {
            pnlTienMat = new Panel
            {
                Location = new Point(24, 228),
                Size = new Size(712, 357),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            // ── Tiền khách đưa ──────────────────────────────────────
            lblTienKhachDua = new Label
            {
                Text = "TIỀN KHÁCH ĐƯA",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(130, 130, 150),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            txtTienKhachDua = new TextBox
            {
                Location = new Point(24, 42),
                Size = new Size(360, 36),
                Font = new Font("Segoe UI", 18F),
                BorderStyle = BorderStyle.None,
                ForeColor = Color.FromArgb(22, 32, 72),
                ReadOnly = true
            };

            var separator = new Panel
            {
                Location = new Point(24, 82),
                Size = new Size(360, 2),
                BackColor = Color.FromArgb(90, 110, 200)
            };

            // ── Divider dọc ─────────────────────────────────────────
            var divider = new Panel
            {
                Location = new Point(415, 16),
                Size = new Size(1, 370),
                BackColor = Color.FromArgb(220, 222, 230)
            };

            // ── Tiền thối (bên phải) ────────────────────────────────
            lblTienThoi = new Label
            {
                Text = "TIỀN THỐI LẠI",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(130, 130, 150),
                AutoSize = true,
                Location = new Point(435, 20)
            };

            lblTienThoiValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 110, 200),
                AutoSize = true,
                Location = new Point(435, 42)
            };

            // ── Mệnh giá tiền thối ──────────────────────────────────
            lblMenhGiaTitle = new Label
            {
                Text = "TỜ TIỀN THỐI",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(130, 130, 150),
                AutoSize = true,
                Location = new Point(435, 100)
            };

            pnlMenhGia = new Panel
            {
                Location = new Point(435, 120),
                Size = new Size(265, 260),
                BackColor = Color.Transparent
            };

            // ── Shortcut tiền nhanh ─────────────────────────────────
            string[] quickAmounts = { "10.000", "20.000", "50.000", "100.000", "200.000", "500.000" };
            int qx = 24, qy = 96;
            foreach (var amt in quickAmounts)
            {
                string val = amt;
                var btn = new Button
                {
                    Text = val,
                    Size = new Size(60, 28),
                    Location = new Point(qx, qy),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(240, 242, 255),
                    ForeColor = Color.FromArgb(22, 32, 72),
                    Font = new Font("Segoe UI", 8F),
                    Tag = val.Replace(".", "")
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 205, 230);
                btn.FlatAppearance.BorderSize = 1;
                btn.Click += (s, e) =>
                {
                    decimal current = ParseMoney(txtTienKhachDua.Text);
                    decimal add = decimal.Parse((string)((Button)s).Tag);
                    txtTienKhachDua.Text = (current + add).ToString("N0");
                    TinhTienThoi();
                };
                pnlTienMat.Controls.Add(btn);
                qx += 64;
            }

            // ── Numpad Layout A: 4×4 ────────────────────────────────
            //  [ 7 ][ 8 ][ 9 ][ ⌫ ]
            //  [ 4 ][ 5 ][ 6 ][000]
            //  [ 1 ][ 2 ][ 3 ][ C ]
            //  [        0        ]
            // ------------------------------------------------------- 
            int startX = 24, startY = 136;
            int btnW = 80, btnH = 46, gap = 8;

            string[,] keys = {
                { "7",   "8", "9", "⌫"  },
                { "4",   "5", "6", "000" },
                { "1",   "2", "3", "C"   },
            };

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    string key = keys[row, col];
                    bool isDel = key == "⌫";
                    bool isClear = key == "C";
                    bool is000 = key == "000";

                    var btn = new Button
                    {
                        Text = key,
                        Size = new Size(btnW, btnH),
                        Location = new Point(startX + col * (btnW + gap), startY + row * (btnH + gap)),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = (isDel || isClear)
                                        ? Color.FromArgb(255, 240, 240)
                                        : is000
                                            ? Color.FromArgb(240, 242, 255)
                                            : Color.FromArgb(247, 248, 252),
                        ForeColor = (isDel || isClear)
                                        ? Color.FromArgb(200, 60, 60)
                                        : Color.FromArgb(22, 32, 72),
                        Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                        Tag = key
                    };
                    btn.FlatAppearance.BorderColor = Color.FromArgb(210, 213, 230);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.Click += NumpadBtn_Click;
                    pnlTienMat.Controls.Add(btn);
                }
            }

            // Hàng cuối: nút 0 trải dài toàn bộ 4 cột
            int rowY = startY + 3 * (btnH + gap);
            int fullWidth = 4 * btnW + 3 * gap;

            var btnZero = new Button
            {
                Text = "0",
                Size = new Size(fullWidth, btnH),
                Location = new Point(startX, rowY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(247, 248, 252),
                ForeColor = Color.FromArgb(22, 32, 72),
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                Tag = "0"
            };
            btnZero.FlatAppearance.BorderColor = Color.FromArgb(210, 213, 230);
            btnZero.FlatAppearance.BorderSize = 1;
            btnZero.Click += NumpadBtn_Click;
            pnlTienMat.Controls.Add(btnZero);

            pnlTienMat.Controls.Add(lblTienKhachDua);
            pnlTienMat.Controls.Add(txtTienKhachDua);
            pnlTienMat.Controls.Add(separator);
            pnlTienMat.Controls.Add(divider);
            pnlTienMat.Controls.Add(lblTienThoi);
            pnlTienMat.Controls.Add(lblTienThoiValue);
            pnlTienMat.Controls.Add(lblMenhGiaTitle);
            pnlTienMat.Controls.Add(pnlMenhGia);
        }
        private void HienThiMenhGia(decimal tienThoi)
        {
            pnlMenhGia.Controls.Clear();

            if (tienThoi <= 0)
            {
                var emptyLabel = new Label
                {
                    Text = "Không có tiền thối",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Location = new Point(0, 0)
                };
                pnlMenhGia.Controls.Add(emptyLabel);
                return;
            }

            decimal con = Math.Floor(tienThoi / 1000) * 1000;
            int y = 0;
            int itemHeight = 34;
            int itemWidth = pnlMenhGia.Width - 8;

            foreach (int mg in MenhGia)
            {
                if (con <= 0) break;

                int soTo = (int)(con / mg);
                if (soTo <= 0) continue;

                con -= soTo * mg;

                Panel row = new Panel
                {
                    Size = new Size(itemWidth, itemHeight),
                    Location = new Point(0, y),
                    BackColor = Color.FromArgb(245, 247, 252),
                    Padding = new Padding(0),
                    Margin = new Padding(0),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblLoaiTien = new Label
                {
                    Text = mg.ToString("N0") + " đ",
                    Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(22, 32, 72),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Location = new Point(10, 0),
                    Size = new Size(140, itemHeight)
                };

                Label lblSoTo = new Label
                {
                    Text = "x" + soTo,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(90, 110, 200),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(itemWidth - 70, 0),
                    Size = new Size(60, itemHeight)
                };

                row.Controls.Add(lblLoaiTien);
                row.Controls.Add(lblSoTo);
                pnlMenhGia.Controls.Add(row);

                y += itemHeight + 6;
            }
        }
        private void BuildMenhGiaSection()
        {
            lblMenhGiaTitle = new Label
            {
                Text = "TỜ TIỀN THỐI",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(130, 130, 150),
                AutoSize = true,
                Location = new Point(24, 310)  // dưới numpad
            };

            pnlMenhGia = new Panel
            {
                Location = new Point(435, 120),
                Size = new Size(250, 260),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };

            pnlTienMat.Controls.Add(lblMenhGiaTitle);
            pnlTienMat.Controls.Add(pnlMenhGia);
        }
        private void NumpadBtn_Click(object sender, EventArgs e)
        {
            string key = (string)((Button)sender).Tag;
            string current = txtTienKhachDua.Text
                .Replace(",", "")
                .Replace(".", "")
                .Replace(" đ", "")
                .Trim();

            if (key == "C")
            {
                txtTienKhachDua.Text = "";
                TinhTienThoi();
                return;
            }

            if (key == "⌫")
            {
                if (current.Length > 0)
                    current = current.Substring(0, current.Length - 1);
            }
            else
            {
                if (current == "0") current = "";
                current += key;
            }

            if (decimal.TryParse(current, out decimal val))
                txtTienKhachDua.Text = val.ToString("N0");
            else
                txtTienKhachDua.Text = "";

            TinhTienThoi();
        }

        private void BuildTransferPanel()
        {
            pnlChuyenKhoan = new Panel
            {
                Location = new Point(24, 228),
                Size = new Size(712, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            picQrCode = new PictureBox
            {
                Location = new Point(6, 6),
                Size = new Size(208, 208),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            picQrCode.LoadCompleted += PicQrCode_LoadCompleted;

            // Panel thông tin bên phải
            var pnlInfo = new Panel
            {
                Location = new Point(228, 6),
                Size = new Size(476, 208),
                BackColor = Color.FromArgb(245, 247, 252),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnTaiLaiQr = new Button
            {
                Text = "Tải lại QR",
                Location = new Point(340, 10),
                Size = new Size(96, 28),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnTaiLaiQr.FlatAppearance.BorderSize = 0;
            btnTaiLaiQr.Click += BtnTaiLaiQr_Click;

            lblBankValue = BuildInfoLabel("🏦  " + TransferBankName, new Point(16, 20));
            lblAccountValue = BuildInfoLabel("🔢  " + TransferAccountNumber, new Point(16, 60));
            lblAccountNameValue = BuildInfoLabel("👤  " + TransferAccountName, new Point(16, 100));
            lblAmountValue = BuildInfoLabel("💰  " + _tongTien.ToString("N0") + " đ", new Point(16, 140));

            lblQrStatus = new Label
            {
                Text = "",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(16, 180)
            };

            pnlInfo.Controls.Add(btnTaiLaiQr);
            pnlInfo.Controls.Add(lblBankValue);
            pnlInfo.Controls.Add(lblAccountValue);
            pnlInfo.Controls.Add(lblAccountNameValue);
            pnlInfo.Controls.Add(lblAmountValue);
            pnlInfo.Controls.Add(lblQrStatus);

            pnlChuyenKhoan.Controls.Add(picQrCode);
            pnlChuyenKhoan.Controls.Add(pnlInfo);
        }

        private Label BuildInfoLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = location
            };
        }

        private void FrmCashPayment_Load(object sender, EventArgs e)
        {
            UpdatePaymentMode();
        }

        private void TxtTienKhachDua_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TinhTienThoi();
            }
        }

        private void BtnTinhTien_Click(object sender, EventArgs e)
        {
            TinhTienThoi();
        }

        private void BtnXacNhan_Click(object sender, EventArgs e)
        {
            if (rdoChuyenKhoan.Checked)
            {
                PaymentMethodLabel = "Chuyển khoản";
                TienKhachDua = _tongTien;
                TienThoi = 0;
                IsConfirmed = true;

                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            decimal tienKhachDua = ParseMoney(txtTienKhachDua.Text);

            if (tienKhachDua < _tongTien)
            {
                MessageBox.Show("Số tiền khách đưa nhỏ hơn tổng tiền.", "Thông báo");
                txtTienKhachDua.Focus();
                txtTienKhachDua.SelectAll();
                return;
            }

            PaymentMethodLabel = "Tiền mặt";
            TienKhachDua = tienKhachDua;
            TienThoi = tienKhachDua - _tongTien;
            IsConfirmed = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            IsConfirmed = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PaymentMethod_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePaymentMode();
        }

        private void BtnTaiLaiQr_Click(object sender, EventArgs e)
        {
            LoadTransferQr();
        }

        private void PicQrCode_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
            {
                lblQrStatus.Text = "Không tải được QR. Kiểm tra mạng rồi bấm Tải lại QR.";
                picQrCode.Image = null;
                return;
            }

            lblQrStatus.Text = "Khách có thể quét QR và bạn xác nhận khi đã nhận chuyển khoản.";
        }

        private void UpdatePaymentMode()
        {
            bool isTransfer = rdoChuyenKhoan.Checked;

            pnlTienMat.Visible = !isTransfer;
            pnlChuyenKhoan.Visible = isTransfer;
            btnTinhTien.Visible = !isTransfer;
            btnXacNhan.Text = isTransfer ? "Xác nhận đã nhận CK" : "Xác nhận";
            this.Text = isTransfer ? "Thanh toán chuyển khoản" : "Thanh toán tiền mặt";
            lblTitle.Text = isTransfer ? "Thanh toán chuyển khoản" : "Thanh toán tiền mặt";
            lblSubtitle.Text = isTransfer
                ? "Khách quét QR để chuyển khoản đúng số tiền của hóa đơn"
                : "Nhập số tiền khách đưa để tính tiền thối";

            if (isTransfer)
            {
                LoadTransferQr();
            }
            else
            {
                txtTienKhachDua.Focus();
                txtTienKhachDua.SelectAll();
            }
        }

        private void LoadTransferQr()
        {
            lblQrStatus.Text = "Đang tạo mã QR thanh toán...";
            picQrCode.Image = null;

            try
            {
                picQrCode.LoadAsync(BuildTransferQrUrl());
            }
            catch
            {
                lblQrStatus.Text = "Không khởi tạo được mã QR. Kiểm tra cấu hình tài khoản nhận tiền.";
            }
        }

        private string BuildTransferQrUrl()
        {
            string amount = decimal.Truncate(_tongTien).ToString("0", CultureInfo.InvariantCulture);
            string addInfo = Uri.EscapeDataString(TransferContent);
            string accountName = Uri.EscapeDataString(TransferAccountName);

            return string.Format(
                CultureInfo.InvariantCulture,
                "https://img.vietqr.io/image/{0}-{1}-{2}.jpg?amount={3}&addInfo={4}&accountName={5}",
                TransferBankCode,
                TransferAccountNumber,
                TransferTemplate,
                amount,
                addInfo,
                accountName);
        }

        private void TinhTienThoi()
        {
            decimal tienKhachDua = ParseMoney(txtTienKhachDua.Text);

            if (tienKhachDua < _tongTien)
            {
                lblTienThoiValue.Text = "0 đ";
                HienThiMenhGia(0);
                return;
            }

            decimal tienThoi = tienKhachDua - _tongTien;
            lblTienThoiValue.Text = tienThoi.ToString("N0") + " đ";
            HienThiMenhGia(tienThoi);
        }

        private decimal ParseMoney(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            string normalized = input
                .Replace("đ", "")
                .Replace(",", "")
                .Replace(".", "")
                .Trim();

            decimal value;
            if (decimal.TryParse(normalized, out value))
            {
                return value;
            }

            return 0;
        }
    }
}
