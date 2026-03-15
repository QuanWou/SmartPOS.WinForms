using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.UI.Forms.Products
{
    public class frmExpiredProducts : Form
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductLotService _productLotService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblKeyword;
        private Label lblFilter;
        private TextBox txtKeyword;
        private ComboBox cboFilter;
        private Button btnSearch;
        private Button btnReload;
        private Button btnEdit;
        private Button btnRestore;
        private DataGridView dgvProducts;

        private List<ProductDTO> _products;
        private List<CategoryDTO> _categories;
        private List<ProductLotDTO> _lots;

        public frmExpiredProducts()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            _productLotService = new ProductLotService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Ng\u1eebng b\u00e1n / h\u1ebft h\u1ea1n";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Ng\u1eebng b\u00e1n v\u00e0 h\u1ebft h\u1ea1n theo l\u00f4",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Theo d\u00f5i s\u1ea3n ph\u1ea9m ng\u1eebng b\u00e1n v\u00e0 c\u00e1c l\u00f4 h\u00e0ng h\u1ebft h\u1ea1n ho\u1eb7c s\u1eafp h\u1ebft h\u1ea1n",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblKeyword = new Label
            {
                Text = "T\u1eeb kh\u00f3a",
                AutoSize = true,
                Location = new Point(20, 84)
            };

            txtKeyword = new TextBox
            {
                Location = new Point(20, 106),
                Size = new Size(240, 27)
            };

            lblFilter = new Label
            {
                Text = "B\u1ed9 l\u1ecdc",
                AutoSize = true,
                Location = new Point(275, 84)
            };

            cboFilter = new ComboBox
            {
                Location = new Point(275, 106),
                Size = new Size(180, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = new Button
            {
                Text = "T\u00ecm ki\u1ebfm",
                Location = new Point(470, 103),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            btnReload = new Button
            {
                Text = "T\u1ea3i l\u1ea1i",
                Location = new Point(570, 103),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += BtnReload_Click;

            btnEdit = new Button
            {
                Text = "M\u1edf s\u1eeda",
                Location = new Point(760, 103),
                Size = new Size(95, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            btnRestore = new Button
            {
                Text = "B\u00e1n l\u1ea1i",
                Location = new Point(865, 103),
                Size = new Size(85, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.Click += BtnRestore_Click;

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 150),
                Size = new Size(930, 460),
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
            this.Controls.Add(lblFilter);
            this.Controls.Add(cboFilter);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnRestore);
            this.Controls.Add(dgvProducts);

            this.Load += FrmExpiredProducts_Load;
        }

        private void FrmExpiredProducts_Load(object sender, EventArgs e)
        {
            _categories = _categoryService.GetAll().ToList();
            LoadFilter();
            LoadIssues();
        }

        private void BuildGridColumns()
        {
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IssueType",
                HeaderText = "IssueType",
                DataPropertyName = "IssueType",
                Visible = false
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaLo",
                HeaderText = "MaLo",
                DataPropertyName = "MaLo",
                Visible = false
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "M\u00e3 SP",
                DataPropertyName = "MaSP",
                Width = 70
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "T\u00ean s\u1ea3n ph\u1ea9m",
                DataPropertyName = "TenSP",
                Width = 220
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaVach",
                HeaderText = "M\u00e3 v\u1ea1ch",
                DataPropertyName = "MaVach",
                Width = 120
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DanhMuc",
                HeaderText = "Danh m\u1ee5c",
                DataPropertyName = "DanhMuc",
                Width = 110
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayNhapLo",
                HeaderText = "Ng\u00e0y nh\u1eadp l\u00f4",
                DataPropertyName = "NgayNhapLo",
                Width = 110
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HanSuDung",
                HeaderText = "H\u1ea1n s\u1eed d\u1ee5ng",
                DataPropertyName = "HanSuDung",
                Width = 110
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongTon",
                HeaderText = "SL",
                DataPropertyName = "SoLuongTon",
                Width = 60
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TinhTrang",
                HeaderText = "T\u00ecnh tr\u1ea1ng",
                DataPropertyName = "TinhTrang",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void LoadFilter()
        {
            cboFilter.Items.Clear();
            cboFilter.Items.Add(new FilterItem(0, "T\u1ea5t c\u1ea3 v\u1ea5n \u0111\u1ec1"));
            cboFilter.Items.Add(new FilterItem(1, "Ng\u1eebng b\u00e1n"));
            cboFilter.Items.Add(new FilterItem(2, "\u0110\u00e3 h\u1ebft h\u1ea1n"));
            cboFilter.Items.Add(new FilterItem(3, "S\u1eafp h\u1ebft h\u1ea1n 7 ng\u00e0y"));
            cboFilter.SelectedIndex = 0;
        }

        private void LoadIssues()
        {
            _products = _productService.GetAll().ToList();
            _lots = _productLotService.GetAll()
                .Where(x => x.SoLuongTonLo > 0)
                .ToList();
            BindGrid(BuildIssueRows());
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(BuildIssueRows());
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboFilter.SelectedIndex = 0;
            LoadIssues();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            ProductDTO product = GetSelectedProduct();
            if (product == null)
            {
                return;
            }

            using (var frm = new frmProductEdit(product))
            {
                frm.ShowDialog(this);
                if (frm.IsSavedSuccessfully)
                {
                    LoadIssues();
                }
            }
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui l\u00f2ng ch\u1ecdn d\u00f2ng c\u1ea7n x\u1eed l\u00fd.", "Th\u00f4ng b\u00e1o");
                return;
            }

            string issueType = dgvProducts.CurrentRow.Cells["IssueType"].Value != null
                ? dgvProducts.CurrentRow.Cells["IssueType"].Value.ToString()
                : string.Empty;

            if (issueType != IssueTypeInactive)
            {
                MessageBox.Show("N\u00fat B\u00e1n l\u1ea1i ch\u1ec9 \u00e1p d\u1ee5ng cho s\u1ea3n ph\u1ea9m \u0111ang ng\u1eebng b\u00e1n.", "Th\u00f4ng b\u00e1o");
                return;
            }

            ProductDTO product = GetSelectedProduct();
            if (product == null)
            {
                return;
            }

            product.TrangThai = true;
            var result = _productService.Update(product);

            MessageBox.Show(
                result.Message,
                "Th\u00f4ng b\u00e1o",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                LoadIssues();
            }
        }

        private ProductDTO GetSelectedProduct()
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui l\u00f2ng ch\u1ecdn s\u1ea3n ph\u1ea9m.", "Th\u00f4ng b\u00e1o");
                return null;
            }

            object value = dgvProducts.CurrentRow.Cells["MaSP"].Value;
            if (value == null || !int.TryParse(value.ToString(), out int maSP))
            {
                MessageBox.Show("Kh\u00f4ng x\u00e1c \u0111\u1ecbnh \u0111\u01b0\u1ee3c s\u1ea3n ph\u1ea9m.", "Th\u00f4ng b\u00e1o");
                return null;
            }

            ProductDTO product = _products.FirstOrDefault(x => x.MaSP == maSP);
            if (product == null)
            {
                MessageBox.Show("Kh\u00f4ng t\u00ecm th\u1ea5y s\u1ea3n ph\u1ea9m.", "Th\u00f4ng b\u00e1o");
            }

            return product;
        }

        private List<IssueRow> BuildIssueRows()
        {
            List<IssueRow> rows = new List<IssueRow>();

            rows.AddRange(_products
                .Where(x => !x.TrangThai)
                .Select(x => new IssueRow
                {
                    IssueType = IssueTypeInactive,
                    MaLo = null,
                    MaSP = x.MaSP,
                    TenSP = x.TenSP,
                    MaVach = x.MaVach,
                    DanhMuc = GetCategoryName(x.MaLoai),
                    NgayNhapLo = "-",
                    HanSuDung = x.HanSuDung.HasValue ? x.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                    SoLuongTon = x.SoLuongTon.ToString(),
                    TinhTrang = "Ng\u1eebng b\u00e1n"
                }));

            rows.AddRange(_lots
                .Where(IsExpiredLot)
                .Select(x => CreateLotIssueRow(x, IssueTypeExpired, "\u0110\u00e3 h\u1ebft h\u1ea1n")));

            rows.AddRange(_lots
                .Where(IsExpiringSoonLot)
                .Select(x => CreateLotIssueRow(x, IssueTypeExpiringSoon, "S\u1eafp h\u1ebft h\u1ea1n")));

            string keyword = txtKeyword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                rows = rows.Where(x =>
                        (!string.IsNullOrWhiteSpace(x.TenSP) && x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(x.MaVach) && x.MaVach.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrWhiteSpace(x.TinhTrang) && x.TinhTrang.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }

            int filterMode = cboFilter.SelectedItem is FilterItem item ? item.Value : 0;
            switch (filterMode)
            {
                case 1:
                    rows = rows.Where(x => x.IssueType == IssueTypeInactive).ToList();
                    break;

                case 2:
                    rows = rows.Where(x => x.IssueType == IssueTypeExpired).ToList();
                    break;

                case 3:
                    rows = rows.Where(x => x.IssueType == IssueTypeExpiringSoon).ToList();
                    break;

                default:
                    break;
            }

            return rows
                .OrderBy(x => GetIssueSortOrder(x.IssueType))
                .ThenBy(x => x.HanSuDungSort ?? DateTime.MaxValue)
                .ThenBy(x => x.NgayNhapLoSort ?? DateTime.MaxValue)
                .ThenBy(x => x.TenSP)
                .ToList();
        }

        private IssueRow CreateLotIssueRow(ProductLotDTO lot, string issueType, string issueLabel)
        {
            ProductDTO product = _products.FirstOrDefault(x => x.MaSP == lot.MaSP);

            return new IssueRow
            {
                IssueType = issueType,
                MaLo = lot.MaLo,
                MaSP = lot.MaSP,
                TenSP = !string.IsNullOrWhiteSpace(lot.TenSP) ? lot.TenSP : product?.TenSP ?? string.Empty,
                MaVach = !string.IsNullOrWhiteSpace(lot.MaVach) ? lot.MaVach : product?.MaVach ?? string.Empty,
                DanhMuc = product != null ? GetCategoryName(product.MaLoai) : string.Empty,
                NgayNhapLo = lot.NgayNhap.ToString("dd/MM/yyyy"),
                HanSuDung = lot.HanSuDung.HasValue ? lot.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                SoLuongTon = lot.SoLuongTonLo.ToString(),
                TinhTrang = issueLabel + " - l\u00f4 nh\u1eadp " + lot.NgayNhap.ToString("dd/MM/yyyy"),
                HanSuDungSort = lot.HanSuDung,
                NgayNhapLoSort = lot.NgayNhap
            };
        }

        private void BindGrid(List<IssueRow> rows)
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = rows;
        }

        private string GetCategoryName(int maLoai)
        {
            CategoryDTO category = _categories?.FirstOrDefault(x => x.MaLoai == maLoai);
            return category != null ? category.TenLoai : string.Empty;
        }

        private bool IsExpiredLot(ProductLotDTO lot)
        {
            return lot != null
                && lot.SoLuongTonLo > 0
                && lot.HanSuDung.HasValue
                && lot.HanSuDung.Value.Date < DateTime.Today;
        }

        private bool IsExpiringSoonLot(ProductLotDTO lot)
        {
            return lot != null
                && lot.SoLuongTonLo > 0
                && lot.HanSuDung.HasValue
                && lot.HanSuDung.Value.Date >= DateTime.Today
                && lot.HanSuDung.Value.Date <= DateTime.Today.AddDays(7);
        }

        private int GetIssueSortOrder(string issueType)
        {
            if (issueType == IssueTypeExpired)
            {
                return 0;
            }

            if (issueType == IssueTypeExpiringSoon)
            {
                return 1;
            }

            return 2;
        }

        private sealed class FilterItem
        {
            public int Value { get; private set; }
            public string Text { get; private set; }

            public FilterItem(int value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private sealed class IssueRow
        {
            public string IssueType { get; set; }

            public int? MaLo { get; set; }

            public int MaSP { get; set; }

            public string TenSP { get; set; }

            public string MaVach { get; set; }

            public string DanhMuc { get; set; }

            public string NgayNhapLo { get; set; }

            public string HanSuDung { get; set; }

            public string SoLuongTon { get; set; }

            public string TinhTrang { get; set; }

            public DateTime? HanSuDungSort { get; set; }

            public DateTime? NgayNhapLoSort { get; set; }
        }

        private const string IssueTypeInactive = "Inactive";
        private const string IssueTypeExpired = "ExpiredLot";
        private const string IssueTypeExpiringSoon = "ExpiringSoonLot";
    }
}
