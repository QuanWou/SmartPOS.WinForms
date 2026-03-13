using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.UI.Forms.Products
{
    public class frmPrintBarcode : Form
    {
        private readonly IProductService _productService;

        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblProduct;
        private ComboBox cboProducts;

        private Label lblBarcode;
        private TextBox txtBarcode;

        private Label lblQuantity;
        private NumericUpDown nudQuantity;

        private Panel pnlPreview;
        private TextBox txtPreview;

        private Button btnLoadProduct;
        private Button btnPreview;
        private Button btnPrint;
        private Button btnClose;

        private List<ProductDTO> _products;

        public frmPrintBarcode()
        {
            _productService = new ProductService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "In mã vạch";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(860, 620);
            this.MinimumSize = new Size(860, 620);
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "In mã vạch sản phẩm",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Chọn sản phẩm, xem trước mã vạch và in tem",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblProduct = new Label
            {
                Text = "Sản phẩm",
                AutoSize = true,
                Location = new Point(20, 95)
            };

            cboProducts = new ComboBox
            {
                Location = new Point(20, 117),
                Size = new Size(280, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboProducts.SelectedIndexChanged += CboProducts_SelectedIndexChanged;

            lblBarcode = new Label
            {
                Text = "Mã vạch",
                AutoSize = true,
                Location = new Point(320, 95)
            };

            txtBarcode = new TextBox
            {
                Location = new Point(320, 117),
                Size = new Size(220, 27),
                ReadOnly = true
            };

            lblQuantity = new Label
            {
                Text = "Số tem",
                AutoSize = true,
                Location = new Point(560, 95)
            };

            nudQuantity = new NumericUpDown
            {
                Location = new Point(560, 117),
                Size = new Size(100, 27),
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };

            btnLoadProduct = new Button
            {
                Text = "Nạp dữ liệu",
                Location = new Point(680, 114),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLoadProduct.FlatAppearance.BorderSize = 0;
            btnLoadProduct.Click += BtnLoadProduct_Click;

            pnlPreview = new Panel
            {
                Location = new Point(20, 170),
                Size = new Size(770, 330),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtPreview = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 11F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 40, 40)
            };

            pnlPreview.Controls.Add(txtPreview);

            btnPreview = new Button
            {
                Text = "Xem trước",
                Location = new Point(470, 520),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPreview.FlatAppearance.BorderSize = 0;
            btnPreview.Click += BtnPreview_Click;

            btnPrint = new Button
            {
                Text = "In tem",
                Location = new Point(580, 520),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

            btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(690, 520),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += BtnClose_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblProduct);
            this.Controls.Add(cboProducts);
            this.Controls.Add(lblBarcode);
            this.Controls.Add(txtBarcode);
            this.Controls.Add(lblQuantity);
            this.Controls.Add(nudQuantity);
            this.Controls.Add(btnLoadProduct);
            this.Controls.Add(pnlPreview);
            this.Controls.Add(btnPreview);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            this.Load += FrmPrintBarcode_Load;
        }

        private void FrmPrintBarcode_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            _products = _productService.GetAll()
                .Where(x => x.TrangThai)
                .OrderBy(x => x.TenSP)
                .ToList();

            cboProducts.Items.Clear();

            foreach (ProductDTO product in _products)
            {
                cboProducts.Items.Add(new ComboBoxItem(product.MaSP, product.TenSP));
            }

            if (cboProducts.Items.Count > 0)
            {
                cboProducts.SelectedIndex = 0;
            }
        }

        private void CboProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedProductInfo();
        }

        private void BtnLoadProduct_Click(object sender, EventArgs e)
        {
            LoadSelectedProductInfo();
            GeneratePreview();
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            GeneratePreview();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPreview.Text))
            {
                GeneratePreview();
            }

            MessageBox.Show("Bước tiếp theo sẽ nối máy in tem / in mã vạch thật.", "Thông báo");
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSelectedProductInfo()
        {
            ProductDTO product = GetSelectedProduct();
            if (product == null)
            {
                txtBarcode.Clear();
                txtPreview.Clear();
                return;
            }

            txtBarcode.Text = product.MaVach ?? string.Empty;
        }

        private void GeneratePreview()
        {
            ProductDTO product = GetSelectedProduct();
            if (product == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.", "Thông báo");
                return;
            }

            int quantity = (int)nudQuantity.Value;
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("========================================");
            builder.AppendLine("SMARTPOS - TEM MÃ VẠCH");
            builder.AppendLine("========================================");
            builder.AppendLine("Sản phẩm : " + product.TenSP);
            builder.AppendLine("Mã SP    : " + product.MaSP);
            builder.AppendLine("Mã vạch  : " + product.MaVach);
            builder.AppendLine("ĐVT      : " + product.DonViTinh);
            builder.AppendLine("Giá bán  : " + product.GiaBan.ToString("N0") + " đ");
            builder.AppendLine("Số tem   : " + quantity);
            builder.AppendLine("========================================");
            builder.AppendLine();

            for (int i = 1; i <= quantity; i++)
            {
                builder.AppendLine("Tem #" + i);
                builder.AppendLine(product.TenSP);
                builder.AppendLine("*" + (product.MaVach ?? string.Empty) + "*");
                builder.AppendLine(product.GiaBan.ToString("N0") + " đ");
                builder.AppendLine("----------------------------------------");
            }

            txtPreview.Text = builder.ToString();
            txtPreview.SelectionStart = 0;
            txtPreview.SelectionLength = 0;
        }

        private ProductDTO GetSelectedProduct()
        {
            if (!(cboProducts.SelectedItem is ComboBoxItem item))
            {
                return null;
            }

            return _products.FirstOrDefault(x => x.MaSP == item.Value);
        }

        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }

            public ComboBoxItem(int value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}