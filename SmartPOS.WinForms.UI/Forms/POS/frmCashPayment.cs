using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.POS
{
    public class frmCashPayment : Form
    {
        private readonly decimal _tongTien;

        public bool IsConfirmed { get; private set; }
        public decimal TienKhachDua { get; private set; }
        public decimal TienThoi { get; private set; }

        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblTongTien;
        private Label lblTongTienValue;

        private Label lblTienKhachDua;
        private TextBox txtTienKhachDua;

        private Label lblTienThoi;
        private Label lblTienThoiValue;

        private Button btnTinhTien;
        private Button btnXacNhan;
        private Button btnDong;

        public frmCashPayment(decimal tongTien)
        {
            _tongTien = tongTien;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Thanh toán tiền mặt";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(460, 360);
            this.MinimumSize = new Size(460, 360);
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "Thanh toán tiền mặt",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 22)
            };

            lblSubtitle = new Label
            {
                Text = "Nhập số tiền khách đưa để tính tiền thối",
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

            lblTienKhachDua = new Label
            {
                Text = "Tiền khách đưa",
                AutoSize = true,
                Location = new Point(24, 176)
            };

            txtTienKhachDua = new TextBox
            {
                Location = new Point(24, 198),
                Size = new Size(390, 28)
            };
            txtTienKhachDua.KeyDown += TxtTienKhachDua_KeyDown;

            lblTienThoi = new Label
            {
                Text = "Tiền thối",
                AutoSize = true,
                Location = new Point(24, 238)
            };

            lblTienThoiValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 110, 200),
                AutoSize = true,
                Location = new Point(24, 260)
            };

            btnTinhTien = new Button
            {
                Text = "Tính tiền thối",
                Location = new Point(24, 300),
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
                Location = new Point(154, 300),
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
                Location = new Point(294, 300),
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
            this.Controls.Add(lblTienKhachDua);
            this.Controls.Add(txtTienKhachDua);
            this.Controls.Add(lblTienThoi);
            this.Controls.Add(lblTienThoiValue);
            this.Controls.Add(btnTinhTien);
            this.Controls.Add(btnXacNhan);
            this.Controls.Add(btnDong);

            this.Load += FrmCashPayment_Load;
        }

        private void FrmCashPayment_Load(object sender, EventArgs e)
        {
            txtTienKhachDua.Focus();
            txtTienKhachDua.SelectAll();
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
            decimal tienKhachDua = ParseMoney(txtTienKhachDua.Text);

            if (tienKhachDua < _tongTien)
            {
                MessageBox.Show("Số tiền khách đưa nhỏ hơn tổng tiền.", "Thông báo");
                txtTienKhachDua.Focus();
                txtTienKhachDua.SelectAll();
                return;
            }

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