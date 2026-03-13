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
    public class frmCategories : Form
    {
        private readonly ICategoryService _categoryService;

        private Label lblTitle;
        private Label lblTenLoai;
        private Label lblMoTa;
        private Label lblTrangThai;

        private TextBox txtTenLoai;
        private TextBox txtMoTa;
        private CheckBox chkTrangThai;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnClear;

        private DataGridView dgvCategories;

        private int _selectedCategoryId = 0;

        public frmCategories()
        {
            _categoryService = new CategoryService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý danh mục";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Quản lý danh mục",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblTenLoai = new Label
            {
                Text = "Tên danh mục",
                AutoSize = true,
                Location = new Point(20, 70)
            };

            txtTenLoai = new TextBox
            {
                Location = new Point(20, 92),
                Size = new Size(260, 27)
            };

            lblMoTa = new Label
            {
                Text = "Mô tả",
                AutoSize = true,
                Location = new Point(300, 70)
            };

            txtMoTa = new TextBox
            {
                Location = new Point(300, 92),
                Size = new Size(320, 27)
            };

            lblTrangThai = new Label
            {
                Text = "Trạng thái",
                AutoSize = true,
                Location = new Point(640, 70)
            };

            chkTrangThai = new CheckBox
            {
                Text = "Đang hoạt động",
                AutoSize = true,
                Location = new Point(640, 95),
                Checked = true
            };

            btnAdd = new Button
            {
                Text = "Thêm",
                Location = new Point(20, 135),
                Size = new Size(85, 34),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button
            {
                Text = "Cập nhật",
                Location = new Point(115, 135),
                Size = new Size(95, 34),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += BtnUpdate_Click;

            btnClear = new Button
            {
                Text = "Làm mới",
                Location = new Point(220, 135),
                Size = new Size(90, 34),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            dgvCategories = new DataGridView
            {
                Location = new Point(20, 190),
                Size = new Size(920, 420),
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
            dgvCategories.SelectionChanged += DgvCategories_SelectionChanged;

            BuildGridColumns();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTenLoai);
            this.Controls.Add(txtTenLoai);
            this.Controls.Add(lblMoTa);
            this.Controls.Add(txtMoTa);
            this.Controls.Add(lblTrangThai);
            this.Controls.Add(chkTrangThai);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnClear);
            this.Controls.Add(dgvCategories);

            this.Load += FrmCategories_Load;
        }

        private void BuildGridColumns()
        {
            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaLoai",
                HeaderText = "Mã loại",
                DataPropertyName = "MaLoai",
                Width = 90
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenLoai",
                HeaderText = "Tên danh mục",
                DataPropertyName = "TenLoai",
                Width = 240
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MoTa",
                HeaderText = "Mô tả",
                DataPropertyName = "MoTa",
                Width = 390
            });

            dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThaiText",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThaiText",
                Width = 150
            });
        }

        private void FrmCategories_Load(object sender, EventArgs e)
        {
            LoadCategories();
            ResetForm();
        }

        private void LoadCategories()
        {
            List<CategoryDTO> categories = _categoryService.GetAll().ToList();

            var viewData = categories.Select(c => new
            {
                c.MaLoai,
                c.TenLoai,
                c.MoTa,
                TrangThaiText = c.TrangThai ? "Đang hoạt động" : "Ngừng hoạt động"
            }).ToList();

            dgvCategories.DataSource = null;
            dgvCategories.DataSource = viewData;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCategoryEdit())
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadCategories();
                    ResetForm();
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId <= 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần cập nhật.", "Thông báo");
                return;
            }

            CategoryDTO category = _categoryService.GetById(_selectedCategoryId);
            if (category == null)
            {
                MessageBox.Show("Không tìm thấy danh mục.", "Thông báo");
                return;
            }

            using (var frm = new frmCategoryEdit(category))
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadCategories();
                    ResetForm();
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void DgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow == null || dgvCategories.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            object cellValue = dgvCategories.CurrentRow.Cells["MaLoai"].Value;
            if (cellValue == null)
            {
                return;
            }

            int maLoai;
            if (!int.TryParse(cellValue.ToString(), out maLoai))
            {
                return;
            }

            CategoryDTO category = _categoryService.GetById(maLoai);
            if (category == null)
            {
                return;
            }

            _selectedCategoryId = category.MaLoai;
            txtTenLoai.Text = category.TenLoai;
            txtMoTa.Text = category.MoTa;
            chkTrangThai.Checked = category.TrangThai;
        }

        private void ResetForm()
        {
            _selectedCategoryId = 0;
            txtTenLoai.Clear();
            txtMoTa.Clear();
            chkTrangThai.Checked = true;
            txtTenLoai.Focus();
            dgvCategories.ClearSelection();
        }
    }
}