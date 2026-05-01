using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Customers
{
    public class frmCustomerLookup : Form
    {
        private readonly ICustomerService _customerService;
        private TextBox txtKeyword;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnSelect;
        private Button btnClose;
        private DataGridView dgvCustomers;
        private List<CustomerDTO> _customers;

        public CustomerDTO SelectedCustomer { get; private set; }

        public frmCustomerLookup()
        {
            _customerService = new CustomerService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Chọn khách hàng";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(760, 520);
            MinimumSize = new Size(680, 460);
            BackColor = Color.FromArgb(248, 249, 251);
            Font = new Font("Segoe UI", 9F);

            var lblTitle = new Label
            {
                Text = "Chọn khách hàng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            txtKeyword = new TextBox
            {
                Location = new Point(20, 70),
                Size = new Size(310, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            btnSearch = MakeButton("Tìm", Color.FromArgb(22, 32, 72), Color.White);
            btnSearch.Location = new Point(340, 68);
            btnSearch.Click += (s, e) => SearchCustomers();

            btnAdd = MakeButton("Thêm hội viên", Color.FromArgb(90, 110, 200), Color.White);
            btnAdd.Size = new Size(120, 32);
            btnAdd.Location = new Point(428, 68);
            btnAdd.Click += BtnAdd_Click;

            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 115),
                Size = new Size(700, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
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
            dgvCustomers.CellDoubleClick += (s, e) => SelectCurrentCustomer();
            BuildGridColumns();
            UiGridHelper.ApplyResponsiveStyle(dgvCustomers);

            btnSelect = MakeButton("Chọn", Color.FromArgb(22, 32, 72), Color.White);
            btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSelect.Click += (s, e) => SelectCurrentCustomer();

            btnClose = MakeButton("Đóng", Color.FromArgb(230, 233, 240), Color.Black);
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblTitle, txtKeyword, btnSearch, btnAdd, dgvCustomers, btnSelect, btnClose });

            Load += FrmCustomerLookup_Load;
            Resize += (s, e) => UpdateLayout();
        }

        private Button MakeButton(string text, Color bg, Color fg)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(78, 32),
                BackColor = bg,
                ForeColor = fg,
                FlatStyle = FlatStyle.Flat
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void BuildGridColumns()
        {
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaKH", HeaderText = "Mã KH", DataPropertyName = "MaKH", Width = 80 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "HoTen", HeaderText = "Họ tên", DataPropertyName = "HoTen", Width = 190 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoDienThoai", HeaderText = "SĐT", DataPropertyName = "SoDienThoai", Width = 120 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "HangThanhVien", HeaderText = "Hạng", DataPropertyName = "HangThanhVien", Width = 95 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiemHienCo", HeaderText = "Điểm", DataPropertyName = "DiemHienCo", Width = 80 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongChiTieuText", HeaderText = "Tổng chi", DataPropertyName = "TongChiTieuText", Width = 120 });
        }

        private void FrmCustomerLookup_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            UpdateLayout();
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAll().Where(x => x.TrangThai).ToList();
            BindGrid(_customers);
        }

        private void SearchCustomers()
        {
            var request = new CustomerSearchRequest
            {
                Keyword = txtKeyword.Text.Trim(),
                TrangThai = true
            };
            _customers = _customerService.Search(request).ToList();
            BindGrid(_customers);
        }

        private void BindGrid(List<CustomerDTO> customers)
        {
            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = customers.Select(x => new
            {
                x.MaKH,
                x.HoTen,
                x.SoDienThoai,
                x.HangThanhVien,
                x.DiemHienCo,
                TongChiTieuText = x.TongChiTieu.ToString("N0") + " đ"
            }).ToList();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCustomerEdit())
            {
                frm.ShowDialog(this);
                if (!frm.IsSavedSuccessfully)
                {
                    return;
                }

                LoadCustomers();
                if (frm.SavedCustomerId.HasValue)
                {
                    SelectedCustomer = _customerService.GetById(frm.SavedCustomerId.Value);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void SelectCurrentCustomer()
        {
            if (dgvCustomers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo");
                return;
            }

            object value = dgvCustomers.CurrentRow.Cells["MaKH"].Value;
            int maKH;
            if (value == null || !int.TryParse(value.ToString(), out maKH))
            {
                MessageBox.Show("Dữ liệu khách hàng không hợp lệ.", "Thông báo");
                return;
            }

            SelectedCustomer = _customerService.GetById(maKH);
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng.", "Thông báo");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void TxtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            SearchCustomers();
        }

        private void UpdateLayout()
        {
            dgvCustomers.SetBounds(20, 115, Math.Max(320, ClientSize.Width - 40), Math.Max(220, ClientSize.Height - 180));
            btnClose.Location = new Point(ClientSize.Width - btnClose.Width - 20, ClientSize.Height - btnClose.Height - 18);
            btnSelect.Location = new Point(btnClose.Left - btnSelect.Width - 10, btnClose.Top);
        }
    }
}
