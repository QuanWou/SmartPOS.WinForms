using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.Common.Session;

namespace SmartPOS.WinForms.UI.UserControls.Navigation
{
    public class UcSidebar : UserControl
    {
        // ── State ─────────────────────────────────────────────────────────
        private bool _collapsed = false;
        private string _activeMenu = "Dashboard";
        private Timer _animTimer;

        // ── Controls ──────────────────────────────────────────────────────
        private Panel _pnlLogo;
        private FlowLayoutPanel _flpMenu;
        private Button _btnToggle;

        // ── Design tokens ─────────────────────────────────────────────────
        private static readonly Color BG_DARK = Color.FromArgb(14, 18, 38);
        private static readonly Color BG_ITEM_HOVER = Color.FromArgb(30, 38, 68);
        private static readonly Color ACTIVE_BG = Color.FromArgb(30, 40, 80);
        private static readonly Color ACTIVE_PILL = Color.FromArgb(90, 110, 200);
        private static readonly Color TEXT_DIM = Color.FromArgb(120, 132, 160);
        private static readonly Color TEXT_BRIGHT = Color.FromArgb(220, 224, 240);
        private static readonly Color TEXT_ACTIVE = Color.White;
        private static readonly Color SEP_COLOR = Color.FromArgb(30, 36, 62);
        private static readonly Color LOGO_ACCENT = Color.FromArgb(90, 110, 200);

        // ── Properties ────────────────────────────────────────────────────
        public int ExpandedWidth { get; set; } = 220;
        public int CollapsedWidth { get; set; } = 64;
        public event EventHandler<string> MenuClicked;

        public void ToggleCollapsed()
        {
            ToggleSidebar();
        }

        // ── Menu data ─────────────────────────────────────────────────────
        private readonly List<(string Icon, string Text, string Key)> _items =
            new List<(string, string, string)>
        {
            ("⊞",  "Tổng quan",            "Dashboard"),
            ("⌁",  "Bán hàng",             "POS"),
            ("🧾", "Hóa đơn",              "Invoices"),
            ("👤", "Người dùng",           "Users"),
            ("◔",  "Báo cáo",              "Reports"),
            ("---","",                     "sep1"),

            ("◫",  "Sản phẩm",             "Products"),
            ("＋", "Thêm sản phẩm",        "CreateProduct"),
            ("⊘",  "Ngừng bán / hết hạn",  "ExpiredProducts"),
            ("◎",  "Sắp hết hàng",         "LowStocks"),
            ("▦",  "Danh mục",             "Category"),
            ("---","",                     "sep2"),

            ("◫",  "Nhập kho",             "ManageStock"),
            ("↺",  "Lịch sử nhập kho",     "StockAdjust"),
        };

        public UcSidebar()
        {
            this.Width = ExpandedWidth;
            this.Dock = DockStyle.Left;
            this.BackColor = BG_DARK;
            this.DoubleBuffered = true;

            BuildUI();
        }

        public void SetActiveMenu(string menuKey)
        {
            if (string.IsNullOrWhiteSpace(menuKey))
            {
                return;
            }

            bool isVisibleMenu = _items.Any(x => x.Key == menuKey && IsMenuVisible(x.Key));
            if (!isVisibleMenu || string.Equals(_activeMenu, menuKey, StringComparison.Ordinal))
            {
                return;
            }

            _activeMenu = menuKey;
            RefreshAllButtons();
        }

        private void BuildUI()
        {
            _pnlLogo = new Panel
            {
                Height = 64,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };
            _pnlLogo.Paint += LogoPanel_Paint;

            _btnToggle = new Button
            {
                Text = "≡",
                Font = new Font("Segoe UI", 14F),
                ForeColor = TEXT_DIM,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 36),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabStop = false
            };
            _btnToggle.FlatAppearance.BorderSize = 0;
            _btnToggle.FlatAppearance.MouseOverBackColor = BG_ITEM_HOVER;
            _btnToggle.Click += BtnToggle_Click;
            _btnToggle.MouseEnter += (s, e) => _btnToggle.ForeColor = TEXT_BRIGHT;
            _btnToggle.MouseLeave += (s, e) => _btnToggle.ForeColor = TEXT_DIM;
            PositionToggle();
            _pnlLogo.Controls.Add(_btnToggle);

            _flpMenu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 6, 8, 12)
            };
            _flpMenu.VerticalScroll.Visible = false;
            _flpMenu.HorizontalScroll.Enabled = false;

            foreach (var item in _items.Where(x => IsMenuVisible(x.Key)))
            {
                _flpMenu.Controls.Add(item.Key.StartsWith("sep")
                    ? (Control)BuildSeparator()
                    : BuildMenuButton(item.Icon, item.Text, item.Key));
            }

            this.Controls.Add(_flpMenu);
            this.Controls.Add(_pnlLogo);
        }

        private bool IsMenuVisible(string key)
        {
            if (key.StartsWith("sep"))
            {
                return true;
            }

            if (!SessionManager.IsStaff)
            {
                return true;
            }

            switch (key)
            {
                case "Dashboard":
                case "POS":
                case "Invoices":
                case "Products":
                case "ExpiredProducts":
                case "LowStocks":
                    return true;

                default:
                    return false;
            }
        }

        private Panel BuildSeparator()
        {
            return new Panel
            {
                Height = 1,
                Width = ExpandedWidth - 24,
                BackColor = SEP_COLOR,
                Margin = new Padding(4, 10, 4, 10)
            };
        }

        private Panel BuildMenuButton(string icon, string text, string key)
        {
            bool active = key == _activeMenu;

            var wrap = new Panel
            {
                Tag = key,
                Width = ExpandedWidth - 16,
                Height = 40,
                BackColor = active ? ACTIVE_BG : Color.Transparent,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 0, 1)
            };
            ApplyRoundedRegion(wrap, 8);

            wrap.Paint += (s, e) =>
            {
                if ((string)wrap.Tag == _activeMenu)
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using var br = new SolidBrush(ACTIVE_PILL);
                    g.FillRectangle(br, 0, 8, 3, wrap.Height - 16);
                }
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 11F),
                ForeColor = active ? TEXT_ACTIVE : TEXT_DIM,
                Size = new Size(36, 40),
                Location = new Point(10, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Tag = key + "_icon"
            };

            var lblText = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, active ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = active ? TEXT_ACTIVE : TEXT_DIM,
                AutoSize = false,
                Size = new Size(wrap.Width - 52, 40),
                Location = new Point(46, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = !_collapsed,
                Tag = key + "_text"
            };

            wrap.Controls.AddRange(new Control[] { lblIcon, lblText });

            Action setHover = () =>
            {
                if ((string)wrap.Tag == _activeMenu) return;
                wrap.BackColor = BG_ITEM_HOVER;
                lblIcon.ForeColor = TEXT_BRIGHT;
                lblText.ForeColor = TEXT_BRIGHT;
            };

            Action clearHover = () =>
            {
                if ((string)wrap.Tag == _activeMenu) return;
                wrap.BackColor = Color.Transparent;
                lblIcon.ForeColor = TEXT_DIM;
                lblText.ForeColor = TEXT_DIM;
            };

            Action doClick = () =>
            {
                _activeMenu = (string)wrap.Tag;
                RefreshAllButtons();
                MenuClicked?.Invoke(this, _activeMenu);
            };

            wrap.MouseEnter += (s, e) => setHover();
            wrap.MouseLeave += (s, e) => clearHover();
            wrap.Click += (s, e) => doClick();

            lblIcon.MouseEnter += (s, e) => setHover();
            lblIcon.MouseLeave += (s, e) => clearHover();
            lblIcon.Click += (s, e) => doClick();

            lblText.MouseEnter += (s, e) => setHover();
            lblText.MouseLeave += (s, e) => clearHover();
            lblText.Click += (s, e) => doClick();

            return wrap;
        }

        private void RefreshAllButtons()
        {
            foreach (Control ctrl in _flpMenu.Controls)
            {
                if (ctrl is Panel wrap && wrap.Tag is string key && !key.StartsWith("sep"))
                {
                    bool active = key == _activeMenu;

                    wrap.BackColor = active ? ACTIVE_BG : Color.Transparent;
                    ApplyRoundedRegion(wrap, 8);
                    wrap.Width = _collapsed ? CollapsedWidth - 12 : ExpandedWidth - 16;
                    wrap.Invalidate();

                    foreach (Control child in wrap.Controls)
                    {
                        if (child is Label lbl)
                        {
                            if (lbl.Tag?.ToString().EndsWith("_icon") == true)
                            {
                                lbl.ForeColor = active ? TEXT_ACTIVE : TEXT_DIM;
                            }
                            else if (lbl.Tag?.ToString().EndsWith("_text") == true)
                            {
                                lbl.ForeColor = active ? TEXT_ACTIVE : TEXT_DIM;
                                lbl.Font = new Font("Segoe UI", 9F,
                                    active ? FontStyle.Bold : FontStyle.Regular);
                                lbl.Visible = !_collapsed;
                                lbl.Width = wrap.Width - 52;
                            }
                        }
                    }
                }
                else if (ctrl is Panel sep && sep.Tag == null)
                {
                    sep.Width = _collapsed ? CollapsedWidth - 20 : ExpandedWidth - 24;
                }
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            ToggleSidebar();
        }

        private void ToggleSidebar()
        {
            _collapsed = !_collapsed;
            _animTimer?.Stop();
            _animTimer?.Dispose();

            int target = _collapsed ? CollapsedWidth : ExpandedWidth;
            _animTimer = new Timer { Interval = 10 };
            _animTimer.Tick += (s, _) =>
            {
                int step = Math.Max(6, Math.Abs(this.Width - target) / 3);
                if (_collapsed)
                    this.Width = Math.Max(this.Width - step, CollapsedWidth);
                else
                    this.Width = Math.Min(this.Width + step, ExpandedWidth);

                PositionToggle();
                _pnlLogo.Invalidate();
                RefreshAllButtons();

                if (this.Width == target)
                {
                    _animTimer.Stop();
                    _animTimer.Dispose();
                    _animTimer = null;
                }
            };
            _animTimer.Start();
        }

        private void PositionToggle()
        {
            if (_btnToggle == null) return;
            _btnToggle.Location = new Point(this.Width - 44, 14);
        }

        private void LogoPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var logoRect = new Rectangle(12, 14, 36, 36);
            using (var br = new SolidBrush(ACTIVE_BG))
                g.FillEllipse(br, logoRect);
            using (var pen = new Pen(LOGO_ACCENT, 2f))
                g.DrawEllipse(pen, logoRect);
            using (var brTxt = new SolidBrush(Color.White))
            using (var fnt = new Font("Segoe UI", 14F, FontStyle.Bold))
                g.DrawString("S", fnt, brTxt, 20, 18);

            if (!_collapsed)
            {
                using var brName = new SolidBrush(Color.White);
                using var fntName = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                g.DrawString("SmartPOS", fntName, brName, 56, 18);

                using var brSub = new SolidBrush(TEXT_DIM);
                using var fntSub = new Font("Segoe UI", 7.5F);
                g.DrawString("Quản lý bán hàng", fntSub, brSub, 57, 36);
            }
        }

        private static void ApplyRoundedRegion(Control ctrl, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            var r = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            ctrl.Region = new Region(path);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animTimer?.Stop();
                _animTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
