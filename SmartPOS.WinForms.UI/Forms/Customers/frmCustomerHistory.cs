using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Customers
{
    public class frmCustomerHistory : Form
    {
        private readonly ICustomerService _customerService;
        private readonly CustomerDTO _customer;

        private Label lblTitle;
        private Label lblSubtitle;
        private TabControl tabControl;
        private DataGridView dgvPurchases;
        private DataGridView dgvPoints;
        private Button btnClose;

        public frmCustomerHistory(CustomerDTO customer)
        {
            _customerService = new CustomerService();
            _customer = customer;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Lịch sử khách hàng";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(860, 560);
            MinimumSize = new Size(760, 480);
            BackColor = Color.FromArgb(248, 249, 251);
            Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = _customer != null ? "Lịch sử " + _customer.HoTen : "Lịch sử khách hàng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblSubtitle = new Label
            {
                Text = _customer != null
                    ? "Điểm hiện có: " + _customer.DiemHienCo.ToString("N0") + " | Tổng chi tiêu: " + _customer.TongChiTieu.ToString("N0") + " đ"
                    : string.Empty,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(120, 132, 160),
                AutoSize = true,
                Location = new Point(22, 50)
            };

            tabControl = new TabControl
            {
                Location = new Point(20, 85),
                Size = new Size(805, 375),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvPurchases = CreateGrid();
            dgvPoints = CreateGrid();
            BuildPurchaseColumns();
            BuildPointColumns();

            var tabPurchases = new TabPage("Lịch sử mua");
            tabPurchases.Controls.Add(dgvPurchases);

            var tabPoints = new TabPage("Lịch sử điểm");
            tabPoints.Controls.Add(dgvPoints);

            tabControl.TabPages.Add(tabPurchases);
            tabControl.TabPages.Add(tabPoints);

            btnClose = new Button
            {
                Text = "Đóng",
                Size = new Size(100, 36),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(tabControl);
            Controls.Add(btnClose);

            Load += FrmCustomerHistory_Load;
            Resize += (s, e) => PositionCloseButton();
        }

        private DataGridView CreateGrid()
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            UiGridHelper.ApplyResponsiveStyle(grid);
            return grid;
        }

        private void BuildPurchaseColumns()
        {
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaHD", HeaderText = "Mã HĐ", DataPropertyName = "MaHD", Width = 80 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayLapText", HeaderText = "Ngày lập", DataPropertyName = "NgayLapText", Width = 130 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongTienTruocGiamText", HeaderText = "Trước giảm", DataPropertyName = "TongTienTruocGiamText", Width = 120 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiemSuDung", HeaderText = "Điểm dùng", DataPropertyName = "DiemSuDung", Width = 90 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiamGiaDiemText", HeaderText = "Giảm", DataPropertyName = "GiamGiaDiemText", Width = 110 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongTienText", HeaderText = "Thanh toán", DataPropertyName = "TongTienText", Width = 120 });
            dgvPurchases.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", DataPropertyName = "TrangThai", Width = 100 });
        }

        private void BuildPointColumns()
        {
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayTaoText", HeaderText = "Thời gian", DataPropertyName = "NgayTaoText", Width = 140 });
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoaiGiaoDich", HeaderText = "Loại", DataPropertyName = "LoaiGiaoDich", Width = 90 });
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "Diem", HeaderText = "Điểm", DataPropertyName = "Diem", Width = 80 });
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaTriGiamText", HeaderText = "Giá trị", DataPropertyName = "GiaTriGiamText", Width = 110 });
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaHDText", HeaderText = "Hóa đơn", DataPropertyName = "MaHDText", Width = 90 });
            dgvPoints.Columns.Add(new DataGridViewTextBoxColumn { Name = "GhiChu", HeaderText = "Ghi chú", DataPropertyName = "GhiChu", Width = 220 });
        }

        private void FrmCustomerHistory_Load(object sender, EventArgs e)
        {
            PositionCloseButton();
            if (_customer == null)
            {
                return;
            }

            dgvPurchases.DataSource = _customerService.GetPurchaseHistory(_customer.MaKH)
                .Select(x => new
                {
                    x.MaHD,
                    NgayLapText = x.NgayLap.ToString("dd/MM/yyyy HH:mm"),
                    TongTienTruocGiamText = x.TongTienTruocGiam.ToString("N0") + " đ",
                    x.DiemSuDung,
                    GiamGiaDiemText = x.GiamGiaDiem.ToString("N0") + " đ",
                    TongTienText = x.TongTien.ToString("N0") + " đ",
                    x.TrangThai
                })
                .ToList();

            dgvPoints.DataSource = _customerService.GetPointHistory(_customer.MaKH)
                .Select(x => new
                {
                    NgayTaoText = x.NgayTao.ToString("dd/MM/yyyy HH:mm"),
                    x.LoaiGiaoDich,
                    x.Diem,
                    GiaTriGiamText = x.GiaTriGiam.ToString("N0") + " đ",
                    MaHDText = x.MaHD.HasValue ? x.MaHD.Value.ToString() : "-",
                    x.GhiChu
                })
                .ToList();
        }

        private void PositionCloseButton()
        {
            btnClose.Location = new Point(ClientSize.Width - btnClose.Width - 24, ClientSize.Height - btnClose.Height - 18);
        }
    }
}
