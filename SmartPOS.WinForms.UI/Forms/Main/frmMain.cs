using SmartPOS.WinForms.UI.Forms.Dashboard;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.UI.Forms.Invoices;
using SmartPOS.WinForms.UI.Forms.POS;
using SmartPOS.WinForms.UI.Forms.Reports;
using SmartPOS.WinForms.UI.Forms.Stock;
using SmartPOS.WinForms.UI.Forms.Users;
using SmartPOS.WinForms.UI.Forms.Products;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.UI.Interfaces;
using SmartPOS.WinForms.UI.UserControls;
using SmartPOS.WinForms.UI.UserControls.Navigation;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
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

        // Track current page to avoid redundant reloads
        private Form _currentPage;
        private string _lastQuickSearchTerm;
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;
        private readonly IProductLotService _productLotService;
        private readonly IUserService _userService;

        // ─────────────────────────────────────────────────────────────────
        public frmMain()
        {
            _invoiceService = new InvoiceService();
            _productService = new ProductService();
            _productLotService = new ProductLotService();
            _userService = new UserService();

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
            topBar.AllowAddNew = !SessionManager.IsStaff;

            topBar.SearchSubmitted += TopBar_SearchSubmitted;
            topBar.SearchCleared += TopBar_SearchCleared;
            topBar.BtnBellClicked += TopBar_BtnBellClicked;
            topBar.BtnSettingsClicked += TopBar_BtnSettingsClicked;
            topBar.BtnAvatarClicked += TopBar_BtnAvatarClicked;

            topBar.BtnPOSClicked += (s, e) =>
            {
                LoadPage(new frmPOS());
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
                            LoadPage(new frmProducts(), true);
                        }
                    }
                    return;
                }

                if (_currentPage is frmExpiredProducts)
                {
                    using (var frm = new frmProductEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmExpiredProducts(), true);
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
                            LoadPage(new frmCategories(), true);
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
                            LoadPage(new frmUsers(), true);
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

            if (page is frmExpiredProducts)
            {
                topBar.TitleText = "Ngừng bán / hết hạn";
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

        private void UpdateTopBarState(Form page)
        {
            if (topBar == null)
            {
                return;
            }

            topBar.AllowAddNew = CanShowAddButton(page);
            topBar.AvatarText = GetCurrentUserInitial();
            topBar.SearchPlaceholder = GetSearchPlaceholder(page);
            topBar.SetSearchText(_lastQuickSearchTerm, true);
        }

        private void TopBar_SearchSubmitted(object sender, string keyword)
        {
            string trimmed = keyword?.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                TopBar_SearchCleared(sender, EventArgs.Empty);
                return;
            }

            _lastQuickSearchTerm = trimmed;

            if (TryNavigateByQuickSearch(trimmed))
            {
                _lastQuickSearchTerm = string.Empty;
                topBar.ClearSearch(true);
                return;
            }

            if (_currentPage is IGlobalSearchHandler searchable)
            {
                searchable.ApplyGlobalSearch(trimmed);
                return;
            }

            if (TryOpenGlobalSearchResults(trimmed))
            {
                return;
            }

            MessageBox.Show(
                "Không tìm thấy chức năng phù hợp. Hãy thử từ khóa như sản phẩm, hóa đơn, nhập kho hoặc POS.",
                "Tìm kiếm trên thanh trên cùng");
        }

        private void TopBar_SearchCleared(object sender, EventArgs e)
        {
            _lastQuickSearchTerm = string.Empty;

            if (_currentPage is IGlobalSearchHandler searchable)
            {
                searchable.ClearGlobalSearch();
            }
        }

        private void ApplyPendingSearch()
        {
            if (string.IsNullOrWhiteSpace(_lastQuickSearchTerm))
            {
                return;
            }

            if (_currentPage is IGlobalSearchHandler searchable)
            {
                searchable.ApplyGlobalSearch(_lastQuickSearchTerm);
            }
        }

        private void TopBar_BtnBellClicked(object sender, EventArgs e)
        {
            Control anchor = sender as Control;
            if (anchor == null)
            {
                return;
            }

            ContextMenuStrip menu = BuildNotificationMenu();
            ShowTopBarMenu(menu, anchor);
        }

        private void TopBar_BtnSettingsClicked(object sender, EventArgs e)
        {
            Control anchor = sender as Control;
            if (anchor == null)
            {
                return;
            }

            ContextMenuStrip menu = BuildSettingsMenu();
            ShowTopBarMenu(menu, anchor);
        }

        private void TopBar_BtnAvatarClicked(object sender, EventArgs e)
        {
            Control anchor = sender as Control;
            if (anchor == null)
            {
                return;
            }

            ContextMenuStrip menu = BuildAccountMenu();
            ShowTopBarMenu(menu, anchor);
        }

        private ContextMenuStrip BuildNotificationMenu()
        {
            ContextMenuStrip menu = CreateTopBarMenu();
            AddMenuHeader(menu, "Thông báo hệ thống");

            try
            {
                int lowStockCount = _productService.GetAll()
                    .Count(x =>
                        x.TrangThai &&
                        x.SoLuongTon <= 10 &&
                        (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today));

                var lots = _productLotService.GetAll()
                    .Where(x => x.SoLuongTonLo > 0)
                    .ToList();

                int expiredLots = lots.Count(x =>
                    x.HanSuDung.HasValue &&
                    x.HanSuDung.Value.Date < DateTime.Today);

                int expiringSoonLots = lots.Count(x =>
                    x.HanSuDung.HasValue &&
                    x.HanSuDung.Value.Date >= DateTime.Today &&
                    x.HanSuDung.Value.Date <= DateTime.Today.AddDays(7));

                int todayInvoices = _invoiceService.GetAll()
                    .Count(x =>
                        x.NgayLap.Date == DateTime.Today &&
                        (!SessionManager.IsStaff ||
                         (SessionManager.CurrentUser != null && x.MaNV == SessionManager.CurrentUser.MaNV)));

                AddMenuAction(menu,
                    "Sản phẩm sắp hết hàng: " + lowStockCount,
                    () => LoadPage(new frmLowStock()));
                AddMenuAction(menu,
                    "Lô hết hạn hoặc sắp hết hạn: " + (expiredLots + expiringSoonLots),
                    () => LoadPage(new frmExpiredProducts()));
                AddMenuAction(menu,
                    "Hóa đơn hôm nay: " + todayInvoices,
                    () => LoadPage(new frmInvoices()));

                if (lowStockCount == 0 && expiredLots == 0 && expiringSoonLots == 0 && todayInvoices == 0)
                {
                    menu.Items.Add(CreateDisabledItem("Không có cảnh báo mới."));
                }
            }
            catch (Exception ex)
            {
                menu.Items.Add(CreateDisabledItem("Không tải được thông báo: " + ex.Message));
            }

            return menu;
        }

        private ContextMenuStrip BuildSettingsMenu()
        {
            ContextMenuStrip menu = CreateTopBarMenu();
            AddMenuHeader(menu, "Thao tác nhanh");

            AddMenuAction(menu, "Làm mới màn hình hiện tại", ReloadCurrentPage);
            AddMenuAction(menu, "Xóa ô tìm kiếm", () =>
            {
                _lastQuickSearchTerm = string.Empty;
                topBar.ClearSearch();
            });
            AddMenuHeader(menu, "Đi đến");
            AddMenuAction(menu, "Về tổng quan", () => LoadPage(new frmDashboard()));
            AddMenuAction(menu, "Mở POS", () => LoadPage(new frmPOS()));
            AddMenuAction(menu, "Mở sản phẩm", () => LoadPage(new frmProducts()));
            AddMenuAction(menu, "Mở hóa đơn", () => LoadPage(new frmInvoices()));
            AddMenuAction(menu, "Mở hàng sắp hết", () => LoadPage(new frmLowStock()));
            AddMenuAction(menu, "Mở hàng hết hạn", () => LoadPage(new frmExpiredProducts()));

            if (!SessionManager.IsStaff)
            {
                AddMenuAction(menu, "Mở danh mục", () => LoadPage(new frmCategories()));
                AddMenuAction(menu, "Mở nhập kho", () => LoadPage(new frmStockIn()));
                AddMenuAction(menu, "Mở lịch sử nhập kho", () => LoadPage(new frmStockHistory()));
                AddMenuAction(menu, "Mở báo cáo", () => LoadPage(new frmReports()));
                AddMenuAction(menu, "Mở người dùng", () => LoadPage(new frmUsers()));
            }

            return menu;
        }

        private ContextMenuStrip BuildAccountMenu()
        {
            ContextMenuStrip menu = CreateTopBarMenu();
            string userName = SessionManager.CurrentUser != null
                ? SessionManager.CurrentUser.TenNV
                : "Chưa đăng nhập";
            string userRole = SessionManager.CurrentUser != null
                ? SessionManager.CurrentUser.Quyen
                : string.Empty;

            AddMenuHeader(menu, userName);
            if (!string.IsNullOrWhiteSpace(userRole))
            {
                menu.Items.Add(CreateDisabledItem("Quyền: " + userRole));
            }

            AddMenuAction(menu, "Xem thông tin tài khoản", ShowCurrentUserProfile);
            AddMenuAction(menu, "Cập nhật thông tin cá nhân", EditCurrentUserProfile);
            AddMenuAction(menu, "Đăng xuất", LogoutCurrentUser);
            AddMenuAction(menu, "Thoát ứng dụng", ExitApplication);

            return menu;
        }

        private ContextMenuStrip CreateTopBarMenu()
        {
            return new ContextMenuStrip
            {
                ShowImageMargin = false
            };
        }

        private void AddMenuHeader(ContextMenuStrip menu, string text)
        {
            if (menu.Items.Count > 0)
            {
                menu.Items.Add(new ToolStripSeparator());
            }

            menu.Items.Add(CreateDisabledItem(text));
            menu.Items.Add(new ToolStripSeparator());
        }

        private ToolStripMenuItem CreateDisabledItem(string text)
        {
            return new ToolStripMenuItem(text) { Enabled = false };
        }

        private void AddMenuAction(ContextMenuStrip menu, string text, Action action)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.Click += (s, e) => action();
            menu.Items.Add(item);
        }

        private void ShowTopBarMenu(ContextMenuStrip menu, Control anchor)
        {
            if (menu == null || anchor == null)
            {
                return;
            }

            menu.Closed += (s, e) => menu.Dispose();
            menu.Show(anchor, new Point(0, anchor.Height + 4));
        }

        private bool TryNavigateByQuickSearch(string keyword)
        {
            string normalized = NormalizeKeyword(keyword);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return false;
            }

            if (MatchesQuickCommand(normalized, "tong quan", "dashboard", "tong hop"))
            {
                LoadPageFromQuickSearch(new frmDashboard());
                return true;
            }

            if (MatchesQuickCommand(normalized, "ban hang", "pos", "quay", "thanh toan"))
            {
                LoadPageFromQuickSearch(new frmPOS());
                return true;
            }

            if (MatchesQuickCommand(normalized, "hoa don", "invoice"))
            {
                LoadPageFromQuickSearch(new frmInvoices());
                return true;
            }

            if (MatchesQuickCommand(normalized, "san pham", "hang hoa", "ma vach"))
            {
                LoadPageFromQuickSearch(new frmProducts());
                return true;
            }

            if (MatchesQuickCommand(normalized, "het han", "ngung ban", "lo het han"))
            {
                LoadPageFromQuickSearch(new frmExpiredProducts());
                return true;
            }

            if (MatchesQuickCommand(normalized, "sap het", "ton kho thap", "low stock"))
            {
                LoadPageFromQuickSearch(new frmLowStock());
                return true;
            }

            if (MatchesQuickCommand(normalized, "lich su nhap", "stock history"))
            {
                if (SessionManager.IsStaff)
                {
                    return false;
                }

                LoadPageFromQuickSearch(new frmStockHistory());
                return true;
            }

            if (MatchesQuickCommand(normalized, "nhap kho", "phieu nhap", "stock in"))
            {
                if (SessionManager.IsStaff)
                {
                    return false;
                }

                LoadPageFromQuickSearch(new frmStockIn());
                return true;
            }

            if (MatchesQuickCommand(normalized, "danh muc", "loai hang", "category"))
            {
                if (SessionManager.IsStaff)
                {
                    return false;
                }

                LoadPageFromQuickSearch(new frmCategories());
                return true;
            }

            if (MatchesQuickCommand(normalized, "nguoi dung", "nhan vien", "tai khoan", "user"))
            {
                if (SessionManager.IsStaff)
                {
                    return false;
                }

                LoadPageFromQuickSearch(new frmUsers());
                return true;
            }

            if (MatchesQuickCommand(normalized, "bao cao", "doanh thu", "report"))
            {
                if (SessionManager.IsStaff)
                {
                    return false;
                }

                LoadPageFromQuickSearch(new frmReports());
                return true;
            }

            return false;
        }

        private void LoadPageFromQuickSearch(Form page)
        {
            LoadPage(page);
        }

        private bool TryOpenGlobalSearchResults(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return false;
            }

            _lastQuickSearchTerm = keyword.Trim();
            string normalized = NormalizeKeyword(keyword);

            if (!SessionManager.IsStaff && ContainsKeyword(normalized, "nhan vien", "tai khoan", "so dien thoai", "user"))
            {
                LoadPage(new frmUsers(), true);
                return true;
            }

            if (ContainsKeyword(normalized, "hoa don", "invoice"))
            {
                LoadPage(new frmInvoices(), true);
                return true;
            }

            if (ContainsKeyword(normalized, "sap het", "ton kho thap", "low stock"))
            {
                LoadPage(new frmLowStock(), true);
                return true;
            }

            if (ContainsKeyword(normalized, "het han", "ngung ban", "lo het han"))
            {
                LoadPage(new frmExpiredProducts(), true);
                return true;
            }

            if (!SessionManager.IsStaff && ContainsKeyword(normalized, "nhap kho", "phieu nhap", "stock"))
            {
                LoadPage(new frmStockHistory(), true);
                return true;
            }

            if (!SessionManager.IsStaff && ContainsKeyword(normalized, "danh muc", "loai hang", "category"))
            {
                LoadPage(new frmCategories(), true);
                return true;
            }

            if (!SessionManager.IsStaff && ContainsKeyword(normalized, "bao cao", "doanh thu", "report"))
            {
                LoadPage(new frmReports(), true);
                return true;
            }

            LoadPage(new frmProducts(), true);
            return true;
        }

        private bool MatchesQuickCommand(string normalizedSource, params string[] commands)
        {
            if (string.IsNullOrWhiteSpace(normalizedSource))
            {
                return false;
            }

            string[] prefixes =
            {
                string.Empty,
                "mo ",
                "xem ",
                "vao ",
                "den ",
                "toi ",
                "di den ",
                "chuyen den "
            };

            foreach (string command in commands)
            {
                string normalizedCommand = NormalizeKeyword(command);
                foreach (string prefix in prefixes)
                {
                    if (string.Equals(normalizedSource, prefix + normalizedCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ContainsKeyword(string normalizedSource, params string[] keywords)
        {
            foreach (string item in keywords)
            {
                if (normalizedSource.Contains(NormalizeKeyword(item)))
                {
                    return true;
                }
            }

            return false;
        }

        private string NormalizeKeyword(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder
                .ToString()
                .Replace('đ', 'd')
                .Normalize(NormalizationForm.FormC);
        }

        private void ReloadCurrentPage()
        {
            if (_currentPage == null)
            {
                return;
            }

            if (_currentPage is frmDashboard)
            {
                LoadPage(new frmDashboard(), true);
                return;
            }

            if (_currentPage is frmPOS)
            {
                LoadPage(new frmPOS(), true);
                return;
            }

            if (_currentPage is frmInvoices)
            {
                LoadPage(new frmInvoices(), true);
                return;
            }

            if (_currentPage is frmUsers)
            {
                LoadPage(new frmUsers(), true);
                return;
            }

            if (_currentPage is frmReports)
            {
                LoadPage(new frmReports(), true);
                return;
            }

            if (_currentPage is frmProducts)
            {
                LoadPage(new frmProducts(), true);
                return;
            }

            if (_currentPage is frmExpiredProducts)
            {
                LoadPage(new frmExpiredProducts(), true);
                return;
            }

            if (_currentPage is frmCategories)
            {
                LoadPage(new frmCategories(), true);
                return;
            }

            if (_currentPage is frmStockIn)
            {
                LoadPage(new frmStockIn(), true);
                return;
            }

            if (_currentPage is frmStockHistory)
            {
                LoadPage(new frmStockHistory(), true);
                return;
            }

            if (_currentPage is frmLowStock)
            {
                LoadPage(new frmLowStock(), true);
            }
        }

        private bool CanShowAddButton(Form page)
        {
            return !SessionManager.IsStaff &&
                (page is frmProducts ||
                 page is frmExpiredProducts ||
                 page is frmCategories ||
                 page is frmUsers);
        }

        private string GetSearchPlaceholder(Form page)
        {
            if (page is frmDashboard)
            {
                return "Mở nhanh chức năng: sản phẩm, hóa đơn, POS...";
            }

            if (page is frmPOS)
            {
                return "Tìm theo tên hoặc mã vạch để bán hàng...";
            }

            if (page is frmProducts)
            {
                return "Tìm theo tên sản phẩm, mã vạch hoặc danh mục...";
            }

            if (page is frmInvoices)
            {
                return "Tìm theo mã hóa đơn, nhân viên hoặc trạng thái...";
            }

            if (page is frmUsers)
            {
                return "Tìm theo tên, tài khoản hoặc số điện thoại...";
            }

            if (page is frmCategories)
            {
                return "Tìm theo tên danh mục hoặc mô tả...";
            }

            if (page is frmExpiredProducts)
            {
                return "Tìm sản phẩm ngừng bán hoặc sắp hết hạn...";
            }

            if (page is frmLowStock)
            {
                return "Tìm sản phẩm sắp hết hàng...";
            }

            if (page is frmStockIn)
            {
                return "Tìm sản phẩm để thêm vào phiếu nhập...";
            }

            if (page is frmStockHistory)
            {
                return "Tìm theo mã phiếu nhập hoặc nhân viên...";
            }

            if (page is frmReports)
            {
                return "Gõ doanh thu, hóa đơn, nhập kho hoặc tồn kho...";
            }

            return "Tìm trong màn hình hiện tại...";
        }

        private string GetCurrentUserInitial()
        {
            if (SessionManager.CurrentUser == null)
            {
                return "A";
            }

            string source = !string.IsNullOrWhiteSpace(SessionManager.CurrentUser.TenNV)
                ? SessionManager.CurrentUser.TenNV.Trim()
                : SessionManager.CurrentUser.TaiKhoan;

            return string.IsNullOrWhiteSpace(source) ? "A" : source.Substring(0, 1);
        }

        private void ShowCurrentUserProfile()
        {
            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Chưa có thông tin tài khoản đang đăng nhập.", "Tài khoản");
                return;
            }

            var user = SessionManager.CurrentUser;
            string phone = string.IsNullOrWhiteSpace(user.SoDienThoai) ? "-" : user.SoDienThoai;
            string address = string.IsNullOrWhiteSpace(user.DiaChi) ? "-" : user.DiaChi;

            MessageBox.Show(
                "Nhân viên: " + user.TenNV + Environment.NewLine +
                "Tài khoản: " + user.TaiKhoan + Environment.NewLine +
                "Quyền: " + user.Quyen + Environment.NewLine +
                "Số điện thoại: " + phone + Environment.NewLine +
                "Địa chỉ: " + address,
                "Thông tin tài khoản",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void EditCurrentUserProfile()
        {
            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Chưa có tài khoản đăng nhập để cập nhật.", "Tài khoản");
                return;
            }

            var currentUser = _userService.GetById(SessionManager.CurrentUser.MaNV) ?? SessionManager.CurrentUser;
            using (var frm = new frmUserEdit(currentUser))
            {
                frm.ShowDialog(this);
                if (!frm.IsSavedSuccessfully)
                {
                    return;
                }

                SessionManager.CurrentUser = _userService.GetById(currentUser.MaNV) ?? currentUser;
                UpdateTopBarState(_currentPage);
                ShowCurrentUserProfile();
            }
        }

        private void LogoutCurrentUser()
        {
            DialogResult result = MessageBox.Show(
                "Bạn có muốn đăng xuất khỏi hệ thống không?",
                "Đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            SessionManager.Clear();
            this.Close();
        }

        private void ExitApplication()
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ứng dụng không?",
                "Thoát ứng dụng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        // ══════════════════════════════════════════════════════════════════
        //  PAGE LOADING
        // ══════════════════════════════════════════════════════════════════
        private void LoadPage(Form page, bool preserveSearch = false)
        {
            if (page == null) return;

            if (!preserveSearch)
            {
                _lastQuickSearchTerm = string.Empty;
            }

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
            UpdateTopBarState(page);
            SyncSidebarSelection(page);
            page.BeginInvoke((Action)(() => ApplyPendingSearch()));
        }

        public void NavigateToPage(Form page)
        {
            LoadPage(page);
        }

        private void SyncSidebarSelection(Form page)
        {
            if (sidebar == null || page == null)
            {
                return;
            }

            string menuKey = GetMenuKeyForPage(page);
            if (!string.IsNullOrWhiteSpace(menuKey))
            {
                sidebar.SetActiveMenu(menuKey);
            }
        }

        private string GetMenuKeyForPage(Form page)
        {
            if (page is frmDashboard)
            {
                return "Dashboard";
            }

            if (page is frmPOS)
            {
                return "POS";
            }

            if (page is frmInvoices)
            {
                return "Invoices";
            }

            if (page is frmUsers)
            {
                return "Users";
            }

            if (page is frmReports)
            {
                return "Reports";
            }

            if (page is frmProducts)
            {
                return "Products";
            }

            if (page is frmExpiredProducts)
            {
                return "ExpiredProducts";
            }

            if (page is frmLowStock)
            {
                return "LowStocks";
            }

            if (page is frmCategories)
            {
                return "Category";
            }

            if (page is frmStockIn)
            {
                return "ManageStock";
            }

            if (page is frmStockHistory)
            {
                return "StockAdjust";
            }

            return string.Empty;
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
            if (!CanAccessMenu(menuKey))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.", "Thông báo");
                return;
            }

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
                    LoadPage(new frmExpiredProducts());
                    break;

                case "LowStocks":
                    LoadPage(new frmLowStock());
                    break;

                case "Category":
                    LoadPage(new frmCategories());
                    break;

                case "ManageStock":
                    LoadPage(new frmStockIn());
                    break;

                case "StockAdjust":
                    LoadPage(new frmStockHistory());
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

                default:
                    NavigatePlaceholder(menuKey);
                    break;
            }
        }

        private bool CanAccessMenu(string menuKey)
        {
            if (!SessionManager.IsStaff)
            {
                return true;
            }

            switch (menuKey)
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
