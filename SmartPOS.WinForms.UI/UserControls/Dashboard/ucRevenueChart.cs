using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.UserControls.Dashboard
{
    // ───────────────────────────────────────────────────────
    //  Shared palette — Monochrome Light
    // ───────────────────────────────────────────────────────
    internal static class Palette
    {
        public static readonly Color BgPage = Color.FromArgb(248, 249, 251); // #F8F9FB
        public static readonly Color Surface = Color.White;
        public static readonly Color Navy = Color.FromArgb(30, 40, 80);    // #1E2850
        public static readonly Color NavyMid = Color.FromArgb(80, 95, 130);
        public static readonly Color NavyLight = Color.FromArgb(150, 162, 190);
        public static readonly Color Border = Color.FromArgb(226, 229, 240);
        public static readonly Color GridLine = Color.FromArgb(236, 239, 248);

        // Accent shades — navy family, no vivid colours
        public static readonly Color AccentDark = Color.FromArgb(30, 40, 80);   // navy fill
        public static readonly Color AccentMid = Color.FromArgb(80, 100, 160); // medium blue
        public static readonly Color AccentLight = Color.FromArgb(200, 208, 232);// pale navy
        public static readonly Color AccentGhost = Color.FromArgb(20, 30, 40, 80); // translucent

        // Positive indicator (muted green‑grey — not vivid)
        public static readonly Color PositiveText = Color.FromArgb(60, 140, 100);
        public static readonly Color PositiveBg = Color.FromArgb(18, 60, 140, 100);
    }

    // ───────────────────────────────────────────────────────
    //  Shared GDI helpers
    // ───────────────────────────────────────────────────────
    internal static class GdiHelper
    {
        /// <summary>Full rounded rectangle path.</summary>
        public static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Max(radius * 2, 1);
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        /// <summary>Rounded top + flat bottom path (for bar caps).</summary>
        public static GraphicsPath RoundedTop(RectangleF rf, int radius)
        {
            var r = Rectangle.Round(rf);
            if (r.Width <= 0 || r.Height <= 0) return new GraphicsPath();
            int d = Math.Min(radius * 2, r.Width);
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
            p.CloseFigure();
            return p;
        }

        /// <summary>Draw the white card background + border.</summary>
        public static void DrawCard(Graphics g, int w, int h, int radius = 16)
        {
            using var path = RoundedRect(new Rectangle(0, 0, w - 1, h - 1), radius);
            g.FillPath(Brushes.White, path);
            using var pen = new Pen(Palette.Border, 1f);
            g.DrawPath(pen, path);
        }

        /// <summary>
        /// Draw a text badge (rounded pill) without relying on emoji for the arrow —
        /// the arrow is drawn as a plain text glyph using a known-safe Unicode char.
        /// </summary>
        public static void DrawBadge(Graphics g, string text, Font font,
                                     Color textColor, Color bgColor,
                                     float x, float y, int radius = 8)
        {
            var sz = g.MeasureString(text, font);
            int padX = 8, padY = 3;
            var rect = new Rectangle((int)x, (int)y,
                                     (int)sz.Width + padX * 2,
                                     (int)sz.Height + padY * 2);
            using var path = RoundedRect(rect, radius);
            using var brush = new SolidBrush(bgColor);
            g.FillPath(brush, path);
            using var tb = new SolidBrush(textColor);
            g.DrawString(text, font, tb, x + padX, y + padY);
        }

        /// <summary>Configure common Graphics quality settings.</summary>
        public static void SetQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }
    }
    public enum RevenueChartType { Line, Bar }
    // ═══════════════════════════════════════════════════════
    //  Revenue Bar Chart  —  Monochrome Light
    // ═══════════════════════════════════════════════════════
    public class UcRevenueChart : UserControl
    {
        public RevenueChartType ChartType { get; set; } = RevenueChartType.Bar;
        private List<RevenueChartItemResponse> _items;
        private string _totalLabel = "0 đ";
        private string _badgeLabel = "+0% so với 7 ngày trước";
        private decimal _maxValue = 100000m;

        public UcRevenueChart()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            _items = CreateFallbackData();
            Resize += (s, e) => Invalidate();
        }

        public void SetData(
            IEnumerable<RevenueChartItemResponse> items,
            string totalLabel,
            string badgeLabel)
        {
            List<RevenueChartItemResponse> normalized = items == null
                ? new List<RevenueChartItemResponse>()
                : items.OrderBy(x => x.Ngay).ToList();

            _items = normalized.Count == 0 ? CreateFallbackData() : normalized;
            _totalLabel = string.IsNullOrWhiteSpace(totalLabel) ? "0 đ" : totalLabel;
            _badgeLabel = string.IsNullOrWhiteSpace(badgeLabel)
                ? "+0% so với 7 ngày trước"
                : badgeLabel;
            _maxValue = NormalizeMaxValue(_items.Max(x => x.DoanhThu));

            Invalidate();
        }

        // --- PHẦN 1: PHƯƠNG THỨC ONPAINT CHÍNH ---
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);

            int W = Width, H = Height;
            if (W < 20 || H < 20) return;

            // 1. Vẽ nền Card
            GdiHelper.DrawCard(g, W, H);

            // 2. Vẽ Header (Tiêu đề, Tổng tiền, Badge)
            DrawHeader(g);

            // 3. Xác định vùng vẽ biểu đồ (Chart Area)
            int cL = 42, cR = W - 14, cT = 90, cB = H - 28;
            if (cR <= cL || cB <= cT) return;
            int cW = cR - cL, cH = cB - cT;

            // 4. Vẽ lưới và nhãn trục Y
            DrawGridAndAxisY(g, cL, cR, cB, cH);

            if (_items == null || _items.Count == 0) return;

            // 5. Tính toán slotW và Render theo ChartType
            bool isBar = this.ChartType == RevenueChartType.Bar;
            float slotW = isBar
                ? (float)cW / Math.Max(1, _items.Count)
                : (float)cW / Math.Max(1, _items.Count - 1);
            if (isBar)
                RenderBarChart(g, cL, cB, cH, slotW);
            else
                RenderLineChart(g, cL, cT, cB, cH, slotW);
        }

        // --- PHẦN 2: CÁC HÀM BỔ TRỢ (Dán bên dưới OnPaint) ---

        private void DrawHeader(Graphics g)
        {
            using var fLabel = new Font("Segoe UI", 9f);
            using var fValue = new Font("Segoe UI", 15f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 8.5f);
            using var bNavy = new SolidBrush(Palette.Navy);
            using var bMid = new SolidBrush(Palette.NavyMid);

            bool positive = !_badgeLabel.StartsWith("-");
            Color textColor = positive ? Palette.PositiveText : Color.FromArgb(170, 70, 70);
            Color bgColor = positive ? Palette.PositiveBg : Color.FromArgb(24, 170, 70, 70);

            g.DrawString("Doanh thu", fLabel, bMid, 18f, 14f);
            g.DrawString(_totalLabel, fValue, bNavy, 18f, 32f);
            GdiHelper.DrawBadge(g, _badgeLabel, fSub, textColor, bgColor, 18f, 60f);
        }

        private void DrawGridAndAxisY(Graphics g, int cL, int cR, int cB, int cH)
        {
            decimal[] ySteps = GetAxisSteps();
            using var gridPen = new Pen(Palette.GridLine, 1f);
            using var fTick = new Font("Segoe UI", 6.5f);
            using var bTick = new SolidBrush(Palette.NavyLight);

            foreach (decimal yv in ySteps)
            {
                float yp = cB - (float)(yv / _maxValue) * cH;
                g.DrawLine(gridPen, cL, yp, cR, yp);
                g.DrawString(FormatCompact(yv), fTick, bTick, 2f, yp - 8f);
            }
        }

        private void RenderBarChart(Graphics g, int cL, int cB, int cH, float slotW)
        {
            float barWidth = Math.Min(20f, slotW * 0.6f); using var bBar = new SolidBrush(Palette.AccentMid);
            using var fDate = new Font("Segoe UI", 6.5f);
            using var bDate = new SolidBrush(Palette.NavyLight);

            for (int i = 0; i < _items.Count; i++)
            {
                float xCenter = cL + (i + 0.5f) * slotW;
                float barH = (float)(_items[i].DoanhThu / _maxValue) * cH;

                if (barH > 0)
                {
                    var rect = new RectangleF(xCenter - barWidth / 2f, cB - barH, barWidth, barH);
                    using var path = GdiHelper.RoundedTop(rect, 4);
                    g.FillPath(bBar, path);
                }
                DrawXLabel(g, _items[i].Ngay, xCenter, cB, fDate, bDate);
            }
        }

        private void RenderLineChart(Graphics g, int cL, int cT, int cB, int cH, float slotW)
        {
            using var fDate = new Font("Segoe UI", 6.5f);
            using var bDate = new SolidBrush(Palette.NavyLight);

            PointF[] points = _items.Select((x, i) => new PointF(
                cL + i * slotW,
                cB - (float)(x.DoanhThu / _maxValue) * cH)).ToArray();

            if (points.Length < 2) return;

            using (var fillBrush = new LinearGradientBrush(new Rectangle(cL, cT, Width, cH + 20),
                   Color.FromArgb(60, Palette.AccentMid), Color.Transparent, 90f))
            {
                PointF[] fillPoints = new PointF[points.Length + 2];
                fillPoints[0] = new PointF(points[0].X, cB);
                Array.Copy(points, 0, fillPoints, 1, points.Length);
                fillPoints[fillPoints.Length - 1] = new PointF(points[points.Length - 1].X, cB);
                g.FillPolygon(fillBrush, fillPoints);
            }

            using var linePen = new Pen(Palette.AccentMid, 2.5f) { LineJoin = LineJoin.Round };
            g.DrawLines(linePen, points);

            using var pBrush = new SolidBrush(Palette.AccentDark);
            foreach (var p in points)
            {
                g.FillEllipse(pBrush, p.X - 3.5f, p.Y - 3.5f, 7f, 7f);
                g.DrawEllipse(Pens.White, p.X - 3.5f, p.Y - 3.5f, 7f, 7f);
            }

            for (int i = 0; i < points.Length; i++)
                DrawXLabel(g, _items[i].Ngay, points[i].X, cB, fDate, bDate);
        }

        private void DrawXLabel(Graphics g, DateTime date, float x, float cB, Font f, Brush b)
        {
            string txt = date.ToString("dd/MM");
            var sz = g.MeasureString(txt, f);
            g.DrawString(txt, f, b, x - sz.Width / 2f, cB + 4f);
        }

        private List<RevenueChartItemResponse> CreateFallbackData()
        {
            DateTime today = DateTime.Today;
            return Enumerable.Range(0, 7)
                .Select(index => new RevenueChartItemResponse
                {
                    Ngay = today.AddDays(index - 6),
                    DoanhThu = 0
                })
                .ToList();
        }

        private decimal NormalizeMaxValue(decimal maxRevenue)
        {
            if (maxRevenue <= 0) return 1000m;

            decimal step = 1m;
            while (maxRevenue / step > 10m)
                step *= 10m;

            return Math.Ceiling(maxRevenue / step) * step;
        }

        private decimal[] GetAxisSteps()
        {
            return new[]
            {
                _maxValue,
                _maxValue * 0.75m,
                _maxValue * 0.5m,
                _maxValue * 0.25m,
                0m
            };
        }

        private string FormatCompact(decimal value)
        {
            if (value >= 1000000m)
            {
                return (value / 1000000m).ToString("0.#", CultureInfo.InvariantCulture) + "M";
            }

            if (value >= 1000m)
            {
                return (value / 1000m).ToString("0.#", CultureInfo.InvariantCulture) + "K";
            }

            return value.ToString("0", CultureInfo.InvariantCulture);
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Companies Weekly Chart  —  Monochrome Light
    // ═══════════════════════════════════════════════════════
    public class UcCompaniesChart : UserControl
    {
        private readonly string[] _days = { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
        private readonly int[] _curr = { 30, 55, 40, 80, 60, 90, 72 };
        private readonly int[] _prev = { 20, 35, 30, 55, 40, 65, 50 };

        public UcCompaniesChart()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            Resize += (s, e) => Invalidate();
            Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);
            int W = Width, H = Height;
            if (W < 20 || H < 20) return;

            GdiHelper.DrawCard(g, W, H);

            // ── Header ────────────────────────────────────
            using var fTitle = new Font("Segoe UI", 11f, FontStyle.Bold);
            using var fBadge = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            using var bNavy = new SolidBrush(Palette.Navy);

            g.DrawString("C\u00f4ng ty", fTitle, bNavy, 16f, 14f);

            // Badge "+6%" — right‑aligned, no emoji
            string badgeText = "+6%  5 c\u00f4ng ty so v\u1edbi th\u00e1ng tr\u01b0\u1edbc";
            var badgeSz = g.MeasureString(badgeText, fBadge);
            float badgeX = W - badgeSz.Width - 28f;
            GdiHelper.DrawBadge(g, badgeText, fBadge,
                                Palette.PositiveText, Palette.PositiveBg,
                                badgeX, 14f, 8);

            // ── Chart area ────────────────────────────────
            int cL = 12, cR = W - 12, cT = 48, cB = H - 36;
            if (cR <= cL || cB <= cT) return;
            int cH = cB - cT, cW = cR - cL;

            float slotW = (float)cW / _days.Length;
            float barW = slotW * 0.26f;

            using var fDay = new Font("Segoe UI", 7f);
            using var bDay = new SolidBrush(Palette.NavyLight);
            using var bPrev = new SolidBrush(Palette.AccentLight);
            using var bCurr = new SolidBrush(Palette.AccentDark);
            using var bPeak = new SolidBrush(Palette.AccentMid);

            int maxVal = 0;
            foreach (int v in _curr) if (v > maxVal) maxVal = v;

            for (int i = 0; i < _days.Length; i++)
            {
                float gx = cL + i * slotW;
                bool isPeak = _curr[i] == maxVal;

                // Prev bar (pale)
                float ph = (float)_prev[i] / 100f * cH;
                float px = gx + slotW * 0.08f;
                if (ph > 0)
                {
                    using var pp = GdiHelper.RoundedTop(new RectangleF(px, cB - ph, barW, ph), 3);
                    g.FillPath(bPrev, pp);
                }

                // Curr bar
                float ch = (float)_curr[i] / 100f * cH;
                float cx2 = px + barW + 3f;
                if (ch > 0)
                {
                    var br = isPeak ? bPeak : bCurr;
                    using var cp = GdiHelper.RoundedTop(new RectangleF(cx2, cB - ch, barW, ch), 3);
                    g.FillPath(br, cp);
                }

                // Peak marker — small filled circle instead of emoji star
                if (isPeak && ch > 8)
                {
                    float mx = cx2 + barW / 2f - 4f;
                    float my = cB - ch - 14f;
                    using var peakBr = new SolidBrush(Palette.AccentMid);
                    g.FillEllipse(peakBr, mx, my, 8f, 8f);
                }

                // Day label centred under the bar pair
                float pairCx = px + barW + 1.5f;
                var dSz = g.MeasureString(_days[i], fDay);
                g.DrawString(_days[i], fDay, bDay,
                             pairCx - dSz.Width / 2f, cB + 4f);
            }

            
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Top Plans Donut Chart  —  Monochrome Light
    // ═══════════════════════════════════════════════════════
    public class UcTopPlansChart : UserControl
    {
        // Monochrome palette: navy, medium, pale
        private readonly (string Label, float Pct, Color Color)[] _plans =
        {
            ("C\u01a1 b\u1ea3n",       60f, Color.FromArgb(30,  40,  80)),   // navy
            ("N\u00e2ng cao",          20f, Color.FromArgb(80,  100, 160)),  // mid
            ("Doanh nghi\u1ec7p",      20f, Color.FromArgb(170, 185, 220)),  // pale
        };

        public UcTopPlansChart()
        {
            DoubleBuffered = true;
            BackColor = Palette.BgPage;
            Resize += (s, e) => Invalidate();
            Paint += OnPaint;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            GdiHelper.SetQuality(g);
            int W = Width, H = Height;
            if (W < 40 || H < 60) return;

            GdiHelper.DrawCard(g, W, H);

            // ── Header ────────────────────────────────────
            using var fTitle = new Font("Segoe UI", 11f, FontStyle.Bold);
            using var bNavy = new SolidBrush(Palette.Navy);
            g.DrawString("G\u00f3i h\u00e0ng \u0111\u1ea7u", fTitle, bNavy, 16f, 14f);

            // ── Donut ─────────────────────────────────────
            int margin = 24;
            int availableSize = H - 110;
            if (availableSize < 60) availableSize = 60;
            if (availableSize > 140) availableSize = 140;

            int dSize = Math.Min(W - margin * 2, availableSize); int donutX = (W - dSize) / 2;
            int donutY = 44;
            var outerR = new Rectangle(donutX, donutY, dSize, dSize);
            float thick = dSize * 0.22f;
            float start = -90f;

            foreach (var plan in _plans)
            {
                float sweep = plan.Pct / 100f * 360f;
                using var br = new SolidBrush(plan.Color);
                g.FillPie(br, outerR, start, sweep);
                start += sweep;
            }

            // Donut hole
            int innerD = (int)(dSize - thick * 2);
            int innerX = donutX + (dSize - innerD) / 2;
            int innerY = donutY + (dSize - innerD) / 2;
            if (innerD > 0)
                g.FillEllipse(Brushes.White, innerX, innerY, innerD, innerD);

            // Centre label
            string centreText = "60%";
            using var fCentre = new Font("Segoe UI", 11f, FontStyle.Bold);
            var cSz = g.MeasureString(centreText, fCentre);
            float cx = donutX + dSize / 2f - cSz.Width / 2f;
            float cy = donutY + dSize / 2f - cSz.Height / 2f;
            g.DrawString(centreText, fCentre, bNavy, cx, cy);

            // ── Legend ────────────────────────────────────
            int ly = donutY + dSize + 14;
            int dotSize = 10;
            using var fLegend = new Font("Segoe UI", 8.5f);
            using var fPct = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var bMid = new SolidBrush(Palette.NavyMid);

            foreach (var plan in _plans)
            {
                if (ly + 20 > H) break; // guard overflow

                using var dotBr = new SolidBrush(plan.Color);
                g.FillEllipse(dotBr, 18, ly + 3, dotSize, dotSize);
                g.DrawString(plan.Label, fLegend, bMid, 34f, (float)ly);

                string pctStr = $"{plan.Pct:0}%";
                var pSz = g.MeasureString(pctStr, fPct);
                g.DrawString(pctStr, fPct, bNavy, W - pSz.Width - 18f, (float)ly);

                ly += 22;
            }
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Utility extension
    // ═══════════════════════════════════════════════════════
    
}
