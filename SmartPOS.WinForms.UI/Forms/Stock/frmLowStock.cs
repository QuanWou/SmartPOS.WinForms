using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Forms.Main;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Stock
{
    public class frmLowStock : Form, IGlobalSearchHandler
    {
        private readonly IProductService _productService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblKeyword;
        private Label lblThreshold;

        private TextBox txtKeyword;
        private NumericUpDown nudThreshold;

        private Button btnSearch;
        private Button btnReload;
        private Button btnStockIn;

        private DataGridView dgvLowStock;

        private List<ProductDTO> _products;

        public frmLowStock()
        {
            _productService = new ProductService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Sản phẩm sắp hết hàng";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Sản phẩm sắp hết hàng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi các sản phẩm có tồn kho thấp để bổ sung kịp thời",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblKeyword = new Label
            {
                Text = "Từ khóa",
                AutoSize = true,
                Location = new Point(20, 85)
            };

            txtKeyword = new TextBox
            {
                Location = new Point(20, 107),
                Size = new Size(220, 27)
            };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            lblThreshold = new Label
            {
                Text = "Ngưỡng tồn",
                AutoSize = true,
                Location = new Point(255, 85)
            };

            nudThreshold = new NumericUpDown
            {
                Location = new Point(255, 107),
                Size = new Size(100, 27),
                Minimum = 0,
                Maximum = 1000,
                Value = 10
            };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(375, 104),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            btnReload = new Button
            {
                Text = "Tải lại",
                Location = new Point(475, 104),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += BtnReload_Click;

            btnStockIn = new Button
            {
                Text = "Nhập kho",
                Location = new Point(565, 104),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnStockIn.FlatAppearance.BorderSize = 0;
            btnStockIn.Click += BtnStockIn_Click;

            dgvLowStock = new DataGridView
            {
                Location = new Point(20, 155),
                Size = new Size(970, 455),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false
            };

            BuildGridColumns();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblKeyword);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(lblThreshold);
            this.Controls.Add(nudThreshold);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnStockIn);
            this.Controls.Add(dgvLowStock);

            this.Load += FrmLowStock_Load;
        }

        private void FrmLowStock_Load(object sender, EventArgs e)
        {
            btnStockIn.Visible = !SessionManager.IsStaff;
            LoadProducts();
            SearchLowStock();
        }

        private void BuildGridColumns()
        {
            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MaSP",
                Width = 80
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 250
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaVach",
                HeaderText = "Mã vạch",
                DataPropertyName = "MaVach",
                Width = 140
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonViTinh",
                HeaderText = "ĐVT",
                DataPropertyName = "DonViTinh",
                Width = 80
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongTon",
                HeaderText = "Tồn kho",
                DataPropertyName = "SoLuongTon",
                Width = 90
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaBan",
                HeaderText = "Giá bán",
                DataPropertyName = "GiaBan",
                Width = 120
            });

            dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DanhGia",
                HeaderText = "Mức độ",
                DataPropertyName = "DanhGia",
                Width = 140
            });
        }

        private void LoadProducts()
        {
            _products = _productService.GetAll()
                .Where(x => x.TrangThai && (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today))
                .ToList();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchLowStock();
        }

        private void TxtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchLowStock();
            }
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            nudThreshold.Value = 10;
            LoadProducts();
            SearchLowStock();
        }

        private void BtnStockIn_Click(object sender, EventArgs e)
        {
            if (dgvLowStock.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần nhập thêm.", "Thông báo");
                return;
            }

            frmMain mainForm = TopLevelControl as frmMain
                ?? Parent?.FindForm() as frmMain
                ?? Application.OpenForms.OfType<frmMain>().FirstOrDefault();

            if (mainForm != null)
            {
                mainForm.NavigateToPage(new frmStockIn());
                return;
            }

            MessageBox.Show("Mở màn hình Nhập kho để bổ sung sản phẩm này.", "Thông báo");
        }

        private void SearchLowStock()
        {
            IEnumerable<ProductDTO> query = _products ?? new List<ProductDTO>();

            int threshold = (int)nudThreshold.Value;
            query = query.Where(x => x.SoLuongTon <= threshold);

            string keyword = txtKeyword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.TenSP) && x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(x.MaVach) && x.MaVach.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            BindGrid(query.OrderBy(x => x.SoLuongTon).ToList());
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtKeyword.Text = keyword ?? string.Empty;
            SearchLowStock();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                return;
            }

            txtKeyword.Clear();
            SearchLowStock();
        }

        private void BindGrid(List<ProductDTO> products)
        {
            var viewData = products.Select(x => new
            {
                x.MaSP,
                x.TenSP,
                x.MaVach,
                x.DonViTinh,
                x.SoLuongTon,
                GiaBan = x.GiaBan.ToString("N0"),
                DanhGia = GetDanhGia(x.SoLuongTon)
            }).ToList();

            dgvLowStock.DataSource = null;
            dgvLowStock.DataSource = viewData;
        }

        private string GetDanhGia(int soLuongTon)
        {
            if (soLuongTon <= 5)
            {
                return "Cần nhập gấp";
            }

            if (soLuongTon <= 10)
            {
                return "Sắp hết";
            }

            return "Theo dõi";
        }
    }
}
