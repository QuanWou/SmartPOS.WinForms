using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.UserControls.Dashboard
{
    // ─────────────────────────────────────────────────────────
    //  Shared base for list panels
    // ─────────────────────────────────────────────────────────
    public abstract class UcListPanel : UserControl
    {
        protected string PanelTitle = "Panel";
        protected FlowLayoutPanel ItemsFlow { get; private set; }

        // Monochrome avatar shades — no vivid colours
        protected static readonly Color[] AvatarShades =
        {
            Color.FromArgb(30,  40,  80),   // navy
            Color.FromArgb(70,  85, 130),   // mid-navy
            Color.FromArgb(110, 125, 170),  // slate
            Color.FromArgb(150, 162, 195),  // light slate
        };

        protected UcListPanel()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            BuildItemsFlow();
            Resize += (s, e) => { Invalidate(); RepositionFlow(); };
            Paint += DrawHeader;
        }

        private void BuildItemsFlow()
        {
            ItemsFlow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                AutoScroll = false,
            };
            Controls.Add(ItemsFlow);
            RepositionFlow();
        }

        private void RepositionFlow()
        {
            const int headerH = 56;
            ItemsFlow.SetBounds(0, headerH, Width, Math.Max(0, Height - headerH));
            foreach (Control c in ItemsFlow.Controls)
                c.Width = ItemsFlow.Width - 2;
        }

        private void DrawHeader(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);
            int W = Width, H = Height;
            if (W < 10 || H < 10) return;

            // Card background
            using var cardPath = GdiHelper.RoundedRect(new Rectangle(0, 0, W - 1, H - 1), 16);
            g.FillPath(Brushes.White, cardPath);
            using var borderPen = new Pen(Palette.Border, 1f);
            g.DrawPath(borderPen, cardPath);
            Region = new Region(cardPath);

            // Title
            using var fTitle = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using var bNavy = new SolidBrush(Palette.Navy);
            g.DrawString(PanelTitle, fTitle, bNavy, 16f, 15f);

            // "View all" — navy link, no orange
            using var fLink = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            using var bLink = new SolidBrush(Palette.AccentMid);
            const string linkText = "Xem tất cả \u203a";
            var linkSz = g.MeasureString(linkText, fLink);
            g.DrawString(linkText, fLink, bLink, W - linkSz.Width - 16f, 17f);

            // Divider
            using var divPen = new Pen(Palette.GridLine, 1f);
            g.DrawLine(divPen, 0, 50, W, 50);
        }

        // ── Shared row helpers ────────────────────────────────

        /// <summary>
        /// Draw avatar circle with initial letter.
        /// avatarIndex picks from <see cref="AvatarShades"/> if color not supplied.
        /// </summary>
        protected static void DrawAvatar(Graphics g, string name, Color fill,
                                         float x, float y, float size = 30f)
        {
            using var br = new SolidBrush(fill);
            g.FillEllipse(br, x, y, size, size);

            string initial = name.Length > 0 ? name[0].ToString().ToUpperInvariant() : "?";
            using var fInit = new Font("Segoe UI", size * 0.38f, FontStyle.Bold);
            var iSz = g.MeasureString(initial, fInit);
            g.DrawString(initial, fInit, Brushes.White,
                         x + size / 2f - iSz.Width / 2f - 1f,
                         y + size / 2f - iSz.Height / 2f);
        }

        /// <summary>Row separator line.</summary>
        protected static void DrawSeparator(Graphics g, int rowWidth, float y)
        {
            using var pen = new Pen(Palette.GridLine, 1f);
            g.DrawLine(pen, 52, y, rowWidth - 14, y);
        }

        /// <summary>
        /// Plan badge — monochrome tints.
        /// </summary>
        protected static void DrawPlanBadge(Graphics g, string text, float x, float y)
        {
            using var font = new Font("Segoe UI", 7f);
            var sz = g.MeasureString(text, font);
            var rect = Rectangle.Round(new RectangleF(x, y, sz.Width + 12f, 17f));

            // All badges: pale navy bg + navy text — no vivid colours
            var bgColor = Color.FromArgb(22, 30, 40, 80);   // translucent navy
            var fgColor = Palette.NavyMid;

            using var bgBr = new SolidBrush(bgColor);
            using var path = GdiHelper.RoundedRect(rect, 6);
            g.FillPath(bgBr, path);

            using var fgBr = new SolidBrush(fgColor);
            g.DrawString(text, font, fgBr, x + 5f, y + 2f);
        }
    }

    // ─────────────────────────────────────────────────────────
    //  Recent Transactions
    // ─────────────────────────────────────────────────────────
    public class UcRecentTransactions : UcListPanel
    {
        public UcRecentTransactions()
        {
            PanelTitle = "Giao dịch gần đây";

            var data = new[]
{
    ("Công ty Sao Việt", "#12457 · 14/01/2025", "+245.000 đ", "Cơ bản",     0),
    ("Công ty Minh Long", "#65974 · 10/01/2025", "+395.000 đ", "Doanh nghiệp", 1),
    ("Nova Systems",      "#33210 · 09/01/2025", "+180.000 đ", "Nâng cao",  2),
    ("Pixel Corp",        "#78543 · 07/01/2025", "+520.000 đ", "Doanh nghiệp", 3),
};

            foreach (var d in data)
                ItemsFlow.Controls.Add(CreateRow(d.Item1, d.Item2, d.Item3, d.Item4,
                                                 AvatarShades[d.Item5 % AvatarShades.Length]));
        }

        private Panel CreateRow(string name, string sub, string amount,
                                string plan, Color avatarColor)
        {
            var row = new Panel { Height = 52, BackColor = Color.Transparent };

            row.Paint += (s, e) =>
            {
                var g = e.Graphics;
                GdiHelper.SetQuality(g);
                int W = row.Width;

                DrawAvatar(g, name, avatarColor, 14f, 11f);

                using var fName = new Font("Segoe UI", 9f, FontStyle.Bold);
                using var fSub = new Font("Segoe UI", 7.5f);
                using var fAmt = new Font("Segoe UI", 10f, FontStyle.Bold);
                using var bNavy = new SolidBrush(Palette.Navy);
                using var bLight = new SolidBrush(Palette.NavyLight);
                using var bPos = new SolidBrush(Palette.PositiveText);

                g.DrawString(name, fName, bNavy, 52f, 11f);
                g.DrawString(sub, fSub, bLight, 52f, 27f);
                g.DrawString(amount, fAmt, bPos, W - 108f, 11f);
                DrawPlanBadge(g, plan, W - 108f, 29f);
                DrawSeparator(g, W, 51f);
            };

            return row;
        }
    }

    // ─────────────────────────────────────────────────────────
    //  Recently Registered
    // ─────────────────────────────────────────────────────────
    public class UcRecentlyRegistered : UcListPanel
    {
        public UcRecentlyRegistered()
        {
            PanelTitle = "Đăng kí gần đây";

            var data = new[]
{
    ("Pitch",    "Cơ bản (Tháng)",        "150 người dùng", 0),
    ("Initech",  "Doanh nghiệp (Năm)",    "200 người dùng", 1),
    ("Soylent",  "Nâng cao (Tháng)",      "80 người dùng",  2),
    ("Umbrella", "Cơ bản (Năm)",          "320 người dùng", 3),
};

            foreach (var d in data)
                ItemsFlow.Controls.Add(CreateRow(d.Item1, d.Item2, d.Item3,
                                                 AvatarShades[d.Item4 % AvatarShades.Length]));
        }

        private Panel CreateRow(string name, string plan, string users, Color avatarColor)
        {
            var row = new Panel { Height = 52, BackColor = Color.Transparent };

            row.Paint += (s, e) =>
            {
                var g = e.Graphics;
                GdiHelper.SetQuality(g);
                int W = row.Width;

                DrawAvatar(g, name, avatarColor, 14f, 11f);

                using var fName = new Font("Segoe UI", 9f, FontStyle.Bold);
                using var fPlan = new Font("Segoe UI", 7.5f);
                using var fUsers = new Font("Segoe UI", 9f, FontStyle.Bold);
                using var bNavy = new SolidBrush(Palette.Navy);
                using var bLight = new SolidBrush(Palette.NavyLight);

                g.DrawString(name, fName, bNavy, 52f, 11f);
                g.DrawString(plan, fPlan, bLight, 52f, 27f);

                var uSz = g.MeasureString(users, fUsers);
                g.DrawString(users, fUsers, bNavy,
                             W - uSz.Width - 16f, 52f / 2f - uSz.Height / 2f);

                DrawSeparator(g, W, 51f);
            };

            return row;
        }
    }

    // ─────────────────────────────────────────────────────────
    //  Recent Plan Expired
    // ─────────────────────────────────────────────────────────
    public class UcRecentPlanExpired : UcListPanel
    {
        public UcRecentPlanExpired()
        {
            PanelTitle = "Gói săp hết hạn";

            var data = new[]
{
    ("Silicon Corp", "Hết hạn: 10/04/2025", 0),
    ("Hubspot",      "Hết hạn: 12/06/2025", 1),
    ("Acme Co.",     "Hết hạn: 03/07/2025", 2),
    ("Initech",      "Hết hạn: 22/07/2025", 3),
};

            foreach (var d in data)
                ItemsFlow.Controls.Add(CreateRow(d.Item1, d.Item2,
                                                 AvatarShades[d.Item3 % AvatarShades.Length]));
        }

        private Panel CreateRow(string name, string expired, Color avatarColor)
        {
            var row = new Panel { Height = 52, BackColor = Color.Transparent };

            // Track hover state for the button
            bool hovering = false;
            Rectangle btnRect = Rectangle.Empty;

            row.Paint += (s, e) =>
            {
                var g = e.Graphics;
                GdiHelper.SetQuality(g);
                int W = row.Width;

                DrawAvatar(g, name, avatarColor, 14f, 11f);

                using var fName = new Font("Segoe UI", 9f, FontStyle.Bold);
                using var fExp = new Font("Segoe UI", 7.5f);
                using var bNavy = new SolidBrush(Palette.Navy);
                // Muted red-grey for expired date — not vivid red
                using var bExp = new SolidBrush(Color.FromArgb(170, 70, 70));

                g.DrawString(name, fName, bNavy, 52f, 11f);
                g.DrawString(expired, fExp, bExp, 52f, 27f);

                // "Send Reminder" button — navy outline, no orange
                int btnW = 104, btnH = 22;
                btnRect = new Rectangle(W - btnW - 14, 15, btnW, btnH);

                var btnBg = hovering
                    ? Color.FromArgb(30, 40, 80)    // filled on hover
                    : Color.FromArgb(12, 30, 40, 80); // ghost

                using var bgBr = new SolidBrush(btnBg);
                using var bPath = GdiHelper.RoundedRect(btnRect, 7);
                g.FillPath(bgBr, bPath);

                using var btnPen = new Pen(Palette.Navy, 1f);
                g.DrawPath(btnPen, bPath);

                using var fBtn = new Font("Segoe UI", 7.5f);
                var tCol = hovering ? Color.White : Palette.Navy;
                using var bBtn = new SolidBrush(tCol);
                var buttonText = "Nhắc gia hạn";
                var tSz = g.MeasureString(buttonText, fBtn);
                g.DrawString(buttonText, fBtn, bBtn,
                                             btnRect.X + (btnRect.Width - tSz.Width) / 2f,
                             btnRect.Y + (btnRect.Height - tSz.Height) / 2f);

                DrawSeparator(g, W, 51f);
            };

            // Hover feedback
            row.MouseMove += (s, e) =>
            {
                bool wasHover = hovering;
                hovering = btnRect.Contains(e.Location);
                row.Cursor = hovering ? Cursors.Hand : Cursors.Default;
                if (hovering != wasHover) row.Invalidate();
            };
            row.MouseLeave += (s, e) =>
            {
                if (hovering) { hovering = false; row.Invalidate(); }
            };

            return row;
        }
    }
}