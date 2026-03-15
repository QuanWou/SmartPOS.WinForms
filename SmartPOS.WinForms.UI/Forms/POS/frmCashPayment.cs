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
        private const string TransferAccountNumber = "19072952746016";
        private const string TransferAccountName = "NHA HANG SMARTPOS";
        private const string TransferContent = "Thanh toan hoa don POS";
        private const string TransferTemplate = "compact2";

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
            this.Size = new Size(760, 560);
            this.MinimumSize = new Size(760, 560);
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
                Location = new Point(24, 472),
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
                Location = new Point(486, 472),
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
                Location = new Point(616, 472),
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
                Size = new Size(712, 220),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTienKhachDua = new Label
            {
                Text = "Tiền khách đưa",
                AutoSize = true,
                Location = new Point(20, 20)
            };

            txtTienKhachDua = new TextBox
            {
                Location = new Point(20, 44),
                Size = new Size(300, 28)
            };
            txtTienKhachDua.KeyDown += TxtTienKhachDua_KeyDown;

            lblTienThoi = new Label
            {
                Text = "Tiền thối",
                AutoSize = true,
                Location = new Point(20, 88)
            };

            lblTienThoiValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 110, 200),
                AutoSize = true,
                Location = new Point(20, 112)
            };

            pnlTienMat.Controls.Add(lblTienKhachDua);
            pnlTienMat.Controls.Add(txtTienKhachDua);
            pnlTienMat.Controls.Add(lblTienThoi);
            pnlTienMat.Controls.Add(lblTienThoiValue);
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

            lblQrHint = new Label
            {
                Text = "Khách quét mã QR dưới đây để chuyển khoản đúng số tiền của hóa đơn.",
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            picQrCode = new PictureBox
            {
                Location = new Point(20, 48),
                Size = new Size(200, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };
            picQrCode.LoadCompleted += PicQrCode_LoadCompleted;

            lblQrStatus = new Label
            {
                Text = "Mã QR sẽ được tạo theo tổng tiền của hóa đơn.",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 204)
            };

            lblBankValue = BuildInfoLabel("Ngân hàng: " + TransferBankName, new Point(252, 58));
            lblAccountValue = BuildInfoLabel("Số tài khoản: " + TransferAccountNumber, new Point(252, 92));
            lblAccountNameValue = BuildInfoLabel("Chủ tài khoản: " + TransferAccountName, new Point(252, 126));
            lblAmountValue = BuildInfoLabel("Số tiền: " + _tongTien.ToString("N0") + " đ", new Point(252, 160));

            btnTaiLaiQr = new Button
            {
                Text = "Tải lại QR",
                Location = new Point(584, 18),
                Size = new Size(106, 30),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnTaiLaiQr.FlatAppearance.BorderSize = 0;
            btnTaiLaiQr.Click += BtnTaiLaiQr_Click;

            pnlChuyenKhoan.Controls.Add(lblQrHint);
            pnlChuyenKhoan.Controls.Add(picQrCode);
            pnlChuyenKhoan.Controls.Add(lblQrStatus);
            pnlChuyenKhoan.Controls.Add(lblBankValue);
            pnlChuyenKhoan.Controls.Add(lblAccountValue);
            pnlChuyenKhoan.Controls.Add(lblAccountNameValue);
            pnlChuyenKhoan.Controls.Add(lblAmountValue);
            pnlChuyenKhoan.Controls.Add(btnTaiLaiQr);
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
                MessageBox.Show("Số tiền khách đưa nhỏ hơn tổng tiền.", "Thông báo");
                lblTienThoiValue.Text = "0 đ";
                return;
            }

            decimal tienThoi = tienKhachDua - _tongTien;
            lblTienThoiValue.Text = tienThoi.ToString("N0") + " đ";
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
