using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
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
        private static readonly Color SoftDanger = Color.FromArgb(245, 247, 252);
        private static readonly Color SoftDangerText = Color.FromArgb(190, 96, 96);
        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private static readonly Color CartCardColor = Color.White;
        private static readonly Color CartBorderSoft = Color.FromArgb(236, 239, 246);
        private static readonly Color QtyBg = Color.FromArgb(245, 247, 252);
        private static readonly Color QtyCenterBg = Color.White;
        private static readonly Color DeleteBg = Color.FromArgb(252, 243, 243);
        private static readonly Color DeleteText = Color.FromArgb(220, 98, 98);
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlTop;
        private Panel pnlSearch;
        private Panel pnlProducts;
        private Panel pnlCartHeader;
        private Panel pnlCartBody;
        private Panel pnlCartFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnScan;
        private Button btnCameraScan;
        private Button btnPhoneScan;
        private Button btnThanhToan;
        private Button btnLamMoi;
        private Button btnSelectCustomer;
        private Button btnClearCustomer;

        private FlowLayoutPanel flpProducts;
        private FlowLayoutPanel flpCartItems;

        private Label lblCustomerTitle;
        private Label lblCustomerName;
        private Label lblCustomerPoints;
        private Label lblRedeemPoints;
        private NumericUpDown nudRedeemPoints;
        private Label lblTamTinh;
        private Label lblTamTinhValue;
        private Label lblGiamGiaDiem;
        private Label lblGiamGiaDiemValue;
        private Label lblTongMon;
        private Label lblTongTien;
        private Label lblTongTienValue;

        private List<ProductDTO> _products;
        private List<CartItem> _cartItems;
        private CustomerDTO _selectedCustomer;

        public frmPOS()
        {
            _productService = new ProductService();
            _invoiceService = new InvoiceService();
            _customerService = new CustomerService();
            _cartItems = new List<CartItem>();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "B\u00e1n h\u00e0ng";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = PageColor;
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;
            this.KeyPreview = true;

            BuildLayout();

            this.Load += FrmPOS_Load;
            this.KeyDown += FrmPOS_KeyDown;
        }

        private void FrmPOS_Load(object sender, EventArgs e)
        {
            LoadProducts();
            UpdateSelectedCustomerView();
            RefreshCartView();
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
            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageColor,
                Padding = new Padding(20, 20, 10, 20)
            };

            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 360,
                BackColor = PageColor,
                Padding = new Padding(10, 20, 20, 20)
            };

            BuildLeftPanel();
            BuildRightPanel();

            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlRight);
        }

        private void BuildLeftPanel()
        {
            pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = SurfaceColor,
                Padding = new Padding(18, 16, 18, 16)
            };

            lblTitle = new Label
            {
                Text = "B\u00e1n h\u00e0ng t\u1ea1i qu\u1ea7y",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = true,
                Location = new Point(18, 14)
            };

            lblSubtitle = new Label
            {
                Text = "T\u00ecm s\u1ea3n ph\u1ea9m theo t\u00ean ho\u1eb7c m\u00e3 v\u1ea1ch, ho\u1eb7c qu\u00e9t b\u1eb1ng camera \u0111\u1ec3 th\u00eam v\u00e0o gi\u1ecf h\u00e0ng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(18, 44)
            };

            pnlSearch = new Panel
            {
                Size = new Size(800, 40),
                Location = new Point(18, 60),
                BackColor = FieldColor
            };

            lblSearch = new Label
            {
                Text = "T\u00ecm ki\u1ebfm",
                AutoSize = true,
                Visible = false
            };

            txtSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(12, 11),
                Width = 300
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnScan = new Button
            {
                Text = "T\u00ecm",
                Size = new Size(72, 28),
                Location = new Point(362, 6),
                BackColor = PrimaryMid,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnScan.FlatAppearance.BorderSize = 0;
            btnScan.Click += BtnScan_Click;

            btnCameraScan = new Button
            {
                Text = "Qu\u00e9t cam",
                Size = new Size(96, 28),
                Location = new Point(440, 6),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCameraScan.FlatAppearance.BorderSize = 0;
            btnCameraScan.Click += BtnCameraScan_Click;

            btnPhoneScan = new Button
            {
                Text = "Qu\u00e9t \u0111t",
                Size = new Size(88, 28),
                Location = new Point(542, 6),
                BackColor = Color.FromArgb(49, 82, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPhoneScan.FlatAppearance.BorderSize = 0;
            btnPhoneScan.Click += BtnPhoneScan_Click;

            btnLamMoi = new Button
            {
                Text = "L\u00e0m m\u1edbi",
                Size = new Size(80, 28),
                Location = new Point(636, 6),
                BackColor = FieldColor,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnScan);
            pnlSearch.Controls.Add(btnCameraScan);
            pnlSearch.Controls.Add(btnPhoneScan);
            pnlSearch.Controls.Add(btnLamMoi);

            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(lblSubtitle);
            pnlTop.Controls.Add(pnlSearch);

            pnlProducts = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(14)
            };

            flpProducts = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = SurfaceColor
            };

            pnlProducts.Controls.Add(flpProducts);

            pnlLeft.Controls.Add(pnlProducts);
            pnlLeft.Controls.Add(pnlTop);
        }

        private void BuildRightPanel()
        {
            pnlCartHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = SurfaceColor,
                Padding = new Padding(16, 14, 16, 10)
            };

            Label lblCartTitle = new Label
            {
                Text = "Gi\u1ecf h\u00e0ng",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            lblTongMon = new Label
            {
                Text = "0 s\u1ea3n ph\u1ea9m",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(230, 18)
            };

            pnlCartHeader.Controls.Add(lblCartTitle);
            pnlCartHeader.Controls.Add(lblTongMon);

            pnlCartFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 284,
                BackColor = SurfaceColor,
                Padding = new Padding(16, 12, 16, 16)
            };

            lblCustomerTitle = new Label
            {
                Text = "Khách hàng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 12)
            };

            lblCustomerName = new Label
            {
                Text = "Khách lẻ",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                Size = new Size(150, 22),
                Location = new Point(16, 32)
            };

            lblCustomerPoints = new Label
            {
                Text = "Điểm: 0",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = false,
                Size = new Size(130, 20),
                Location = new Point(16, 54)
            };

            btnSelectCustomer = new Button
            {
                Text = "Chọn",
                Size = new Size(64, 28),
                Location = new Point(196, 28),
                BackColor = FieldColor,
                ForeColor = PrimaryDark,
                FlatStyle = FlatStyle.Flat
            };
            btnSelectCustomer.FlatAppearance.BorderSize = 0;
            btnSelectCustomer.Click += BtnSelectCustomer_Click;

            btnClearCustomer = new Button
            {
                Text = "Xóa",
                Size = new Size(50, 28),
                Location = new Point(266, 28),
                BackColor = FieldColor,
                ForeColor = DangerColor,
                FlatStyle = FlatStyle.Flat
            };
            btnClearCustomer.FlatAppearance.BorderSize = 0;
            btnClearCustomer.Click += BtnClearCustomer_Click;

            lblRedeemPoints = new Label
            {
                Text = "Đổi điểm",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 84)
            };

            nudRedeemPoints = new NumericUpDown
            {
                Location = new Point(92, 80),
                Size = new Size(80, 26),
                Minimum = 0,
                Maximum = 0,
                ThousandsSeparator = true,
                Enabled = false
            };
            nudRedeemPoints.ValueChanged += (s, e) => RefreshCartSummary();

            lblTamTinh = new Label
            {
                Text = "Tạm tính",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 118)
            };

            lblTamTinhValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(150, 22),
                Location = new Point(166, 114)
            };

            lblGiamGiaDiem = new Label
            {
                Text = "Giảm từ điểm",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 142)
            };

            lblGiamGiaDiemValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 160, 100),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(150, 22),
                Location = new Point(166, 138)
            };

            lblTongTien = new Label
            {
                Text = "Cần thanh toán",
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 166)
            };

            lblTongTienValue = new Label
            {
                Text = "0 \u0111",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = true,
                Location = new Point(16, 186)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh to\u00e1n",
                Size = new Size(310, 38),
                Location = new Point(16, 234),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.Click += BtnThanhToan_Click;

            pnlCartFooter.Controls.Add(lblCustomerTitle);
            pnlCartFooter.Controls.Add(lblCustomerName);
            pnlCartFooter.Controls.Add(lblCustomerPoints);
            pnlCartFooter.Controls.Add(btnSelectCustomer);
            pnlCartFooter.Controls.Add(btnClearCustomer);
            pnlCartFooter.Controls.Add(lblRedeemPoints);
            pnlCartFooter.Controls.Add(nudRedeemPoints);
            pnlCartFooter.Controls.Add(lblTamTinh);
            pnlCartFooter.Controls.Add(lblTamTinhValue);
            pnlCartFooter.Controls.Add(lblGiamGiaDiem);
            pnlCartFooter.Controls.Add(lblGiamGiaDiemValue);
            pnlCartFooter.Controls.Add(lblTongTien);
            pnlCartFooter.Controls.Add(lblTongTienValue);
            pnlCartFooter.Controls.Add(btnThanhToan);

            pnlCartBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(16, 12, 16, 12)
            };

            flpCartItems = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = SurfaceColor,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            pnlCartBody.Controls.Add(flpCartItems);

            pnlRight.Controls.Add(pnlCartBody);
            pnlRight.Controls.Add(pnlCartFooter);
            pnlRight.Controls.Add(pnlCartHeader);
        }

        private void LoadProducts()
        {
            _products = _productService.GetAll()
                .Where(x => x.TrangThai && !IsExpiredProduct(x))
                .ToList();

            RenderProducts(_products);
        }

        private void RenderProducts(List<ProductDTO> products)
        {
            flpProducts.Controls.Clear();

            foreach (ProductDTO product in products)
            {
                flpProducts.Controls.Add(BuildProductCard(product));
            }
        }

        private Control BuildProductCard(ProductDTO product)
        {
            Panel card = new Panel
            {
                Size = new Size(165, 130),
                Margin = new Padding(8),
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
                Size = new Size(135, 36),
                Location = new Point(12, 14)
            };

            Label lblMa = new Label
            {
                Text = "M\u00e3: " + product.MaVach,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(12, 58)
            };

            Label lblGia = new Label
            {
                Text = product.GiaBan.ToString("N0") + " \u0111",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = PrimaryMid,
                AutoSize = true,
                Location = new Point(12, 84)
            };

            Button btnThem = new Button
            {
                Text = "Th\u00eam",
                Size = new Size(56, 26),
                Location = new Point(95, 80),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 55, 100);
            btnThem.Click += (s, e) => AddToCart(product);

            card.Controls.Add(lblTen);
            card.Controls.Add(lblMa);
            card.Controls.Add(lblGia);
            card.Controls.Add(btnThem);

            return card;
        }

        private void AddToCart(ProductDTO product)
        {
            if (product == null)
            {
                return;
            }

            if (product.SoLuongTon <= 0)
            {
                MessageBox.Show("S\u1ea3n ph\u1ea9m \u0111\u00e3 h\u1ebft h\u00e0ng.", "Th\u00f4ng b\u00e1o");
                return;
            }

            if (IsExpiredProduct(product))
            {
                MessageBox.Show("S\u1ea3n ph\u1ea9m \u0111\u00e3 h\u1ebft h\u1ea1n s\u1eed d\u1ee5ng.", "Th\u00f4ng b\u00e1o");
                return;
            }

            CartItem existing = _cartItems.FirstOrDefault(x => x.MaSP == product.MaSP);
            int soLuongTrongGio = existing != null ? existing.SoLuong : 0;

            if (soLuongTrongGio + 1 > product.SoLuongTon)
            {
                MessageBox.Show("S\u1ed1 l\u01b0\u1ee3ng v\u01b0\u1ee3t qu\u00e1 t\u1ed3n kho.", "Th\u00f4ng b\u00e1o");
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
            lblTongMon.Text = tongMon + " s\u1ea3n ph\u1ea9m";
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

            if (lblTamTinhValue != null)
            {
                lblTamTinhValue.Text = tamTinh.ToString("N0") + " \u0111";
            }

            if (lblGiamGiaDiemValue != null)
            {
                lblGiamGiaDiemValue.Text = discount.ToString("N0") + " \u0111";
            }

            lblTongTienValue.Text = payable.ToString("N0") + " \u0111";
        }

        private decimal GetCartSubtotal()
        {
            return _cartItems.Sum(x => x.SoLuong * x.DonGia);
        }

        private decimal GetPointDiscount()
        {
            if (nudRedeemPoints == null || _selectedCustomer == null)
            {
                return 0;
            }

            return _customerService.CalculateRedeemValue((int)nudRedeemPoints.Value);
        }

        private decimal GetPayableTotal()
        {
            return Math.Max(0, GetCartSubtotal() - GetPointDiscount());
        }

        private int GetMaxRedeemPoints(decimal subtotal)
        {
            if (_selectedCustomer == null || subtotal <= 0)
            {
                return 0;
            }

            int maxByOrder = (int)Math.Floor(subtotal / 100m);
            return Math.Max(0, Math.Min(_selectedCustomer.DiemHienCo, maxByOrder));
        }

        private void RenderCartItems()
        {
            flpCartItems.SuspendLayout();
            flpCartItems.Controls.Clear();

            if (_cartItems.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Gi\u1ecf h\u00e0ng \u0111ang tr\u1ed1ng",
                    ForeColor = TextSoft,
                    Font = new Font("Segoe UI", 10F),
                    Size = new Size(280, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 20, 0, 0)
                };

                flpCartItems.Controls.Add(lblEmpty);
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

            Panel card = new Panel
            {
                Size = new Size(296, 116),
                Margin = new Padding(0, 0, 0, 12),
                BackColor = CartCardColor,
                BorderStyle = BorderStyle.None
            };
            card.Paint += ModernCartItemCard_Paint;

            Label lblTen = new Label
            {
                Text = item.TenSP,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = TextMain,
                AutoSize = false,
                Size = new Size(190, 24),
                Location = new Point(16, 14)
            };

            Button btnXoa = new Button
            {
                Text = "×",
                Size = new Size(30, 30),
                Location = new Point(248, 10),
                BackColor = DeleteBg,
                ForeColor = DeleteText,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.MouseOverBackColor = DeleteBg;
            btnXoa.FlatAppearance.MouseDownBackColor = DeleteBg;
            btnXoa.Click += (s, e) => RemoveCartItem(item.MaSP);

            Label lblGia = new Label
            {
                Text = item.DonGia.ToString("N0") + " đ x " + item.SoLuong,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 44)
            };

            Panel qtyWrap = new Panel
            {
                Size = new Size(118, 34),
                Location = new Point(16, 70),
                BackColor = QtyBg,
                BorderStyle = BorderStyle.None
            };
            qtyWrap.Paint += (s, e) =>
            {
                Control c = s as Control;
                using (var pen = new Pen(CartBorderSoft))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
                }
            };

            Button btnGiam = new Button
            {
                Text = "−",
                Size = new Size(34, 34),
                Location = new Point(0, 0),
                BackColor = QtyBg,
                ForeColor = TextMain,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnGiam.FlatAppearance.BorderSize = 0;
            btnGiam.FlatAppearance.MouseOverBackColor = QtyBg;
            btnGiam.FlatAppearance.MouseDownBackColor = QtyBg;
            btnGiam.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, -1);

            Label lblSoLuong = new Label
            {
                Text = item.SoLuong.ToString(),
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                BackColor = QtyCenterBg,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(50, 30),
                Location = new Point(34, 2)
            };
            lblSoLuong.Paint += (s, e) =>
            {
                Control c = s as Control;
                using (var pen = new Pen(CartBorderSoft))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
                }
            };

            Button btnTang = new Button
            {
                Text = "+",
                Size = new Size(34, 34),
                Location = new Point(84, 0),
                BackColor = QtyBg,
                ForeColor = canIncrease ? TextMain : TextSoft,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = canIncrease ? Cursors.Hand : Cursors.Default,
                TabStop = false,
                Enabled = canIncrease
            };
            btnTang.FlatAppearance.BorderSize = 0;
            btnTang.FlatAppearance.MouseOverBackColor = QtyBg;
            btnTang.FlatAppearance.MouseDownBackColor = QtyBg;
            btnTang.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, 1);

            qtyWrap.Controls.Add(btnGiam);
            qtyWrap.Controls.Add(lblSoLuong);
            qtyWrap.Controls.Add(btnTang);

            Label lblThanhTien = new Label
            {
                Text = thanhTien.ToString("N0") + " đ",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(112, 24),
                Location = new Point(166, 75)
            };

            card.Controls.Add(lblTen);
            card.Controls.Add(btnXoa);
            card.Controls.Add(lblGia);
            card.Controls.Add(qtyWrap);
            card.Controls.Add(lblThanhTien);

            return card;
        }
        private void ModernCartItemCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
            using (Pen pen = new Pen(CartBorderSoft))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }
        private Button CreateSoftButton(
    string text,
    Size size,
    Point location,
    Color backColor,
    Color foreColor,
    bool enabled)
        {
            Button button = new Button
            {
                Text = text,
                Size = size,
                Location = location,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Enabled = enabled,
                TabStop = false,
                Cursor = enabled ? Cursors.Hand : Cursors.Default
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = backColor;
            button.FlatAppearance.MouseDownBackColor = backColor;

            return button;
        }
        private Button CreateCircleButton(
            string text,
            Color backColor,
            Color foreColor,
            Point location,
            bool enabled)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(28, 28),
                Location = location,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Enabled = enabled,
                TabStop = false
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = backColor;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, button.Width, button.Height);
                button.Region = new Region(path);
            }

            return button;
        }

        private string GetStockText(CartItem item)
        {
            ProductDTO product = _products.FirstOrDefault(x => x.MaSP == item.MaSP);
            if (product == null)
            {
                return "T\u1ed3n: --";
            }

            return "T\u1ed3n: " + product.SoLuongTon;
        }

        private void UpdateCartItemQuantity(int maSP, int delta)
        {
            CartItem item = _cartItems.FirstOrDefault(x => x.MaSP == maSP);
            if (item == null)
            {
                return;
            }

            if (delta > 0 && !CanIncreaseCartItemQuantity(item))
            {
                MessageBox.Show("S\u1ed1 l\u01b0\u1ee3ng \u0111\u00e3 ch\u1ea1m m\u1ee9c t\u1ed3n kho hi\u1ec7n t\u1ea1i.", "Th\u00f4ng b\u00e1o");
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
            if (item == null)
            {
                return;
            }

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
            return product != null
                && product.HanSuDung.HasValue
                && product.HanSuDung.Value.Date < DateTime.Today;
        }

        private void SearchProducts()
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                RenderProducts(_products);
                return;
            }

            ProductDTO barcodeMatchedProduct = _products.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.MaVach) &&
                string.Equals(x.MaVach.Trim(), keyword, StringComparison.OrdinalIgnoreCase));

            if (barcodeMatchedProduct != null)
            {
                AddToCart(barcodeMatchedProduct);
                txtSearch.Clear();
                RenderProducts(_products);
                return;
            }

            List<ProductDTO> filtered = _products.Where(x =>
                (!string.IsNullOrWhiteSpace(x.TenSP) && x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrWhiteSpace(x.MaVach) && x.MaVach.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
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
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                return;
            }

            txtSearch.Clear();
            SearchProducts();
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void BtnCameraScan_Click(object sender, EventArgs e)
        {
            using (frmCameraScanner frm = new frmCameraScanner(
                "Quét mã bán hàng",
                "Đưa mã vạch sản phẩm vào giữa khung hình. Khi nhận được mã, hệ thống sẽ tự thêm sản phẩm vào giỏ hàng."))
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(frm.ScannedCode))
                {
                    return;
                }

                txtSearch.Text = frm.ScannedCode;
                SearchProducts();
            }
        }

        private void BtnPhoneScan_Click(object sender, EventArgs e)
        {
            using (frmPhoneScannerBridge frm = new frmPhoneScannerBridge(
                "Qu\u00e9t b\u1eb1ng \u0111i\u1ec7n tho\u1ea1i",
                "M\u1edf \u0111i\u1ec7n tho\u1ea1i c\u00f9ng m\u1ea1ng, qu\u00e9t QR \u0111\u1ec3 m\u1edf trang g\u1eedi m\u00e3, r\u1ed3i ch\u1ee5p \u1ea3nh barcode b\u1eb1ng camera \u0111i\u1ec7n tho\u1ea1i."))
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(frm.ScannedCode))
                {
                    return;
                }

                txtSearch.Text = frm.ScannedCode;
                SearchProducts();
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchProducts();
            }
        }

        private void BtnSelectCustomer_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCustomerLookup())
            {
                if (frm.ShowDialog(this) != DialogResult.OK || frm.SelectedCustomer == null)
                {
                    return;
                }

                _selectedCustomer = frm.SelectedCustomer;
                UpdateSelectedCustomerView();
                RefreshCartSummary();
            }
        }

        private void BtnClearCustomer_Click(object sender, EventArgs e)
        {
            _selectedCustomer = null;
            if (nudRedeemPoints != null)
            {
                nudRedeemPoints.Value = 0;
            }
            UpdateSelectedCustomerView();
            RefreshCartSummary();
        }

        private void UpdateSelectedCustomerView()
        {
            if (lblCustomerName == null)
            {
                return;
            }

            if (_selectedCustomer == null)
            {
                lblCustomerName.Text = "Khách lẻ";
                lblCustomerPoints.Text = "Điểm: 0";
                btnClearCustomer.Enabled = false;
                return;
            }

            lblCustomerName.Text = _selectedCustomer.HoTen;
            lblCustomerPoints.Text = "Điểm: " + _selectedCustomer.DiemHienCo.ToString("N0") + " | " + _selectedCustomer.HangThanhVien;
            btnClearCustomer.Enabled = true;
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Gi\u1ecf h\u00e0ng \u0111ang tr\u1ed1ng.", "Th\u00f4ng b\u00e1o");
                return;
            }

            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Kh\u00f4ng x\u00e1c \u0111\u1ecbnh \u0111\u01b0\u1ee3c nh\u00e2n vi\u00ean \u0111\u0103ng nh\u1eadp.", "Th\u00f4ng b\u00e1o");
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

            string paymentMethodLabel;

            if (tongTien <= 0)
            {
                paymentMethodLabel = "Đổi điểm toàn bộ";
            }
            else
            {
                using (frmCashPayment frmCash = new frmCashPayment(tongTien))
                {
                    DialogResult dialogResult = frmCash.ShowDialog(this);

                    if (dialogResult != DialogResult.OK || !frmCash.IsConfirmed)
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
                GhiChu = "B\u00e1n t\u1ea1i qu\u1ea7y - " + paymentMethodLabel,
                ChiTietHoaDon = _cartItems.Select(x => new InvoiceDetailDTO
                {
                    MaSP = x.MaSP,
                    SoLuong = x.SoLuong,
                    DonGiaLucBan = x.DonGia,
                    ThanhTien = x.SoLuong * x.DonGia
                }).ToList()
            };

            OperationResult result = _invoiceService.Checkout(request);

            MessageBox.Show(
                result.Message,
                "Th\u00f4ng b\u00e1o",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                int? maHD = result.DataId;

                _cartItems.Clear();
                _selectedCustomer = null;
                if (nudRedeemPoints != null)
                {
                    nudRedeemPoints.Value = 0;
                }
                UpdateSelectedCustomerView();
                RefreshCartView();
                LoadProducts();

                if (maHD.HasValue && maHD.Value > 0)
                {
                    DialogResult viewResult = MessageBox.Show(
                        "Thanh to\u00e1n th\u00e0nh c\u00f4ng. B\u1ea1n c\u00f3 mu\u1ed1n xem chi ti\u1ebft h\u00f3a \u0111\u01a1n kh\u00f4ng?",
                        "Th\u00f4ng b\u00e1o",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (viewResult == DialogResult.Yes)
                    {
                        using (frmInvoiceDetails frm = new frmInvoiceDetails(maHD.Value))
                        {
                            frm.ShowDialog(this);
                        }
                    }
                }
            }
        }

        private void CartItemCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null)
            {
                return;
            }

            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            }
        }

        private void ProductCard_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null)
            {
                return;
            }

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
