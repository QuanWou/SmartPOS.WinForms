using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Linq;
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

    public enum DashboardListTone
    {
        Warning,
        Danger
    }

    public sealed class DashboardInsightItem
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string BadgeText { get; set; }
    }

    public sealed class DashboardTopProductItem
    {
        public string ProductName { get; set; }

        public int QuantitySold { get; set; }

        public decimal Revenue { get; set; }
    }

    public class UcInsightListPanel : UserControl
    {
        private List<DashboardInsightItem> _items = new List<DashboardInsightItem>();
        private string _title = "Danh sách";
        private string _subtitle = string.Empty;
        private string _emptyMessage = "Không có dữ liệu";

        public DashboardListTone Tone { get; set; } = DashboardListTone.Warning;

        public UcInsightListPanel()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            Cursor = Cursors.Hand;
            Resize += (s, e) => Invalidate();
            Paint += OnPaint;
        }

        public void SetData(
            string title,
            string subtitle,
            IEnumerable<DashboardInsightItem> items,
            string emptyMessage)
        {
            _title = string.IsNullOrWhiteSpace(title) ? "Danh sách" : title;
            _subtitle = subtitle ?? string.Empty;
            _emptyMessage = string.IsNullOrWhiteSpace(emptyMessage)
                ? "Không có dữ liệu"
                : emptyMessage;
            _items = items == null
                ? new List<DashboardInsightItem>()
                : items.Where(x => x != null).Take(5).ToList();

            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);
            int width = Width;
            int height = Height;
            if (width < 40 || height < 40)
            {
                return;
            }

            GdiHelper.DrawCard(g, width, height);

            using var titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 8f);
            using var titleBrush = new SolidBrush(Palette.Navy);
            using var subBrush = new SolidBrush(Palette.NavyLight);
            using var dividerPen = new Pen(Palette.GridLine, 1f);

            g.DrawString(_title, titleFont, titleBrush, 16f, 14f);
            g.DrawString(_subtitle, subFont, subBrush, 16f, 33f);
            g.DrawLine(dividerPen, 0, 56, width, 56);

            if (_items.Count == 0)
            {
                using var emptyFont = new Font("Segoe UI", 9f, FontStyle.Regular);
                using var emptyBrush = new SolidBrush(Palette.NavyLight);
                var emptyRect = new RectangleF(16f, 72f, width - 32f, height - 88f);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_emptyMessage, emptyFont, emptyBrush, emptyRect, sf);
                return;
            }

            Color accent = Tone == DashboardListTone.Warning
                ? Color.FromArgb(148, 88, 0)
                : Color.FromArgb(156, 56, 56);
            Color badgeBg = Tone == DashboardListTone.Warning
                ? Color.FromArgb(255, 246, 220)
                : Color.FromArgb(255, 237, 236);
            Color iconBg = Tone == DashboardListTone.Warning
                ? Color.FromArgb(255, 239, 202)
                : Color.FromArgb(255, 228, 226);

            using var titleRowFont = new Font("Segoe UI", 9f, FontStyle.Bold);
            using var subtitleRowFont = new Font("Segoe UI", 7.75f, FontStyle.Regular);
            using var badgeFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            using var titleRowBrush = new SolidBrush(Palette.Navy);
            using var subtitleRowBrush = new SolidBrush(Palette.NavyLight);
            using var accentBrush = new SolidBrush(accent);
            using var iconBrush = new SolidBrush(iconBg);
            using var badgeTextBrush = new SolidBrush(accent);
            using var noWrap = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            const int rowHeight = 38;
            float y = 68f;
            for (int i = 0; i < _items.Count; i++)
            {
                DashboardInsightItem item = _items[i];
                float circleX = 16f;
                float circleY = y + 2f;
                g.FillEllipse(iconBrush, circleX, circleY, 26f, 26f);

                string rank = (i + 1).ToString();
                using var rankFont = new Font("Segoe UI", 8f, FontStyle.Bold);
                var rankSize = g.MeasureString(rank, rankFont);
                g.DrawString(
                    rank,
                    rankFont,
                    accentBrush,
                    circleX + 13f - rankSize.Width / 2f,
                    circleY + 13f - rankSize.Height / 2f);

                float textX = 52f;
                float textWidth = width - 160f;
                g.DrawString(
                    item.Title ?? string.Empty,
                    titleRowFont,
                    titleRowBrush,
                    new RectangleF(textX, y + 1f, textWidth, 16f),
                    noWrap);
                g.DrawString(
                    item.Subtitle ?? string.Empty,
                    subtitleRowFont,
                    subtitleRowBrush,
                    new RectangleF(textX, y + 18f, textWidth, 14f),
                    noWrap);

                string badgeText = item.BadgeText ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(badgeText))
                {
                    SizeF badgeSize = g.MeasureString(badgeText, badgeFont);
                    Rectangle badgeRect = new Rectangle(
                        width - (int)badgeSize.Width - 32,
                        (int)y + 7,
                        (int)badgeSize.Width + 16,
                        20);

                    using var badgePath = GdiHelper.RoundedRect(badgeRect, 7);
                    using var badgeBrush = new SolidBrush(badgeBg);
                    g.FillPath(badgeBrush, badgePath);
                    g.DrawString(
                        badgeText,
                        badgeFont,
                        badgeTextBrush,
                        badgeRect.X + 8f,
                        badgeRect.Y + 3f);
                }

                if (i < _items.Count - 1)
                {
                    g.DrawLine(dividerPen, 16, y + rowHeight, width - 16, y + rowHeight);
                }

                y += rowHeight;
            }
        }
    }

    public class UcTopProductsChart : UserControl
    {
        private List<DashboardTopProductItem> _items = new List<DashboardTopProductItem>();
        private string _subtitle = "30 ngày gần nhất";

        private static readonly Color[] BarColors =
        {
            Color.FromArgb(30, 40, 80),
            Color.FromArgb(54, 72, 120),
            Color.FromArgb(80, 100, 160),
            Color.FromArgb(112, 130, 182),
            Color.FromArgb(150, 162, 190)
        };

        public UcTopProductsChart()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            Resize += (s, e) => Invalidate();
            Paint += OnPaint;
        }

        public void SetData(IEnumerable<DashboardTopProductItem> items, string subtitle)
        {
            _items = items == null
                ? new List<DashboardTopProductItem>()
                : items.Where(x => x != null).Take(5).ToList();
            _subtitle = string.IsNullOrWhiteSpace(subtitle)
                ? "30 ngày gần nhất"
                : subtitle;

            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);
            int width = Width;
            int height = Height;
            if (width < 40 || height < 40)
            {
                return;
            }

            GdiHelper.DrawCard(g, width, height);

            using var titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 8f);
            using var titleBrush = new SolidBrush(Palette.Navy);
            using var subBrush = new SolidBrush(Palette.NavyLight);

            g.DrawString("Top 5 sản phẩm bán chạy", titleFont, titleBrush, 16f, 14f);
            g.DrawString(_subtitle, subFont, subBrush, 16f, 33f);

            if (_items.Count == 0)
            {
                using var emptyFont = new Font("Segoe UI", 9f, FontStyle.Regular);
                var emptyRect = new RectangleF(16f, 60f, width - 32f, height - 76f);
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString("Chưa có dữ liệu bán hàng để hiển thị.", emptyFont, subBrush, emptyRect, sf);
                return;
            }

            int maxQuantity = Math.Max(1, _items.Max(x => x.QuantitySold));
            using var nameFont = new Font("Segoe UI", 8.75f, FontStyle.Bold);
            using var detailFont = new Font("Segoe UI", 7.75f, FontStyle.Regular);
            using var qtyFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var nameBrush = new SolidBrush(Palette.Navy);
            using var detailBrush = new SolidBrush(Palette.NavyLight);
            using var trackBrush = new SolidBrush(Color.FromArgb(235, 237, 247));
            using var noWrap = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            const int rowHeight = 46;
            float y = 64f;
            for (int i = 0; i < _items.Count; i++)
            {
                DashboardTopProductItem item = _items[i];
                using var rankFont = new Font("Segoe UI", 8f, FontStyle.Bold);
                using var rankBrush = new SolidBrush(BarColors[i % BarColors.Length]);
                g.DrawString((i + 1).ToString(), rankFont, rankBrush, 16f, y + 2f);

                float textX = 34f;
                g.DrawString(
                    item.ProductName ?? string.Empty,
                    nameFont,
                    nameBrush,
                    new RectangleF(textX, y, width - 150f, 16f),
                    noWrap);

                g.DrawString(
                    "Doanh thu " + item.Revenue.ToString("N0") + " đ",
                    detailFont,
                    detailBrush,
                    new RectangleF(textX, y + 18f, width - 150f, 14f),
                    noWrap);

                string qtyLabel = item.QuantitySold.ToString("N0") + " đã bán";
                SizeF qtySize = g.MeasureString(qtyLabel, qtyFont);
                float qtyX = width - qtySize.Width - 16f;
                g.DrawString(qtyLabel, qtyFont, nameBrush, qtyX, y + 1f);

                Rectangle trackRect = new Rectangle((int)textX, (int)y + 33, width - 112, 8);
                using var trackPath = GdiHelper.RoundedRect(trackRect, 4);
                g.FillPath(trackBrush, trackPath);

                int fillWidth = (int)((float)item.QuantitySold / maxQuantity * trackRect.Width);
                if (fillWidth > 0)
                {
                    Rectangle fillRect = new Rectangle(trackRect.X, trackRect.Y, fillWidth, trackRect.Height);
                    using var fillPath = GdiHelper.RoundedRect(fillRect, 4);
                    using var fillBrush = new SolidBrush(BarColors[i % BarColors.Length]);
                    g.FillPath(fillBrush, fillPath);
                }

                y += rowHeight;
            }
        }
    }
}
