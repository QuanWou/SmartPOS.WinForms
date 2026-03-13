using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.UserControls.Navigation
{
    public class UcTopBar : UserControl
    {
        public event EventHandler BtnPOSClicked;
        public event EventHandler BtnAddNewClicked;

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
        private string _titleText = "Dashboard";

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
        public UcTopBar()
        {
            this.Height = 60;
            this.Dock = DockStyle.Top;
            this.BackColor = BG;
            this.DoubleBuffered = true;

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
            var btnPOS = MakeTextButton("⊞  POS", ACCENT_DARK, Color.White, 88, 36);
            btnPOS.Click += (s, e) => BtnPOSClicked?.Invoke(this, e);

            var btnAdd = MakeTextButton("＋  Thêm mới", ACCENT_MID, Color.White, 108, 36);
            btnAdd.Click += (s, e) => BtnAddNewClicked?.Invoke(this, e);
            btnAdd.MouseEnter += (s, e) => btnAdd.BackColor = Color.FromArgb(70, 90, 180);
            btnAdd.MouseLeave += (s, e) => btnAdd.BackColor = ACCENT_MID;

            var btnBell = MakeIconButton("🔔");
            var btnSettings = MakeIconButton("⚙");
            var btnAvatar = MakeAvatarButton("A", 36);

            const int PAD = 8;
            const int TOP = 12;
            int x = 0;

            btnAvatar.Location = new Point(x, TOP); x += btnAvatar.Width + PAD;
            btnBell.Location = new Point(x, TOP); x += btnBell.Width + PAD;
            btnSettings.Location = new Point(x, TOP); x += btnSettings.Width + PAD + 4;
            btnPOS.Location = new Point(x, TOP); x += btnPOS.Width + PAD;
            btnAdd.Location = new Point(x, TOP); x += btnAdd.Width;

            p.Width = x;
            p.Controls.AddRange(new Control[]
            {
                btnAvatar, btnBell, btnSettings, btnPOS, btnAdd
            });
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

        public new string Text { get => _inner.Text; set => _inner.Text = value; }

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
            _inner.Text = "Tìm kiếm...";
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