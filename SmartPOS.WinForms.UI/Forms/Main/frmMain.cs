using SmartPOS.WinForms.UI.Forms.Dashboard;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.UI.Forms.Invoices;
using SmartPOS.WinForms.UI.Forms.POS;
using SmartPOS.WinForms.UI.Forms.Reports;
using SmartPOS.WinForms.UI.Forms.Stock;
using SmartPOS.WinForms.UI.Forms.Users;
using SmartPOS.WinForms.UI.Forms.Products;
using SmartPOS.WinForms.UI.Forms.Customers;
using SmartPOS.WinForms.UI.Forms.ChatBot;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.UI.Interfaces;
using SmartPOS.WinForms.UI.UserControls;
using SmartPOS.WinForms.UI.UserControls.ChatBot;
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
        private Panel pnlPageHost;    // active page area, next to AI chat
        private Panel pnlRight;       // topBar + pnlContent (right column)
        private UcAiChatPanel aiChatPanel;
        private Button btnAiFloat;

        // Track current page to avoid redundant reloads
        private Form _currentPage;
        private string _lastQuickSearchTerm;
        private ContextMenuStrip _activeTopBarMenu;
        private bool _aiChatWide;
        private bool _isAiFloatDragging;
        private bool _aiFloatDragMoved;
        private Point _aiFloatDragStartMouse;
        private Point _aiFloatStartLocation;
        private Point? _aiFloatCustomLocation;
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

                if (_currentPage is frmCustomers)
                {
                    using (var frm = new frmCustomerEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmCustomers(), true);
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
            pnlContent.Resize += (s, e) => UpdateAiFloatButtonLayout();

            pnlPageHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BG,
                Padding = new Padding(0)
            };

            aiChatPanel = new UcAiChatPanel
            {
                Dock = DockStyle.Right,
                Width = 360,
                Visible = false
            };
            aiChatPanel.CloseRequested += (s, e) => SetAiChatVisible(false);
            aiChatPanel.ExpandRequested += (s, e) => ToggleAiChatWidth();

            btnAiFloat = new RobotAiButton
            {
                Size = new Size(50, 50),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            btnAiFloat.Click += BtnAiFloat_Click;
            btnAiFloat.MouseDown += BtnAiFloat_MouseDown;
            btnAiFloat.MouseMove += BtnAiFloat_MouseMove;
            btnAiFloat.MouseUp += BtnAiFloat_MouseUp;

            pnlContent.Controls.Add(pnlPageHost);
            pnlContent.Controls.Add(aiChatPanel);
            pnlContent.Controls.Add(btnAiFloat);
            UpdateAiFloatButtonLayout();

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

        private void OpenAiChatPanel(string question = null)
        {
            SetAiChatVisible(true);

            if (!string.IsNullOrWhiteSpace(question))
            {
                aiChatPanel.Ask(question);
            }
        }

        private void LoadChatBotPage(bool preserveSearch = false)
        {
            SetAiChatVisible(false);
            LoadPage(new frmChatBot(), preserveSearch);
        }

        private void SetAiChatVisible(bool visible)
        {
            if (aiChatPanel == null)
            {
                return;
            }

            aiChatPanel.Visible = visible;
            if (visible)
            {
                aiChatPanel.ReloadFromSession();
                aiChatPanel.BringToFront();
            }

            pnlContent.PerformLayout();
            UpdateAiFloatButtonLayout();

            if (visible)
            {
                aiChatPanel.FocusInput();
            }
        }

        private void ToggleAiChatWidth()
        {
            if (aiChatPanel == null)
            {
                return;
            }

            _aiChatWide = !_aiChatWide;
            aiChatPanel.Width = _aiChatWide ? 430 : 360;
            pnlContent.PerformLayout();
            UpdateAiFloatButtonLayout();
        }

        private void UpdateAiFloatButtonLayout()
        {
            if (pnlContent == null || btnAiFloat == null)
            {
                return;
            }

            // Nếu panel AI đang mở, nút nổi sẽ nằm bên trái panel chat.
            // Nếu panel AI đang đóng, nút nằm sát góc phải màn hình.
            int rightOffset = aiChatPanel != null && aiChatPanel.Visible
                ? aiChatPanel.Width + 24
                : 24;

            int bottomOffset = 24;

            Point target;

            if (_aiFloatCustomLocation.HasValue)
            {
                target = ClampAiFloatLocation(_aiFloatCustomLocation.Value);
                _aiFloatCustomLocation = target;
            }
            else
            {
                int x = pnlContent.ClientSize.Width - rightOffset - btnAiFloat.Width;
                int y = pnlContent.ClientSize.Height - bottomOffset - btnAiFloat.Height;

                target = ClampAiFloatLocation(new Point(x, y));
            }

            btnAiFloat.Location = target;

            

            btnAiFloat.BringToFront();
        }

        private Point ClampAiFloatLocation(Point location)
        {
            if (pnlContent == null || btnAiFloat == null)
            {
                return location;
            }

            const int leftMargin = 12;
            const int topMargin = 12;
            const int bottomMargin = 12;

            // Nếu panel AI đang mở thì chừa vùng bên phải cho panel chat
            int rightReserved = aiChatPanel != null && aiChatPanel.Visible
                ? aiChatPanel.Width + 12
                : 12;

            int maxX = pnlContent.ClientSize.Width - btnAiFloat.Width - rightReserved;
            int maxY = pnlContent.ClientSize.Height - btnAiFloat.Height - bottomMargin;

            maxX = Math.Max(leftMargin, maxX);
            maxY = Math.Max(topMargin, maxY);

            int x = Math.Max(leftMargin, Math.Min(maxX, location.X));
            int y = Math.Max(topMargin, Math.Min(maxY, location.Y));

            return new Point(x, y);
        }

        private void BtnAiFloat_Click(object sender, EventArgs e)
        {
            if (_aiFloatDragMoved)
            {
                _aiFloatDragMoved = false;
                return;
            }

            SetAiChatVisible(!aiChatPanel.Visible);
        }

        private void BtnAiFloat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            _isAiFloatDragging = true;
            _aiFloatDragMoved = false;
            _aiFloatDragStartMouse = pnlContent.PointToClient(Cursor.Position);
            _aiFloatStartLocation = btnAiFloat.Location;
            btnAiFloat.Capture = true;
            btnAiFloat.Cursor = Cursors.SizeAll;
        }

        private void BtnAiFloat_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isAiFloatDragging)
            {
                return;
            }

            Point currentMouse = pnlContent.PointToClient(Cursor.Position);
            int dx = currentMouse.X - _aiFloatDragStartMouse.X;
            int dy = currentMouse.Y - _aiFloatDragStartMouse.Y;

            if (Math.Abs(dx) > 3 || Math.Abs(dy) > 3)
            {
                _aiFloatDragMoved = true;
            }

            Point next = ClampAiFloatLocation(new Point(
                _aiFloatStartLocation.X + dx,
                _aiFloatStartLocation.Y + dy));
            _aiFloatCustomLocation = next;
            btnAiFloat.Location = next;
            btnAiFloat.BringToFront();
        }

        private void BtnAiFloat_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isAiFloatDragging)
            {
                return;
            }

            _isAiFloatDragging = false;
            btnAiFloat.Capture = false;
            btnAiFloat.Cursor = Cursors.Hand;
            _aiFloatCustomLocation = ClampAiFloatLocation(btnAiFloat.Location);
        }

        private static void ApplyCircleRegion(Control control)
        {
            if (control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, control.Width, control.Height);
                control.Region = new Region(path);
            }
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

            if (page is frmCustomers)
            {
                topBar.TitleText = "Khách hàng";
                return;
            }

            if (page is frmChatBot)
            {
                topBar.TitleText = "Trợ lý AI";
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
            UpdateTopBarNotificationCount();
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
                TopBarNotificationSummary summary = BuildNotificationSummary();
                topBar.NotificationCount = summary.TotalAlertCount;

                AddMenuInfo(menu, "Cảnh báo cần xử lý: " + summary.TotalAlertCount);
                AddMenuInfo(menu, "Doanh thu hôm nay: " + summary.TodayRevenue.ToString("N0") + " đ");
                AddMenuInfo(menu, "Hóa đơn hôm nay: " + summary.TodayInvoices);
                AddMenuSeparator(menu);

                AddMenuAction(menu,
                    "Sản phẩm tồn kho thấp: " + summary.LowStockProducts,
                    () => LoadPage(new frmLowStock()),
                    summary.LowStockProducts > 0);
                AddMenuAction(menu,
                    "Lô đã hết hạn: " + summary.ExpiredLots,
                    () => LoadPage(new frmExpiredProducts()),
                    summary.ExpiredLots > 0);
                AddMenuAction(menu,
                    "Lô sắp hết hạn trong 7 ngày: " + summary.ExpiringSoonLots,
                    () => LoadPage(new frmExpiredProducts()),
                    summary.ExpiringSoonLots > 0);
                AddMenuAction(menu,
                    "Xem danh sách hóa đơn hôm nay",
                    () => LoadPage(new frmInvoices()),
                    summary.TodayInvoices > 0);

                AddMenuSeparator(menu);
                AddMenuAction(menu, "Làm mới thông báo", UpdateTopBarNotificationCount);

                if (summary.TotalAlertCount == 0)
                {
                    AddMenuInfo(menu, "Không có cảnh báo tồn kho/hạn sử dụng mới.");
                }
            }
            catch (Exception ex)
            {
                AddMenuInfo(menu, "Không tải được thông báo: " + ex.Message);
            }

            return menu;
        }

        private void UpdateTopBarNotificationCount()
        {
            if (topBar == null)
            {
                return;
            }

            try
            {
                topBar.NotificationCount = BuildNotificationSummary().TotalAlertCount;
            }
            catch
            {
                topBar.NotificationCount = 0;
            }
        }

        private TopBarNotificationSummary BuildNotificationSummary()
        {
            DateTime today = DateTime.Today;
            var products = _productService.GetAll().ToList();
            var lots = _productLotService.GetAll()
                .Where(x => x.SoLuongTonLo > 0 && x.TrangThaiSanPham)
                .ToList();
            var invoices = _invoiceService.GetAll().ToList();

            if (SessionManager.IsStaff && SessionManager.CurrentUser != null)
            {
                invoices = invoices
                    .Where(x => x.MaNV == SessionManager.CurrentUser.MaNV)
                    .ToList();
            }

            return new TopBarNotificationSummary
            {
                LowStockProducts = products.Count(x =>
                    x.TrangThai &&
                    x.SoLuongTon <= 10 &&
                    (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= today)),
                ExpiredLots = lots.Count(x =>
                    x.HanSuDung.HasValue &&
                    x.HanSuDung.Value.Date < today),
                ExpiringSoonLots = lots.Count(x =>
                    x.HanSuDung.HasValue &&
                    x.HanSuDung.Value.Date >= today &&
                    x.HanSuDung.Value.Date <= today.AddDays(7)),
                TodayInvoices = invoices.Count(x => x.NgayLap.Date == today),
                TodayRevenue = invoices
                    .Where(x =>
                        x.NgayLap.Date == today &&
                        string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                    .Sum(x => x.TongTien)
            };
        }

        private ContextMenuStrip BuildSettingsMenu()
        {
            ContextMenuStrip menu = CreateTopBarMenu();
            AddMenuHeader(menu, "Thao tác nhanh");

            AddMenuInfo(menu, "Màn hình hiện tại: " + (topBar != null ? topBar.TitleText : string.Empty));
            AddMenuSeparator(menu);
            AddMenuAction(menu, "Đưa con trỏ vào ô tìm kiếm", () => topBar.FocusSearch());
            AddMenuAction(menu, "Làm mới màn hình hiện tại", ReloadCurrentPage);
            AddMenuAction(menu, "Xóa ô tìm kiếm", () =>
            {
                _lastQuickSearchTerm = string.Empty;
                topBar.ClearSearch();
            });
            AddMenuAction(menu, "Thu gọn / mở rộng menu trái", () => sidebar.ToggleCollapsed());

            AddMenuHeader(menu, "Đi đến");
            AddMenuAction(menu, "Về tổng quan", () => LoadPage(new frmDashboard()));
            AddMenuAction(menu, "Mở POS", () => LoadPage(new frmPOS()));
            AddMenuAction(menu, "Mở sản phẩm", () => LoadPage(new frmProducts()));
            AddMenuAction(menu, "Mở hóa đơn", () => LoadPage(new frmInvoices()));
            AddMenuAction(menu, "Mở khách hàng", () => LoadPage(new frmCustomers()));
            AddMenuAction(menu, "Mở trợ lý AI", () => LoadChatBotPage());
            AddMenuAction(menu, "Mở hàng sắp hết", () => LoadPage(new frmLowStock()));
            AddMenuAction(menu, "Mở hàng hết hạn", () => LoadPage(new frmExpiredProducts()));

            if (!SessionManager.IsStaff)
            {
                AddMenuAction(menu, "Mở danh mục", () => LoadPage(new frmCategories()));
                AddMenuAction(menu, "Mở nhập kho", () => LoadPage(new frmStockIn()));
                AddMenuAction(menu, "Mở lịch sử nhập kho", () => LoadPage(new frmStockHistory()));
                AddMenuAction(menu, "Mở báo cáo", () => LoadPage(new frmReports()));
                AddMenuAction(menu, "Mở người dùng", () => LoadPage(new frmUsers()));
                AddMenuHeader(menu, "Tạo nhanh");
                AddMenuAction(menu, "Thêm hội viên mới", () =>
                {
                    using (var frm = new frmCustomerEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmCustomers());
                        }
                    }
                });
                AddMenuAction(menu, "Thêm sản phẩm mới", () =>
                {
                    using (var frm = new frmProductEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmProducts());
                        }
                    }
                });
                AddMenuAction(menu, "Thêm người dùng mới", () =>
                {
                    using (var frm = new frmUserEdit())
                    {
                        frm.ShowDialog(this);
                        if (frm.IsSavedSuccessfully)
                        {
                            LoadPage(new frmUsers());
                        }
                    }
                });
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
            string userAccount = SessionManager.CurrentUser != null
                ? SessionManager.CurrentUser.TaiKhoan
                : string.Empty;
            string phone = SessionManager.CurrentUser != null && !string.IsNullOrWhiteSpace(SessionManager.CurrentUser.SoDienThoai)
                ? SessionManager.CurrentUser.SoDienThoai
                : "-";

            AddMenuHeader(menu, userName);
            if (!string.IsNullOrWhiteSpace(userAccount))
            {
                AddMenuInfo(menu, "Tài khoản: " + userAccount);
            }
            if (!string.IsNullOrWhiteSpace(userRole))
            {
                AddMenuInfo(menu, "Quyền: " + userRole);
            }
            AddMenuInfo(menu, "Số điện thoại: " + phone);
            AddMenuSeparator(menu);

            AddMenuAction(menu, "Xem thông tin tài khoản", ShowCurrentUserProfile);
            AddMenuAction(menu, "Cập nhật thông tin cá nhân", EditCurrentUserProfile);
            if (!SessionManager.IsStaff)
            {
                AddMenuAction(menu, "Quản lý người dùng", () => LoadPage(new frmUsers()));
            }
            AddMenuSeparator(menu);
            AddMenuAction(menu, "Đăng xuất", LogoutCurrentUser);
            AddMenuAction(menu, "Thoát ứng dụng", ExitApplication);

            return menu;
        }

        private ContextMenuStrip CreateTopBarMenu()
        {
            return new ContextMenuStrip
            {
                ShowImageMargin = false,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 50, 80),
                Padding = new Padding(6)
            };
        }

        private void AddMenuHeader(ContextMenuStrip menu, string text)
        {
            if (menu.Items.Count > 0)
            {
                menu.Items.Add(new ToolStripSeparator());
            }

            menu.Items.Add(CreateDisabledItem(text, true));
            menu.Items.Add(new ToolStripSeparator());
        }

        private ToolStripMenuItem CreateDisabledItem(string text, bool isHeader = false)
        {
            return new ToolStripMenuItem(text)
            {
                Enabled = false,
                Font = new Font("Segoe UI", 9F, isHeader ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = isHeader ? Color.FromArgb(22, 32, 72) : Color.FromArgb(90, 100, 125)
            };
        }

        private void AddMenuInfo(ContextMenuStrip menu, string text)
        {
            menu.Items.Add(CreateDisabledItem(text));
        }

        private void AddMenuSeparator(ContextMenuStrip menu)
        {
            if (menu.Items.Count > 0 && !(menu.Items[menu.Items.Count - 1] is ToolStripSeparator))
            {
                menu.Items.Add(new ToolStripSeparator());
            }
        }

        private void AddMenuAction(ContextMenuStrip menu, string text, Action action, bool enabled = true)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text)
            {
                Enabled = enabled
            };

            if (enabled)
            {
                item.Click += (s, e) => RunTopBarAction(action);
            }

            menu.Items.Add(item);
        }

        private void ShowTopBarMenu(ContextMenuStrip menu, Control anchor)
        {
            if (menu == null || anchor == null)
            {
                return;
            }

            DisposeActiveTopBarMenu();
            _activeTopBarMenu = menu;
            menu.Closed += TopBarMenu_Closed;
            menu.Disposed += TopBarMenu_Disposed;
            menu.Show(anchor, new Point(0, anchor.Height + 4));
        }

        private void TopBarMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            if (menu == null)
            {
                return;
            }

            if (ReferenceEquals(_activeTopBarMenu, menu))
            {
                _activeTopBarMenu = null;
            }

            // Dispose after WinForms finishes the drop-down close/click pipeline.
            BeginInvokeIfPossible(() =>
            {
                if (!menu.IsDisposed)
                {
                    menu.Dispose();
                }
            });
        }

        private void TopBarMenu_Disposed(object sender, EventArgs e)
        {
            if (ReferenceEquals(_activeTopBarMenu, sender))
            {
                _activeTopBarMenu = null;
            }
        }

        private void DisposeActiveTopBarMenu()
        {
            ContextMenuStrip menu = _activeTopBarMenu;
            _activeTopBarMenu = null;

            if (menu == null || menu.IsDisposed)
            {
                return;
            }

            menu.Closed -= TopBarMenu_Closed;
            menu.Disposed -= TopBarMenu_Disposed;
            menu.Dispose();
        }

        private void RunTopBarAction(Action action)
        {
            if (action == null)
            {
                return;
            }

            BeginInvokeIfPossible(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không thực hiện được thao tác:\n" + ex.Message,
                        "Lỗi thao tác",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            });
        }

        private void BeginInvokeIfPossible(Action action)
        {
            if (action == null || IsDisposed)
            {
                return;
            }

            if (IsHandleCreated)
            {
                BeginInvoke(action);
                return;
            }

            action();
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

            if (MatchesQuickCommand(normalized, "tro ly ai", "tro ly", "chat bot", "chatbot", "bot", "ai"))
            {
                LoadChatBotPage();
                return true;
            }

            if (MatchesQuickCommand(normalized, "hoa don", "invoice"))
            {
                LoadPageFromQuickSearch(new frmInvoices());
                return true;
            }

            if (MatchesQuickCommand(normalized, "khach hang", "hoi vien", "customer", "member"))
            {
                LoadPageFromQuickSearch(new frmCustomers());
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

            if (ContainsKeyword(normalized, "tro ly ai", "chat bot", "chatbot", "hoi bot", "bot"))
            {
                LoadChatBotPage(true);
                return true;
            }

            if (!SessionManager.IsStaff && ContainsKeyword(normalized, "nhan vien", "tai khoan", "so dien thoai", "user"))
            {
                LoadPage(new frmUsers(), true);
                return true;
            }

            if (ContainsKeyword(normalized, "khach hang", "hoi vien", "customer", "member"))
            {
                LoadPage(new frmCustomers(), true);
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

            if (_currentPage is frmCustomers)
            {
                LoadPage(new frmCustomers(), true);
                return;
            }

            if (_currentPage is frmChatBot)
            {
                LoadChatBotPage(true);
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
            if (page is frmCustomers)
            {
                return true;
            }

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

            if (page is frmCustomers)
            {
                return "Tìm theo tên khách hàng, số điện thoại hoặc hạng...";
            }

            if (page is frmChatBot)
            {
                return "Hỏi bot về doanh thu, tồn kho, hóa đơn hoặc POS...";
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
                pnlPageHost.Controls.Remove(_currentPage);
                _currentPage.Dispose();
            }

            _currentPage = page;

            page.TopLevel = false;
            page.FormBorderStyle = FormBorderStyle.None;
            page.Dock = DockStyle.Fill;
            page.BackColor = BG;

            pnlPageHost.SuspendLayout();
            pnlPageHost.Controls.Add(page);
            page.Show();
            page.BringToFront();
            pnlPageHost.ResumeLayout(true);
            pnlPageHost.PerformLayout();
            UpdateAiFloatButtonLayout();

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

            if (page is frmCustomers)
            {
                return "Customers";
            }

            if (page is frmChatBot)
            {
                return "ChatBot";
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
            _currentPage.Width = pnlPageHost.ClientSize.Width;
            _currentPage.Height = pnlPageHost.ClientSize.Height;
            _currentPage.PerformLayout();
            _currentPage.Refresh();
            UpdateAiFloatButtonLayout();
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

                case "Customers":
                    LoadPage(new frmCustomers());
                    break;

                case "ChatBot":
                    LoadChatBotPage();
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
                case "Customers":
                case "ChatBot":
                case "Products":
                case "ExpiredProducts":
                case "LowStocks":
                    return true;

                default:
                    return false;
            }
        }

        private class TopBarNotificationSummary
        {
            public int LowStockProducts { get; set; }

            public int ExpiredLots { get; set; }

            public int ExpiringSoonLots { get; set; }

            public int TodayInvoices { get; set; }

            public decimal TodayRevenue { get; set; }

            public int TotalAlertCount
            {
                get { return LowStockProducts + ExpiredLots + ExpiringSoonLots; }
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
                DisposeActiveTopBarMenu();
                _currentPage?.Dispose();
            }
            base.Dispose(disposing);
        }


        // code hiện tại...

        private class RobotAiButton : Button
        {
            private bool _hover;

            private static readonly Color PrimaryDark = Color.FromArgb(43, 122, 241);
            private static readonly Color PrimaryLight = Color.FromArgb(70, 160, 255);
            private static readonly Color Face = Color.FromArgb(245, 248, 255);
            private static readonly Color DarkArea = Color.FromArgb(20, 40, 70);
            private static readonly Color EyeGlow = Color.FromArgb(0, 230, 255);
            private static readonly Color Mouth = Color.FromArgb(20, 40, 70);
            private static readonly Color Online = Color.FromArgb(34, 197, 94);

            public RobotAiButton()
            {
                Text = string.Empty;
                FlatStyle = FlatStyle.Flat;
                BackColor = Color.Transparent;
                ForeColor = Color.White;
                TabStop = false;

                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseDownBackColor = Color.Transparent;
                FlatAppearance.MouseOverBackColor = Color.Transparent;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor,
                    true);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                _hover = true;
                Invalidate();
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _hover = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);

                // Không bo sát quá để tránh cắt mất glow.
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, Width, Height);
                    Region = new Region(path);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                e.Graphics.Clear(Parent?.BackColor ?? Color.White);

                DrawGlow(e.Graphics);
                DrawMainCircle(e.Graphics);
                DrawRobot(e.Graphics);
                DrawOnlineDot(e.Graphics);
            }

            private void DrawGlow(Graphics g)
            {
                Rectangle glowRect = new Rectangle(1, 1, Width - 2, Height - 2);

                using (GraphicsPath glowPath = new GraphicsPath())
                {
                    glowPath.AddEllipse(glowRect);

                    using (PathGradientBrush glowBrush = new PathGradientBrush(glowPath))
                    {
                        glowBrush.CenterColor = Color.FromArgb(_hover ? 95 : 65, 70, 160, 255);
                        glowBrush.SurroundColors = new[] { Color.FromArgb(0, 70, 160, 255) };
                        g.FillPath(glowBrush, glowPath);
                    }
                }
            }

            private void DrawMainCircle(Graphics g)
            {
                Rectangle circleRect = GetCircleRect();

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    circleRect,
                    _hover ? PrimaryLight : PrimaryDark,
                    _hover ? PrimaryDark : PrimaryLight,
                    LinearGradientMode.Vertical))
                {
                    g.FillEllipse(brush, circleRect);
                }

                using (Pen borderPen = new Pen(Color.FromArgb(110, Color.White), 1))
                {
                    g.DrawEllipse(borderPen, circleRect);
                }
            }

            private Rectangle GetCircleRect()
            {
                return new Rectangle(5, 5, Width - 11, Height - 11);
            }

            private void DrawRobot(Graphics g)
            {
                Rectangle bounds = GetCircleRect();

                float centerX = bounds.X + bounds.Width / 2f;

                float faceW = bounds.Width * 0.62f;
                float faceH = bounds.Height * 0.46f;
                float faceX = centerX - faceW / 2f;
                float faceY = bounds.Y + bounds.Height * 0.36f;

                RectangleF faceRect = new RectangleF(faceX, faceY, faceW, faceH);

                DrawAntenna(g, bounds, faceRect);
                DrawEars(g, faceRect);
                DrawFace(g, faceRect);
                DrawEyeArea(g, faceRect);
                DrawMouth(g, faceRect);
            }

            private void DrawAntenna(Graphics g, Rectangle bounds, RectangleF faceRect)
            {
                float centerX = bounds.X + bounds.Width / 2f;
                float startY = faceRect.Y - 2;
                float topY = bounds.Y + bounds.Height * 0.20f;

                using (Pen pen = new Pen(Face, 2f))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    g.DrawLine(pen, centerX, startY, centerX, topY + 4);
                    g.DrawLine(pen, centerX, topY + 4, centerX - 5, topY);
                    g.DrawLine(pen, centerX, topY + 4, centerX + 5, topY);
                }

                using (SolidBrush brush = new SolidBrush(Face))
                {
                    g.FillEllipse(brush, centerX - 2.5f, topY - 2.5f, 5, 5);
                }
            }

            private void DrawEars(Graphics g, RectangleF faceRect)
            {
                float earW = faceRect.Width * 0.16f;
                float earH = faceRect.Height * 0.42f;
                float earY = faceRect.Y + faceRect.Height * 0.34f;

                using (SolidBrush brush = new SolidBrush(Face))
                {
                    g.FillEllipse(brush, faceRect.X - earW * 0.55f, earY, earW, earH);
                    g.FillEllipse(brush, faceRect.Right - earW * 0.45f, earY, earW, earH);
                }
            }

            private void DrawFace(Graphics g, RectangleF faceRect)
            {
                using (GraphicsPath facePath = RoundedRectF(faceRect, faceRect.Height / 2f))
                using (SolidBrush faceBrush = new SolidBrush(Face))
                using (Pen facePen = new Pen(Color.FromArgb(210, 225, 245), 1))
                {
                    g.FillPath(faceBrush, facePath);
                    g.DrawPath(facePen, facePath);
                }
            }

            private void DrawEyeArea(Graphics g, RectangleF faceRect)
            {
                float darkW = faceRect.Width * 0.78f;
                float darkH = faceRect.Height * 0.50f;
                float darkX = faceRect.X + (faceRect.Width - darkW) / 2f;
                float darkY = faceRect.Y + faceRect.Height * 0.23f;

                RectangleF darkRect = new RectangleF(darkX, darkY, darkW, darkH);

                using (GraphicsPath darkPath = RoundedRectF(darkRect, darkH / 2f))
                using (SolidBrush darkBrush = new SolidBrush(DarkArea))
                {
                    g.FillPath(darkBrush, darkPath);
                }

                DrawEyes(g, darkRect);
            }

            private void DrawEyes(Graphics g, RectangleF darkRect)
            {
                float eyeW = darkRect.Width * 0.18f;
                float eyeH = darkRect.Height * 0.46f;
                float eyeY = darkRect.Y + (darkRect.Height - eyeH) / 2f;

                RectangleF leftEye = new RectangleF(
                    darkRect.X + darkRect.Width * 0.23f,
                    eyeY,
                    eyeW,
                    eyeH);

                RectangleF rightEye = new RectangleF(
                    darkRect.Right - darkRect.Width * 0.23f - eyeW,
                    eyeY,
                    eyeW,
                    eyeH);

                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(70, EyeGlow)))
                using (SolidBrush eyeBrush = new SolidBrush(EyeGlow))
                {
                    g.FillEllipse(glowBrush, leftEye.X - 2, leftEye.Y - 2, leftEye.Width + 4, leftEye.Height + 4);
                    g.FillEllipse(glowBrush, rightEye.X - 2, rightEye.Y - 2, rightEye.Width + 4, rightEye.Height + 4);

                    g.FillEllipse(eyeBrush, leftEye);
                    g.FillEllipse(eyeBrush, rightEye);
                }
            }

            private void DrawMouth(Graphics g, RectangleF faceRect)
            {
                float mouthW = faceRect.Width * 0.18f;
                float mouthY = faceRect.Y + faceRect.Height * 0.76f;
                float mouthX1 = faceRect.X + (faceRect.Width - mouthW) / 2f;
                float mouthX2 = mouthX1 + mouthW;

                using (Pen mouthPen = new Pen(Mouth, 1.5f))
                {
                    mouthPen.StartCap = LineCap.Round;
                    mouthPen.EndCap = LineCap.Round;
                    g.DrawLine(mouthPen, mouthX1, mouthY, mouthX2, mouthY);
                }
            }

            private void DrawOnlineDot(Graphics g)
            {
                Rectangle circleRect = GetCircleRect();

                int outerSize = Math.Max(9, Width / 5);
                int innerSize = outerSize - 4;

                int outerX = circleRect.Right - outerSize + 2;
                int outerY = circleRect.Bottom - outerSize + 2;

                using (SolidBrush whiteBrush = new SolidBrush(Color.White))
                using (SolidBrush greenBrush = new SolidBrush(Online))
                {
                    g.FillEllipse(whiteBrush, outerX, outerY, outerSize, outerSize);
                    g.FillEllipse(greenBrush, outerX + 2, outerY + 2, innerSize, innerSize);
                }
            }

            private static GraphicsPath RoundedRectF(RectangleF rect, float radius)
            {
                float d = radius * 2f;

                if (d > rect.Width)
                {
                    d = rect.Width;
                }

                if (d > rect.Height)
                {
                    d = rect.Height;
                }

                GraphicsPath path = new GraphicsPath();

                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                return path;
            }
        }

    }
}
