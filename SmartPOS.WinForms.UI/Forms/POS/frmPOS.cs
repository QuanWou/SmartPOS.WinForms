using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Forms.Customers;
using SmartPOS.WinForms.UI.Forms.Invoices;
using SmartPOS.WinForms.UI.Forms.Shared;
using SmartPOS.WinForms.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.POS
{
    public class frmPOS : Form, IGlobalSearchHandler
    {
        private static readonly Color SurfaceColor = Color.White;
        private static readonly Color PageColor = Color.FromArgb(248, 249, 251);
        private static readonly Color FieldColor = Color.FromArgb(245, 247, 252);
        private static readonly Color BorderColor = Color.FromArgb(232, 235, 244);
        private static readonly Color PrimaryDark = Color.FromArgb(22, 32, 72);
        private static readonly Color PrimaryMid = Color.FromArgb(90, 110, 200);
        private static readonly Color TextMain = Color.FromArgb(14, 18, 38);
        private static readonly Color TextSoft = Color.FromArgb(120, 132, 160);
        private static readonly Color CardTint = Color.FromArgb(249, 251, 255);
        private static readonly Color DangerColor = Color.FromArgb(227, 88, 88);
        private static readonly Color CartCardColor = Color.White;
        private static readonly Color CartBorderSoft = Color.FromArgb(236, 239, 246);
        private static readonly Color QtyBg = Color.FromArgb(245, 247, 252);
        private static readonly Color QtyCenterBg = Color.White;
        private static readonly Color DeleteBg = Color.FromArgb(252, 243, 243);
        private static readonly Color DeleteText = Color.FromArgb(220, 98, 98);
        private static readonly Color TabActiveColor = PrimaryMid;
        private static readonly Color TabInactiveColor = TextSoft;

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;

        private const int ProductCardWidth = 168;
        private const int ProductCardHeight = 118;
        private const int ProductCardMargin = 8;
        private const int SearchButtonSize = 34;
        private const int SearchButtonGap = 8;
        private const int SearchBarPadding = 12;
        private const int CartPadding = 12;
        private const int EM_SETCUEBANNER = 0x1501;

        private Panel pnlCategoryTabs;
        private Panel pnlSearchBar;
        private Panel pnlSearchInput;
        private Panel pnlContent;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlCartHeader;
        private Panel pnlCartBody;
        private Panel pnlCartFooter;
        private Panel pnlProducts;

        private FlowLayoutPanel flpCategoryTabs;
        private FlowLayoutPanel flpProducts;
        private FlowLayoutPanel flpCartItems;

        private TextBox txtSearch;
        private Button btnScan;
        private Button btnCameraScan;
        private Button btnPhoneScan;

        private Label lblCartTitle;
        private Label lblTongMon;
        private Button btnCartDelete;
        private Button btnCartMore;

        private Label lblCustomerDisplayName;
        private Label lblCustomerPoints;
        private Label lblRedeemPoints;
        private NumericUpDown nudRedeemPoints;
        private Label lblTamTinhValue;
        private Label lblGiamGiaDiemValue;
        private Label lblTongTien;
        private Label lblTongTienValue;
        private Button btnThanhToan;
        private Button btnClearCustomer;
        private Button btnSelectCustomer;

        private List<ProductDTO> _products;
        private List<CategoryDTO> _categories;
        private Dictionary<int, string> _categoryNameById;
        private Dictionary<int, int> _categoryOrderById;
        private List<CartItem> _cartItems;
        private CustomerDTO _selectedCustomer;
        private int _selectedCategoryId = -1;

        public frmPOS()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            _invoiceService = new InvoiceService();
            _customerService = new CustomerService();
            _products = new List<ProductDTO>();
            _categories = new List<CategoryDTO>();
            _categoryNameById = new Dictionary<int, string>();
            _categoryOrderById = new Dictionary<int, int>();
            _cartItems = new List<CartItem>();

            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private void InitializeComponent()
        {
            this.Text = "Bán hàng";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = PageColor;
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;
            this.KeyPreview = true;

            BuildLayout();

            this.Load += FrmPOS_Load;
            this.FormClosed += FrmPOS_FormClosed;
            this.KeyDown += FrmPOS_KeyDown;
        }

        private void FrmPOS_Load(object sender, EventArgs e)
        {
            PhoneScanBridgeHub.InvoiceCreated += PhoneScanBridgeHub_InvoiceCreated;
            LoadProducts();
            UpdateSelectedCustomerView();
            RefreshCartView();
        }

        private void FrmPOS_FormClosed(object sender, FormClosedEventArgs e)
        {
            PhoneScanBridgeHub.InvoiceCreated -= PhoneScanBridgeHub_InvoiceCreated;
        }

        private void PhoneScanBridgeHub_InvoiceCreated(int invoiceId)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            this.BeginInvoke(new Action(() =>
            {
                LoadProducts();
                RefreshCartView();
                MessageBox.Show("Đã nhận hóa đơn #" + invoiceId + " từ app điện thoại.", "SmartPOS");
            }));
        }

        private void FrmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                e.SuppressKeyPress = true;
                BtnThanhToan_Click(this, EventArgs.Empty);
                return;
            }

            if (e.KeyCode == Keys.F2)
            {
                e.SuppressKeyPress = true;
                txtSearch.Focus();
                txtSearch.SelectAll();
            }
        }

        private void BuildLayout()
        {
            pnlCategoryTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = SurfaceColor,
                Padding = new Padding(12, 8, 12, 8),
                BorderStyle = BorderStyle.None
            };
            pnlCategoryTabs.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.DrawLine(pen, 0, pnlCategoryTabs.Height - 1, pnlCategoryTabs.Width, pnlCategoryTabs.Height - 1);
                }
            };

            flpCategoryTabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = true,
                WrapContents = false,
                BackColor = SurfaceColor,
                Padding = new Padding(0)
            };
            pnlCategoryTabs.Controls.Add(flpCategoryTabs);

            pnlSearchBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = SurfaceColor,
                Padding = new Padding(12, 8, 12, 8),
                BorderStyle = BorderStyle.None
            };
            pnlSearchBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.DrawLine(pen, 0, pnlSearchBar.Height - 1, pnlSearchBar.Width, pnlSearchBar.Height - 1);
                }
            };

            pnlSearchInput = new Panel
            {
                Location = new Point(12, 8),
                Size = new Size(800, 34),
                BackColor = FieldColor,
                BorderStyle = BorderStyle.None
            };
            pnlSearchInput.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlSearchInput.Width - 1, pnlSearchInput.Height - 1);
                }
            };

            txtSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(10, 7),
                Width = 750,
                BackColor = FieldColor,
                ForeColor = TextMain,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            SetTextBoxCueBanner(txtSearch, "Tìm sản phẩm (tên, mã vạch, SKU...)");
            txtSearch.KeyDown += TxtSearch_KeyDown;
            pnlSearchInput.Controls.Add(txtSearch);

            btnScan = CreateSearchButton("🔍", BtnScan_Click);
            btnCameraScan = CreateSearchButton("📷", BtnCameraScan_Click);
            btnPhoneScan = CreateSearchButton("📱", BtnPhoneScan_Click);

            pnlSearchBar.Controls.Add(pnlSearchInput);
            pnlSearchBar.Controls.Add(btnScan);
            pnlSearchBar.Controls.Add(btnCameraScan);
            pnlSearchBar.Controls.Add(btnPhoneScan);
            pnlSearchBar.Resize += (s, e) => LayoutSearchBar();
            LayoutSearchBar();

            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageColor,
                Padding = new Padding(0)
            };

            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageColor,
                Padding = new Padding(15, 15, 8, 15)
            };

            pnlProducts = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(12)
            };

            flpProducts = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0),
                BackColor = SurfaceColor
            };

            pnlProducts.Controls.Add(flpProducts);
            pnlLeft.Controls.Add(pnlProducts);

            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 360,
                BackColor = PageColor,
                Padding = new Padding(8, 15, 15, 15)
            };

            BuildCartSection();

            pnlContent.Controls.Add(pnlLeft);
            pnlContent.Controls.Add(pnlRight);

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlSearchBar);
            this.Controls.Add(pnlCategoryTabs);
        }

        private Button CreateSearchButton(string text, EventHandler clickHandler)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(SearchButtonSize, SearchButtonSize),
                BackColor = FieldColor,
                ForeColor = TextSoft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += clickHandler;
            return button;
        }

        private void BuildCartSection()
        {
            pnlCartHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = SurfaceColor,
                Padding = new Padding(12, 8, 12, 8)
            };

            lblCartTitle = new Label
            {
                Text = "Giỏ hàng",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = true,
                Location = new Point(12, 10)
            };

            lblTongMon = new Label
            {
                Text = "0 sản phẩm",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(190, 12)
            };

            btnCartDelete = new Button
            {
                Text = "🗑",
                Size = new Size(30, 30),
                BackColor = FieldColor,
                ForeColor = TextSoft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F),
                Cursor = Cursors.Hand
            };
            btnCartDelete.FlatAppearance.BorderSize = 0;
            btnCartDelete.Click += BtnCartDelete_Click;

            btnCartMore = new Button
            {
                Text = "⋯",
                Size = new Size(30, 30),
                BackColor = FieldColor,
                ForeColor = TextSoft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            btnCartMore.FlatAppearance.BorderSize = 0;
            btnCartMore.Click += BtnCartMore_Click;

            pnlCartHeader.Controls.Add(lblCartTitle);
            pnlCartHeader.Controls.Add(lblTongMon);
            pnlCartHeader.Controls.Add(btnCartDelete);
            pnlCartHeader.Controls.Add(btnCartMore);
            pnlCartHeader.Resize += (s, e) => LayoutCartHeader();
            LayoutCartHeader();

            pnlCartBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(8)
            };

            flpCartItems = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = SurfaceColor,
                Padding = new Padding(0)
            };

            pnlCartBody.Controls.Add(flpCartItems);

            pnlCartFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 280,
                BackColor = SurfaceColor,
                Padding = new Padding(12, 10, 12, 12)
            };

            Label lblCustomerSection = new Label
            {
                Text = "Khách hàng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 8)
            };

            lblCustomerDisplayName = new Label
            {
                Text = "Khách lẻ",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                Size = new Size(160, 22),
                Location = new Point(12, 28)
            };

            btnSelectCustomer = new Button
            {
                Text = "Chọn",
                Size = new Size(50, 24),
                BackColor = FieldColor,
                ForeColor = PrimaryDark,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F)
            };
            btnSelectCustomer.FlatAppearance.BorderSize = 0;
            btnSelectCustomer.Click += BtnSelectCustomer_Click;

            btnClearCustomer = new Button
            {
                Text = "Xóa",
                Size = new Size(40, 24),
                BackColor = FieldColor,
                ForeColor = DangerColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F)
            };
            btnClearCustomer.FlatAppearance.BorderSize = 0;
            btnClearCustomer.Click += BtnClearCustomer_Click;

            lblCustomerPoints = new Label
            {
                Text = "Điểm: 0",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = false,
                Size = new Size(150, 18),
                Location = new Point(12, 48)
            };

            lblRedeemPoints = new Label
            {
                Text = "Đổi điểm",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 68)
            };

            nudRedeemPoints = new NumericUpDown
            {
                Location = new Point(70, 64),
                Size = new Size(70, 22),
                Minimum = 0,
                Maximum = 0,
                ThousandsSeparator = true,
                Enabled = false,
                Font = new Font("Segoe UI", 8.5F)
            };
            nudRedeemPoints.ValueChanged += (s, e) => RefreshCartSummary();

            Label lblTamTinhLabel = new Label
            {
                Text = "Tạm tính",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 90)
            };

            lblTamTinhValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(140, 18),
                Location = new Point(190, 90)
            };

            Label lblGiamGiaDiemLabel = new Label
            {
                Text = "Giảm từ điểm",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 110)
            };

            lblGiamGiaDiemValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 160, 100),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(140, 18),
                Location = new Point(190, 110)
            };

            lblTongTien = new Label
            {
                Text = "Thanh tiền",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 132)
            };

            lblTongTienValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(318, 34),
                Location = new Point(12, 148)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh toán (F1)",
                Size = new Size(330, 36),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.Click += BtnThanhToan_Click;

            pnlCartFooter.Controls.Add(lblCustomerSection);
            pnlCartFooter.Controls.Add(lblCustomerDisplayName);
            pnlCartFooter.Controls.Add(btnSelectCustomer);
            pnlCartFooter.Controls.Add(btnClearCustomer);
            pnlCartFooter.Controls.Add(lblCustomerPoints);
            pnlCartFooter.Controls.Add(lblRedeemPoints);
            pnlCartFooter.Controls.Add(nudRedeemPoints);
            pnlCartFooter.Controls.Add(lblTamTinhLabel);
            pnlCartFooter.Controls.Add(lblTamTinhValue);
            pnlCartFooter.Controls.Add(lblGiamGiaDiemLabel);
            pnlCartFooter.Controls.Add(lblGiamGiaDiemValue);
            pnlCartFooter.Controls.Add(lblTongTien);
            pnlCartFooter.Controls.Add(lblTongTienValue);
            pnlCartFooter.Controls.Add(btnThanhToan);
            pnlCartFooter.Resize += (s, e) => LayoutCartFooter();
            LayoutCartFooter();

            pnlRight.Controls.Add(pnlCartBody);
            pnlRight.Controls.Add(pnlCartFooter);
            pnlRight.Controls.Add(pnlCartHeader);
        }

        private void LayoutSearchBar()
        {
            if (pnlSearchBar == null || pnlSearchInput == null || txtSearch == null)
            {
                return;
            }

            int contentWidth = Math.Max(0, pnlSearchBar.ClientSize.Width - SearchBarPadding * 2);
            int buttonsWidth = SearchButtonSize * 3 + SearchButtonGap * 2;
            int searchWidth = Math.Max(120, contentWidth - buttonsWidth - SearchButtonGap);
            int left = SearchBarPadding;
            int top = 8;
            int buttonLeft = left + searchWidth + SearchButtonGap;

            pnlSearchInput.SetBounds(left, top, searchWidth, SearchButtonSize);
            txtSearch.Width = Math.Max(60, pnlSearchInput.ClientSize.Width - 20);
            btnScan.Location = new Point(buttonLeft, top);
            btnCameraScan.Location = new Point(btnScan.Right + SearchButtonGap, top);
            btnPhoneScan.Location = new Point(btnCameraScan.Right + SearchButtonGap, top);
        }

        private void LayoutCartHeader()
        {
            if (pnlCartHeader == null || lblTongMon == null || btnCartDelete == null || btnCartMore == null)
            {
                return;
            }

            int right = Math.Max(CartPadding + 120, pnlCartHeader.ClientSize.Width - CartPadding);
            btnCartMore.Location = new Point(right - btnCartMore.Width, 8);
            btnCartDelete.Location = new Point(btnCartMore.Left - btnCartDelete.Width - 6, 8);
            lblTongMon.Location = new Point(
                Math.Max(lblCartTitle.Right + 8, btnCartDelete.Left - lblTongMon.Width - 8),
                12);
        }

        private void LayoutCartFooter()
        {
            if (pnlCartFooter == null)
            {
                return;
            }

            int left = CartPadding;
            int right = Math.Max(left + 180, pnlCartFooter.ClientSize.Width - CartPadding);

            if (btnClearCustomer != null && btnSelectCustomer != null)
            {
                btnClearCustomer.Location = new Point(right - btnClearCustomer.Width, 28);
                btnSelectCustomer.Location = new Point(btnClearCustomer.Left - btnSelectCustomer.Width - 6, 28);
            }

            if (lblCustomerDisplayName != null)
            {
                int nameRight = btnSelectCustomer != null ? btnSelectCustomer.Left - 8 : right;
                lblCustomerDisplayName.Size = new Size(Math.Max(100, nameRight - left), 22);
            }

            if (lblCustomerPoints != null)
            {
                lblCustomerPoints.Size = new Size(Math.Max(120, right - left), 18);
            }

            int valueWidth = Math.Min(170, Math.Max(120, right - left - 110));
            int valueLeft = right - valueWidth;
            if (lblTamTinhValue != null)
            {
                lblTamTinhValue.SetBounds(valueLeft, 90, valueWidth, 18);
            }

            if (lblGiamGiaDiemValue != null)
            {
                lblGiamGiaDiemValue.SetBounds(valueLeft, 110, valueWidth, 18);
            }

            if (lblTongTienValue != null)
            {
                lblTongTienValue.SetBounds(left, 148, right - left, 34);
            }

            if (btnThanhToan != null)
            {
                btnThanhToan.SetBounds(left, pnlCartFooter.ClientSize.Height - btnThanhToan.Height - CartPadding, right - left, btnThanhToan.Height);
            }
        }

        private int GetCartItemCardWidth()
        {
            if (flpCartItems == null || flpCartItems.ClientSize.Width <= 0)
            {
                return 305;
            }

            return Math.Max(280, flpCartItems.ClientSize.Width - flpCartItems.Padding.Horizontal - 2);
        }

        private static void SetTextBoxCueBanner(TextBox textBox, string cueBanner)
        {
            if (textBox == null || string.IsNullOrWhiteSpace(cueBanner))
            {
                return;
            }

            if (textBox.IsHandleCreated)
            {
                SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, cueBanner);
                return;
            }

            textBox.HandleCreated += (s, e) => SendMessage(textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, cueBanner);
        }

        private void LoadProducts()
        {
            LoadCategories();
            BuildCategoryTabs();

            _products = (_productService.GetAll() ?? new List<ProductDTO>())
                .Where(x => x.TrangThai && !IsExpiredProduct(x))
                .ToList();

            _selectedCategoryId = -1;
            RenderProducts(_products);
        }

        private void LoadCategories()
        {
            _categories = (_categoryService.GetAll() ?? new List<CategoryDTO>())
                .OrderBy(x => x.TenLoai)
                .ToList();

            _categoryNameById = _categories
                .GroupBy(x => x.MaLoai)
                .ToDictionary(x => x.Key, x => x.First().TenLoai);

            _categoryOrderById = _categories
                .Select((category, index) => new { category.MaLoai, Index = index })
                .GroupBy(x => x.MaLoai)
                .ToDictionary(x => x.Key, x => x.First().Index);
        }

        private void BuildCategoryTabs()
        {
            flpCategoryTabs.Controls.Clear();

            flpCategoryTabs.Controls.Add(CreateCategoryTab("Tất cả", -1, true));

            foreach (CategoryDTO category in _categories.OrderBy(x => GetCategorySortIndex(x.MaLoai)).ThenBy(x => x.TenLoai))
            {
                flpCategoryTabs.Controls.Add(CreateCategoryTab(category.TenLoai, category.MaLoai, false));
            }
        }

        private Button CreateCategoryTab(string text, int categoryId, bool isActive)
        {
            Button button = new Button
            {
                Text = text,
                AutoSize = true,
                Height = 34,
                MinimumSize = new Size(70, 34),
                BackColor = isActive ? TabActiveColor : SurfaceColor,
                ForeColor = isActive ? Color.White : TabInactiveColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Margin = new Padding(4, 0, 4, 0),
                Cursor = Cursors.Hand,
                Tag = categoryId
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (s, e) =>
            {
                _selectedCategoryId = categoryId;
                UpdateCategoryTabsUI();
                FilterAndRenderProducts();
            };
            return button;
        }

        private void UpdateCategoryTabsUI()
        {
            foreach (Button button in flpCategoryTabs.Controls.OfType<Button>())
            {
                int categoryId = (int)button.Tag;
                bool isActive = categoryId == _selectedCategoryId;
                button.BackColor = isActive ? TabActiveColor : SurfaceColor;
                button.ForeColor = isActive ? Color.White : TabInactiveColor;
            }
        }

        private void FilterAndRenderProducts()
        {
            List<ProductDTO> products = _products ?? new List<ProductDTO>();
            List<ProductDTO> filtered = _selectedCategoryId == -1
                ? products
                : products.Where(x => x.MaLoai == _selectedCategoryId).ToList();

            RenderProducts(filtered);
        }

        private void RenderProducts(List<ProductDTO> products)
        {
            List<ProductDTO> visibleProducts = products ?? new List<ProductDTO>();

            flpProducts.SuspendLayout();
            flpProducts.Controls.Clear();

            if (visibleProducts.Count == 0)
            {
                flpProducts.Controls.Add(new Label
                {
                    Text = "Không tìm thấy sản phẩm phù hợp.",
                    Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                    ForeColor = TextSoft,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Size = new Size(700, 100),
                    Margin = new Padding(0, 20, 0, 0)
                });
                flpProducts.ResumeLayout();
                return;
            }

            foreach (ProductDTO product in visibleProducts)
            {
                flpProducts.Controls.Add(BuildProductCard(product));
            }

            flpProducts.ResumeLayout();
        }

        private Control BuildProductCard(ProductDTO product)
        {
            Panel card = new Panel
            {
                Size = new Size(ProductCardWidth, ProductCardHeight),
                Margin = new Padding(ProductCardMargin),
                BackColor = CardTint,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };
            card.Paint += ProductCard_Paint;

            Label lblTen = new Label
            {
                Text = product.TenSP,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                Size = new Size(ProductCardWidth - 24, 34),
                Location = new Point(12, 12)
            };

            Label lblMa = new Label
            {
                Text = "Mã: " + product.MaVach,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 52)
            };

            Label lblGia = new Label
            {
                Text = product.GiaBan.ToString("N0") + " đ",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = PrimaryMid,
                AutoSize = true,
                Location = new Point(12, 78)
            };

            Button btnThem = new Button
            {
                Text = "Thêm",
                Size = new Size(56, 26),
                Location = new Point(ProductCardWidth - 68, 76),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += (s, e) => AddToCart(product);

            card.Controls.Add(lblTen);
            card.Controls.Add(lblMa);
            card.Controls.Add(lblGia);
            card.Controls.Add(btnThem);

            return card;
        }

        private void AddToCart(ProductDTO product)
        {
            if (product == null) return;

            if (product.SoLuongTon <= 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng.", "Thông báo");
                return;
            }

            if (IsExpiredProduct(product))
            {
                MessageBox.Show("Sản phẩm đã hết hạn sử dụng.", "Thông báo");
                return;
            }

            CartItem existing = _cartItems.FirstOrDefault(x => x.MaSP == product.MaSP);
            int currentQuantity = existing != null ? existing.SoLuong : 0;

            if (currentQuantity + 1 > product.SoLuongTon)
            {
                MessageBox.Show("Số lượng vượt quá tồn kho.", "Thông báo");
                return;
            }

            if (existing == null)
            {
                _cartItems.Add(new CartItem
                {
                    MaSP = product.MaSP,
                    TenSP = product.TenSP,
                    SoLuong = 1,
                    DonGia = product.GiaBan
                });
            }
            else
            {
                existing.SoLuong += 1;
            }

            RefreshCartView();
        }

        private void RefreshCartView()
        {
            RenderCartItems();

            int tongMon = _cartItems.Sum(x => x.SoLuong);
            lblTongMon.Text = tongMon + " sản phẩm";
            LayoutCartHeader();
            RefreshCartSummary();
        }

        private void RefreshCartSummary()
        {
            decimal tamTinh = GetCartSubtotal();
            int maxRedeemPoints = GetMaxRedeemPoints(tamTinh);

            if (nudRedeemPoints != null)
            {
                if (nudRedeemPoints.Value > maxRedeemPoints)
                {
                    nudRedeemPoints.Value = maxRedeemPoints;
                }

                if (nudRedeemPoints.Maximum != maxRedeemPoints)
                {
                    nudRedeemPoints.Maximum = maxRedeemPoints;
                }

                nudRedeemPoints.Enabled = _selectedCustomer != null && maxRedeemPoints > 0;
            }

            decimal discount = GetPointDiscount();
            decimal payable = Math.Max(0, tamTinh - discount);

            lblTamTinhValue.Text = tamTinh.ToString("N0") + " đ";
            lblGiamGiaDiemValue.Text = discount.ToString("N0") + " đ";
            lblTongTienValue.Text = payable.ToString("N0") + " đ";
        }

        private decimal GetCartSubtotal() => _cartItems.Sum(x => x.SoLuong * x.DonGia);

        private decimal GetPointDiscount()
        {
            if (nudRedeemPoints == null || _selectedCustomer == null) return 0;
            return _customerService.CalculateRedeemValue((int)nudRedeemPoints.Value);
        }

        private decimal GetPayableTotal() => Math.Max(0, GetCartSubtotal() - GetPointDiscount());

        private int GetMaxRedeemPoints(decimal subtotal)
        {
            if (_selectedCustomer == null || subtotal <= 0) return 0;
            int maxByOrder = (int)Math.Floor(subtotal / LoyaltyConstants.RedeemValuePerPoint);
            return Math.Max(0, Math.Min(_selectedCustomer.DiemHienCo, maxByOrder));
        }

        private void RenderCartItems()
        {
            flpCartItems.SuspendLayout();
            flpCartItems.Controls.Clear();

            if (_cartItems.Count == 0)
            {
                flpCartItems.Controls.Add(new Label
                {
                    Text = "Giỏ hàng đang trống",
                    ForeColor = TextSoft,
                    Font = new Font("Segoe UI", 9F),
                    Size = new Size(300, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 10, 0, 0)
                });
                flpCartItems.ResumeLayout();
                return;
            }

            foreach (CartItem item in _cartItems)
            {
                flpCartItems.Controls.Add(BuildCartItemCard(item));
            }

            flpCartItems.ResumeLayout();
        }

        private Control BuildCartItemCard(CartItem item)
        {
            decimal thanhTien = item.SoLuong * item.DonGia;
            bool canIncrease = CanIncreaseCartItemQuantity(item);
            int cardWidth = GetCartItemCardWidth();

            Panel card = new Panel
            {
                Size = new Size(cardWidth, 85),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = CartCardColor,
                BorderStyle = BorderStyle.None
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(CartBorderSoft))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            Label lblTen = new Label
            {
                Text = item.TenSP,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = TextMain,
                AutoSize = false,
                Size = new Size(Math.Max(140, cardWidth - 58), 20),
                Location = new Point(12, 10)
            };

            Button btnXoa = new Button
            {
                Text = "×",
                Size = new Size(24, 24),
                Location = new Point(cardWidth - 31, 8),
                BackColor = DeleteBg,
                ForeColor = DeleteText,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += (s, e) => RemoveCartItem(item.MaSP);

            Label lblGia = new Label
            {
                Text = item.DonGia.ToString("N0") + " đ x " + item.SoLuong,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 32)
            };

            Panel qtyWrap = new Panel
            {
                Size = new Size(90, 28),
                Location = new Point(12, 50),
                BackColor = QtyBg,
                BorderStyle = BorderStyle.None
            };
            qtyWrap.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(CartBorderSoft))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, qtyWrap.Width - 1, qtyWrap.Height - 1);
                }
            };

            Button btnGiam = new Button
            {
                Text = "−",
                Size = new Size(26, 28),
                Location = new Point(0, 0),
                BackColor = QtyBg,
                ForeColor = TextMain,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnGiam.FlatAppearance.BorderSize = 0;
            btnGiam.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, -1);

            Label lblSoLuong = new Label
            {
                Text = item.SoLuong.ToString(),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                BackColor = QtyCenterBg,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(38, 26),
                Location = new Point(26, 1)
            };
            lblSoLuong.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(CartBorderSoft))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, lblSoLuong.Width - 1, lblSoLuong.Height - 1);
                }
            };

            Button btnTang = new Button
            {
                Text = "+",
                Size = new Size(26, 28),
                Location = new Point(64, 0),
                BackColor = QtyBg,
                ForeColor = canIncrease ? TextMain : TextSoft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = canIncrease ? Cursors.Hand : Cursors.Default,
                TabStop = false,
                Enabled = canIncrease
            };
            btnTang.FlatAppearance.BorderSize = 0;
            btnTang.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, 1);

            qtyWrap.Controls.Add(btnGiam);
            qtyWrap.Controls.Add(lblSoLuong);
            qtyWrap.Controls.Add(btnTang);

            Label lblThanhTien = new Label
            {
                Text = thanhTien.ToString("N0") + " đ",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(Math.Max(120, cardWidth - 120), 22),
                Location = new Point(108, 54)
            };

            card.Controls.Add(lblTen);
            card.Controls.Add(btnXoa);
            card.Controls.Add(lblGia);
            card.Controls.Add(qtyWrap);
            card.Controls.Add(lblThanhTien);

            return card;
        }

        private void UpdateCartItemQuantity(int maSP, int delta)
        {
            CartItem item = _cartItems.FirstOrDefault(x => x.MaSP == maSP);
            if (item == null) return;

            if (delta > 0 && !CanIncreaseCartItemQuantity(item))
            {
                MessageBox.Show("Số lượng đã chạm mức tồn kho.", "Thông báo");
                return;
            }

            item.SoLuong += delta;
            if (item.SoLuong <= 0)
            {
                _cartItems.Remove(item);
            }

            RefreshCartView();
        }

        private void RemoveCartItem(int maSP)
        {
            CartItem item = _cartItems.FirstOrDefault(x => x.MaSP == maSP);
            if (item == null) return;

            _cartItems.Remove(item);
            RefreshCartView();
        }

        private bool CanIncreaseCartItemQuantity(CartItem item)
        {
            ProductDTO product = _products.FirstOrDefault(x => x.MaSP == item.MaSP);
            return product != null && item.SoLuong < product.SoLuongTon;
        }

        private bool IsExpiredProduct(ProductDTO product)
        {
            return product != null && product.HanSuDung.HasValue && product.HanSuDung.Value.Date < DateTime.Today;
        }

        private void SearchProducts()
        {
            string keyword = txtSearch.Text.Trim();
            List<ProductDTO> products = _products ?? new List<ProductDTO>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _selectedCategoryId = -1;
                UpdateCategoryTabsUI();
                RenderProducts(products);
                return;
            }

            ProductDTO barcodeMatchedProduct = products.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.MaVach) &&
                string.Equals(x.MaVach.Trim(), keyword, StringComparison.OrdinalIgnoreCase));

            if (barcodeMatchedProduct != null)
            {
                AddToCart(barcodeMatchedProduct);
                txtSearch.Clear();
                _selectedCategoryId = -1;
                UpdateCategoryTabsUI();
                RenderProducts(products);
                return;
            }

            List<ProductDTO> filtered = products.Where(x =>
                (!string.IsNullOrWhiteSpace(x.TenSP) && x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrWhiteSpace(x.MaVach) && x.MaVach.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                GetCategoryName(x.MaLoai).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            RenderProducts(filtered);
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtSearch.Text = keyword ?? string.Empty;
            SearchProducts();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text)) return;
            txtSearch.Clear();
            SearchProducts();
        }

        private string GetCategoryName(int maLoai)
        {
            string categoryName;
            if (_categoryNameById != null && _categoryNameById.TryGetValue(maLoai, out categoryName) && !string.IsNullOrWhiteSpace(categoryName))
            {
                return categoryName;
            }
            return "Chưa phân loại";
        }

        private int GetCategorySortIndex(int maLoai)
        {
            int sortIndex;
            if (_categoryOrderById != null && _categoryOrderById.TryGetValue(maLoai, out sortIndex))
            {
                return sortIndex;
            }
            return int.MaxValue;
        }

        private void BtnScan_Click(object sender, EventArgs e) => SearchProducts();

        private void BtnCameraScan_Click(object sender, EventArgs e)
        {
            using (frmCameraScanner frm = new frmCameraScanner("Quét mã bán hàng", "Đưa mã vạch sản phẩm vào giữa khung hình."))
            {
                if (frm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(frm.ScannedCode))
                {
                    txtSearch.Text = frm.ScannedCode;
                    SearchProducts();
                }
            }
        }

        private void BtnPhoneScan_Click(object sender, EventArgs e)
        {
            using (frmPhoneScannerBridge frm = new frmPhoneScannerBridge(
                "Quét bằng điện thoại",
                "Mở app SmartPOS Scanner trên điện thoại, quét QR này để kết nối, sau đó quét barcode sản phẩm. Có thể quét nhiều mã liên tiếp cho đến khi bấm Đóng.",
                false))
            {
                frm.ScanReceived += PhoneScannerBridge_ScanReceived;
                try
                {
                    frm.ShowDialog(this);
                }
                finally
                {
                    frm.ScanReceived -= PhoneScannerBridge_ScanReceived;
                }
            }
        }

        private void PhoneScannerBridge_ScanReceived(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => PhoneScannerBridge_ScanReceived(code)));
                return;
            }

            txtSearch.Text = code.Trim();
            SearchProducts();
        }

        private void BtnCartDelete_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _cartItems.Clear();
            if (nudRedeemPoints != null) nudRedeemPoints.Value = 0;
            RefreshCartView();
        }

        private void BtnCartMore_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Làm mới sản phẩm", null, (s, args) =>
            {
                // Ensure UI actions run after menu is closed to avoid accessing disposed menu
                this.BeginInvoke(new Action(() =>
                {
                    txtSearch.Clear();
                    LoadProducts();
                    RefreshCartView();
                }));
            });
            // No explicit disposal; let GC handle it after menu is closed
            menu.Show(btnCartMore, new Point(0, btnCartMore.Height));
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchProducts();
                e.SuppressKeyPress = true;
            }
        }

        private void BtnSelectCustomer_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCustomerLookup())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && frm.SelectedCustomer != null)
                {
                    _selectedCustomer = frm.SelectedCustomer;
                    UpdateSelectedCustomerView();
                    RefreshCartSummary();
                }
            }
        }

        private void BtnClearCustomer_Click(object sender, EventArgs e)
        {
            _selectedCustomer = null;
            if (nudRedeemPoints != null) nudRedeemPoints.Value = 0;
            UpdateSelectedCustomerView();
            RefreshCartSummary();
        }

        private void UpdateSelectedCustomerView()
        {
            lblCustomerDisplayName.Text = _selectedCustomer != null ? _selectedCustomer.HoTen : "Khách lẻ";
            lblCustomerPoints.Text = _selectedCustomer != null
                ? "Điểm: " + _selectedCustomer.DiemHienCo.ToString("N0") + " | " + _selectedCustomer.HangThanhVien
                : "Điểm: 0";
            btnClearCustomer.Enabled = _selectedCustomer != null;
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.", "Thông báo");
                return;
            }

            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Không xác định được nhân viên đăng nhập.", "Thông báo");
                return;
            }

            int diemSuDung = nudRedeemPoints != null ? (int)nudRedeemPoints.Value : 0;
            decimal tongTien = GetPayableTotal();

            OperationResult pointValidation = _customerService.ValidatePointRedemption(
                _selectedCustomer != null ? (int?)_selectedCustomer.MaKH : null,
                diemSuDung,
                GetCartSubtotal());

            if (!pointValidation.IsSuccess)
            {
                MessageBox.Show(pointValidation.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethodLabel = tongTien <= 0 ? "Đổi điểm toàn bộ" : "Tiền mặt";

            if (tongTien > 0)
            {
                using (frmCashPayment frmCash = new frmCashPayment(tongTien))
                {
                    if (frmCash.ShowDialog(this) != DialogResult.OK || !frmCash.IsConfirmed)
                    {
                        return;
                    }
                    paymentMethodLabel = frmCash.PaymentMethodLabel;
                }
            }

            CheckoutRequest request = new CheckoutRequest
            {
                MaNV = SessionManager.CurrentUser.MaNV,
                MaKH = _selectedCustomer != null ? (int?)_selectedCustomer.MaKH : null,
                DiemSuDung = diemSuDung,
                GhiChu = "Bán tại quầy - " + paymentMethodLabel,
                ChiTietHoaDon = _cartItems.Select(x => new InvoiceDetailDTO
                {
                    MaSP = x.MaSP,
                    SoLuong = x.SoLuong,
                    DonGiaLucBan = x.DonGia,
                    ThanhTien = x.SoLuong * x.DonGia
                }).ToList()
            };

            OperationResult result = _invoiceService.Checkout(request);

            MessageBox.Show(result.Message, "Thông báo", MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                _cartItems.Clear();
                _selectedCustomer = null;
                if (nudRedeemPoints != null) nudRedeemPoints.Value = 0;
                UpdateSelectedCustomerView();
                RefreshCartView();
                LoadProducts();

                if (result.DataId.HasValue && result.DataId.Value > 0)
                {
                    if (MessageBox.Show("Thanh toán thành công. Bạn có muốn xem chi tiết hóa đơn không?",
                        "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (frmInvoiceDetails frm = new frmInvoiceDetails(result.DataId.Value))
                        {
                            frm.ShowDialog(this);
                        }
                    }
                }
            }
        }

        private void ProductCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            }
        }

        private class CartItem
        {
            public int MaSP { get; set; }
            public string TenSP { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
        }
    }
}
