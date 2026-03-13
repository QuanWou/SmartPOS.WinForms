using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.UI.Forms.Users
{
    public class frmUsers : Form
    {
        private readonly IUserService _userService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblKeyword;
        private Label lblRole;
        private Label lblStatus;

        private TextBox txtKeyword;
        private ComboBox cboRole;
        private ComboBox cboStatus;

        private Button btnSearch;
        private Button btnReload;
        private Button btnAdd;
        private Button btnEdit;

        private DataGridView dgvUsers;

        private List<UserDTO> _users;

        public frmUsers()
        {
            _userService = new UserService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Người dùng";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Quản lý người dùng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi tài khoản đăng nhập và phân quyền hệ thống",
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

            lblRole = new Label
            {
                Text = "Quyền",
                AutoSize = true,
                Location = new Point(255, 85)
            };

            cboRole = new ComboBox
            {
                Location = new Point(255, 107),
                Size = new Size(140, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            lblStatus = new Label
            {
                Text = "Trạng thái",
                AutoSize = true,
                Location = new Point(410, 85)
            };

            cboStatus = new ComboBox
            {
                Location = new Point(410, 107),
                Size = new Size(140, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(570, 104),
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
                Location = new Point(670, 104),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += BtnReload_Click;

            btnAdd = new Button
            {
                Text = "Thêm",
                Location = new Point(760, 104),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Sửa",
                Location = new Point(840, 104),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;

            dgvUsers = new DataGridView
            {
                Location = new Point(20, 155),
                Size = new Size(960, 455),
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
            this.Controls.Add(lblRole);
            this.Controls.Add(cboRole);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cboStatus);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(dgvUsers);

            this.Load += FrmUsers_Load;
        }

        private void FrmUsers_Load(object sender, EventArgs e)
        {
            LoadRoleFilter();
            LoadStatusFilter();
            LoadUsers();
        }

        private void LoadRoleFilter()
        {
            cboRole.Items.Clear();
            cboRole.Items.Add(new ComboBoxItem(string.Empty, "Tất cả"));
            cboRole.Items.Add(new ComboBoxItem(RoleConstants.Admin, "Admin"));
            cboRole.Items.Add(new ComboBoxItem(RoleConstants.Staff, "Staff"));
            cboRole.SelectedIndex = 0;
        }

        private void LoadStatusFilter()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add(new ComboBoxItem(string.Empty, "Tất cả"));
            cboStatus.Items.Add(new ComboBoxItem("1", "Đang hoạt động"));
            cboStatus.Items.Add(new ComboBoxItem("0", "Đã khóa"));
            cboStatus.SelectedIndex = 0;
        }

        private void LoadUsers()
        {
            _users = _userService.GetAll().ToList();
            BindGrid(_users);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchUsers();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboRole.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            LoadUsers();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new frmUserEdit())
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadUsers();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa.", "Thông báo");
                return;
            }

            object cellValue = dgvUsers.CurrentRow.Cells["MaNV"].Value;
            if (cellValue == null)
            {
                MessageBox.Show("Không xác định được người dùng cần sửa.", "Thông báo");
                return;
            }

            int maNV;
            if (!int.TryParse(cellValue.ToString(), out maNV))
            {
                MessageBox.Show("Dữ liệu người dùng không hợp lệ.", "Thông báo");
                return;
            }

            UserDTO user = _userService.GetById(maNV);
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng.", "Thông báo");
                return;
            }

            using (var frm = new frmUserEdit(user))
            {
                frm.ShowDialog(this);

                if (frm.IsSavedSuccessfully)
                {
                    LoadUsers();
                }
            }
        }

        private void SearchUsers()
        {
            IEnumerable<UserDTO> query = _users ?? new List<UserDTO>();

            string keyword = txtKeyword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.TenNV) && x.TenNV.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(x.TaiKhoan) && x.TaiKhoan.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(x.SoDienThoai) && x.SoDienThoai.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            if (cboRole.SelectedItem is ComboBoxItem roleItem && !string.IsNullOrWhiteSpace(roleItem.Value))
            {
                query = query.Where(x => string.Equals(x.Quyen, roleItem.Value, StringComparison.OrdinalIgnoreCase));
            }

            if (cboStatus.SelectedItem is ComboBoxItem statusItem && !string.IsNullOrWhiteSpace(statusItem.Value))
            {
                bool isActive = statusItem.Value == "1";
                query = query.Where(x => x.TrangThai == isActive);
            }

            BindGrid(query.ToList());
        }

        private void BindGrid(List<UserDTO> users)
        {
            var viewData = users.Select(x => new
            {
                x.MaNV,
                x.TenNV,
                x.TaiKhoan,
                x.Quyen,
                x.SoDienThoai,
                x.DiaChi,
                TrangThaiText = x.TrangThai ? "Đang hoạt động" : "Đã khóa"
            }).ToList();

            dgvUsers.DataSource = null;
            dgvUsers.DataSource = viewData;
        }

        private void BuildGridColumns()
        {
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaNV",
                HeaderText = "Mã NV",
                DataPropertyName = "MaNV",
                Width = 80
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNV",
                HeaderText = "Tên nhân viên",
                DataPropertyName = "TenNV",
                Width = 220
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TaiKhoan",
                HeaderText = "Tài khoản",
                DataPropertyName = "TaiKhoan",
                Width = 150
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quyen",
                HeaderText = "Quyền",
                DataPropertyName = "Quyen",
                Width = 100
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoDienThoai",
                HeaderText = "Số điện thoại",
                DataPropertyName = "SoDienThoai",
                Width = 130
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiaChi",
                HeaderText = "Địa chỉ",
                DataPropertyName = "DiaChi",
                Width = 180
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThaiText",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThaiText",
                Width = 120
            });
        }

        private class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public ComboBoxItem(string value, string text)
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