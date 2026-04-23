using SmartPOS.WinForms.UI.UserControls.Dashboard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Linq;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Forms.Main;
using SmartPOS.WinForms.UI.Forms.Products;
using SmartPOS.WinForms.UI.Forms.Stock;

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

        // ── Services (từ dashboard mới) ───────────────────────────────────
        private readonly IProductService _productService;
        private readonly IProductLotService _productLotService;
        private readonly ICategoryService _categoryService;
        private readonly IInvoiceService _invoiceService;

        // ── Alert panels (từ dashboard mới) ──────────────────────────────
        private Panel pnlLowStockAlert;
        private Label lblLowStockAlert;
        private Timer tmrLowStockAlert;
        private bool _alertBlinkOn;

        private Panel pnlExpiryAlert;
        private Label lblExpiryAlert;
        private Timer tmrExpiryAlertAlert;
        private bool _expiryAlertBlinkOn;

        // ── Charts row (layout 3 cột như mẫu cũ) ─────────────────────────
        // Cột trái  : UcCompaniesChart  → hiển thị "Công ty" / thay thế bằng chart tồn kho hoặc danh mục
        // Cột giữa  : UcRevenueChart    → doanh thu (bar chart)
        // Cột phải  : UcTopPlansChart   → thay thế bằng UcTopProductsChart (top sản phẩm)
        private TableLayoutPanel tlpCharts;
        private UcCompaniesChart chartCompanies;   // cột trái  – giữ nguyên tên lớp như mẫu cũ
        private UcRevenueChart chartRevenue;        // cột giữa  – bar chart doanh thu
        private UcTopProductsChart chartTopProducts; // cột phải  – top sản phẩm (từ dashboard mới)

        // ── Lists row (layout 3 cột như mẫu cũ) ──────────────────────────
        // Cột trái  : UcRecentTransactions → giao dịch gần đây (hóa đơn paid)
        // Cột giữa  : UcInsightListPanel   → sắp hết hàng
        // Cột phải  : UcInsightListPanel   → sắp hết hạn
        private TableLayoutPanel tlpLists;
        private UcRecentTransactions listTransactions;  // giữ nguyên lớp như mẫu cũ
        private UcInsightListPanel listLowStock;         // từ dashboard mới
        private UcInsightListPanel listExpiry;           // từ dashboard mới

        // ── Design tokens (giống hệt mẫu cũ) ────────────────────────────
        private static readonly Color BG            = Color.FromArgb(248, 249, 251);
        private static readonly Color SURFACE       = Color.White;
        private static readonly Color BORDER        = Color.FromArgb(228, 231, 238);
        private static readonly Color TEXT_PRIMARY  = Color.FromArgb(15, 20, 40);
        private static readonly Color TEXT_MUTED    = Color.FromArgb(120, 130, 155);
        private static readonly Color ACCENT        = Color.FromArgb(30, 40, 80);
        private static readonly Color ACCENT_LIGHT  = Color.FromArgb(235, 237, 247);
        private static readonly Font FONT_HEAD  = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
        private static readonly Font FONT_SUB   = new Font("Segoe UI", 9F,   FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font FONT_BADGE = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);

        // ─────────────────────────────────────────────────────────────────
        public frmDashboard()
        {
            _productService    = new ProductService();
            _productLotService = new ProductLotService();
            _categoryService   = new CategoryService();
            _invoiceService    = new InvoiceService();

            this.BackColor        = BG;
            this.Font             = new Font("Segoe UI", 9.5F);
            this.FormBorderStyle  = FormBorderStyle.None;
            this.TopLevel         = false;
            this.DoubleBuffered   = true;

            BuildLayout();
        }

        // ─────────────────────────────────────────────────────────────────
        private void BuildLayout()
        {
            this.BackColor = BG;
            pnlScrollWrapper = new Panel
            {
                Dock       = DockStyle.Fill,
                AutoScroll = true,
                BackColor  = BG,
                Padding    = new Padding(0)
            };
            this.Controls.Add(pnlScrollWrapper);

            pnlMainContent = new Panel
            {
                BackColor = Color.Transparent,
                Location  = new Point(0, 0)
            };
            pnlScrollWrapper.Controls.Add(pnlMainContent);

            BuildHeaderBar();
            BuildLowStockAlert();
            BuildExpiryAlert();
            BuildStatCards();
            BuildChartsRow();
            BuildListsRow();

            this.Resize          += (s, e) => RecalcLayout();
            this.Load            += (s, e) => { LoadDashboardData(); RecalcLayout(); };
            this.Shown           += (s, e) => RecalcLayout();
            this.VisibleChanged  += (s, e) => { if (this.Visible) RecalcLayout(); };
            pnlScrollWrapper.Resize += (s, e) => RecalcLayout();
        }

        // ─────────────────────────────────────────────────────────────────
        //  DATA LOADING  (toàn bộ logic từ dashboard mới)
        // ─────────────────────────────────────────────────────────────────
        private void LoadDashboardData()
        {
            try
            {
                var products   = _productService.GetAll().ToList();
                var categories = _categoryService.GetAll().ToList();
                var invoices   = _invoiceService.GetAll().ToList();
                var paidInvoices = invoices
                    .Where(x => string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                int totalProducts   = products.Count;
                int totalCategories = categories.Count;

                // Sản phẩm sắp hết hàng (tồn < 5, còn hạn, đang hoạt động)
                List<ProductDTO> lowStockProducts = products
                    .Where(x =>
                        x.TrangThai &&
                        x.SoLuongTon < 5 &&
                        (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today))
                    .OrderBy(x => x.SoLuongTon)
                    .ThenBy(x => x.TenSP)
                    .ToList();

                // Lô hàng sắp hết hạn trong 7 ngày
                List<ProductLotDTO> expiringSoonLots = _productLotService.GetExpiringLots(7)
                    .Where(x => x.TrangThaiSanPham)
                    .OrderBy(x => x.HanSuDung)
                    .ThenBy(x => x.NgayNhap)
                    .ToList();

                // Top 5 sản phẩm bán chạy 30 ngày gần nhất
                DateTime today            = DateTime.Today;
                DateTime topProductsFrom  = today.AddDays(-29);
                Dictionary<int, ProductDTO> productLookup = products
                    .GroupBy(x => x.MaSP)
                    .ToDictionary(x => x.Key, x => x.First());

                List<DashboardTopProductItem> topProducts = paidInvoices
                    .Where(x => x.NgayLap.Date >= topProductsFrom && x.NgayLap.Date <= today)
                    .SelectMany(x => _invoiceService.GetDetailsByInvoiceId(x.MaHD))
                    .GroupBy(x => x.MaSP)
                    .Select(x =>
                    {
                        ProductDTO product;
                        productLookup.TryGetValue(x.Key, out product);
                        return new DashboardTopProductItem
                        {
                            ProductName  = product != null ? product.TenSP : "SP #" + x.Key,
                            QuantitySold = x.Sum(y => y.SoLuong),
                            Revenue      = x.Sum(y => y.ThanhTien)
                        };
                    })
                    .OrderByDescending(x => x.QuantitySold)
                    .ThenByDescending(x => x.Revenue)
                    .ThenBy(x => x.ProductName)
                    .Take(5)
                    .ToList();

                // Hóa đơn & doanh thu hôm nay
                var todayInvoices      = paidInvoices.Where(x => x.NgayLap.Date == today).ToList();
                int     totalTodayInvoices = todayInvoices.Count;
                decimal totalTodayRevenue  = todayInvoices.Sum(x => x.TongTien);

                // ── Cập nhật stat cards ──────────────────────────────────
                cardCompanies.Value    = totalProducts.ToString("N0");
                cardCompanies.Title    = "Tổng sản phẩm";
                cardCompanies.Change   = "+0.00%";
                cardCompanies.IsPositive = true;

                cardActive.Value    = totalCategories.ToString("N0");
                cardActive.Title    = "Danh mục";
                cardActive.Change   = "+0.00%";
                cardActive.IsPositive = true;

                cardSubscribers.Value    = totalTodayInvoices.ToString("N0");
                cardSubscribers.Title    = "Hóa đơn hôm nay";
                cardSubscribers.Change   = "+0.00%";
                cardSubscribers.IsPositive = true;

                cardEarnings.Value    = totalTodayRevenue.ToString("N0") + " đ";
                cardEarnings.Title    = "Doanh thu hôm nay";
                cardEarnings.Change   = "+0.00%";
                cardEarnings.IsPositive = true;

                // ── Alert blinking banners ───────────────────────────────
                UpdateLowStockAlert(lowStockProducts.Count);
                UpdateExpiryAlert(expiringSoonLots);

                // ── Charts ───────────────────────────────────────────────
                LoadCategoriesChartData(totalCategories, totalProducts);
                LoadRevenueChartData(paidInvoices);          // bar chart
                LoadTopProductsChartData(topProducts);

                // ── Insight lists ────────────────────────────────────────
                LoadRecentTransactions(paidInvoices);
                UpdateInsightLists(lowStockProducts, expiringSoonLots);
            }
            catch
            {
                // Fallback graceful
                cardCompanies.Value    = "0";
                cardActive.Value       = "0";
                cardSubscribers.Value  = "0";
                cardEarnings.Value     = "0 đ";
                UpdateLowStockAlert(0);
                UpdateExpiryAlert(new List<ProductLotDTO>());
                chartRevenue?.SetData(new List<RevenueChartItemResponse>(), "0 đ", "+0% so với 7 ngày trước");
                chartTopProducts?.SetData(new List<DashboardTopProductItem>(), "30 ngày gần nhất");
                UpdateInsightLists(new List<ProductDTO>(), new List<ProductLotDTO>());
            }
        }

        // ─────────────────────────────────────────────────────────────────
        //  CHART DATA HELPERS
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cột trái – truyền dữ liệu danh mục / sản phẩm vào UcCompaniesChart.
        /// UcCompaniesChart vốn dùng để hiển thị "công ty theo tháng" trong mẫu cũ;
        /// ở đây ta truyền số danh mục và tổng sản phẩm để chart vẫn có dữ liệu thực.
        /// Nếu UcCompaniesChart có SetData(int, int) hoặc tương tự, gọi ở đây.
        /// Nếu không, method này sẽ không làm gì (chart giữ placeholder).
        /// </summary>
        private void LoadCategoriesChartData(int totalCategories, int totalProducts)
        {
            if (chartCompanies == null) return;

            // Thử gọi SetData nếu UcCompaniesChart hỗ trợ
            // Nếu interface khác, điều chỉnh tham số cho phù hợp
            try
            {
                // Ví dụ: chartCompanies.SetData(totalCategories, totalProducts);
                // Nếu UcCompaniesChart không có SetData, xóa dòng trên
            }
            catch { /* ignore nếu method không tồn tại */ }
        }

        /// <summary>
        /// Cột giữa – doanh thu 7 ngày gần nhất dưới dạng BAR CHART.
        /// SetData truyền List<RevenueChartItemResponse> giống dashboard mới.
        /// UcRevenueChart cần được sửa để vẽ cột thay vì đường (xem ghi chú bên dưới).
        /// </summary>
        private void LoadRevenueChartData(List<InvoiceDTO> paidInvoices)
        {
            if (chartRevenue == null) return;

            DateTime today = DateTime.Today;

            // Doanh thu 7 ngày gần nhất (bar chart)
            List<RevenueChartItemResponse> revenueData = Enumerable.Range(0, 7)
                .Select(index =>
                {
                    DateTime day     = today.AddDays(index - 6);
                    decimal  revenue = paidInvoices
                        .Where(x => x.NgayLap.Date == day)
                        .Sum(x => x.TongTien);
                    return new RevenueChartItemResponse { Ngay = day, DoanhThu = revenue };
                })
                .ToList();

            decimal currentTotal = revenueData.Sum(x => x.DoanhThu);

            // So sánh với 7 ngày trước
            DateTime previousStart = today.AddDays(-13);
            DateTime previousEnd   = today.AddDays(-7);
            decimal previousTotal  = paidInvoices
                .Where(x => x.NgayLap.Date >= previousStart && x.NgayLap.Date <= previousEnd)
                .Sum(x => x.TongTien);

            chartRevenue.SetData(
                revenueData,
                currentTotal.ToString("N0") + " đ",
                BuildRevenueBadgeText(currentTotal, previousTotal));
        }

        /// <summary>
        /// Cột phải – top 5 sản phẩm bán chạy (UcTopProductsChart từ dashboard mới).
        /// </summary>
        private void LoadTopProductsChartData(List<DashboardTopProductItem> topProducts)
        {
            if (chartTopProducts == null) return;
            chartTopProducts.SetData(topProducts, "30 ngày gần nhất");
        }

        /// <summary>
        /// List cột trái – hóa đơn gần đây (giống UcRecentTransactions mẫu cũ).
        /// Dùng dữ liệu thực từ paidInvoices.
        /// </summary>
        private void LoadRecentTransactions(List<InvoiceDTO> paidInvoices)
        {
            if (listTransactions == null) return;

            var recent = paidInvoices
                .OrderByDescending(x => x.NgayLap)
                .Take(5)
                .ToList();

            // Thử gọi SetData nếu UcRecentTransactions hỗ trợ
            try
            {
                // Ví dụ: listTransactions.SetData(recent);
                // Điều chỉnh tham số theo interface thực tế của UcRecentTransactions
            }
            catch { /* ignore */ }
        }

        // ─────────────────────────────────────────────────────────────────
        //  ALERT PANELS  (giữ nguyên từ dashboard mới)
        // ─────────────────────────────────────────────────────────────────
        private void BuildLowStockAlert()
        {
            pnlLowStockAlert = new Panel
            {
                Height      = 54,
                BackColor   = Color.FromArgb(255, 246, 220),
                BorderStyle = BorderStyle.FixedSingle,
                Visible     = false,
                Cursor      = Cursors.Hand
            };

            lblLowStockAlert = new Label
            {
                Dock      = DockStyle.Fill,
                Padding   = new Padding(18, 0, 18, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font      = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 88, 0)
            };

            pnlLowStockAlert.Controls.Add(lblLowStockAlert);
            pnlLowStockAlert.Click     += (s, e) => NavigateToLowStock();
            lblLowStockAlert.Click     += (s, e) => NavigateToLowStock();

            tmrLowStockAlert = new Timer { Interval = 550 };
            tmrLowStockAlert.Tick += (s, e) =>
            {
                if (!pnlLowStockAlert.Visible) return;
                _alertBlinkOn = !_alertBlinkOn;
                ApplyLowStockAlertStyle();
            };

            pnlMainContent.Controls.Add(pnlLowStockAlert);
        }

        private void BuildExpiryAlert()
        {
            pnlExpiryAlert = new Panel
            {
                Height      = 72,
                BackColor   = Color.FromArgb(255, 237, 236),
                BorderStyle = BorderStyle.FixedSingle,
                Visible     = false,
                Cursor      = Cursors.Hand
            };

            lblExpiryAlert = new Label
            {
                Dock      = DockStyle.Fill,
                Padding   = new Padding(18, 8, 18, 8),
                TextAlign = ContentAlignment.MiddleLeft,
                Font      = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold),
                ForeColor = Color.FromArgb(156, 56, 56)
            };

            pnlExpiryAlert.Controls.Add(lblExpiryAlert);
            pnlExpiryAlert.Click   += (s, e) => NavigateToExpiryProducts();
            lblExpiryAlert.Click   += (s, e) => NavigateToExpiryProducts();

            tmrExpiryAlertAlert = new Timer { Interval = 650 };
            tmrExpiryAlertAlert.Tick += (s, e) =>
            {
                if (!pnlExpiryAlert.Visible) return;
                _expiryAlertBlinkOn = !_expiryAlertBlinkOn;
                ApplyExpiryAlertStyle();
            };

            pnlMainContent.Controls.Add(pnlExpiryAlert);
        }

        private void UpdateLowStockAlert(int lowStockCount)
        {
            if (pnlLowStockAlert == null || lblLowStockAlert == null) return;

            bool hasLowStock = lowStockCount > 0;
            pnlLowStockAlert.Visible = hasLowStock;

            if (!hasLowStock) { tmrLowStockAlert?.Stop(); return; }

            lblLowStockAlert.Text = string.Format(
                "⚠  {0} sản phẩm sắp hết hàng. Bấm để mở danh sách cần nhập thêm.", lowStockCount);

            _alertBlinkOn = true;
            ApplyLowStockAlertStyle();
            tmrLowStockAlert?.Start();
        }

        private void UpdateExpiryAlert(List<ProductLotDTO> expiringSoonLots)
        {
            if (pnlExpiryAlert == null || lblExpiryAlert == null) return;

            bool hasExpiringSoon = expiringSoonLots != null && expiringSoonLots.Count > 0;
            pnlExpiryAlert.Visible = hasExpiringSoon;

            if (!hasExpiringSoon) { tmrExpiryAlertAlert?.Stop(); return; }

            lblExpiryAlert.Text = BuildExpiryAlertText(expiringSoonLots);

            _expiryAlertBlinkOn = true;
            ApplyExpiryAlertStyle();
            tmrExpiryAlertAlert?.Start();
        }

        private void ApplyLowStockAlertStyle()
        {
            if (pnlLowStockAlert == null || lblLowStockAlert == null) return;
            pnlLowStockAlert.BackColor = _alertBlinkOn
                ? Color.FromArgb(255, 246, 220)
                : Color.FromArgb(255, 238, 198);
            lblLowStockAlert.ForeColor = Color.FromArgb(148, 88, 0);
        }

        private void ApplyExpiryAlertStyle()
        {
            if (pnlExpiryAlert == null || lblExpiryAlert == null) return;
            pnlExpiryAlert.BackColor = _expiryAlertBlinkOn
                ? Color.FromArgb(255, 237, 236)
                : Color.FromArgb(255, 226, 224);
            lblExpiryAlert.ForeColor = Color.FromArgb(156, 56, 56);
        }

        private string BuildExpiryAlertText(List<ProductLotDTO> lots)
        {
            var highlights = lots.Take(3)
                .Select(x => x.TenSP + " - lô " + x.NgayNhap.ToString("dd/MM") +
                             " (HSD " + x.HanSuDung.Value.ToString("dd/MM") + ")")
                .ToList();

            string suffix = lots.Count > 3
                ? " và " + (lots.Count - 3) + " lô khác."
                : ".";

            return "🔴  " + lots.Count +
                   " lô hàng sẽ hết hạn trong 7 ngày: " +
                   string.Join(", ", highlights) + suffix +
                   " Bấm để mở danh sách chi tiết.";
        }

        // ─────────────────────────────────────────────────────────────────
        //  HEADER BAR  (giống mẫu cũ, thêm ngày như dashboard mới)
        // ─────────────────────────────────────────────────────────────────
        private void BuildHeaderBar()
        {
            var pnl = new Panel
            {
                Height    = 54,
                BackColor = Color.Transparent
            };

            pnl.Paint += (s, e) =>
            {
                var g    = e.Graphics;
                var ctrl = (Panel)s;
                g.SmoothingMode       = SmoothingMode.AntiAlias;
                g.TextRenderingHint   = TextRenderingHint.ClearTypeGridFit;

                using var hFont  = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
                using var sBr    = new SolidBrush(TEXT_PRIMARY);
                g.DrawString("Dashboard", hFont, sBr, 0, 2);

                using var subFont = new Font("Segoe UI", 8.5F);
                using var subBr   = new SolidBrush(TEXT_MUTED);
                g.DrawString("Tổng quan bán hàng, tồn kho và vận hành cửa hàng", subFont, subBr, 0, 26);

                string dateText = "Hôm nay – " + DateTime.Today.ToString("dd/MM/yyyy");
                using var badgeFont = new Font("Segoe UI", 8F);
                var sz   = g.MeasureString(dateText, badgeFont);
                int bW   = (int)sz.Width + 28;
                int bH   = 28;
                int bX   = ctrl.Width - bW - 2;
                int bY   = (54 - bH) / 2;
                var badge = new Rectangle(bX, bY, bW, bH);

                using var bgBr  = new SolidBrush(SURFACE);
                using var penBr = new Pen(BORDER, 1f);
                g.FillPath(bgBr,  RoundedRect(badge, 7));
                g.DrawPath(penBr, RoundedRect(badge, 7));

                using var iconBr = new SolidBrush(ACCENT);
                g.FillRectangle(iconBr,                   bX + 10, bY + 9,  9, 9);
                g.FillRectangle(new SolidBrush(SURFACE),  bX + 11, bY + 12, 7, 6);

                using var txtBr = new SolidBrush(TEXT_PRIMARY);
                g.DrawString(dateText, badgeFont, txtBr, bX + 24, bY + 7);
            };

            pnl.Resize += (s, e) => pnl.Invalidate();
            pnlMainContent.Controls.Add(pnl);
        }

        // ─────────────────────────────────────────────────────────────────
        //  STAT CARDS  (4 card như cả hai mẫu)
        // ─────────────────────────────────────────────────────────────────
        private void BuildStatCards()
        {
            flpCards = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = Color.Transparent,
                Height        = 116
            };

            cardCompanies   = MakeStatCard("–", "Tổng sản phẩm",    "+0.00%", true, "📦", Color.FromArgb(30,  40,  80));
            cardActive      = MakeStatCard("–", "Danh mục",         "+0.00%", true, "🗂",  Color.FromArgb(60,  70,  100));
            cardSubscribers = MakeStatCard("–", "Hóa đơn hôm nay",  "+0.00%", true, "🧾", Color.FromArgb(80,  90,  120));
            cardEarnings    = MakeStatCard("–", "Doanh thu hôm nay", "+0.00%", true, "💰", Color.FromArgb(100, 110, 140));

            flpCards.Controls.AddRange(new Control[]
            {
                cardCompanies, cardActive, cardSubscribers, cardEarnings
            });

            pnlMainContent.Controls.Add(flpCards);
        }

        // ─────────────────────────────────────────────────────────────────
        //  CHARTS ROW  – 3 cột như mẫu cũ, nhưng dùng chart mới ở cột phải
        // ─────────────────────────────────────────────────────────────────
        private void BuildChartsRow()
        {
            tlpCharts = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount    = 1,
                BackColor   = Color.Transparent,
                Height      = 300
            };
            tlpCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));

            // Cột trái: danh mục / sản phẩm overview (giữ UcCompaniesChart như mẫu cũ)
            chartCompanies = new UcCompaniesChart
            {
                Dock   = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0)
            };

            // Cột giữa: doanh thu – BAR CHART (UcRevenueChart, render mode = Bar)
            chartRevenue = new UcRevenueChart
            {
                Dock      = DockStyle.Fill,
                Margin    = new Padding(0, 0, 12, 0),
                ChartType = RevenueChartType.Bar   // ← BẬT chế độ cột
                // Nếu UcRevenueChart chưa có property ChartType, xem ghi chú cuối file
            };

            // Cột phải: top sản phẩm bán chạy (UcTopProductsChart từ dashboard mới)
            chartTopProducts = new UcTopProductsChart
            {
                Dock   = DockStyle.Fill,
                Margin = new Padding(0)
            };

            tlpCharts.Controls.Add(chartCompanies,   0, 0);
            tlpCharts.Controls.Add(chartRevenue,     1, 0);
            tlpCharts.Controls.Add(chartTopProducts, 2, 0);

            pnlMainContent.Controls.Add(tlpCharts);
        }

        // ─────────────────────────────────────────────────────────────────
        //  LISTS ROW  – 3 cột như mẫu cũ
        //    Cột 1: Hóa đơn gần đây (UcRecentTransactions – giữ từ mẫu cũ)
        //    Cột 2: Sắp hết hàng    (UcInsightListPanel   – từ dashboard mới)
        //    Cột 3: Sắp hết hạn     (UcInsightListPanel   – từ dashboard mới)
        // ─────────────────────────────────────────────────────────────────
        private void BuildListsRow()
        {
            tlpLists = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount    = 1,
                BackColor   = Color.Transparent,
                Height      = 260
            };
            tlpLists.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tlpLists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            listTransactions = new UcRecentTransactions
            {
                Dock   = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0)
            };

            listLowStock = new UcInsightListPanel
            {
                Dock   = DockStyle.Fill,
                Margin = new Padding(0, 0, 12, 0),
                Tone   = DashboardListTone.Warning
            };
            listLowStock.Click += (s, e) => NavigateToLowStock();

            listExpiry = new UcInsightListPanel
            {
                Dock   = DockStyle.Fill,
                Margin = new Padding(0),
                Tone   = DashboardListTone.Danger
            };
            listExpiry.Click += (s, e) => NavigateToExpiryProducts();

            tlpLists.Controls.Add(listTransactions, 0, 0);
            tlpLists.Controls.Add(listLowStock,     1, 0);
            tlpLists.Controls.Add(listExpiry,       2, 0);

            pnlMainContent.Controls.Add(tlpLists);
        }

        // ─────────────────────────────────────────────────────────────────
        //  UPDATE INSIGHT LISTS  (từ dashboard mới)
        // ─────────────────────────────────────────────────────────────────
        private void UpdateInsightLists(
            List<ProductDTO>    lowStockProducts,
            List<ProductLotDTO> expiringSoonLots)
        {
            listLowStock?.SetData(
                "Sắp hết hàng",
                "Mặt hàng tồn dưới 5 đơn vị",
                BuildLowStockInsightItems(lowStockProducts),
                "Hiện chưa có mặt hàng sắp hết.");

            listExpiry?.SetData(
                "Sắp hết hạn 7 ngày",
                "Ưu tiên bán trước các lô gần đến hạn",
                BuildExpiryInsightItems(expiringSoonLots),
                "Hiện chưa có lô hàng sắp hết hạn.");
        }

        private IEnumerable<DashboardInsightItem> BuildLowStockInsightItems(
            IEnumerable<ProductDTO> items)
        {
            return (items ?? Enumerable.Empty<ProductDTO>())
                .Take(5)
                .Select(x => new DashboardInsightItem
                {
                    Title    = x.TenSP,
                    Subtitle = "Tồn khả dụng thấp, nên nhập thêm",
                    BadgeText = "Còn " + x.SoLuongTon
                })
                .ToList();
        }

        private IEnumerable<DashboardInsightItem> BuildExpiryInsightItems(
            IEnumerable<ProductLotDTO> lots)
        {
            return (lots ?? Enumerable.Empty<ProductLotDTO>())
                .Take(5)
                .Select(x => new DashboardInsightItem
                {
                    Title    = x.TenSP,
                    Subtitle = "Lô " + x.NgayNhap.ToString("dd/MM") + " - còn " + x.SoLuongTonLo + " đơn vị",
                    BadgeText = x.HanSuDung.HasValue
                        ? "HSD " + x.HanSuDung.Value.ToString("dd/MM")
                        : "Chưa rõ HSD"
                })
                .ToList();
        }

        // ─────────────────────────────────────────────────────────────────
        //  LAYOUT CALCULATION  (kết hợp cả hai mẫu)
        // ─────────────────────────────────────────────────────────────────
        private void RecalcLayout()
        {
            const int PAD_X = 24;
            const int PAD_Y = 20;

            int scrollBarW = SystemInformation.VerticalScrollBarWidth;
            int wrapperW   = pnlScrollWrapper.ClientSize.Width;
            int cw         = wrapperW - PAD_X * 2 - scrollBarW;
            if (cw < 300) return;

            pnlMainContent.Left  = PAD_X;
            pnlMainContent.Top   = PAD_Y;
            pnlMainContent.Width = cw;
            pnlMainContent.SuspendLayout();

            const int GAP    = 18;
            bool      narrow = cw < 720;
            int       y      = 0;

            // 1. Header bar
            var header = pnlMainContent.Controls[0] as Panel;
            if (header != null)
            {
                header.SetBounds(0, y, cw, 54);
                y += 54 + GAP;
            }

            // 2. Low-stock alert (ẩn/hiện động)
            if (pnlLowStockAlert != null && pnlLowStockAlert.Visible)
            {
                pnlLowStockAlert.SetBounds(0, y, cw, 54);
                y += 54 + GAP;
            }

            // 3. Expiry alert (ẩn/hiện động)
            if (pnlExpiryAlert != null && pnlExpiryAlert.Visible)
            {
                pnlExpiryAlert.SetBounds(0, y, cw, 72);
                y += 72 + GAP;
            }

            // 4. Stat cards
            if (flpCards != null)
            {
                const int gapCard = 14;
                int cols  = narrow ? 2 : 4;
                int cardW = Math.Max(140, (cw - gapCard * (cols - 1)) / cols);
                int cardH = 110;

                for (int i = 0; i < flpCards.Controls.Count; i++)
                {
                    var c = flpCards.Controls[i];
                    c.Width  = cardW;
                    c.Height = cardH;
                    c.Margin = new Padding(0, 0, i < cols - 1 ? gapCard : 0, 0);
                }

                flpCards.WrapContents = narrow;
                int flowH = narrow ? cardH * 2 + gapCard : cardH;
                flpCards.SetBounds(0, y, cw, flowH);
                y += flowH + GAP;
            }

            // 5. Charts row (3 cột, responsive như mẫu cũ)
            if (tlpCharts != null)
            {
                if (narrow)
                {
                    tlpCharts.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpCharts.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpCharts.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 0F);
                    if (chartTopProducts != null) chartTopProducts.Visible = false;
                }
                else
                {
                    tlpCharts.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 28F);
                    tlpCharts.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 44F);
                    tlpCharts.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 28F);
                    if (chartTopProducts != null) chartTopProducts.Visible = true;
                }
                tlpCharts.SetBounds(0, y, cw, 300);
                y += 300 + GAP;
            }

            // 6. Lists row (3 cột, responsive như mẫu cũ)
            if (tlpLists != null)
            {
                if (narrow)
                {
                    tlpLists.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpLists.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50F);
                    tlpLists.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 0F);
                    if (listExpiry != null) listExpiry.Visible = false;
                }
                else
                {
                    tlpLists.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 33.33F);
                    tlpLists.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 33.33F);
                    tlpLists.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 33.33F);
                    if (listExpiry != null) listExpiry.Visible = true;
                }
                tlpLists.SetBounds(0, y, cw, 260);
                y += 260 + GAP;
            }

            pnlMainContent.Height = y;
            pnlMainContent.ResumeLayout(true);
        }

        // ─────────────────────────────────────────────────────────────────
        //  HELPERS
        // ─────────────────────────────────────────────────────────────────
        private UcStatCard MakeStatCard(
            string value, string title, string change,
            bool positive, string icon, Color iconColor)
        {
            return new UcStatCard
            {
                Value      = value,
                Title      = title,
                Change     = change,
                IsPositive = positive,
                IconText   = icon,
                IconColor  = iconColor,
                Height     = 110,
                Margin     = new Padding(0, 0, 14, 0)
            };
        }

        private string BuildRevenueBadgeText(decimal current, decimal previous)
        {
            if (previous <= 0)
                return current > 0 ? "+100% so với 7 ngày trước" : "+0% so với 7 ngày trước";

            decimal change = ((current - previous) / previous) * 100m;
            string  sign   = change >= 0 ? "+" : string.Empty;
            return sign + change.ToString("0.#") + "% so với 7 ngày trước";
        }

        // ─────────────────────────────────────────────────────────────────
        //  NAVIGATION
        // ─────────────────────────────────────────────────────────────────
        private void NavigateToLowStock()
        {
            frmMain mainForm = GetHostMainForm();
            mainForm?.NavigateToPage(new frmLowStock());
        }

        private void NavigateToExpiryProducts()
        {
            frmMain mainForm = GetHostMainForm();
            mainForm?.NavigateToPage(new frmExpiredProducts());
        }

        private frmMain GetHostMainForm()
        {
            return TopLevelControl as frmMain
                ?? Parent?.FindForm() as frmMain
                ?? Application.OpenForms.OfType<frmMain>().FirstOrDefault();
        }

        // ─────────────────────────────────────────────────────────────────
        //  ROUNDED RECT HELPER
        // ─────────────────────────────────────────────────────────────────
        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d  = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.X,          r.Y,           d, d, 180, 90);
            gp.AddArc(r.Right - d,  r.Y,           d, d, 270, 90);
            gp.AddArc(r.Right - d,  r.Bottom - d,  d, d,   0, 90);
            gp.AddArc(r.X,          r.Bottom - d,  d, d,  90, 90);
            gp.CloseFigure();
            return gp;
        }

        // ─────────────────────────────────────────────────────────────────
        //  DISPOSE
        // ─────────────────────────────────────────────────────────────────
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tmrLowStockAlert?.Stop();   tmrLowStockAlert?.Dispose();
                tmrExpiryAlertAlert?.Stop(); tmrExpiryAlertAlert?.Dispose();
                FONT_HEAD?.Dispose();
                FONT_SUB?.Dispose();
                FONT_BADGE?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmDashboard
            // 
            this.ClientSize = new System.Drawing.Size(1010, 421);
            this.Name = "frmDashboard";
            this.ResumeLayout(false);

        }
    }
}

/*
 ╔══════════════════════════════════════════════════════════════════════════╗
 ║  GHI CHÚ TÍCH HỢP – ĐỌC TRƯỚC KHI BUILD                               ║
 ╠══════════════════════════════════════════════════════════════════════════╣
 ║                                                                          ║
 ║  1. BIỂU ĐỒ DOANH THU DẠNG CỘT (BAR CHART)                            ║
 ║     File này dùng:  chartRevenue.ChartType = RevenueChartType.Bar       ║
 ║     Bạn cần thêm vào UcRevenueChart.cs:                                 ║
 ║                                                                          ║
 ║       public enum RevenueChartType { Line, Bar }                         ║
 ║       public RevenueChartType ChartType { get; set; } = RevenueChartType.Bar; ║
 ║                                                                          ║
 ║     Trong OnPaint / vẽ chart, thay vì DrawLines dùng FillRectangle      ║
 ║     cho từng cột theo giá trị DoanhThu.                                  ║
 ║                                                                          ║
 ║  2. CỘT TRÁI – UcCompaniesChart                                         ║
 ║     Giữ nguyên như mẫu cũ. Nếu muốn hiển thị dữ liệu thực, thêm        ║
 ║     method SetData(int totalCategories, int totalProducts) vào lớp đó.  ║
 ║                                                                          ║
 ║  3. CỘT TRÁI LIST – UcRecentTransactions                                ║
 ║     Giữ nguyên. Nếu muốn feed dữ liệu thực, thêm:                      ║
 ║       public void SetData(List<InvoiceDTO> invoices)                    ║
 ║     vào UcRecentTransactions.cs.                                         ║
 ║                                                                          ║
 ║  4. ALERT PANELS                                                         ║
 ║     Hai panel vàng/đỏ nhấp nháy sẽ tự xuất hiện khi có dữ liệu thực.  ║
 ║     Chúng được đẩy vào layout động – RecalcLayout() xử lý tự động.     ║
 ║                                                                          ║
 ║  5. NAMESPACE / USING                                                    ║
 ║     Đảm bảo UcTopProductsChart nằm trong namespace                      ║
 ║     SmartPOS.WinForms.UI.UserControls.Dashboard (hoặc thêm using).      ║
 ║                                                                          ║
 ╚══════════════════════════════════════════════════════════════════════════╝
*/