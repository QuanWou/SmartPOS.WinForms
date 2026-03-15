using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Forms.Invoices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.POS
{
    public class frmPOS : Form
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

        private readonly IProductService _productService;
        private readonly IInvoiceService _invoiceService;

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
        private Button btnThanhToan;
        private Button btnLamMoi;

        private FlowLayoutPanel flpProducts;
        private FlowLayoutPanel flpCartItems;

        private Label lblTongMon;
        private Label lblTongTien;
        private Label lblTongTienValue;

        private List<ProductDTO> _products;
        private List<CartItem> _cartItems;

        public frmPOS()
        {
            _productService = new ProductService();
            _invoiceService = new InvoiceService();
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
                Text = "T\u00ecm s\u1ea3n ph\u1ea9m theo t\u00ean ho\u1eb7c m\u00e3 v\u1ea1ch \u0111\u1ec3 th\u00eam v\u00e0o gi\u1ecf h\u00e0ng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(18, 44)
            };

            pnlSearch = new Panel
            {
                Size = new Size(620, 40),
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
                Width = 430
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnScan = new Button
            {
                Text = "T\u00ecm",
                Size = new Size(80, 28),
                Location = new Point(450, 6),
                BackColor = PrimaryMid,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnScan.FlatAppearance.BorderSize = 0;
            btnScan.Click += BtnScan_Click;

            btnLamMoi = new Button
            {
                Text = "L\u00e0m m\u1edbi",
                Size = new Size(80, 28),
                Location = new Point(536, 6),
                BackColor = FieldColor,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnScan);
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
                Height = 120,
                BackColor = SurfaceColor,
                Padding = new Padding(16, 12, 16, 16)
            };

            lblTongTien = new Label
            {
                Text = "T\u1ed5ng ti\u1ec1n",
                Font = new Font("Segoe UI", 10F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(16, 14)
            };

            lblTongTienValue = new Label
            {
                Text = "0 \u0111",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                AutoSize = true,
                Location = new Point(16, 38)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh to\u00e1n",
                Size = new Size(310, 38),
                Location = new Point(16, 74),
                BackColor = PrimaryDark,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.Click += BtnThanhToan_Click;

            pnlCartFooter.Controls.Add(lblTongTien);
            pnlCartFooter.Controls.Add(lblTongTienValue);
            pnlCartFooter.Controls.Add(btnThanhToan);

            pnlCartBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceColor,
                Padding = new Padding(16, 10, 16, 10)
            };

            flpCartItems = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = SurfaceColor
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
            decimal tongTien = _cartItems.Sum(x => x.SoLuong * x.DonGia);

            lblTongMon.Text = tongMon + " s\u1ea3n ph\u1ea9m";
            lblTongTienValue.Text = tongTien.ToString("N0") + " \u0111";
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
                Size = new Size(280, 118),
                Margin = new Padding(0, 0, 0, 12),
                BackColor = CardTint,
                BorderStyle = BorderStyle.None
            };
            card.Paint += CartItemCard_Paint;

            Label lblTen = new Label
            {
                Text = item.TenSP,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = TextMain,
                AutoSize = false,
                Size = new Size(208, 26),
                Location = new Point(14, 12)
            };

            Label lblCongThucGia = new Label
            {
                Text = item.DonGia.ToString("N0") + " \u0111 x " + item.SoLuong + " = " + thanhTien.ToString("N0") + " \u0111",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = PrimaryMid,
                AutoSize = true,
                Location = new Point(14, 46)
            };

            Button btnXoa = CreateCircleButton(
                "X",
                DangerColor,
                Color.White,
                new Point(236, 12),
                true);
            btnXoa.Click += (s, e) => RemoveCartItem(item.MaSP);

            Button btnGiam = CreateCircleButton(
                "-",
                FieldColor,
                TextMain,
                new Point(14, 76),
                true);
            btnGiam.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, -1);

            Label lblSoLuong = new Label
            {
                Text = item.SoLuong.ToString(),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = PrimaryDark,
                Size = new Size(32, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(48, 78)
            };

            Button btnTang = CreateCircleButton(
                "+",
                canIncrease ? PrimaryMid : FieldColor,
                canIncrease ? Color.White : TextSoft,
                new Point(82, 76),
                canIncrease);
            btnTang.Click += (s, e) => UpdateCartItemQuantity(item.MaSP, 1);

            Label lblTonKho = new Label
            {
                Text = GetStockText(item),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextSoft,
                AutoSize = true,
                Location = new Point(126, 83)
            };

            card.Controls.Add(lblTen);
            card.Controls.Add(lblCongThucGia);
            card.Controls.Add(btnXoa);
            card.Controls.Add(btnGiam);
            card.Controls.Add(lblSoLuong);
            card.Controls.Add(btnTang);
            card.Controls.Add(lblTonKho);

            return card;
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

        private void BtnScan_Click(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchProducts();
            }
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

            decimal tongTien = _cartItems.Sum(x => x.SoLuong * x.DonGia);

            string paymentMethodLabel;

            using (frmCashPayment frmCash = new frmCashPayment(tongTien))
            {
                DialogResult dialogResult = frmCash.ShowDialog(this);

                if (dialogResult != DialogResult.OK || !frmCash.IsConfirmed)
                {
                    return;
                }

                paymentMethodLabel = frmCash.PaymentMethodLabel;
            }

            CheckoutRequest request = new CheckoutRequest
            {
                MaNV = SessionManager.CurrentUser.MaNV,
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
