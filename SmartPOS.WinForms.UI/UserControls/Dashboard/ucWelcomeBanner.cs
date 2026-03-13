using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.UserControls.Dashboard
{
    public class UcWelcomeBanner : UserControl
    {
        public UcWelcomeBanner()
        {
            this.Height = 110;
            this.Dock = DockStyle.Top;
            this.DoubleBuffered = true;
            this.Margin = new Padding(0, 0, 0, 16);
            this.Paint += UcWelcomeBanner_Paint;

            var btnCompanies = CreateTab("Companies", true);
            var btnPackages = CreateTab("All Packages", false);
            btnCompanies.Location = new Point(this.Width - 240, 34);
            btnPackages.Location = new Point(this.Width - 130, 34);
            btnCompanies.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPackages.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.Controls.Add(btnCompanies);
            this.Controls.Add(btnPackages);
        }

        private Button CreateTab(string text, bool active)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(100, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = active ? Color.FromArgb(25, 30, 55) : Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, active ? FontStyle.Bold : FontStyle.Regular),
                Cursor = Cursors.Hand,
                Region = RoundedRegion(new Size(100, 36), 8)
            };
            btn.FlatAppearance.BorderSize = active ? 0 : 1;
            btn.FlatAppearance.BorderColor = Color.White;
            return btn;
        }

        private void UcWelcomeBanner_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int W = this.Width, H = this.Height;

            // Gradient orange background
            var rect = new Rectangle(0, 0, W, H);
            using var bgBrush = new LinearGradientBrush(
                new PointF(0, 0), new PointF(W, H),
                Color.FromArgb(255, 165, 30),
                Color.FromArgb(255, 100, 0));
            var path = RoundedPath(rect, 16);
            g.FillPath(bgBrush, path);
            this.Region = new Region(path);

            // Decorative circles (right side)
            using var circlePen = new Pen(Color.FromArgb(30, 255, 255, 255), 1);
            g.DrawEllipse(circlePen, W - 160, -40, 200, 200);
            g.DrawEllipse(circlePen, W - 100, H - 80, 160, 160);
            using var circleBrush = new SolidBrush(Color.FromArgb(15, 255, 255, 255));
            g.FillEllipse(circleBrush, W - 120, -20, 130, 130);

            // Title
            g.DrawString("Welcome Back, Adrian",
                new Font("Segoe UI", 16F, FontStyle.Bold),
                Brushes.White, 24, 22);

            // Subtitle
            g.DrawString("14 New Companies Subscribed Today !!!",
                new Font("Segoe UI", 9.5F),
                new SolidBrush(Color.FromArgb(230, 255, 230, 180)), 26, 54);
        }

        private GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            var p = new GraphicsPath();
            int d = radius * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private Region RoundedRegion(Size size, int radius)
        {
            var p = new GraphicsPath();
            int d = radius * 2;
            p.AddArc(0, 0, d, d, 180, 90);
            p.AddArc(size.Width - d, 0, d, d, 270, 90);
            p.AddArc(size.Width - d, size.Height - d, d, d, 0, 90);
            p.AddArc(0, size.Height - d, d, d, 90, 90);
            p.CloseFigure();
            return new Region(p);
        }
    }
}
