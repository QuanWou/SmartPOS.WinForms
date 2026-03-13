using System;
using System.Drawing;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Products
{
    public class frmCategoryEdit : Form
    {
        private readonly ICategoryService _categoryService;
        private readonly CategoryDTO _editingCategory;
        private readonly bool _isEditMode;

        public bool IsSavedSuccessfully { get; private set; }

        private Label lblTitle;
        private Label lblTenLoai;
        private Label lblMoTa;

        private TextBox txtTenLoai;
        private TextBox txtMoTa;
        private CheckBox chkTrangThai;

        private Button btnSave;
        private Button btnClose;

        public frmCategoryEdit()
        {
            _categoryService = new CategoryService();
            _isEditMode = false;

            InitializeComponent();
        }

        public frmCategoryEdit(CategoryDTO category)
        {
            _categoryService = new CategoryService();
            _editingCategory = category;
            _isEditMode = category != null;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Cập nhật danh mục" : "Thêm danh mục";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(520, 380);
            this.MinimumSize = new Size(520, 380);
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = _isEditMode ? "Cập nhật danh mục" : "Thêm danh mục",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblTenLoai = new Label
            {
                Text = "Tên danh mục",
                AutoSize = true,
                Location = new Point(24, 75)
            };

            txtTenLoai = new TextBox
            {
                Location = new Point(24, 97),
                Size = new Size(450, 28)
            };

            lblMoTa = new Label
            {
                Text = "Mô tả",
                AutoSize = true,
                Location = new Point(24, 135)
            };

            txtMoTa = new TextBox
            {
                Location = new Point(24, 157),
                Size = new Size(450, 90),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            chkTrangThai = new CheckBox
            {
                Text = "Đang hoạt động",
                AutoSize = true,
                Location = new Point(24, 265),
                Checked = true
            };

            btnSave = new Button
            {
                Text = _isEditMode ? "Cập nhật" : "Lưu",
                Location = new Point(264, 300),
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
                Location = new Point(374, 300),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTenLoai);
            this.Controls.Add(txtTenLoai);
            this.Controls.Add(lblMoTa);
            this.Controls.Add(txtMoTa);
            this.Controls.Add(chkTrangThai);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnClose);

            this.Load += FrmCategoryEdit_Load;
        }

        private void FrmCategoryEdit_Load(object sender, EventArgs e)
        {
            if (_isEditMode && _editingCategory != null)
            {
                txtTenLoai.Text = _editingCategory.TenLoai;
                txtMoTa.Text = _editingCategory.MoTa;
                chkTrangThai.Checked = _editingCategory.TrangThai;
            }
            else
            {
                chkTrangThai.Checked = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                OperationResult result;

                if (_isEditMode && _editingCategory != null)
                {
                    CategoryDTO category = new CategoryDTO
                    {
                        MaLoai = _editingCategory.MaLoai,
                        TenLoai = txtTenLoai.Text,
                        MoTa = txtMoTa.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    result = _categoryService.Update(category);
                }
                else
                {
                    CategoryDTO category = new CategoryDTO
                    {
                        TenLoai = txtTenLoai.Text,
                        MoTa = txtMoTa.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    result = _categoryService.Insert(category);
                }

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
    }
}