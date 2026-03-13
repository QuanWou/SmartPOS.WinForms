using SmartPOS.WinForms.UI.UserControls.Dashboard;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
namespace SmartPOS.WinForms.UI.Forms.Dashboard
{
    public class frmDashboard : Form
    {
        // ── Layout containers ─────────────────────────────────────────────
        private Panel pnlScrollWrapper;
        private Panel pnlMainContent;

        // ── Stat cards ────────────────────────────────────────────────────
        private FlowLayoutPanel flpCards;
        private UcStatCard cardCompanies;
        private UcStatCard cardActive;
        private UcStatCard cardSubscribers;
        private UcStatCard cardEarnings;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IInvoiceService _invoiceService;
        // ── Charts row ────────────────────────────────────────────────────
        private TableLayoutPanel tlpCharts;
        private UcCompaniesChart chartCompanies;
        private UcRevenueChart chartRevenue;
        private UcTopPlansChart chartTopPlans;

        // ── Lists row ─────────────────────────────────────────────────────
        private TableLayoutPanel tlpLists;
        private UcRecentTransactions listTransactions;
        private UcRecentlyRegistered listRegistered;
        private UcRecentPlanExpired listExpired;

        // ── Design tokens ─────────────────────────────────────────────────
        private static readonly Color BG = Color.FromArgb(248, 249, 251);
        private static readonly Color SURFACE = Color.White;
        private static readonly Color BORDER = Color.FromArgb(228, 231, 238);
        private static readonly Color TEXT_PRIMARY = Color.FromArgb(15, 20, 40);
        private static readonly Color TEXT_MUTED = Color.FromArgb(120, 130, 155);
        private static readonly Color ACCENT = Color.FromArgb(30, 40, 80);
        private static readonly Color ACCENT_LIGHT = Color.FromArgb(235, 237, 247);
        private static readonly Font FONT_HEAD = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
        private static readonly Font FONT_SUB = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font FONT_BADGE = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);

        public frmDashboard()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            _invoiceService = new InvoiceService();

            this.BackColor = BG;
            this.Font = new Font("Segoe UI", 9.5F);
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.DoubleBuffered = true;

            BuildLayout();
        }

        // ─────────────────────────────────────────────────────────────────
        private void BuildLayout()
        {
            pnlScrollWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BG,
                Padding = new Padding(0)   // NO padding here — causes drift
            };
            this.Controls.Add(pnlScrollWrapper);

            pnlMainContent = new Panel
            {
                BackColor = Color.Transparent,
                Location = new Point(0, 0)
            };
            pnlScrollWrapper.Controls.Add(pnlMainContent);

            BuildHeaderBar();
            BuildStatCards();
            BuildChartsRow();
            BuildListsRow();

            this.Resize += (s, e) => RecalcLayout();
            this.Load += (s, e) =>
            {
                LoadDashboardData();
                RecalcLayout();
            }; this.Shown += (s, e) => RecalcLayout();
            this.VisibleChanged += (s, e) => { if (this.Visible) RecalcLayout(); };

            // Re-layout when the scroll panel itself resizes (e.g. splitter move)
            pnlScrollWrapper.Resize += (s, e) => RecalcLayout();
        }
        private void LoadDashboardData()
        {
            try
            {
                var products = _productService.GetAll().ToList();
                var categories = _categoryService.GetAll().ToList();
                var invoices = _invoiceService.GetAll().ToList();

                int totalProducts = products.Count;
                int totalCategories = categories.Count;

                DateTime today = DateTime.Today;
                var todayInvoices = invoices
                    .Where(x =>
                        x.NgayLap.Date == today &&
                        string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                int totalTodayInvoices = todayInvoices.Count;
                decimal totalTodayRevenue = todayInvoices.Sum(x => x.TongTien);

                cardCompanies.Value = totalProducts.ToString("N0");
                cardCompanies.Title = "Tổng sản phẩm";
                cardCompanies.Change = "+0.00%";
                cardCompanies.IsPositive = true;

                cardActive.Value = totalCategories.ToString("N0");
                cardActive.Title = "Danh mục";
                cardActive.Change = "+0.00%";
                cardActive.IsPositive = true;

                cardSubscribers.Value = totalTodayInvoices.ToString("N0");
                cardSubscribers.Title = "Hóa đơn hôm nay";
                cardSubscribers.Change = "+0.00%";
                cardSubscribers.IsPositive = true;

                cardEarnings.Value = totalTodayRevenue.ToString("N0") + " đ";
                cardEarnings.Title = "Doanh thu hôm nay";
                cardEarnings.Change = "+0.00%";
                cardEarnings.IsPositive = true;
            }
            catch
            {
                cardCompanies.Value = "0";
                cardActive.Value = "0";
                cardSubscribers.Value = "0";
                cardEarnings.Value = "0 đ";
            }
        }
        // ─────────────────────────────────────────────────────────────────
        private void BuildHeaderBar()
        {
            var pnl = new Panel
            {
                Height = 54,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 16)
            };

            pnl.Paint += (s, e) =>
            {
                var g = e.Graphics;
                var ctrl = (Panel)s;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                using var hFont = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
                using var sBr = new SolidBrush(TEXT_PRIMARY);
                g.DrawString("Dashboard", hFont, sBr, 0, 2);

                using var subFont = new Font("Segoe UI", 8.5F);
                using var subBr = new SolidBrush(TEXT_MUTED);
                g.DrawString("Tổng quan bán hàng, tồn kho và vận hành cửa hàng", subFont, subBr, 0, 26);

                const string dateText = "Today Overview";
                using var badgeFont = new Font("Segoe UI", 8F);
                var sz = g.MeasureString(dateText, badgeFont);
                int bW = (int)sz.Width + 28;
                int bH = 28;
                int bX = ctrl.Width - bW - 2;
                int bY = (54 - bH) / 2;
                var badge = new Rectangle(bX, bY, bW, bH);

                using var bgBr = new SolidBrush(SURFACE);
                using var penBr = new Pen(BORDER, 1f);
                g.FillPath(bgBr, RoundedRect(badge, 7));
                g.DrawPath(penBr, RoundedRect(badge, 7));

                using var iconBr = new SolidBrush(ACCENT);
                g.FillRectangle(iconBr, bX + 10, bY + 9, 9, 9);
                g.FillRectangle(new SolidBrush(SURFACE), bX + 11, bY + 12, 7, 6);

                using var txtBr = new SolidBrush(TEXT_PRIMARY);
                g.DrawString(dateText, badgeFont, txtBr, bX + 24, bY + 7);
            };

            pnl.Resize += (s, e) => pnl.Invalidate();
            pnlMainContent.Controls.Add(pnl);
        }

        // ─────────────────────────────────────────────────────────────────
        private void BuildStatCards()
        {
            flpCards = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 18),
                Height = 116
            };

            cardCompanies = MakeStatCard("13", "Tổng sản phẩm", "+2.00%", true, "📦", Color.FromArgb(30, 40, 80));
            cardActive = MakeStatCard("5", "Danh mục", "+0.00%", true, "🗂", Color.FromArgb(60, 70, 100));
            cardSubscribers = MakeStatCard("3", "Hóa đơn hôm nay", "+1.00%", true, "🧾", Color.FromArgb(80, 90, 120));
            cardEarnings = MakeStatCard("144,000", "Doanh thu hôm nay", "+8.50%", true, "💰", Color.FromArgb(100, 110, 140));

            flpCards.Controls.AddRange(new Control[]
            {
                cardCompanies, cardActive, cardSubscribers, cardEarnings
            });

            pnlMainContent.Controls.Add(flpCards);
        }

        // ─────────────────────────────────────────────────────────────────
        private void BuildChartsRow()
        {
            tlpCharts = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 18),
                Height = 300
            };
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));

            chartCompanies = new UcCompaniesChart { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 0) };
            chartRevenue = new UcRevenueChart { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 0) };
            chartTopPlans = new UcTopPlansChart { Dock = DockStyle.Fill };

            tlpCharts.Controls.Add(chartCompanies, 0, 0);
            tlpCharts.Controls.Add(chartRevenue, 1, 0);
            tlpCharts.Controls.Add(chartTopPlans, 2, 0);

            pnlMainContent.Controls.Add(tlpCharts);
        }

        // ─────────────────────────────────────────────────────────────────
        private void BuildListsRow()
        {
            tlpLists = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Height = 260
            };
            tlpLists.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            listTransactions = new UcRecentTransactions { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 0) };
            listRegistered = new UcRecentlyRegistered { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 0) };
            listExpired = new UcRecentPlanExpired { Dock = DockStyle.Fill };

            tlpLists.Controls.Add(listTransactions, 0, 0);
            tlpLists.Controls.Add(listRegistered, 1, 0);
            tlpLists.Controls.Add(listExpired, 2, 0);

            pnlMainContent.Controls.Add(tlpLists);
        }

        // ─────────────────────────────────────────────────────────────────
        // FIX: pnlMainContent must always be at Left=0, Top=0 inside the
        //      scroll wrapper's Padding-adjusted client area.
        // The old code used ClientSize.Width - 18 which could drift right
        // when the scrollbar appeared/disappeared.
        // ─────────────────────────────────────────────────────────────────
        private void RecalcLayout()
        {
            // Padding values (replaces Panel.Padding which caused drift)
            const int PAD_X = 24;
            const int PAD_Y = 20;

            int scrollBarW = SystemInformation.VerticalScrollBarWidth;
            int wrapperW = pnlScrollWrapper.ClientSize.Width;

            // Content width = wrapper width minus both horizontal paddings and scrollbar
            int cw = wrapperW - PAD_X * 2 - scrollBarW;
            if (cw < 300) return;

            // Always pin content to padded top-left corner
            pnlMainContent.Left = PAD_X;
            pnlMainContent.Top = PAD_Y;
            pnlMainContent.Width = cw;
            pnlMainContent.SuspendLayout();

            const int GAP = 18;
            bool narrow = cw < 720;
            int y = 0;

            // Header bar
            var header = pnlMainContent.Controls[0] as Panel;
            if (header != null)
            {
                header.SetBounds(0, y, cw, 54);
                y += 54 + GAP;
            }

            // Stat cards
            if (flpCards != null)
            {
                int gapCard = 14;
                int cols = narrow ? 2 : 4;
                int cardW = Math.Max(140, (cw - gapCard * (cols - 1)) / cols);
                int cardH = 110;

                for (int i = 0; i < flpCards.Controls.Count; i++)
                {
                    var c = flpCards.Controls[i];
                    c.Width = cardW;
                    c.Height = cardH;
                    c.Margin = new Padding(0, 0, i < cols - 1 ? gapCard : 0, 0);
                }

                flpCards.WrapContents = narrow;
                int flowH = narrow ? cardH * 2 + gapCard : cardH;
                flpCards.SetBounds(0, y, cw, flowH);
                y += flowH + GAP;
            }

            // Charts row
            if (tlpCharts != null)
            {
                if (narrow)
                {
                    tlpCharts.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpCharts.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpCharts.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 0F);
                    chartTopPlans.Visible = false;
                }
                else
                {
                    tlpCharts.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 28F);
                    tlpCharts.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 44F);
                    tlpCharts.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 28F);
                    chartTopPlans.Visible = true;
                }
                tlpCharts.SetBounds(0, y, cw, 300);
                y += 300 + GAP;
            }

            // Lists row
            if (tlpLists != null)
            {
                if (narrow)
                {
                    tlpLists.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpLists.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpLists.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 0F);
                    listExpired.Visible = false;
                }
                else
                {
                    tlpLists.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 33.33F);
                    tlpLists.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 33.33F);
                    tlpLists.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 33.33F);
                    listExpired.Visible = true;
                }
                tlpLists.SetBounds(0, y, cw, 260);
                y += 260 + GAP;
            }

            pnlMainContent.Height = y;
            pnlMainContent.ResumeLayout(true);
        }

        // ─────────────────────────────────────────────────────────────────
        private UcStatCard MakeStatCard(
            string value, string title, string change,
            bool positive, string icon, Color iconColor)
        {
            return new UcStatCard
            {
                Value = value,
                Title = title,
                Change = change,
                IsPositive = positive,
                IconText = icon,
                IconColor = iconColor,
                Height = 110,
                Margin = new Padding(0, 0, 14, 0)
            };
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FONT_HEAD?.Dispose();
                FONT_SUB?.Dispose();
                FONT_BADGE?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}