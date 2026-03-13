using SmartPOS.WinForms.UI.Forms.Dashboard;
using SmartPOS.WinForms.UI.Forms.Invoices;
using SmartPOS.WinForms.UI.Forms.POS;
using SmartPOS.WinForms.UI.Forms.Reports;
using SmartPOS.WinForms.UI.Forms.Stock;
using SmartPOS.WinForms.UI.Forms.Users;
using SmartPOS.WinForms.UI.Forms.Products;
using SmartPOS.WinForms.UI.UserControls;
using SmartPOS.WinForms.UI.UserControls.Navigation;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.Main
{
    public class frmMain : Form
    {
        // ── Design tokens ─────────────────────────────────────────────────
        private static readonly Color BG = Color.FromArgb(248, 249, 251);
        private static readonly Color SURFACE = Color.White;
        private static readonly Color BORDER = Color.FromArgb(228, 231, 238);
        private static readonly Color ACCENT = Color.FromArgb(22, 32, 72);   // deep navy

        // ── Controls ──────────────────────────────────────────────────────
        private UcSidebar sidebar;
        private UcTopBar topBar;
        private Panel pnlContent;     // holds the active page
        private Panel pnlRight;       // topBar + pnlContent (right column)
        private Panel pnlSidebarWrap; // fixed-width left column

        // Track current page to avoid redundant reloads
        private Form _currentPage;

        // ─────────────────────────────────────────────────────────────────
        public frmMain()
        {
            this.Text = "SmartPOS";
            this.MinimumSize = new Size(960, 640);
            this.Size = new Size(1280, 820);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BG;
            this.Font = new Font("Segoe UI", 9.5F);
            this.DoubleBuffered = true;

            BuildLayout();

            // Initial page after handle is created to avoid layout timing issues
            this.Load += (s, e) => LoadPage(new frmDashboard());
        }

        // ══════════════════════════════════════════════════════════════════
        //  BUILD LAYOUT
        //
        //  Structure:
        //  ┌─────────────────────────────────────────────┐
        //  │  pnlSidebarWrap (fixed width, left)         │
        //  │  ┌─────────────────────────────────────┐    │
        //  │  │  sidebar (UcSidebar)                │    │
        //  │  └─────────────────────────────────────┘    │
        //  ├─────────────────────────────────────────────┤
        //  │  pnlRight (Fill)                            │
        //  │  ┌─────────────────────────────────────┐    │
        //  │  │  topBar  (Dock=Top)                 │    │
        //  │  ├─────────────────────────────────────┤    │
        //  │  │  pnlContent (Dock=Fill)             │    │
        //  │  └─────────────────────────────────────┘    │
        //  └─────────────────────────────────────────────┘
        // ══════════════════════════════════════════════════════════════════
        private void BuildLayout()
        {
            // ── Sidebar ──────────────────────────────────────────────────
            sidebar = new UcSidebar
            {
                ExpandedWidth = 210,
                CollapsedWidth = 64,
                Dock = DockStyle.Left
            };
            sidebar.MenuClicked += Sidebar_MenuClicked;

            // When sidebar expands/collapses, force active page to re-layout
            sidebar.SizeChanged += (s, e) => RefreshCurrentPage();

            // ── Thin separator line between sidebar and content ───────────
            var sep = new Panel
            {
                Dock = DockStyle.Left,
                Width = 1,
                BackColor = BORDER
            };

            // ── Top bar ──────────────────────────────────────────────────
            topBar = new UcTopBar
            {
                Dock = DockStyle.Top,
                Height = 56
            };

            topBar.BtnPOSClicked += (s, e) =>
            {
                NavigatePlaceholder("Bán hàng");
            };

            topBar.BtnAddNewClicked += (s, e) =>
            {
                if (_currentPage is frmProducts)
                {
                    using (var frm = new frmProductEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmProducts());
                        }
                    }
                    return;
                }

                if (_currentPage is frmCategories)
                {
                    using (var frm = new frmCategoryEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmCategories());
                        }
                    }
                    return;
                }

                if (_currentPage is frmUsers)
                {
                    using (var frm = new frmUserEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmUsers());
                        }
                    }
                    return;
                }

                MessageBox.Show("Chức năng thêm mới chưa được hỗ trợ ở màn hình này.", "Thông báo");
            };


            // ── Content area ─────────────────────────────────────────────
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BG,
                Padding = new Padding(0)
            };

            // ── Right column (topBar + content) ──────────────────────────
            pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BG,
                Padding = new Padding(0)
            };

            // Add order matters: Fill first, then Top (Controls are z-ordered)
            pnlRight.Controls.Add(pnlContent);   // Fill – added first
            pnlRight.Controls.Add(topBar);        // Top  – added second, paints over Fill

            // ── Assemble form ────────────────────────────────────────────
            // Add order: right first (Fill), then sep, then sidebar (Left)
            this.Controls.Add(pnlRight);
            this.Controls.Add(sep);
            this.Controls.Add(sidebar);
        }
        private void UpdateTopBarTitle(Form page)
        {
            if (topBar == null || page == null)
            {
                return;
            }

            if (page is frmDashboard)
            {
                topBar.TitleText = "Tổng quan";
                return;
            }

            if (page is frmPOS)
            {
                topBar.TitleText = "Bán hàng";
                return;
            }

            if (page is frmInvoices)
            {
                topBar.TitleText = "Hóa đơn";
                return;
            }

            if (page is frmUsers)
            {
                topBar.TitleText = "Người dùng";
                return;
            }

            if (page is frmReports)
            {
                topBar.TitleText = "Báo cáo";
                return;
            }

            if (page is frmProducts)
            {
                topBar.TitleText = "Sản phẩm";
                return;
            }

            if (page is frmCategories)
            {
                topBar.TitleText = "Danh mục";
                return;
            }

            if (page is frmStockIn)
            {
                topBar.TitleText = "Nhập kho";
                return;
            }

            if (page is frmStockHistory)
            {
                topBar.TitleText = "Lịch sử nhập kho";
                return;
            }

            if (page is frmLowStock)
            {
                topBar.TitleText = "Sắp hết hàng";
                return;
            }

            topBar.TitleText = page.Text;
        }
        // ══════════════════════════════════════════════════════════════════
        //  PAGE LOADING
        // ══════════════════════════════════════════════════════════════════
        private void LoadPage(Form page)
        {
            if (page == null) return;

            if (_currentPage != null && !_currentPage.IsDisposed)
            {
                pnlContent.Controls.Remove(_currentPage);
                _currentPage.Dispose();
            }

            _currentPage = page;

            page.TopLevel = false;
            page.FormBorderStyle = FormBorderStyle.None;
            page.Dock = DockStyle.Fill;
            page.BackColor = BG;

            pnlContent.SuspendLayout();
            pnlContent.Controls.Add(page);
            page.Show();
            page.BringToFront();
            pnlContent.ResumeLayout(true);
            pnlContent.PerformLayout();

            UpdateTopBarTitle(page);
        }

        // Force the current page to re-measure itself (e.g. after sidebar toggle)
        private void RefreshCurrentPage()
        {
            if (_currentPage == null || _currentPage.IsDisposed) return;
            _currentPage.Width = pnlContent.ClientSize.Width;
            _currentPage.Height = pnlContent.ClientSize.Height;
            _currentPage.PerformLayout();
            _currentPage.Refresh();
        }

        // ══════════════════════════════════════════════════════════════════
        //  MENU NAVIGATION
        // ══════════════════════════════════════════════════════════════════
        private void Sidebar_MenuClicked(object sender, string menuKey)
        {
            switch (menuKey)
            {
                case "Dashboard":
                    LoadPage(new frmDashboard());
                    break;

                case "Products":
                    LoadPage(new frmProducts());
                    break;

                case "CreateProduct":
                    using (var frm = new frmProductEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmProducts());
                        }
                    }
                    break;

                case "ExpiredProducts":
                    NavigatePlaceholder("Sản phẩm ngừng bán / hết hạn");
                    break;

                case "LowStocks":
                    LoadPage(new frmLowStock());
                    break;

                case "Category":
                    LoadPage(new frmCategories());
                    break;

                case "SubCategory":
                    NavigatePlaceholder("Danh mục phụ");
                    break;

                case "ManageStock":
                    LoadPage(new frmStockIn());
                    break;

                case "StockAdjust":
                    LoadPage(new frmStockHistory());
                    break;

                case "StockTransfer":
                    NavigatePlaceholder("Chuyển kho");
                    break;

                case "POS":
                    LoadPage(new frmPOS());
                    break;

                case "Invoices":
                    LoadPage(new frmInvoices());
                    break;

                case "Reports":
                    LoadPage(new frmReports());
                    break;

                case "Users":
                    LoadPage(new frmUsers());
                    break;

                case "SuperAdmin":
                    NavigatePlaceholder("Quản trị hệ thống");
                    break;

                case "Applications":
                    NavigatePlaceholder("Ứng dụng");
                    break;

                case "Layouts":
                    NavigatePlaceholder("Giao diện");
                    break;

                default:
                    NavigatePlaceholder(menuKey);
                    break;
            }
        }

        // Placeholder page while real pages are not yet built
        private void NavigatePlaceholder(string pageName)
        {
            var page = new Form
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = BG
            };

            var lbl = new Label
            {
                Text = pageName,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 185, 200),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            page.Controls.Add(lbl);
            LoadPage(page);
        }

        // ══════════════════════════════════════════════════════════════════
        //  DISPOSE
        // ══════════════════════════════════════════════════════════════════
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currentPage?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}