using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using SmartPOS.WinForms.Common.Session;

namespace SmartPOS.WinForms.UI.UserControls.Navigation
{
    public class UcTopBar : UserControl
    {
        public event EventHandler BtnPOSClicked;
        public event EventHandler BtnAddNewClicked;
        public event EventHandler<string> SearchSubmitted;
        public event EventHandler SearchCleared;
        public event EventHandler BtnBellClicked;
        public event EventHandler BtnSettingsClicked;
        public event EventHandler BtnAvatarClicked;

        private static readonly Color BG = Color.White;
        private static readonly Color BORDER_CLR = Color.FromArgb(232, 235, 244);
        private static readonly Color TEXT_MAIN = Color.FromArgb(14, 18, 38);
        private static readonly Color TEXT_DIM = Color.FromArgb(120, 132, 160);
        private static readonly Color FIELD_BG = Color.FromArgb(245, 247, 252);
        private static readonly Color ACCENT_DARK = Color.FromArgb(22, 32, 72);
        private static readonly Color ACCENT_MID = Color.FromArgb(90, 110, 200);

        private Label _lblTitle;
        private RoundedSearchBox _searchBox;
        private Panel _rightPanel;
        private Button _btnPOS;
        private Button _btnAdd;
        private Button _btnBell;
        private Button _btnSettings;
        private Button _btnAvatar;
        private string _titleText = "Dashboard";
        private bool _allowAddNew = true;
        private bool _suppressSearchClearedEvent;
        private readonly Timer _searchTimer;
        private readonly ToolTip _toolTip;
        private string _lastSubmittedKeyword = string.Empty;

        public string TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = string.IsNullOrWhiteSpace(value) ? "SmartPOS" : value;
                if (_lblTitle != null)
                {
                    _lblTitle.Text = _titleText;
                    RepositionControls();
                    Invalidate();
                }
            }
        }

        public bool AllowAddNew
        {
            get { return _allowAddNew; }
            set
            {
                _allowAddNew = value;
                UpdateRightPanelLayout();
            }
        }

        public string SearchText
        {
            get { return _searchBox?.Text ?? string.Empty; }
            set { SetSearchText(value, true); }
        }

        public string SearchPlaceholder
        {
            get { return _searchBox?.PlaceholderText ?? string.Empty; }
            set
            {
                if (_searchBox != null)
                {
                    _searchBox.PlaceholderText = value;
                }
            }
        }

        public string AvatarText
        {
            get { return _btnAvatar?.Text ?? string.Empty; }
            set
            {
                if (_btnAvatar != null)
                {
                    _btnAvatar.Text = string.IsNullOrWhiteSpace(value)
                        ? "A"
                        : value.Trim().Substring(0, 1).ToUpperInvariant();
                }
            }
        }

        public void ClearSearch(bool suppressEvent = false)
        {
            SetSearchText(string.Empty, suppressEvent);
        }

        public void SetSearchText(string value, bool suppressEvent = true)
        {
            if (_searchBox == null)
            {
                return;
            }

            _searchTimer.Stop();
            _suppressSearchClearedEvent = suppressEvent;
            _searchBox.Text = value ?? string.Empty;
            _lastSubmittedKeyword = string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
            _suppressSearchClearedEvent = false;
        }

        public UcTopBar()
        {
            this.Height = 60;
            this.Dock = DockStyle.Top;
            this.BackColor = BG;
            this.DoubleBuffered = true;
            _allowAddNew = !SessionManager.IsStaff;
            _searchTimer = new Timer { Interval = 350 };
            _searchTimer.Tick += SearchTimer_Tick;
            _toolTip = new ToolTip
            {
                ShowAlways = true,
                InitialDelay = 150,
                ReshowDelay = 100,
                AutoPopDelay = 5000
            };

            BuildUI();
        }

        private void BuildUI()
        {
            _lblTitle = new Label
            {
                Text = _titleText,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = TEXT_MAIN,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            _searchBox = new RoundedSearchBox
            {
                Width = 260,
                Height = 36,
                BackColor = FIELD_BG
            };

            _searchBox.InnerKeyDown += SearchBox_KeyDown;
            _searchBox.InnerTextChanged += SearchBox_TextChanged;
            _toolTip.SetToolTip(_searchBox, "Nhập từ khóa để tìm kiếm hoặc mở nhanh chức năng.");

            _rightPanel = new Panel
            {
                Height = 60,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            BuildRightPanel(_rightPanel);

            this.Controls.Add(_lblTitle);
            this.Controls.Add(_searchBox);
            this.Controls.Add(_rightPanel);

            this.Paint += TopBar_Paint;
            this.Resize += (s, e) => RepositionControls();
            this.Load += (s, e) => RepositionControls();
        }

        private void BuildRightPanel(Panel p)
        {
            _btnPOS = MakeTextButton("⊞  POS", ACCENT_DARK, Color.White, 88, 36);
            _btnPOS.Click += (s, e) => BtnPOSClicked?.Invoke(_btnPOS, e);
            _toolTip.SetToolTip(_btnPOS, "Mở màn hình bán hàng.");

            _btnAdd = MakeTextButton("＋  Thêm mới", ACCENT_MID, Color.White, 108, 36);
            _btnAdd.Click += (s, e) => BtnAddNewClicked?.Invoke(_btnAdd, e);
            _btnAdd.MouseEnter += (s, e) => _btnAdd.BackColor = Color.FromArgb(70, 90, 180);
            _btnAdd.MouseLeave += (s, e) => _btnAdd.BackColor = ACCENT_MID;
            _toolTip.SetToolTip(_btnAdd, "Thêm bản ghi mới theo màn hình hiện tại.");

            _btnBell = MakeIconButton("🔔");
            _btnBell.Click += (s, e) => BtnBellClicked?.Invoke(_btnBell, e);
            _toolTip.SetToolTip(_btnBell, "Xem cảnh báo và thông báo hệ thống.");
            _btnSettings = MakeIconButton("⚙");
            _btnSettings.Click += (s, e) => BtnSettingsClicked?.Invoke(_btnSettings, e);
            _toolTip.SetToolTip(_btnSettings, "Mở các thao tác nhanh.");
            _btnAvatar = MakeAvatarButton("A", 36);
            _btnAvatar.Click += (s, e) => BtnAvatarClicked?.Invoke(_btnAvatar, e);
            _toolTip.SetToolTip(_btnAvatar, "Mở menu tài khoản.");

            p.Controls.AddRange(new Control[]
            {
                _btnAvatar, _btnBell, _btnSettings, _btnPOS, _btnAdd
            });
            UpdateRightPanelLayout();
        }

        private void UpdateRightPanelLayout()
        {
            if (_rightPanel == null || _btnPOS == null || _btnAdd == null)
            {
                return;
            }

            _btnAdd.Visible = _allowAddNew;

            const int PAD = 8;
            const int TOP = 12;
            int x = 0;

            _btnAvatar.Location = new Point(x, TOP);
            x += _btnAvatar.Width + PAD;

            _btnBell.Location = new Point(x, TOP);
            x += _btnBell.Width + PAD;

            _btnSettings.Location = new Point(x, TOP);
            x += _btnSettings.Width + PAD + 4;

            _btnPOS.Location = new Point(x, TOP);
            x += _btnPOS.Width;

            if (_btnAdd.Visible)
            {
                x += PAD;
                _btnAdd.Location = new Point(x, TOP);
                x += _btnAdd.Width;
            }

            _rightPanel.Width = x;
            if (IsHandleCreated)
            {
                RepositionControls();
                Invalidate();
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            SubmitSearch(true);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSearchClearedEvent)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_searchBox.Text))
            {
                _searchTimer.Stop();
                _lastSubmittedKeyword = string.Empty;
                SearchCleared?.Invoke(this, EventArgs.Empty);
                return;
            }

            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            SubmitSearch(false);
        }

        private void SubmitSearch(bool force)
        {
            string keyword = _searchBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }

            if (!force && string.Equals(_lastSubmittedKeyword, keyword, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _lastSubmittedKeyword = keyword;
            SearchSubmitted?.Invoke(this, keyword);
        }

        private void RepositionControls()
        {
            const int SIDE_PAD = 22;
            int h = this.Height;

            _lblTitle.Location = new Point(SIDE_PAD,
                (h - _lblTitle.PreferredHeight) / 2);

            _rightPanel.Location = new Point(
                this.Width - _rightPanel.Width - SIDE_PAD,
                0);

            int searchLeft = _lblTitle.Right + 16;
            int searchRight = _rightPanel.Left - 16;
            int searchW = Math.Min(280, searchRight - searchLeft);
            if (searchW > 60)
            {
                _searchBox.Width = searchW;
                _searchBox.Location = new Point(
                    searchLeft + (searchRight - searchLeft - searchW) / 2,
                    (h - _searchBox.Height) / 2);
            }
        }

        private void TopBar_Paint(object sender, PaintEventArgs e)
        {
            using var pen = new Pen(BORDER_CLR, 1f);
            e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width, this.Height - 1);
        }

        private Button MakeTextButton(string text, Color bg, Color fg, int w, int h)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = fg,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bg, 0.1f);
            ApplyRoundedRegion(btn, 9);
            return btn;
        }

        private Button MakeIconButton(string icon)
        {
            var btn = new Button
            {
                Text = icon,
                Size = new Size(36, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = FIELD_BG,
                ForeColor = TEXT_DIM,
                Font = new Font("Segoe UI", 13F),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 235, 248);
            ApplyRoundedRegion(btn, 9);
            return btn;
        }

        private Button MakeAvatarButton(string initial, int size)
        {
            var btn = new Button
            {
                Text = initial,
                Size = new Size(size, size),
                FlatStyle = FlatStyle.Flat,
                BackColor = ACCENT_DARK,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 55, 100);
            ApplyRoundedRegion(btn, size / 2);
            return btn;
        }

        private static void ApplyRoundedRegion(Control ctrl, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            int w = ctrl.Width, h = ctrl.Height;
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(w - d, 0, d, d, 270, 90);
            path.AddArc(w - d, h - d, d, d, 0, 90);
            path.AddArc(0, h - d, d, d, 90, 90);
            path.CloseFigure();
            ctrl.Region = new Region(path);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _searchTimer?.Stop();
                _searchTimer?.Dispose();
                _toolTip?.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public class RoundedSearchBox : UserControl
    {
        private readonly TextBox _inner;
        private const int RADIUS = 10;

        private static readonly Color FIELD_BG = Color.FromArgb(245, 247, 252);
        private static readonly Color BORDER_IDLE = Color.FromArgb(220, 225, 238);
        private static readonly Color BORDER_FOCUS = Color.FromArgb(90, 110, 200);
        private static readonly Color TEXT_COLOR = Color.FromArgb(40, 50, 80);
        private static readonly Color PLACEHOLDER = Color.FromArgb(160, 170, 195);
        private string _placeholderText = "Tìm kiếm...";

        public event KeyEventHandler InnerKeyDown;
        public event EventHandler InnerTextChanged;

        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = string.IsNullOrWhiteSpace(value) ? "Tìm kiếm..." : value.Trim();

                if (_inner == null)
                {
                    return;
                }

                if (_inner.ForeColor == PLACEHOLDER || string.IsNullOrWhiteSpace(_inner.Text))
                {
                    SetPlaceholder();
                }
            }
        }

        public new string Text
        {
            get => _inner.ForeColor == PLACEHOLDER ? string.Empty : _inner.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    SetPlaceholder();
                    return;
                }

                _inner.ForeColor = TEXT_COLOR;
                _inner.Text = value;
            }
        }

        public RoundedSearchBox()
        {
            this.DoubleBuffered = true;
            this.BackColor = FIELD_BG;
            this.Cursor = Cursors.IBeam;

            _inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = FIELD_BG,
                ForeColor = TEXT_COLOR,
                Font = new Font("Segoe UI", 9.5F),
                TabStop = true
            };

            SetPlaceholder();

            _inner.KeyDown += (s, e) => InnerKeyDown?.Invoke(this, e);
            _inner.TextChanged += (s, e) => InnerTextChanged?.Invoke(this, e);

            _inner.Enter += (s, e) =>
            {
                if (_inner.ForeColor == PLACEHOLDER)
                {
                    _inner.Text = "";
                    _inner.ForeColor = TEXT_COLOR;
                }
                this.Invalidate();
            };

            _inner.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_inner.Text))
                    SetPlaceholder();
                this.Invalidate();
            };

            this.Controls.Add(_inner);
            this.Resize += (s, e) => { LayoutInner(); UpdateRegion(); };
            this.Paint += SearchBox_Paint;
            this.Click += (s, e) => _inner.Focus();

            LayoutInner();
            UpdateRegion();
        }

        private void SetPlaceholder()
        {
            _inner.Text = _placeholderText;
            _inner.ForeColor = PLACEHOLDER;
        }

        private void LayoutInner()
        {
            _inner.Location = new Point(30, (this.Height - _inner.Height) / 2);
            _inner.Width = this.Width - 38;
        }

        private void UpdateRegion()
        {
            var path = RoundedPath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), RADIUS);
            this.Region = new Region(path);
        }

        private void SearchBox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            var path = RoundedPath(rect, RADIUS);
            bool focus = _inner.Focused;

            using (var br = new SolidBrush(FIELD_BG))
                g.FillPath(br, path);

            using (var pen = new Pen(focus ? BORDER_FOCUS : BORDER_IDLE, focus ? 1.5f : 1f))
                g.DrawPath(pen, path);

            int cx = 16, cy = this.Height / 2 - 1;
            using var iconPen = new Pen(focus ? BORDER_FOCUS : PLACEHOLDER, 1.5f);
            g.DrawEllipse(iconPen, cx - 6, cy - 6, 10, 10);
            g.DrawLine(iconPen, cx + 2, cy + 2, cx + 6, cy + 6);
        }

        private static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
