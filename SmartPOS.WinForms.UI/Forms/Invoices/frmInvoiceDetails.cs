using SmartPOS.WinForms.BLL.Interfaces;
using System.IO;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Forms.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Invoices
{
    public class frmInvoiceDetails : Form
    {
        private readonly int _maHD;
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblInvoiceInfo;
        private Label lblTamTinh;
        private Label lblTamTinhValue;
        private Label lblGiamGiaUuDai;
        private Label lblGiamGiaUuDaiValue;
        private Label lblGiamGiaDiem;
        private Label lblGiamGiaDiemValue;
        private Label lblTongTien;
        private Label lblTongTienValue;
        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private Button btnPrint;
        private Button btnClose;

        private DataGridView dgvDetails;

        public frmInvoiceDetails(int maHD)
        {
            _maHD = maHD;
            _invoiceService = new InvoiceService();
            _userService = new UserService();
            _productService = new ProductService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi tiết hóa đơn";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(900, 620);
            this.MinimumSize = new Size(900, 620);
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "Chi tiết hóa đơn",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Xem thông tin hóa đơn và danh sách sản phẩm đã bán",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblInvoiceInfo = new Label
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
                Text = "Cần thanh toán",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(20, 548)
            };

            lblTongTienValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 570)
            };

            lblTamTinh = MakeSummaryLabel("Tạm tính", 20, 475);
            lblTamTinhValue = MakeSummaryValueLabel(150, 475);

            lblGiamGiaUuDai = MakeSummaryLabel("Giảm ưu đãi", 20, 500);
            lblGiamGiaUuDaiValue = MakeSummaryValueLabel(150, 500);

            lblGiamGiaDiem = MakeSummaryLabel("Giảm từ điểm", 20, 525);
            lblGiamGiaDiemValue = MakeSummaryValueLabel(150, 525);

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

            btnPrint = new Button
            {
                Text = "In hóa đơn",
                Location = new Point(650, 492),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

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
            this.Controls.Add(lblInvoiceInfo);
            this.Controls.Add(dgvDetails);
            this.Controls.Add(lblTamTinh);
            this.Controls.Add(lblTamTinhValue);
            this.Controls.Add(lblGiamGiaUuDai);
            this.Controls.Add(lblGiamGiaUuDaiValue);
            this.Controls.Add(lblGiamGiaDiem);
            this.Controls.Add(lblGiamGiaDiemValue);
            this.Controls.Add(lblTongTien);
            this.Controls.Add(lblTongTienValue);
            this.Controls.Add(lblGhiChu);
            this.Controls.Add(txtGhiChu);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            this.Load += FrmInvoiceDetails_Load;
            this.Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void FrmInvoiceDetails_Load(object sender, EventArgs e)
        {
            LoadInvoiceDetails();
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
                Name = "DonGia",
                HeaderText = "Đơn giá",
                DataPropertyName = "DonGia",
                Width = 140
            });

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien",
                Width = 160
            });
        }

        private Label MakeSummaryLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private Label MakeSummaryValueLabel(int x, int y)
        {
            return new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(125, 22),
                Location = new Point(x, y - 3)
            };
        }

        private void LoadInvoiceDetails()
        {
            InvoiceDTO invoice = _invoiceService.GetById(_maHD);
            if (invoice == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn.", "Thông báo");
                this.Close();
                return;
            }

            if (SessionManager.IsStaff &&
                SessionManager.CurrentUser != null &&
                invoice.MaNV != SessionManager.CurrentUser.MaNV)
            {
                MessageBox.Show("Bạn chỉ được xem hóa đơn do mình tạo.", "Thông báo");
                this.Close();
                return;
            }

            UserDTO user = _userService.GetById(invoice.MaNV);
            List<InvoiceDetailDTO> details = _invoiceService.GetDetailsByInvoiceId(_maHD).ToList();

            lblInvoiceInfo.Text =
                "Mã HĐ: " + invoice.MaHD +
                "   |   Ngày lập: " + invoice.NgayLap.ToString("dd/MM/yyyy HH:mm") +
                "   |   Nhân viên: " + (user != null ? user.TenNV : string.Empty);

            decimal subtotal = invoice.TongTienTruocGiam > 0
                ? invoice.TongTienTruocGiam
                : details.Sum(x => x.ThanhTien);
            bool hasOfferDiscount = invoice.GiamGiaUuDai > 0 || invoice.MaUuDai.HasValue;
            bool hasPointDiscount = invoice.DiemSuDung > 0 || invoice.GiamGiaDiem > 0;

            lblTamTinhValue.Text = subtotal.ToString("N0") + " đ";
            lblGiamGiaUuDaiValue.Text = invoice.GiamGiaUuDai.ToString("N0") + " đ";
            lblGiamGiaUuDai.Text = invoice.PhanTramUuDai > 0
                ? "Giảm ưu đãi (" + invoice.PhanTramUuDai.ToString("N0") + "%)"
                : "Giảm ưu đãi";
            lblGiamGiaDiem.Text = invoice.DiemSuDung > 0
                ? "Giảm từ điểm (" + invoice.DiemSuDung.ToString("N0") + " điểm)"
                : "Giảm từ điểm";
            lblGiamGiaDiemValue.Text = invoice.GiamGiaDiem.ToString("N0") + " đ";
            lblGiamGiaUuDai.Visible = hasOfferDiscount;
            lblGiamGiaUuDaiValue.Visible = hasOfferDiscount;
            lblGiamGiaDiem.Visible = hasPointDiscount;
            lblGiamGiaDiemValue.Visible = hasPointDiscount;
            lblTongTienValue.Text = invoice.TongTien.ToString("N0") + " đ";
            txtGhiChu.Text = invoice.GhiChu ?? string.Empty;

            var viewData = details.Select(x =>
            {
                ProductDTO product = _productService.GetById(x.MaSP);

                return new
                {
                    x.MaSP,
                    TenSP = product != null ? product.TenSP : string.Empty,
                    x.SoLuong,
                    DonGia = x.DonGiaLucBan.ToString("N0"),
                    ThanhTien = x.ThanhTien.ToString("N0")
                };
            }).ToList();

            dgvDetails.DataSource = null;
            dgvDetails.DataSource = viewData;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            using (var frm = new SmartPOS.WinForms.UI.Forms.Reports.frmInvoicePrintPreview(_maHD))
            {
                frm.ShowDialog(this);
            }
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
            int summaryValueLeft = 150;
            int summaryY = infoTop;

            lblTamTinh.Location = new Point(left, summaryY);
            lblTamTinhValue.Location = new Point(summaryValueLeft, summaryY - 3);
            summaryY += 25;

            if (lblGiamGiaUuDai.Visible)
            {
                lblGiamGiaUuDai.Location = new Point(left, summaryY);
                lblGiamGiaUuDaiValue.Location = new Point(summaryValueLeft, summaryY - 3);
                summaryY += 25;
            }

            if (lblGiamGiaDiem.Visible)
            {
                lblGiamGiaDiem.Location = new Point(left, summaryY);
                lblGiamGiaDiemValue.Location = new Point(summaryValueLeft, summaryY - 3);
                summaryY += 25;
            }

            lblTongTien.Location = new Point(left, summaryY);
            lblTongTienValue.Location = new Point(left, summaryY + 23);
            lblGhiChu.Location = new Point(330, infoTop);

            btnClose.Location = new Point(ClientSize.Width - right - btnClose.Width, infoTop + 17);
            btnPrint.Location = new Point(btnClose.Left - 10 - btnPrint.Width, infoTop + 17);
            txtGhiChu.SetBounds(
                330,
                infoTop + 22,
                Math.Max(220, btnPrint.Left - 20 - 330),
                txtGhiChu.Height);
        }
    }
}
