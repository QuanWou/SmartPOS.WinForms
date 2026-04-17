using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Helpers;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Stock
{
    public class frmStockHistory : Form, IGlobalSearchHandler
    {
        private readonly IStockInService _stockInService;
        private readonly IUserService _userService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblKeyword;
        private Label lblFromDate;
        private Label lblToDate;

        private TextBox txtKeyword;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;

        private Button btnSearch;
        private Button btnReload;
        private Button btnViewDetail;

        private DataGridView dgvStockHistory;

        private List<StockInDTO> _stockIns;
        private List<UserDTO> _users;

        public frmStockHistory()
        {
            _stockInService = new StockInService();
            _userService = new UserService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Lịch sử nhập kho";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Lịch sử nhập kho",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi các phiếu nhập hàng và chi tiết cập nhật tồn kho",
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

            lblFromDate = new Label
            {
                Text = "Từ ngày",
                AutoSize = true,
                Location = new Point(255, 85)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(255, 107),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblToDate = new Label
            {
                Text = "Đến ngày",
                AutoSize = true,
                Location = new Point(420, 85)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(420, 107),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(590, 104),
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
                Location = new Point(690, 104),
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
                Location = new Point(780, 104),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewDetail.FlatAppearance.BorderSize = 0;
            btnViewDetail.Click += BtnViewDetail_Click;

            dgvStockHistory = new DataGridView
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
            UiGridHelper.ApplyResponsiveStyle(dgvStockHistory);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblKeyword);
            this.Controls.Add(txtKeyword);
            this.Controls.Add(lblFromDate);
            this.Controls.Add(dtpFromDate);
            this.Controls.Add(lblToDate);
            this.Controls.Add(dtpToDate);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnReload);
            this.Controls.Add(btnViewDetail);
            this.Controls.Add(dgvStockHistory);

            this.Load += FrmStockHistory_Load;
            this.Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void FrmStockHistory_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;

            LoadData();
            UpdateResponsiveLayout();
        }

        private void LoadData()
        {
            _users = _userService.GetAll().ToList();
            _stockIns = _stockInService.GetAll().ToList();
            BindGrid(_stockIns);
        }

        private void BuildGridColumns()
        {
            dgvStockHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaPN",
                HeaderText = "Mã PN",
                DataPropertyName = "MaPN",
                Width = 80
            });

            dgvStockHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayNhap",
                HeaderText = "Ngày nhập",
                DataPropertyName = "NgayNhap",
                Width = 160
            });

            dgvStockHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NhanVien",
                HeaderText = "Nhân viên",
                DataPropertyName = "NhanVien",
                Width = 180
            });

            dgvStockHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TongTien",
                HeaderText = "Tổng tiền",
                DataPropertyName = "TongTien",
                Width = 140
            });

            dgvStockHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                HeaderText = "Ghi chú",
                DataPropertyName = "GhiChu",
                Width = 300
            });
        }

        private void BindGrid(List<StockInDTO> stockIns)
        {
            var viewData = stockIns.Select(x => new
            {
                x.MaPN,
                NgayNhap = x.NgayNhap.ToString("dd/MM/yyyy HH:mm"),
                NhanVien = GetNhanVienName(x.MaNV),
                TongTien = x.TongTien.ToString("N0"),
                x.GhiChu
            }).ToList();

            dgvStockHistory.DataSource = null;
            dgvStockHistory.DataSource = viewData;
        }

        private string GetNhanVienName(int maNV)
        {
            var user = _users?.FirstOrDefault(x => x.MaNV == maNV);
            return user != null ? user.TenNV : string.Empty;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchStockHistory();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            BindGrid(_stockIns ?? new List<StockInDTO>());
        }

        private void BtnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvStockHistory.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xem.", "Thông báo");
                return;
            }

            object cellValue = dgvStockHistory.CurrentRow.Cells["MaPN"].Value;
            if (cellValue == null)
            {
                MessageBox.Show("Không xác định được phiếu nhập.", "Thông báo");
                return;
            }

            int maPN;
            if (!int.TryParse(cellValue.ToString(), out maPN))
            {
                MessageBox.Show("Dữ liệu phiếu nhập không hợp lệ.", "Thông báo");
                return;
            }

            using (var frm = new frmStockInDetails(maPN))
            {
                frm.ShowDialog(this);
            }
        }

        private void SearchStockHistory()
        {
            IEnumerable<StockInDTO> query = _stockIns ?? new List<StockInDTO>();

            string keyword = txtKeyword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.MaPN.ToString().Contains(keyword) ||
                    (!string.IsNullOrWhiteSpace(x.GhiChu) && x.GhiChu.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    GetNhanVienName(x.MaNV).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            query = query.Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDate);

            BindGrid(query.ToList());
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtKeyword.Text = keyword ?? string.Empty;
            SearchStockHistory();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                return;
            }

            txtKeyword.Clear();
            SearchStockHistory();
        }

        private void UpdateResponsiveLayout()
        {
            int left = 20;
            int right = 20;
            int buttonTop = 104;
            int gap = 10;
            int x = ClientSize.Width - right;

            btnViewDetail.Location = new Point(x - btnViewDetail.Width, buttonTop);
            x = btnViewDetail.Left - gap;

            btnReload.Location = new Point(x - btnReload.Width, buttonTop);
            x = btnReload.Left - gap;

            btnSearch.Location = new Point(x - btnSearch.Width, buttonTop);

            dgvStockHistory.SetBounds(
                left,
                155,
                Math.Max(320, ClientSize.Width - left - right),
                Math.Max(220, ClientSize.Height - 175));
        }
    }
}
