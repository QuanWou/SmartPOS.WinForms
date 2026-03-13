using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

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

    // ═══════════════════════════════════════════════════════
    //  Revenue Bar Chart  —  Monochrome Light
    // ═══════════════════════════════════════════════════════
    public class UcRevenueChart : UserControl
    {
        private readonly string[] _months =
            { "T1","T2","T3","T4","T5","T6","T7","T8","T9","T10","T11","T12" };
        private readonly int[] _values =
            { 45000,38000,52000,41000,68000,55000,72000,60000,65000,48000,20000,58000 };

        private const int MaxValue = 80000;

        public UcRevenueChart()
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
            using var fLabel = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fValue = new Font("Segoe UI", 15f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            using var bNavy = new SolidBrush(Palette.Navy);
            using var bMid = new SolidBrush(Palette.NavyMid);
            using var bPos = new SolidBrush(Palette.PositiveText);

            g.DrawString("Doanh thu", fLabel, bMid, 18f, 14f);
            g.DrawString("45.787.000 \u0111", fValue, bNavy, 18f, 32f);

            // Badge: "+40% so v\u1edbi n\u0103m ngo\u00e1i" — use ASCII arrow, no emoji
            GdiHelper.DrawBadge(g, "+40% so v\u1edbi n\u0103m ngo\u00e1i", fSub,
                                Palette.PositiveText, Palette.PositiveBg,
                                18f, 60f);

            // ── Chart area ────────────────────────────────
            int cL = 42, cR = W - 14, cT = 90, cB = H - 28;
            if (cR <= cL || cB <= cT) return;
            int cW = cR - cL, cH = cB - cT;

            // Grid lines + Y labels
            int[] ySteps = { 80000, 60000, 40000, 20000, 0 };
            using var gridPen = new Pen(Palette.GridLine, 1f);
            using var fTick = new Font("Segoe UI", 6.5f);
            using var bTick = new SolidBrush(Palette.NavyLight);

            foreach (int yv in ySteps)
            {
                float yp = cB - (float)yv / MaxValue * cH;
                g.DrawLine(gridPen, cL, yp, cR, yp);
                string lbl = yv == 0 ? "0" : (yv / 1000) + "K";
                g.DrawString(lbl, fTick, bTick, 2f, yp - 8f);
            }

            // Bars — navy gradient
            float slotW = (float)cW / _months.Length;
            float barW = slotW * 0.55f;

            using var fMonth = new Font("Segoe UI", 6.5f);
            using var bMonth = new SolidBrush(Palette.NavyLight);

            for (int i = 0; i < _months.Length; i++)
            {
                float bx = cL + i * slotW + (slotW - barW) / 2f;
                float bh = (float)_values[i] / MaxValue * cH;
                float by = cB - bh;
                if (bh < 1f) bh = 1f;

                var rf = new RectangleF(bx, by, barW, bh);
                using var grad = new LinearGradientBrush(
                    new PointF(bx, by),
                    new PointF(bx, cB),
                    Palette.AccentMid,
                    Palette.AccentLight);
                using var barPath = GdiHelper.RoundedTop(rf, 4);
                g.FillPath(grad, barPath);

                // Month label — centred under bar
                var mSz = g.MeasureString(_months[i], fMonth);
                g.DrawString(_months[i], fMonth, bMonth,
                             bx + barW / 2f - mSz.Width / 2f, cB + 4f);
            }
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
            string badgeText = "+6%";
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

            // Footer note
            using var fNote = new Font("Segoe UI", 8f);
            using var bPos = new SolidBrush(Palette.PositiveText);
            g.DrawString("+6%  5 c\u00f4ng ty so v\u1edbi th\u00e1ng tr\u01b0\u1edbc",
                         fNote, bPos, 14f, H - 24f);
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