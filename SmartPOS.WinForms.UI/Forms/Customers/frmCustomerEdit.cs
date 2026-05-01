using System;
using System.Drawing;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Customers
{
    public class frmCustomerEdit : Form
    {
        private readonly ICustomerService _customerService;
        private readonly CustomerDTO _editingCustomer;
        private readonly bool _isEditMode;

        private Label lblTitle;
        private Label lblHoTen;
        private Label lblSoDienThoai;
        private Label lblDiaChi;
        private Label lblHint;
        private TextBox txtHoTen;
        private TextBox txtSoDienThoai;
        private TextBox txtDiaChi;
        private CheckBox chkTrangThai;
        private Button btnSave;
        private Button btnClose;

        public bool IsSavedSuccessfully { get; private set; }

        public int? SavedCustomerId { get; private set; }

        public frmCustomerEdit()
        {
            _customerService = new CustomerService();
            _isEditMode = false;
            InitializeComponent();
        }

        public frmCustomerEdit(CustomerDTO customer)
        {
            _customerService = new CustomerService();
            _editingCustomer = customer;
            _isEditMode = customer != null;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = _isEditMode ? "Cập nhật khách hàng" : "Thêm hội viên";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(540, 430);
            MinimumSize = new Size(540, 430);
            MaximizeBox = false;
            BackColor = Color.FromArgb(248, 249, 251);
            Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = _isEditMode ? "Cập nhật khách hàng" : "Thêm hội viên",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            lblHint = new Label
            {
                Text = "Số điện thoại là mã định danh hội viên và không được trùng.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 132, 160),
                AutoSize = true,
                Location = new Point(26, 54)
            };

            lblHoTen = MakeLabel("Họ tên", 24, 95);
            txtHoTen = MakeTextBox(24, 118, 470);

            lblSoDienThoai = MakeLabel("Số điện thoại", 24, 158);
            txtSoDienThoai = MakeTextBox(24, 181, 220);

            lblDiaChi = MakeLabel("Địa chỉ", 24, 221);
            txtDiaChi = MakeTextBox(24, 244, 470);

            chkTrangThai = new CheckBox
            {
                Text = "Đang hoạt động",
                AutoSize = true,
                Checked = true,
                Location = new Point(24, 292)
            };

            btnSave = new Button
            {
                Text = _isEditMode ? "Cập nhật" : "Lưu",
                Location = new Point(284, 330),
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
                Location = new Point(394, 330),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                lblTitle, lblHint, lblHoTen, txtHoTen, lblSoDienThoai, txtSoDienThoai,
                lblDiaChi, txtDiaChi, chkTrangThai, btnSave, btnClose
            });

            Load += FrmCustomerEdit_Load;
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private TextBox MakeTextBox(int x, int y, int width)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 28)
            };
        }

        private void FrmCustomerEdit_Load(object sender, EventArgs e)
        {
            if (!_isEditMode || _editingCustomer == null)
            {
                return;
            }

            txtHoTen.Text = _editingCustomer.HoTen;
            txtSoDienThoai.Text = _editingCustomer.SoDienThoai;
            txtDiaChi.Text = _editingCustomer.DiaChi;
            chkTrangThai.Checked = _editingCustomer.TrangThai;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                OperationResult result;
                if (_isEditMode && _editingCustomer != null)
                {
                    result = _customerService.Update(new CustomerDTO
                    {
                        MaKH = _editingCustomer.MaKH,
                        HoTen = txtHoTen.Text,
                        SoDienThoai = txtSoDienThoai.Text,
                        DiaChi = txtDiaChi.Text,
                        TrangThai = chkTrangThai.Checked
                    });
                }
                else
                {
                    result = _customerService.Insert(new CustomerDTO
                    {
                        HoTen = txtHoTen.Text,
                        SoDienThoai = txtSoDienThoai.Text,
                        DiaChi = txtDiaChi.Text,
                        TrangThai = true
                    });
                }

                MessageBox.Show(
                    result.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.IsSuccess)
                {
                    IsSavedSuccessfully = true;
                    SavedCustomerId = result.DataId ?? (_editingCustomer != null ? (int?)_editingCustomer.MaKH : null);
                    Close();
                }
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    }
}
