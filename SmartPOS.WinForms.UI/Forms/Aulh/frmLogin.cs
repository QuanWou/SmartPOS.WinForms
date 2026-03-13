using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Forms.Main;

namespace SmartPOS.WinForms.UI.Forms.Aulh
{
    public class frmLogin : Form
    {
        // ── Left brand panel ──────────────────────────────────────────────
        private Panel pnlBrand;
        private Label lblBrandLogo;
        private Label lblBrandName;
        private Label lblBrandTagline;

        // ── Right form panel ──────────────────────────────────────────────
        private Panel pnlForm;
        private Label lblWelcome;
        private Label lblInstruction;

        private Label lblTaiKhoan;
        private RoundedTextBox txtTaiKhoan;

        private Label lblMatKhau;
        private RoundedTextBox txtMatKhau;

        private CheckBox chkHienMatKhau;
        private RoundedButton btnDangNhap;
        private Label lblThoat;

        // ── Loading overlay ───────────────────────────────────────────────
        private Panel pnlLoading;
        private Label lblLoadingText;
        private LoadingSpinner spinner;

        private readonly IAuthService _authService;

        public frmLogin()
        {
            _authService = new AuthService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "SmartPOS — Đăng nhập";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(820, 520);
            this.MinimumSize = new Size(820, 520);
            this.MaximumSize = new Size(820, 520);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(22, 32, 72);
            this.Font = new Font("Segoe UI", 9.5F);

            // ── Brand panel (left 340px) ───────────────────────────────────
            pnlBrand = new Panel
            {
                Size = new Size(340, 520),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(22, 32, 72)
            };
            pnlBrand.Paint += PnlBrand_Paint;

            lblBrandLogo = new Label
            {
                Text = "⬡",
                Font = new Font("Segoe UI", 40F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(52, 150)
            };

            lblBrandName = new Label
            {
                Text = "SmartPOS",
                Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(52, 205)
            };

            lblBrandTagline = new Label
            {
                Text = "Hệ thống quản lý bán hàng\nthông minh & hiệu quả",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(180, 190, 220),
                AutoSize = true,
                Location = new Point(52, 258)
            };

            pnlBrand.Controls.Add(lblBrandLogo);
            pnlBrand.Controls.Add(lblBrandName);
            pnlBrand.Controls.Add(lblBrandTagline);

            // ── Form panel (right 480px) ───────────────────────────────────
            pnlForm = new Panel
            {
                Size = new Size(480, 520),
                Location = new Point(340, 0),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            lblWelcome = new Label
            {
                Text = "Chào mừng trở lại",
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(60, 80)
            };

            lblInstruction = new Label
            {
                Text = "Nhập thông tin đăng nhập để tiếp tục",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(60, 116)
            };

            // Tài khoản
            lblTaiKhoan = new Label
            {
                Text = "TÀI KHOẢN",
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 110, 140),
                AutoSize = true,
                Location = new Point(60, 168)
            };

            txtTaiKhoan = new RoundedTextBox
            {
                Location = new Point(60, 188),
                Size = new Size(360, 42),
                PlaceholderText = "Nhập tài khoản..."
            };

            // Mật khẩu
            lblMatKhau = new Label
            {
                Text = "MẬT KHẨU",
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 110, 140),
                AutoSize = true,
                Location = new Point(60, 252)
            };

            txtMatKhau = new RoundedTextBox
            {
                Location = new Point(60, 272),
                Size = new Size(360, 42),
                UseSystemPasswordChar = true,
                PlaceholderText = "Nhập mật khẩu..."
            };

            chkHienMatKhau = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 110, 140),
                AutoSize = true,
                Location = new Point(60, 324),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            chkHienMatKhau.CheckedChanged += (s, e) =>
            {
                txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            };

            // Nút đăng nhập
            btnDangNhap = new RoundedButton
            {
                Text = "ĐĂNG NHẬP",
                Size = new Size(360, 48),
                Location = new Point(60, 358),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.Click += BtnDangNhap_Click;

            // Thoát
            lblThoat = new Label
            {
                Text = "Thoát ứng dụng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(160, 170, 190),
                AutoSize = true,
                Cursor = Cursors.Hand,
                Location = new Point(170, 415)
            };
            lblThoat.Click += (s, e) => Application.Exit();
            lblThoat.MouseEnter += (s, e) => lblThoat.ForeColor = Color.FromArgb(22, 32, 72);
            lblThoat.MouseLeave += (s, e) => lblThoat.ForeColor = Color.FromArgb(160, 170, 190);

            // Key handlers
            txtTaiKhoan.KeyDown += Input_KeyDown;
            txtMatKhau.KeyDown += Input_KeyDown;

            pnlForm.Controls.Add(lblWelcome);
            pnlForm.Controls.Add(lblInstruction);
            pnlForm.Controls.Add(lblTaiKhoan);
            pnlForm.Controls.Add(txtTaiKhoan);
            pnlForm.Controls.Add(lblMatKhau);
            pnlForm.Controls.Add(txtMatKhau);
            pnlForm.Controls.Add(chkHienMatKhau);
            pnlForm.Controls.Add(btnDangNhap);
            pnlForm.Controls.Add(lblThoat);

            // ── Loading overlay ────────────────────────────────────────────
            pnlLoading = new Panel
            {
                Size = this.Size,
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(200, 22, 32, 72),
                Visible = false
            };

            spinner = new LoadingSpinner
            {
                Size = new Size(48, 48),
                Location = new Point(386, 220),
                BackColor = Color.Transparent
            };

            lblLoadingText = new Label
            {
                Text = "Đang vào hệ thống...",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblLoadingText.Location = new Point(
                (820 - TextRenderer.MeasureText(lblLoadingText.Text, lblLoadingText.Font).Width) / 2,
                284);

            pnlLoading.Controls.Add(spinner);
            pnlLoading.Controls.Add(lblLoadingText);

            // ── Assemble form ──────────────────────────────────────────────
            this.Controls.Add(pnlBrand);
            this.Controls.Add(pnlForm);
            this.Controls.Add(pnlLoading);
            pnlLoading.BringToFront();

            // Drag to move (borderless)
            pnlBrand.MouseDown += DragForm;
            pnlForm.MouseDown += DragForm;

            this.Load += (s, e) => txtTaiKhoan.Focus();
        }

        // ── Drag support ──────────────────────────────────────────────────
        private void DragForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(this.Handle, 0xA1, 0x2, 0);
            }
        }

        // ── Brand panel decorative paint ──────────────────────────────────
        private void PnlBrand_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Subtle circle accents
            using (var pen = new Pen(Color.FromArgb(40, 255, 255, 255), 1.5f))
            {
                g.DrawEllipse(pen, -60, 320, 260, 260);
                g.DrawEllipse(pen, 180, -80, 200, 200);
            }

            using (var brush = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            {
                g.FillEllipse(brush, 240, 380, 160, 160);
            }
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) ThucHienDangNhap();
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            ThucHienDangNhap();
        }

        private void ThucHienDangNhap()
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                ShakePanel(pnlForm);
                ShowFieldError(string.IsNullOrEmpty(taiKhoan) ? txtTaiKhoan : txtMatKhau);
                return;
            }

            SetLoadingState(true);

            try
            {
                LoginRequest request = new LoginRequest
                {
                    TaiKhoan = taiKhoan,
                    MatKhau = matKhau
                };

                LoginResponse response = _authService.Login(request);

                if (!response.IsSuccess)
                {
                    SetLoadingState(false);

                    ShowInlineError(response.Message);
                    txtMatKhau.SelectAll();
                    txtMatKhau.Focus();
                    return;
                }
                if (_lblError != null)
                {
                    _lblError.Visible = false;
                }
                SessionManager.CurrentUser = response.User;

                // Ngắn delay để hiệu ứng loading hiện ra mượt hơn
                System.Threading.Thread.Sleep(600);

                this.Hide();
                SetLoadingState(false);

                using (frmMain mainForm = new frmMain())
                {
                    mainForm.ShowDialog();
                }

                SessionManager.Clear();
                txtMatKhau.Text = string.Empty;
                this.Show();if (!response.IsSuccess)
                txtTaiKhoan.Focus();
            }
            catch (Exception ex)
            {
                SetLoadingState(false);

                if (!this.Visible)
                {
                    this.Show();
                }

                MessageBox.Show(
                    "Có lỗi xảy ra:\n" + ex.Message,
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SetLoadingState(bool loading)
        {
            pnlLoading.Visible = loading;
            btnDangNhap.Enabled = !loading;
            if (loading) spinner.Start(); else spinner.Stop();
            this.Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
            Application.DoEvents();
        }

        // ── Inline lỗi (không dùng MessageBox) ───────────────────────────
        private Label _lblError;
        private void ShowInlineError(string message)
        {
            if (_lblError == null)
            {
                _lblError = new Label
                {
                    AutoSize = false,
                    Size = new Size(360, 28),
                    Location = new Point(60, 316),
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(200, 60, 60),
                    BackColor = Color.FromArgb(255, 235, 235),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(8, 0, 0, 0)
                };
                pnlForm.Controls.Add(_lblError);
            }
            _lblError.Text = "⚠  " + message;
            _lblError.Visible = true;
            _lblError.BringToFront();
        }

        private void ShowFieldError(Control field)
        {
            field.BackColor = Color.FromArgb(255, 240, 240);
            var timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, e) =>
            {
                field.BackColor = Color.White;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        // ── Panel shake animation ─────────────────────────────────────────
        private void ShakePanel(Panel panel)
        {
            int origX = panel.Left;
            int[] offsets = { -6, 6, -5, 5, -3, 3, 0 };
            int i = 0;
            var timer = new System.Windows.Forms.Timer { Interval = 30 };
            timer.Tick += (s, e) =>
            {
                if (i < offsets.Length) panel.Left = origX + offsets[i++];
                else { panel.Left = origX; timer.Stop(); timer.Dispose(); }
            };
            timer.Start();
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  Helper: Rounded TextBox
    // ═══════════════════════════════════════════════════════════════════════
    public class RoundedTextBox : Panel
    {
        private TextBox _inner;
        public string PlaceholderText { get; set; }
        public bool UseSystemPasswordChar
        {
            get => _inner.UseSystemPasswordChar;
            set => _inner.UseSystemPasswordChar = value;
        }
        public override string Text { get => _inner.Text; set => _inner.Text = value; }
        public new event KeyEventHandler KeyDown { add => _inner.KeyDown += value; remove => _inner.KeyDown -= value; }

        public void SelectAll() => _inner.SelectAll();
        public new void Focus() => _inner.Focus();

        public RoundedTextBox()
        {
            this.BackColor = Color.White;
            this.Padding = new Padding(12, 0, 12, 0);
            this.Cursor = Cursors.IBeam;

            _inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10.5F),
                Dock = DockStyle.None,
                Width = this.Width - 24,
                Location = new Point(12, 0)
            };

            this.Controls.Add(_inner);
            this.Paint += RoundedTextBox_Paint;
            this.Resize += (s, e) => LayoutInner();
            _inner.GotFocus += (s, e) => { this.Invalidate(); };
            _inner.LostFocus += (s, e) => { this.Invalidate(); };
        }

        private void LayoutInner()
        {
            _inner.Width = this.Width - 24;
            _inner.Top = (this.Height - _inner.Height) / 2;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            LayoutInner();
        }

        private void RoundedTextBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            bool focused = _inner.Focused;
            Color border = focused ? Color.FromArgb(22, 32, 72) : Color.FromArgb(210, 215, 225);
            int r = 8;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedRect(rect, r))
            using (var brush = new SolidBrush(Color.White))
            using (var pen = new Pen(border, focused ? 2f : 1.5f))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        private GraphicsPath RoundedRect(Rectangle b, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  Helper: Rounded Button
    // ═══════════════════════════════════════════════════════════════════════
    public class RoundedButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int r = 10;
            using (var path = RoundedRect(rect, r))
            using (var brush = new SolidBrush(this.Enabled
                ? Color.FromArgb(22, 32, 72)
                : Color.FromArgb(150, 160, 180)))
            {
                g.FillPath(brush, path);
            }
            TextRenderer.DrawText(g, this.Text, this.Font, rect, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.BackColor = Color.FromArgb(35, 48, 100);
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = Color.FromArgb(22, 32, 72);
            this.Invalidate();
        }

        private GraphicsPath RoundedRect(Rectangle b, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  Helper: Loading Spinner
    // ═══════════════════════════════════════════════════════════════════════
    public class LoadingSpinner : Control
    {
        private System.Windows.Forms.Timer _timer;
        private int _angle = 0;

        public LoadingSpinner()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint, true);
            _timer = new System.Windows.Forms.Timer { Interval = 30 };
            _timer.Tick += (s, e) => { _angle = (_angle + 12) % 360; Invalidate(); };
        }

        public void Start() => _timer.Start();
        public void Stop() { _timer.Stop(); Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int sz = Math.Min(Width, Height) - 4;
            Rectangle rect = new Rectangle(2, 2, sz, sz);

            using (var pen = new Pen(Color.FromArgb(60, 255, 255, 255), 4f))
                g.DrawEllipse(pen, rect);

            using (var pen = new Pen(Color.White, 4f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawArc(pen, rect, _angle, 90);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  P/Invoke for borderless drag
    // ═══════════════════════════════════════════════════════════════════════
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}