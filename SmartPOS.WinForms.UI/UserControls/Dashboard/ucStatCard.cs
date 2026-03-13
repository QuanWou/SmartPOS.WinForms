using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.UserControls.Dashboard
{
    public class UcStatCard : UserControl
    {
        // ── Backing fields ────────────────────────────────────────────────
        private string _title = "Tổng số";
        private string _value = "0";
        private string _change = "+0%";
        private bool _isPositive = true;
        private Color _iconColor = Color.FromArgb(60, 80, 180);
        private string _iconText = "■";
        private bool _hovered = false;

        // ── Public properties ─────────────────────────────────────────────
        public string Title { get => _title; set { _title = value; Invalidate(); } }
        public string Value { get => _value; set { _value = value; Invalidate(); } }
        public string Change { get => _change; set { _change = value; Invalidate(); } }
        public bool IsPositive { get => _isPositive; set { _isPositive = value; Invalidate(); } }
        public Color IconColor { get => _iconColor; set { _iconColor = value; Invalidate(); } }
        public string IconText { get => _iconText; set { _iconText = value; Invalidate(); } }

        // Mini sparkline data (0–100)
        public int[] ChartData { get; set; } = { 30, 50, 38, 65, 48, 75, 55, 85, 65, 92 };

        // ── Design tokens ─────────────────────────────────────────────────
        private static readonly Color SURFACE = Color.White;
        private static readonly Color SURFACE_HOV = Color.FromArgb(249, 250, 255);
        private static readonly Color BORDER = Color.FromArgb(228, 232, 244);
        private static readonly Color BORDER_HOV = Color.FromArgb(200, 210, 240);
        private static readonly Color TEXT_VALUE = Color.FromArgb(12, 16, 40);
        private static readonly Color TEXT_TITLE = Color.FromArgb(130, 142, 168);
        private static readonly Color POS_FG = Color.FromArgb(22, 160, 80);
        private static readonly Color POS_BG = Color.FromArgb(28, 22, 160, 80);
        private static readonly Color NEG_FG = Color.FromArgb(200, 50, 50);
        private static readonly Color NEG_BG = Color.FromArgb(28, 200, 50, 50);

        // ─────────────────────────────────────────────────────────────────
        public UcStatCard()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent; // parent paints bg; we handle our own surface
            this.Size = new Size(240, 110);
            this.Cursor = Cursors.Hand;

            this.Paint += OnCardPaint;
            this.Resize += (s, e) => Invalidate();
        }
        private void UpdateRegion()
{
    using var path = RoundedPath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 14);
    this.Region = new Region(path);
}
        // ══════════════════════════════════════════════════════════════════
        //  PAINT
        // ══════════════════════════════════════════════════════════════════
        private void OnCardPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int W = this.Width, H = this.Height;
            var cardRect = new Rectangle(0, 0, W - 1, H - 1);
            var cardPath = RoundedPath(cardRect, 14);

            // ── Surface ───────────────────────────────────────────────────
            using (var br = new SolidBrush(_hovered ? SURFACE_HOV : SURFACE))
                g.FillPath(br, cardPath);

            // ── Drop shadow (two-pass soft border) ────────────────────────
            using (var shadowPen = new Pen(Color.FromArgb(10, 0, 0, 40), 2f))
                g.DrawPath(shadowPen, RoundedPath(new Rectangle(1, 2, W - 3, H - 2), 14));

            // ── Card border ───────────────────────────────────────────────
            using (var borderPen = new Pen(_hovered ? BORDER_HOV : BORDER, 1f))
                g.DrawPath(borderPen, cardPath);

            // Clip to card shape so nothing bleeds outside rounded corners
            g.SetClip(cardPath);
            this.Resize += (s, e) =>
            {
                UpdateRegion();
                Invalidate();
            };
            UpdateRegion();
            // ── Icon badge (top-left) ─────────────────────────────────────
            DrawIconBadge(g);

            // ── Change badge (top-right) ──────────────────────────────────
            DrawChangeBadge(g, W);

            // ── Value ─────────────────────────────────────────────────────
            using var valFont = new Font("Segoe UI", 17F, FontStyle.Bold);
            using var valBr = new SolidBrush(TEXT_VALUE);
            g.DrawString(_value, valFont, valBr, 16, H - 48);

            // ── Title ─────────────────────────────────────────────────────
            using var titleFont = new Font("Segoe UI", 8F);
            using var titleBr = new SolidBrush(TEXT_TITLE);
            g.DrawString(_title, titleFont, titleBr, 16, H - 22);

            // ── Sparkline ─────────────────────────────────────────────────
            if (ChartData != null && ChartData.Length > 1)
                DrawSparkline(g, W, H);

            g.ResetClip();
        }

        // ── Icon badge ────────────────────────────────────────────────────
        private void DrawIconBadge(Graphics g)
        {
            var iconRect = new Rectangle(14, 14, 38, 38);

            using var bgBr = new SolidBrush(Color.FromArgb(22, _iconColor));
            using var bgPath = RoundedPath(iconRect, 10);
            g.FillPath(bgBr, bgPath);

            using var iconBr = new SolidBrush(_iconColor);
            using var iconFont = new Font("Segoe UI Symbol", 12F, FontStyle.Bold);

            string iconText = string.IsNullOrWhiteSpace(_iconText) ? "■" : _iconText;
            SizeF textSize = g.MeasureString(iconText, iconFont);

            float x = iconRect.X + (iconRect.Width - textSize.Width) / 2f;
            float y = iconRect.Y + (iconRect.Height - textSize.Height) / 2f - 1f;

            g.DrawString(iconText, iconFont, iconBr, x, y);
        }

        // ── Change badge ──────────────────────────────────────────────────
        private void DrawChangeBadge(Graphics g, int W)
        {
            string arrow = _isPositive ? "↑" : "↓";
            string label = $"{arrow} {_change}";
            Color fg = _isPositive ? POS_FG : NEG_FG;
            Color bg = _isPositive ? POS_BG : NEG_BG;

            using var fnt = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            var sz = g.MeasureString(label, fnt);
            float padX = 8f, padY = 4f;
            float bw = sz.Width + padX * 2;
            float bh = sz.Height + padY * 2;
            float bx = W - bw - 12;
            float by = 12;

            var badgeRect = new RectangleF(bx, by, bw, bh);
            using var bgBr = new SolidBrush(bg);
            using var fgBr = new SolidBrush(fg);
            g.FillPath(bgBr, RoundedPath(Rectangle.Round(badgeRect), 7));
            g.DrawString(label, fnt, fgBr, bx + padX, by + padY);
        }

        // ── Sparkline ─────────────────────────────────────────────────────
        private void DrawSparkline(Graphics g, int W, int H)
        {
            const int CHART_W = 64;
            const int CHART_H = 28;
            int cx = W - CHART_W - 12;
            int cy = H - CHART_H - 12;

            var data = ChartData;
            int count = data.Length;

            // Build polyline points
            var pts = new PointF[count];
            for (int i = 0; i < count; i++)
            {
                float px = cx + (float)i / (count - 1) * CHART_W;
                float py = cy + CHART_H - (Math.Max(0, Math.Min(100, data[i])) / 100f) * CHART_H;
                pts[i] = new PointF(px, py);
            }

            Color lineClr = _isPositive
                ? Color.FromArgb(60, 160, 110)
                : Color.FromArgb(200, 80, 80);

            // Gradient fill under line
            var fillPts = new PointF[count + 2];
            fillPts[0] = new PointF(pts[0].X, cy + CHART_H);
            pts.CopyTo(fillPts, 1);
            fillPts[count + 1] = new PointF(pts[count - 1].X, cy + CHART_H);

            using (var grad = new LinearGradientBrush(
                new PointF(cx, cy), new PointF(cx, cy + CHART_H),
                Color.FromArgb(55, lineClr), Color.FromArgb(4, lineClr)))
                g.FillPolygon(grad, fillPts);

            // Line
            using (var pen = new Pen(lineClr, 1.8f) { LineJoin = LineJoin.Round })
                g.DrawLines(pen, pts);

            // End dot
            var last = pts[count - 1];
            using (var dotBr = new SolidBrush(lineClr))
                g.FillEllipse(dotBr, last.X - 3f, last.Y - 3f, 6f, 6f);
            g.FillEllipse(Brushes.White, last.X - 1.5f, last.Y - 1.5f, 3f, 3f);
        }

        // ══════════════════════════════════════════════════════════════════
        //  HOVER
        // ══════════════════════════════════════════════════════════════════
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hovered = false;
            Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════
        //  HELPER
        // ══════════════════════════════════════════════════════════════════
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