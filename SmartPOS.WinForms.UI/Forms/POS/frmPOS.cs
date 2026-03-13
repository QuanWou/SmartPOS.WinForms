using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.UI.Forms.Invoices;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.POS
{
    public class frmPOS : Form
    {
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
        private ListView lvCart;

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
            this.Text = "Bán hàng";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            BuildLayout();

            this.Load += FrmPOS_Load;
        }

        private void FrmPOS_Load(object sender, EventArgs e)
        {
            LoadProducts();
            RefreshCartView();
        }

        private void BuildLayout()
        {
            pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 251),
                Padding = new Padding(20, 20, 10, 20)
            };

            pnlRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 360,
                BackColor = Color.FromArgb(248, 249, 251),
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
                BackColor = Color.White,
                Padding = new Padding(18, 16, 18, 16)
            };

            lblTitle = new Label
            {
                Text = "Bán hàng tại quầy",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(18, 14)
            };

            lblSubtitle = new Label
            {
                Text = "Tìm sản phẩm theo tên hoặc mã vạch để thêm vào giỏ hàng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(18, 44)
            };

            pnlSearch = new Panel
            {
                Size = new Size(620, 40),
                Location = new Point(18, 60),
                BackColor = Color.FromArgb(245, 247, 252)
            };

            lblSearch = new Label
            {
                Text = "Tìm kiếm",
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
                Text = "Tìm",
                Size = new Size(80, 28),
                Location = new Point(450, 6),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnScan.FlatAppearance.BorderSize = 0;
            btnScan.Click += BtnScan_Click;

            btnLamMoi = new Button
            {
                Text = "Làm mới",
                Size = new Size(80, 28),
                Location = new Point(536, 6),
                BackColor = Color.FromArgb(230, 233, 240),
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
                BackColor = Color.White,
                Padding = new Padding(14)
            };

            flpProducts = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.White
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
                BackColor = Color.White,
                Padding = new Padding(16, 14, 16, 10)
            };

            var lblCartTitle = new Label
            {
                Text = "Giỏ hàng",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(16, 14)
            };

            lblTongMon = new Label
            {
                Text = "0 sản phẩm",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(230, 18)
            };

            pnlCartHeader.Controls.Add(lblCartTitle);
            pnlCartHeader.Controls.Add(lblTongMon);

            pnlCartFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(16, 12, 16, 16)
            };

            lblTongTien = new Label
            {
                Text = "Tổng tiền",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(16, 14)
            };

            lblTongTienValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(16, 38)
            };

            btnThanhToan = new Button
            {
                Text = "Thanh toán",
                Size = new Size(310, 38),
                Location = new Point(16, 74),
                BackColor = Color.FromArgb(22, 32, 72),
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
                BackColor = Color.White,
                Padding = new Padding(16, 10, 16, 10)
            };

            lvCart = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };

            lvCart.Columns.Add("Sản phẩm", 150);
            lvCart.Columns.Add("SL", 45);
            lvCart.Columns.Add("Đơn giá", 90);
            lvCart.DoubleClick += LvCart_DoubleClick;

            pnlCartBody.Controls.Add(lvCart);

            pnlRight.Controls.Add(pnlCartBody);
            pnlRight.Controls.Add(pnlCartFooter);
            pnlRight.Controls.Add(pnlCartHeader);
        }

        private void LoadProducts()
        {
            _products = _productService.GetAll()
                .Where(x => x.TrangThai)
                .ToList();

            RenderProducts(_products);
        }

        private void RenderProducts(List<ProductDTO> products)
        {
            flpProducts.Controls.Clear();

            foreach (var product in products)
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
                BackColor = Color.FromArgb(250, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Label lblTen = new Label
            {
                Text = product.TenSP,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                Size = new Size(135, 36),
                Location = new Point(12, 14)
            };

            Label lblMa = new Label
            {
                Text = "Mã: " + product.MaVach,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(12, 58)
            };

            Label lblGia = new Label
            {
                Text = product.GiaBan.ToString("N0") + " đ",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 110, 200),
                AutoSize = true,
                Location = new Point(12, 84)
            };

            Button btnThem = new Button
            {
                Text = "Thêm",
                Size = new Size(56, 26),
                Location = new Point(95, 80),
                BackColor = Color.FromArgb(22, 32, 72),
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
            if (product == null)
            {
                return;
            }

            if (product.SoLuongTon <= 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng.", "Thông báo");
                return;
            }

            CartItem existing = _cartItems.FirstOrDefault(x => x.MaSP == product.MaSP);
            int soLuongTrongGio = existing != null ? existing.SoLuong : 0;

            if (soLuongTrongGio + 1 > product.SoLuongTon)
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
            lvCart.Items.Clear();

            foreach (var item in _cartItems)
            {
                var listItem = new ListViewItem(item.TenSP);
                listItem.SubItems.Add(item.SoLuong.ToString());
                listItem.SubItems.Add(item.DonGia.ToString("N0"));
                listItem.Tag = item.MaSP;
                lvCart.Items.Add(listItem);
            }

            int tongMon = _cartItems.Sum(x => x.SoLuong);
            decimal tongTien = _cartItems.Sum(x => x.SoLuong * x.DonGia);

            lblTongMon.Text = tongMon + " sản phẩm";
            lblTongTienValue.Text = tongTien.ToString("N0") + " đ";
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

        private void LvCart_DoubleClick(object sender, EventArgs e)
        {
            if (lvCart.SelectedItems.Count == 0)
            {
                return;
            }

            int maSP = (int)lvCart.SelectedItems[0].Tag;
            CartItem item = _cartItems.FirstOrDefault(x => x.MaSP == maSP);
            if (item == null)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Chọn Yes để giảm 1 số lượng.\nChọn No để xóa sản phẩm khỏi giỏ hàng.",
                "Cập nhật giỏ hàng",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.Yes)
            {
                item.SoLuong -= 1;

                if (item.SoLuong <= 0)
                {
                    _cartItems.Remove(item);
                }
            }
            else if (result == DialogResult.No)
            {
                _cartItems.Remove(item);
            }

            RefreshCartView();
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

            decimal tongTien = _cartItems.Sum(x => x.SoLuong * x.DonGia);

            using (var frmCash = new frmCashPayment(tongTien))
            {
                var dialogResult = frmCash.ShowDialog(this);

                if (dialogResult != DialogResult.OK || !frmCash.IsConfirmed)
                {
                    return;
                }
            }

            CheckoutRequest request = new CheckoutRequest
            {
                MaNV = SessionManager.CurrentUser.MaNV,
                GhiChu = "Bán tại quầy",
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
                "Thông báo",
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
                        "Thanh toán thành công. Bạn có muốn xem chi tiết hóa đơn không?",
                        "Thông báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (viewResult == DialogResult.Yes)
                    {
                        using (var frm = new SmartPOS.WinForms.UI.Forms.Invoices.frmInvoiceDetails(maHD.Value))
                        {
                            frm.ShowDialog(this);
                        }
                    }
                }
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