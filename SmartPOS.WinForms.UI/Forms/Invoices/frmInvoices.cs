using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;

namespace SmartPOS.WinForms.UI.Forms.Invoices
{
    public class frmInvoices : Form
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblKeyword;
        private Label lblFromDate;
        private Label lblToDate;
        private Label lblStatus;

        private TextBox txtKeyword;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private ComboBox cboStatus;

        private Button btnSearch;
        private Button btnReload;
        private Button btnViewDetail;

        private DataGridView dgvInvoices;

        private List<InvoiceDTO> _invoices;
        private List<UserDTO> _users;

        public frmInvoices()
        {
            _invoiceService = new InvoiceService();
            _userService = new UserService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Hóa đơn";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Quản lý hóa đơn",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi danh sách hóa đơn bán hàng và trạng thái thanh toán",
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
                Size = new Size(180, 27)
            };

            lblFromDate = new Label
            {
                Text = "Từ ngày",
                AutoSize = true,
                Location = new Point(215, 85)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(215, 107),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblToDate = new Label
            {
                Text = "Đến ngày",
                AutoSize = true,
                Location = new Point(380, 85)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(380, 107),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblStatus = new Label
            {
                Text = "Trạng thái",
                AutoSize = true,
                Location = new Point(545, 85)
            };

            cboStatus = new ComboBox
            {
                Location = new Point(545, 107),
                Size = new Size(140, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(700, 104),
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
                Location = new Point(800, 104),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += BtnReload_Click;

            btnViewDetail = new Button
            {
                Text = "Xem chi tiết",
                Location = new Point(890, 104),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewDetail.FlatAppearance.BorderSize = 0;
            btnViewDetail.Click += BtnViewDetail_Click;

            dgvInvoices = new DataGridView
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
            this.Controls.Add(lblFromDate);
            this.Controls.Add(dtpFromDate);
            this.Controls.Add(lblToDate);
            this.Controls.Add(dtpToDate);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cboStatus);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnViewDetail);
            this.Controls.Add(dgvInvoices);

            this.Load += FrmInvoices_Load;
        }

        private void FrmInvoices_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsStaff)
            {
                lblTitle.Text = "Hóa đơn của tôi";
                lblSubtitle.Text = "Theo dõi các hóa đơn do bạn tạo và trạng thái thanh toán";
            }

            LoadStatusFilter();
            LoadData();

            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
        }

        private void LoadStatusFilter()
        {
            cboStatus.Items.Clear();
            cboStatus.Items.Add(new ComboBoxItem(string.Empty, "Tất cả"));
            cboStatus.Items.Add(new ComboBoxItem("Paid", "Đã thanh toán"));
            cboStatus.Items.Add(new ComboBoxItem("Cancelled", "Đã hủy"));
            cboStatus.SelectedIndex = 0;
        }

        private void LoadData()
        {
            _users = _userService.GetAll().ToList();
            _invoices = _invoiceService.GetAll().ToList();

            if (SessionManager.IsStaff && SessionManager.CurrentUser != null)
            {
                _invoices = _invoices
                    .Where(x => x.MaNV == SessionManager.CurrentUser.MaNV)
                    .ToList();
            }

            BindGrid(_invoices);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchInvoices();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboStatus.SelectedIndex = 0;
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            BindGrid(_invoices ?? new List<InvoiceDTO>());
        }

        private void BtnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvInvoices.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xem.", "Thông báo");
                return;
            }

            object cellValue = dgvInvoices.CurrentRow.Cells["MaHD"].Value;
            if (cellValue == null)
            {
                MessageBox.Show("Không xác định được hóa đơn.", "Thông báo");
                return;
            }

            int maHD;
            if (!int.TryParse(cellValue.ToString(), out maHD))
            {
                MessageBox.Show("Dữ liệu hóa đơn không hợp lệ.", "Thông báo");
                return;
            }

            using (var frm = new frmInvoiceDetails(maHD))
            {
                frm.ShowDialog(this);
            }
        }

        private void SearchInvoices()
        {
            IEnumerable<InvoiceDTO> query = _invoices ?? new List<InvoiceDTO>();

            string keyword = txtKeyword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.MaHD.ToString().Contains(keyword) ||
                    (!string.IsNullOrWhiteSpace(x.GhiChu) && x.GhiChu.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    GetNhanVienName(x.MaNV).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate);

            if (cboStatus.SelectedItem is ComboBoxItem statusItem && !string.IsNullOrWhiteSpace(statusItem.Value))
            {
                query = query.Where(x => string.Equals(x.TrangThai, statusItem.Value, StringComparison.OrdinalIgnoreCase));
            }

            BindGrid(query.ToList());
        }

        private void BindGrid(List<InvoiceDTO> invoices)
        {
            var viewData = invoices.Select(x => new
            {
                x.MaHD,
                NgayLap = x.NgayLap.ToString("dd/MM/yyyy HH:mm"),
                NhanVien = GetNhanVienName(x.MaNV),
                TongTien = x.TongTien.ToString("N0"),
                x.GhiChu,
                TrangThai = GetTrangThaiText(x.TrangThai)
            }).ToList();

            dgvInvoices.DataSource = null;
            dgvInvoices.DataSource = viewData;
        }

        private string GetNhanVienName(int maNV)
        {
            var user = _users?.FirstOrDefault(x => x.MaNV == maNV);
            return user != null ? user.TenNV : string.Empty;
        }

        private string GetTrangThaiText(string trangThai)
        {
            if (string.Equals(trangThai, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã thanh toán";
            }

            if (string.Equals(trangThai, "Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã hủy";
            }

            return trangThai ?? string.Empty;
        }

        private void BuildGridColumns()
        {
            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaHD",
                HeaderText = "Mã HĐ",
                DataPropertyName = "MaHD",
                Width = 80
            });

            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayLap",
                HeaderText = "Ngày lập",
                DataPropertyName = "NgayLap",
                Width = 150
            });

            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NhanVien",
                HeaderText = "Nhân viên",
                DataPropertyName = "NhanVien",
                Width = 180
            });

            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TongTien",
                HeaderText = "Tổng tiền",
                DataPropertyName = "TongTien",
                Width = 140
            });

            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                HeaderText = "Ghi chú",
                DataPropertyName = "GhiChu",
                Width = 250
            });

            dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = "TrangThai",
                Width = 130
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
