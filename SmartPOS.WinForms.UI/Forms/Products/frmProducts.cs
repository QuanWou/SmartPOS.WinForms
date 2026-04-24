using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Helpers;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Products
{
    public class frmProducts : Form, IGlobalSearchHandler
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        private Label lblTitle;
        private Label lblKeyword;
        private Label lblCategory;
        private Label lblStatus;

        private TextBox txtKeyword;
        private ComboBox cboCategory;
        private ComboBox cboStatus;

        private Button btnSearch;
        private Button btnReload;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        private DataGridView dgvProducts;

        private List<CategoryDTO> _categories;

        public frmProducts()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý sản phẩm";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Quản lý sản phẩm",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblKeyword = new Label
            {
                Text = "Từ khóa",
                AutoSize = true,
                Location = new Point(20, 70)
            };

            txtKeyword = new TextBox
            {
                Location = new Point(20, 92),
                Size = new Size(220, 27)
            };

            lblCategory = new Label
            {
                Text = "Danh mục",
                AutoSize = true,
                Location = new Point(255, 70)
            };

            cboCategory = new ComboBox
            {
                Location = new Point(255, 92),
                Size = new Size(180, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            lblStatus = new Label
            {
                Text = "Trạng thái",
                AutoSize = true,
                Location = new Point(450, 70)
            };

            cboStatus = new ComboBox
            {
                Location = new Point(450, 92),
                Size = new Size(140, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(610, 90),
                Size = new Size(95, 32),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            btnReload = new Button
            {
                Text = "Tải lại",
                Location = new Point(715, 90),
                Size = new Size(85, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += BtnReload_Click;

            btnAdd = new Button
            {
                Text = "Thêm",
                Location = new Point(810, 90),
                Size = new Size(75, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Sửa",
                Location = new Point(895, 90),
                Size = new Size(75, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Xóa",
                Location = new Point(980, 90),
                Size = new Size(75, 32),
                BackColor = Color.FromArgb(220, 80, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(950, 470),
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
            UiGridHelper.ApplyResponsiveStyle(dgvProducts);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblKeyword);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cboCategory);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cboStatus);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(dgvProducts);

            this.Load += FrmProducts_Load;
            this.Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void BuildGridColumns()
        {
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MaSP",
                Width = 70
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 220
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaVach",
                HeaderText = "Mã vạch",
                DataPropertyName = "MaVach",
                Width = 120
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonViTinh",
                HeaderText = "ĐVT",
                DataPropertyName = "DonViTinh",
                Width = 80
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhap",
                HeaderText = "Giá nhập",
                DataPropertyName = "GiaNhap",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaBan",
                HeaderText = "Giá bán",
                DataPropertyName = "GiaBan",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongTon",
                HeaderText = "Tồn kho",
                DataPropertyName = "SoLuongTon",
                Width = 80
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenLoai",
                HeaderText = "Danh mục",
                DataPropertyName = "TenLoai",
                Width = 120
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HanSuDungText",
                HeaderText = "Hạn sử dụng",
                DataPropertyName = "HanSuDungText",
                Width = 110
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThaiText",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThaiText",
                Width = 90
            });
        }

        private void FrmProducts_Load(object sender, EventArgs e)
        {
            ApplyRoleAccess();
            LoadCategoryFilter();
            LoadStatusFilter();
            LoadProducts();
            UpdateResponsiveLayout();
        }

        private void ApplyRoleAccess()
        {
            bool canManageProducts = !SessionManager.IsStaff;
            btnAdd.Visible = canManageProducts;
            btnEdit.Visible = canManageProducts;
            btnDelete.Visible = canManageProducts;
        }

        private void LoadCategoryFilter()
        {
            _categories = _categoryService.GetAll().ToList();

            cboCategory.Items.Clear();
            cboCategory.Items.Add(new ComboBoxItem(0, "Tất cả"));

            foreach (var category in _categories)
            {
                cboCategory.Items.Add(new ComboBoxItem(category.MaLoai, category.TenLoai));
            }

            cboCategory.SelectedIndex = 0;
        }

        private void LoadStatusFilter()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add(new ComboBoxItem(-1, "Tất cả"));
            cboStatus.Items.Add(new ComboBoxItem(1, "Đang bán"));
            cboStatus.Items.Add(new ComboBoxItem(0, "Ngừng bán"));
            cboStatus.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            var products = _productService.GetAll().ToList();
            BindGrid(products);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboCategory.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            LoadProducts();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new frmProductEdit())
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            int maSP = GetSelectedProductId("sửa");
            if (maSP <= 0)
            {
                return;
            }

            ProductDTO product = _productService.GetById(maSP);
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm.", "Thông báo");
                return;
            }

            using (var frm = new frmProductEdit(product))
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int maSP = GetSelectedProductId("xóa");
            if (maSP <= 0)
            {
                return;
            }

            ProductDTO product = _productService.GetById(maSP);
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm.", "Thông báo");
                return;
            }

            bool isActiveProduct = product.TrangThai;
            string message = isActiveProduct
                ? string.Format("Bạn có chắc muốn chuyển sản phẩm \"{0}\" sang Ngừng bán?", product.TenSP)
                : string.Format("Bạn có chắc muốn xóa vĩnh viễn sản phẩm \"{0}\"?", product.TenSP);

            DialogResult confirm = MessageBox.Show(
                message,
                isActiveProduct ? "Xác nhận ngừng bán" : "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            OperationResult result = _productService.Delete(maSP);

            if (result.IsSuccess)
            {
                SearchProducts();
                return;
            }

            MessageBox.Show(
                result.Message,
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private int GetSelectedProductId(string actionName)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần " + actionName + ".", "Thông báo");
                return 0;
            }

            object cellValue = dgvProducts.CurrentRow.Cells["MaSP"].Value;
            if (cellValue == null)
            {
                MessageBox.Show("Không xác định được sản phẩm cần " + actionName + ".", "Thông báo");
                return 0;
            }

            int maSP;
            if (!int.TryParse(cellValue.ToString(), out maSP))
            {
                MessageBox.Show("Dữ liệu sản phẩm không hợp lệ.", "Thông báo");
                return 0;
            }

            return maSP;
        }

        private void SearchProducts()
        {
            var request = new ProductSearchRequest
            {
                Keyword = txtKeyword.Text.Trim()
            };

            if (cboCategory.SelectedItem is ComboBoxItem categoryItem && categoryItem.Value > 0)
            {
                request.MaLoai = categoryItem.Value;
            }

            if (cboStatus.SelectedItem is ComboBoxItem statusItem)
            {
                if (statusItem.Value == 1)
                {
                    request.TrangThai = true;
                }
                else if (statusItem.Value == 0)
                {
                    request.TrangThai = false;
                }
            }

            var products = _productService.Search(request).ToList();
            BindGrid(products);
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtKeyword.Text = keyword ?? string.Empty;
            SearchProducts();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                return;
            }

            txtKeyword.Clear();
            SearchProducts();
        }

        private void BindGrid(List<ProductDTO> products)
        {
            var viewData = products.Select(p => new
            {
                p.MaSP,
                p.TenSP,
                p.MaVach,
                p.DonViTinh,
                p.GiaNhap,
                p.GiaBan,
                p.SoLuongTon,
                TenLoai = GetCategoryName(p.MaLoai),
                HanSuDungText = p.HanSuDung.HasValue ? p.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                TrangThaiText = p.TrangThai ? "Đang bán" : "Ngừng bán"
            }).ToList();

            dgvProducts.DataSource = null;
            dgvProducts.DataSource = viewData;
        }

        private string GetCategoryName(int maLoai)
        {
            var category = _categories?.FirstOrDefault(x => x.MaLoai == maLoai);
            return category != null ? category.TenLoai : string.Empty;
        }

        private void UpdateResponsiveLayout()
        {
            int left = 20;
            int right = 20;
            int buttonTop = 90;
            int gap = 10;
            int filterGap = 15;
            int preferredKeywordWidth = 220;
            int preferredCategoryWidth = 180;
            int preferredStatusWidth = 140;
            int minKeywordWidth = 150;
            int minCategoryWidth = 125;
            int minStatusWidth = 100;
            int preferredFilterWidth = preferredKeywordWidth + preferredCategoryWidth + preferredStatusWidth + (filterGap * 2);
            int minFilterWidth = minKeywordWidth + minCategoryWidth + minStatusWidth + (filterGap * 2);
            int buttonGroupWidth = btnSearch.Width + btnReload.Width + gap;

            if (btnAdd.Visible)
            {
                buttonGroupWidth += btnAdd.Width + gap;
            }

            if (btnEdit.Visible)
            {
                buttonGroupWidth += btnEdit.Width + gap;
            }

            if (btnDelete.Visible)
            {
                buttonGroupWidth += btnDelete.Width + gap;
            }

            bool splitButtonRow = ClientSize.Width - left - right < minFilterWidth + filterGap + buttonGroupWidth;
            int gridTop = splitButtonRow ? 180 : 140;

            if (splitButtonRow)
            {
                buttonTop = 130;
            }

            int x = ClientSize.Width - right;

            if (btnDelete.Visible)
            {
                btnDelete.Location = new Point(x - btnDelete.Width, buttonTop);
                x = btnDelete.Left - gap;
            }

            if (btnEdit.Visible)
            {
                btnEdit.Location = new Point(x - btnEdit.Width, buttonTop);
                x = btnEdit.Left - gap;
            }

            if (btnAdd.Visible)
            {
                btnAdd.Location = new Point(x - btnAdd.Width, buttonTop);
                x = btnAdd.Left - gap;
            }

            btnReload.Location = new Point(x - btnReload.Width, buttonTop);
            x = btnReload.Left - gap;

            btnSearch.Location = new Point(x - btnSearch.Width, buttonTop);

            int filterRight = splitButtonRow
                ? ClientSize.Width - right
                : btnSearch.Left - filterGap;
            int availableFilterWidth = Math.Max(260, filterRight - left);
            int keywordWidth = preferredKeywordWidth;
            int categoryWidth = preferredCategoryWidth;
            int statusWidth = preferredStatusWidth;

            if (availableFilterWidth < preferredFilterWidth)
            {
                if (availableFilterWidth >= minFilterWidth)
                {
                    int extraWidth = availableFilterWidth - minFilterWidth;
                    keywordWidth = minKeywordWidth + (extraWidth / 2);
                    categoryWidth = minCategoryWidth + (extraWidth / 3);
                    statusWidth = availableFilterWidth - keywordWidth - categoryWidth - (filterGap * 2);
                }
                else
                {
                    int narrowGap = 8;
                    int fieldWidth = Math.Max(70, (availableFilterWidth - (narrowGap * 2)) / 3);
                    filterGap = narrowGap;
                    keywordWidth = fieldWidth;
                    categoryWidth = fieldWidth;
                    statusWidth = Math.Max(70, availableFilterWidth - keywordWidth - categoryWidth - (filterGap * 2));
                }
            }

            lblKeyword.Location = new Point(left, 70);
            txtKeyword.SetBounds(left, 92, keywordWidth, txtKeyword.Height);

            int categoryLeft = txtKeyword.Right + filterGap;
            lblCategory.Location = new Point(categoryLeft, 70);
            cboCategory.SetBounds(categoryLeft, 92, categoryWidth, cboCategory.Height);

            int statusLeft = cboCategory.Right + filterGap;
            lblStatus.Location = new Point(statusLeft, 70);
            cboStatus.SetBounds(statusLeft, 92, statusWidth, cboStatus.Height);

            dgvProducts.SetBounds(
                left,
                gridTop,
                Math.Max(320, ClientSize.Width - left - right),
                Math.Max(220, ClientSize.Height - gridTop - 20));
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
