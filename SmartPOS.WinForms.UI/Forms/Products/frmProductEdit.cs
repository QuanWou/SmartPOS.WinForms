using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Products
{
    public class frmProductEdit : Form
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ProductDTO _editingProduct;
        private readonly bool _isEditMode;

        public bool IsSavedSuccessfully { get; private set; }

        private Label lblTitle;
        private Label lblTenSP;
        private Label lblMaVach;
        private Label lblDonViTinh;
        private Label lblGiaNhap;
        private Label lblGiaBan;
        private Label lblSoLuongTon;
        private Label lblMaLoai;
        private Label lblHinhAnh;
        private Label lblHanSuDung;
        private Label lblMoTa;

        private TextBox txtTenSP;
        private TextBox txtMaVach;
        private TextBox txtDonViTinh;
        private TextBox txtGiaNhap;
        private TextBox txtGiaBan;
        private TextBox txtSoLuongTon;
        private ComboBox cboMaLoai;
        private TextBox txtHinhAnh;
        private DateTimePicker dtpHanSuDung;
        private TextBox txtMoTa;
        private CheckBox chkTrangThai;

        private Button btnSave;
        private Button btnClose;

        private List<CategoryDTO> _categories;

        public frmProductEdit()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            _isEditMode = false;

            InitializeComponent();
        }

        public frmProductEdit(ProductDTO product)
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            _editingProduct = product;
            _isEditMode = product != null;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Cập nhật sản phẩm" : "Thêm sản phẩm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(620, 680);
            this.MinimumSize = new Size(620, 680);
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = _isEditMode ? "Cập nhật sản phẩm" : "Thêm sản phẩm",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblTenSP = new Label { Text = "Tên sản phẩm", AutoSize = true, Location = new Point(24, 75) };
            txtTenSP = new TextBox { Location = new Point(24, 97), Size = new Size(550, 28) };

            lblMaVach = new Label { Text = "Mã vạch", AutoSize = true, Location = new Point(24, 135) };
            txtMaVach = new TextBox { Location = new Point(24, 157), Size = new Size(250, 28) };

            lblDonViTinh = new Label { Text = "Đơn vị tính", AutoSize = true, Location = new Point(324, 135) };
            txtDonViTinh = new TextBox { Location = new Point(324, 157), Size = new Size(250, 28) };

            lblGiaNhap = new Label { Text = "Giá nhập", AutoSize = true, Location = new Point(24, 195) };
            txtGiaNhap = new TextBox { Location = new Point(24, 217), Size = new Size(170, 28) };

            lblGiaBan = new Label { Text = "Giá bán", AutoSize = true, Location = new Point(214, 195) };
            txtGiaBan = new TextBox { Location = new Point(214, 217), Size = new Size(170, 28) };

            lblSoLuongTon = new Label { Text = "Tồn khả dụng", AutoSize = true, Location = new Point(404, 195) };
            txtSoLuongTon = new TextBox
            {
                Location = new Point(404, 217),
                Size = new Size(170, 28),
                ReadOnly = true,
                BackColor = Color.WhiteSmoke
            };

            lblMaLoai = new Label { Text = "Danh mục", AutoSize = true, Location = new Point(24, 255) };
            cboMaLoai = new ComboBox
            {
                Location = new Point(24, 277),
                Size = new Size(250, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            lblHinhAnh = new Label { Text = "Hình ảnh", AutoSize = true, Location = new Point(324, 255) };
            txtHinhAnh = new TextBox { Location = new Point(324, 277), Size = new Size(250, 28) };

            lblHanSuDung = new Label { Text = "HSD gần nhất theo lô", AutoSize = true, Location = new Point(24, 315) };
            dtpHanSuDung = new DateTimePicker
            {
                Location = new Point(24, 337),
                Size = new Size(250, 28),
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Enabled = false
            };

            lblMoTa = new Label { Text = "Mô tả", AutoSize = true, Location = new Point(24, 375) };
            txtMoTa = new TextBox
            {
                Location = new Point(24, 397),
                Size = new Size(550, 110),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            chkTrangThai = new CheckBox
            {
                Text = "Đang kinh doanh",
                AutoSize = true,
                Location = new Point(24, 522),
                Checked = true
            };

            btnSave = new Button
            {
                Text = _isEditMode ? "Cập nhật" : "Lưu",
                Location = new Point(364, 580),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(474, 580),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTenSP);
            this.Controls.Add(txtTenSP);
            this.Controls.Add(lblMaVach);
            this.Controls.Add(txtMaVach);
            this.Controls.Add(lblDonViTinh);
            this.Controls.Add(txtDonViTinh);
            this.Controls.Add(lblGiaNhap);
            this.Controls.Add(txtGiaNhap);
            this.Controls.Add(lblGiaBan);
            this.Controls.Add(txtGiaBan);
            this.Controls.Add(lblSoLuongTon);
            this.Controls.Add(txtSoLuongTon);
            this.Controls.Add(lblMaLoai);
            this.Controls.Add(cboMaLoai);
            this.Controls.Add(lblHinhAnh);
            this.Controls.Add(txtHinhAnh);
            this.Controls.Add(lblHanSuDung);
            this.Controls.Add(dtpHanSuDung);
            this.Controls.Add(lblMoTa);
            this.Controls.Add(txtMoTa);
            this.Controls.Add(chkTrangThai);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnClose);

            this.Load += FrmProductEdit_Load;
        }

        private void FrmProductEdit_Load(object sender, EventArgs e)
        {
            LoadCategories();

            if (_isEditMode && _editingProduct != null)
            {
                txtTenSP.Text = _editingProduct.TenSP;
                txtMaVach.Text = _editingProduct.MaVach;
                txtDonViTinh.Text = _editingProduct.DonViTinh;
                txtGiaNhap.Text = _editingProduct.GiaNhap.ToString("0.##");
                txtGiaBan.Text = _editingProduct.GiaBan.ToString("0.##");
                txtSoLuongTon.Text = _editingProduct.SoLuongTon.ToString();
                txtHinhAnh.Text = _editingProduct.HinhAnh;
                txtMoTa.Text = _editingProduct.MoTa;
                chkTrangThai.Checked = _editingProduct.TrangThai;
                dtpHanSuDung.Checked = _editingProduct.HanSuDung.HasValue;
                if (_editingProduct.HanSuDung.HasValue)
                {
                    dtpHanSuDung.Value = _editingProduct.HanSuDung.Value;
                }

                SelectCategory(_editingProduct.MaLoai);
            }
            else
            {
                chkTrangThai.Checked = true;
                dtpHanSuDung.Checked = false;
                txtSoLuongTon.Text = "0";
                if (cboMaLoai.Items.Count > 0)
                {
                    cboMaLoai.SelectedIndex = 0;
                }
            }
        }

        private void LoadCategories()
        {
            _categories = _categoryService.GetAll().ToList();

            cboMaLoai.Items.Clear();
            foreach (var category in _categories)
            {
                cboMaLoai.Items.Add(new ComboBoxItem(category.MaLoai, category.TenLoai));
            }
        }

        private void SelectCategory(int maLoai)
        {
            for (int i = 0; i < cboMaLoai.Items.Count; i++)
            {
                var item = cboMaLoai.Items[i] as ComboBoxItem;
                if (item != null && item.Value == maLoai)
                {
                    cboMaLoai.SelectedIndex = i;
                    return;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                decimal giaNhap;
                decimal giaBan;
                int soLuongTon;

                if (!decimal.TryParse(txtGiaNhap.Text.Trim(), out giaNhap))
                {
                    MessageBox.Show("Giá nhập không hợp lệ.", "Thông báo");
                    txtGiaNhap.Focus();
                    return;
                }

                if (!decimal.TryParse(txtGiaBan.Text.Trim(), out giaBan))
                {
                    MessageBox.Show("Giá bán không hợp lệ.", "Thông báo");
                    txtGiaBan.Focus();
                    return;
                }

                if (!int.TryParse(txtSoLuongTon.Text.Trim(), out soLuongTon))
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ.", "Thông báo");
                    txtSoLuongTon.Focus();
                    return;
                }

                int maLoai = 0;
                if (cboMaLoai.SelectedItem is ComboBoxItem selectedCategory)
                {
                    maLoai = selectedCategory.Value;
                }

                ProductDTO product = new ProductDTO
                {
                    MaSP = _isEditMode && _editingProduct != null ? _editingProduct.MaSP : 0,
                    TenSP = txtTenSP.Text,
                    MaVach = txtMaVach.Text,
                    DonViTinh = txtDonViTinh.Text,
                    GiaNhap = giaNhap,
                    GiaBan = giaBan,
                    SoLuongTon = soLuongTon,
                    MaLoai = maLoai,
                    HinhAnh = txtHinhAnh.Text,
                    MoTa = txtMoTa.Text,
                    HanSuDung = dtpHanSuDung.Checked ? (DateTime?)dtpHanSuDung.Value.Date : null,
                    TrangThai = chkTrangThai.Checked,
                    NgayTao = _isEditMode && _editingProduct != null ? _editingProduct.NgayTao : DateTime.Now
                };

                OperationResult result = _isEditMode
                    ? _productService.Update(product)
                    : _productService.Insert(product);

                MessageBox.Show(
                    result.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.IsSuccess)
                {
                    IsSavedSuccessfully = true;
                    this.Close();
                }
            }
            finally
            {
                btnSave.Enabled = true;
            }
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
