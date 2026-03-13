using System;
using System.Drawing;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Constants;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Users
{
    public class frmUserEdit : Form
    {
        private readonly IUserService _userService;
        private readonly UserDTO _editingUser;
        private readonly bool _isEditMode;

        public bool IsSavedSuccessfully { get; private set; }

        private Label lblTitle;
        private Label lblTenNV;
        private Label lblTaiKhoan;
        private Label lblMatKhau;
        private Label lblQuyen;
        private Label lblSoDienThoai;
        private Label lblDiaChi;

        private TextBox txtTenNV;
        private TextBox txtTaiKhoan;
        private TextBox txtMatKhau;
        private ComboBox cboQuyen;
        private TextBox txtSoDienThoai;
        private TextBox txtDiaChi;
        private CheckBox chkTrangThai;

        private Button btnSave;
        private Button btnClose;

        public frmUserEdit()
        {
            _userService = new UserService();
            _isEditMode = false;

            InitializeComponent();
        }

        public frmUserEdit(UserDTO user)
        {
            _userService = new UserService();
            _editingUser = user;
            _isEditMode = user != null;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Cập nhật người dùng" : "Thêm người dùng";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(520, 500);
            this.MinimumSize = new Size(520, 500);
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = _isEditMode ? "Cập nhật người dùng" : "Thêm người dùng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblTenNV = new Label
            {
                Text = "Tên nhân viên",
                AutoSize = true,
                Location = new Point(24, 75)
            };

            txtTenNV = new TextBox
            {
                Location = new Point(24, 97),
                Size = new Size(450, 28)
            };

            lblTaiKhoan = new Label
            {
                Text = "Tài khoản",
                AutoSize = true,
                Location = new Point(24, 135)
            };

            txtTaiKhoan = new TextBox
            {
                Location = new Point(24, 157),
                Size = new Size(210, 28)
            };

            lblMatKhau = new Label
            {
                Text = _isEditMode ? "Mật khẩu mới (để trống nếu không đổi)" : "Mật khẩu",
                AutoSize = true,
                Location = new Point(248, 135)
            };

            txtMatKhau = new TextBox
            {
                Location = new Point(248, 157),
                Size = new Size(226, 28),
                UseSystemPasswordChar = true
            };

            lblQuyen = new Label
            {
                Text = "Quyền",
                AutoSize = true,
                Location = new Point(24, 195)
            };

            cboQuyen = new ComboBox
            {
                Location = new Point(24, 217),
                Size = new Size(210, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboQuyen.Items.Add(RoleConstants.Admin);
            cboQuyen.Items.Add(RoleConstants.Staff);

            lblSoDienThoai = new Label
            {
                Text = "Số điện thoại",
                AutoSize = true,
                Location = new Point(248, 195)
            };

            txtSoDienThoai = new TextBox
            {
                Location = new Point(248, 217),
                Size = new Size(226, 28)
            };

            lblDiaChi = new Label
            {
                Text = "Địa chỉ",
                AutoSize = true,
                Location = new Point(24, 255)
            };

            txtDiaChi = new TextBox
            {
                Location = new Point(24, 277),
                Size = new Size(450, 28)
            };

            chkTrangThai = new CheckBox
            {
                Text = "Đang hoạt động",
                AutoSize = true,
                Location = new Point(24, 322),
                Checked = true
            };

            btnSave = new Button
            {
                Text = _isEditMode ? "Cập nhật" : "Lưu",
                Location = new Point(264, 380),
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
                Location = new Point(374, 380),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTenNV);
            this.Controls.Add(txtTenNV);
            this.Controls.Add(lblTaiKhoan);
            this.Controls.Add(txtTaiKhoan);
            this.Controls.Add(lblMatKhau);
            this.Controls.Add(txtMatKhau);
            this.Controls.Add(lblQuyen);
            this.Controls.Add(cboQuyen);
            this.Controls.Add(lblSoDienThoai);
            this.Controls.Add(txtSoDienThoai);
            this.Controls.Add(lblDiaChi);
            this.Controls.Add(txtDiaChi);
            this.Controls.Add(chkTrangThai);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnClose);

            this.Load += FrmUserEdit_Load;
        }

        private void FrmUserEdit_Load(object sender, EventArgs e)
        {
            if (_isEditMode && _editingUser != null)
            {
                txtTenNV.Text = _editingUser.TenNV;
                txtTaiKhoan.Text = _editingUser.TaiKhoan;
                txtMatKhau.Text = string.Empty;
                cboQuyen.SelectedItem = _editingUser.Quyen;
                txtSoDienThoai.Text = _editingUser.SoDienThoai;
                txtDiaChi.Text = _editingUser.DiaChi;
                chkTrangThai.Checked = _editingUser.TrangThai;
            }
            else
            {
                cboQuyen.SelectedIndex = 0;
                chkTrangThai.Checked = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                OperationResult result;

                if (_isEditMode && _editingUser != null)
                {
                    var user = new UserDTO
                    {
                        MaNV = _editingUser.MaNV,
                        TenNV = txtTenNV.Text,
                        TaiKhoan = txtTaiKhoan.Text,
                        MatKhauHash = txtMatKhau.Text,
                        Quyen = cboQuyen.SelectedItem?.ToString(),
                        SoDienThoai = txtSoDienThoai.Text,
                        DiaChi = txtDiaChi.Text,
                        TrangThai = chkTrangThai.Checked,
                        NgayTao = _editingUser.NgayTao
                    };

                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        user.MatKhauHash = _editingUser.MatKhauHash;
                    }

                    result = _userService.Update(user);
                }
                else
                {
                    var user = new UserDTO
                    {
                        TenNV = txtTenNV.Text,
                        TaiKhoan = txtTaiKhoan.Text,
                        MatKhauHash = txtMatKhau.Text,
                        Quyen = cboQuyen.SelectedItem?.ToString(),
                        SoDienThoai = txtSoDienThoai.Text,
                        DiaChi = txtDiaChi.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    result = _userService.Insert(user);
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