using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Stock
{
    public class frmStockInDetails : Form
    {
        private readonly int _maPN;
        private readonly IStockInService _stockInService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblStockInInfo;
        private Label lblTongTien;
        private Label lblTongTienValue;
        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private Button btnClose;

        private DataGridView dgvDetails;

        public frmStockInDetails(int maPN)
        {
            _maPN = maPN;
            _stockInService = new StockInService();
            _userService = new UserService();
            _productService = new ProductService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi tiết phiếu nhập";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(900, 620);
            this.MinimumSize = new Size(900, 620);
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "Chi tiết phiếu nhập",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Xem thông tin phiếu nhập và danh sách sản phẩm đã nhập kho",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblStockInInfo = new Label
            {
                Text = string.Empty,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(20, 90)
            };

            dgvDetails = new DataGridView
            {
                Location = new Point(20, 125),
                Size = new Size(840, 330),
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
            UiGridHelper.ApplyResponsiveStyle(dgvDetails);

            lblTongTien = new Label
            {
                Text = "Tổng tiền",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(20, 475)
            };

            lblTongTienValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 500)
            };

            lblGhiChu = new Label
            {
                Text = "Ghi chú",
                AutoSize = true,
                Location = new Point(260, 475)
            };

            txtGhiChu = new TextBox
            {
                Location = new Point(260, 497),
                Size = new Size(360, 27),
                ReadOnly = true
            };

            btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(760, 492),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += BtnClose_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblStockInInfo);
            this.Controls.Add(dgvDetails);
            this.Controls.Add(lblTongTien);
            this.Controls.Add(lblTongTienValue);
            this.Controls.Add(lblGhiChu);
            this.Controls.Add(txtGhiChu);
            this.Controls.Add(btnClose);

            this.Load += FrmStockInDetails_Load;
            this.Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void FrmStockInDetails_Load(object sender, EventArgs e)
        {
            LoadStockInDetails();
            UpdateResponsiveLayout();
        }

        private void BuildGridColumns()
        {
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MaSP",
                Width = 80
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 280
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong",
                HeaderText = "Số lượng",
                DataPropertyName = "SoLuong",
                Width = 90
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhap",
                HeaderText = "Giá nhập",
                DataPropertyName = "GiaNhap",
                Width = 120
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HanSuDung",
                HeaderText = "HSD",
                DataPropertyName = "HanSuDung",
                Width = 110
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien",
                Width = 140
            });
        }

        private void LoadStockInDetails()
        {
            StockInDTO stockIn = _stockInService.GetById(_maPN);
            if (stockIn == null)
            {
                MessageBox.Show("Không tìm thấy phiếu nhập.", "Thông báo");
                this.Close();
                return;
            }

            UserDTO user = _userService.GetById(stockIn.MaNV);
            IEnumerable<StockInDetailDTO> details = _stockInService.GetDetailsByStockInId(_maPN);

            lblStockInInfo.Text =
                "Mã PN: " + stockIn.MaPN +
                "   |   Ngày nhập: " + stockIn.NgayNhap.ToString("dd/MM/yyyy HH:mm") +
                "   |   Nhân viên: " + (user != null ? user.TenNV : string.Empty);

            lblTongTienValue.Text = stockIn.TongTien.ToString("N0") + " đ";
            txtGhiChu.Text = stockIn.GhiChu ?? string.Empty;

            var viewData = details.Select(x =>
            {
                ProductDTO product = _productService.GetById(x.MaSP);

                return new
                {
                    x.MaSP,
                    TenSP = product != null ? product.TenSP : string.Empty,
                    x.SoLuong,
                    GiaNhap = x.GiaNhapLucNhap.ToString("N0"),
                    HanSuDung = x.HanSuDung.HasValue ? x.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                    ThanhTien = x.ThanhTien.ToString("N0")
                };
            }).ToList();

            dgvDetails.DataSource = null;
            dgvDetails.DataSource = viewData;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateResponsiveLayout()
        {
            const int left = 20;
            const int right = 20;
            const int gridTop = 125;
            int footerTop = Math.Max(475, ClientSize.Height - 125);

            dgvDetails.SetBounds(
                left,
                gridTop,
                Math.Max(400, ClientSize.Width - left - right),
                Math.Max(220, footerTop - gridTop - 20));

            int infoTop = dgvDetails.Bottom + 20;
            lblTongTien.Location = new Point(left, infoTop);
            lblTongTienValue.Location = new Point(left, infoTop + 25);
            lblGhiChu.Location = new Point(260, infoTop);

            btnClose.Location = new Point(ClientSize.Width - right - btnClose.Width, infoTop + 17);
            txtGhiChu.SetBounds(
                260,
                infoTop + 22,
                Math.Max(260, btnClose.Left - 20 - 260),
                txtGhiChu.Height);
        }
    }
}
