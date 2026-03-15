using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using System.Text;
using System.IO;
using System.Globalization;
using SmartPOS.WinForms.Common.Session;
namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmReports : Form
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IStockInService _stockInService;
        private readonly IProductService _productService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblFromDate;
        private Label lblToDate;
        private Label lblReportType;

        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private ComboBox cboReportType;

        private Button btnGenerate;
        private Button btnPreview;
        private Button btnExport;

        private Panel pnlSummary;
        private Panel pnlReportViewer;

        private Label lblTongDoanhThu;
        private Label lblTongHoaDon;
        private Label lblTongNhapKho;
        private Label lblTonKhoThap;

        public frmReports()
        {
            _invoiceService = new InvoiceService();
            _stockInService = new StockInService();
            _productService = new ProductService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo cáo";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Báo cáo thống kê",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Tổng hợp doanh thu, hóa đơn, nhập kho và tồn kho theo thời gian",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            lblFromDate = new Label
            {
                Text = "Từ ngày",
                AutoSize = true,
                Location = new Point(20, 90)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(20, 112),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblToDate = new Label
            {
                Text = "Đến ngày",
                AutoSize = true,
                Location = new Point(185, 90)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(185, 112),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblReportType = new Label
            {
                Text = "Loại báo cáo",
                AutoSize = true,
                Location = new Point(350, 90)
            };

            cboReportType = new ComboBox
            {
                Location = new Point(350, 112),
                Size = new Size(180, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboReportType.Items.Add("Báo cáo doanh thu");
            cboReportType.Items.Add("Báo cáo hóa đơn");
            cboReportType.Items.Add("Báo cáo nhập kho");
            cboReportType.Items.Add("Báo cáo tồn kho");
            cboReportType.SelectedIndex = 0;

            btnGenerate = new Button
            {
                Text = "Tạo báo cáo",
                Location = new Point(550, 109),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += BtnGenerate_Click;

            btnPreview = new Button
            {
                Text = "Xem trước",
                Location = new Point(660, 109),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPreview.FlatAppearance.BorderSize = 0;
            btnPreview.Click += BtnPreview_Click;

            btnExport = new Button
            {
                Text = "Xuất file",
                Location = new Point(760, 109),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;

            BuildSummaryPanel();
            BuildReportViewerPanel();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblFromDate);
            this.Controls.Add(dtpFromDate);
            this.Controls.Add(lblToDate);
            this.Controls.Add(dtpToDate);
            this.Controls.Add(lblReportType);
            this.Controls.Add(cboReportType);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(btnPreview);
            this.Controls.Add(btnExport);
            this.Controls.Add(pnlSummary);
            this.Controls.Add(pnlReportViewer);

            this.Load += FrmReports_Load;
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsStaff)
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo tổng.", "Thông báo");
                this.BeginInvoke(new Action(Close));
                return;
            }

            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            LoadSummary();
            RenderSelectedReport();
        }

        private void BuildSummaryPanel()
        {
            pnlSummary = new Panel
            {
                Location = new Point(20, 160),
                Size = new Size(970, 90),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            TableLayoutPanel summaryGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(18, 12, 18, 12)
            };
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblTongDoanhThu = BuildSummaryItem("Tổng doanh thu", "0 đ");
            lblTongHoaDon = BuildSummaryItem("Số hóa đơn", "0");
            lblTongNhapKho = BuildSummaryItem("Phiếu nhập", "0");
            lblTonKhoThap = BuildSummaryItem("Sắp hết hàng", "0");

            summaryGrid.Controls.Add(lblTongDoanhThu, 0, 0);
            summaryGrid.Controls.Add(lblTongHoaDon, 1, 0);
            summaryGrid.Controls.Add(lblTongNhapKho, 2, 0);
            summaryGrid.Controls.Add(lblTonKhoThap, 3, 0);

            pnlSummary.Controls.Add(summaryGrid);
        }

        private Label BuildSummaryItem(string title, string value)
        {
            return new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 0, 6, 0),
                Padding = new Padding(10, 4, 10, 4),
                Text = title + Environment.NewLine + value,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private void BuildReportViewerPanel()
        {
            pnlReportViewer = new Panel
            {
                Location = new Point(20, 270),
                Size = new Size(970, 340),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblPlaceholder = new Label
            {
                Text = "Chọn loại báo cáo rồi bấm Tạo báo cáo",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 185, 200),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlReportViewer.Controls.Add(lblPlaceholder);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            LoadSummary();
            RenderSelectedReport();
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            LoadSummary();
            RenderSelectedReport();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            string selected = cboReportType.SelectedItem != null
                ? cboReportType.SelectedItem.ToString()
                : string.Empty;

            if (selected != "Báo cáo doanh thu")
            {
                MessageBox.Show("Chưa hỗ trợ xuất file cho loại báo cáo này.", "Thông báo");
                return;
            }

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            var invoices = _invoiceService.GetAll()
                .Where(x =>
                    x.NgayLap >= fromDate &&
                    x.NgayLap <= toDate &&
                    string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var revenueByDate = invoices
                .GroupBy(x => x.NgayLap.Date)
                .Select(g => new
                {
                    Ngay = g.Key.ToString("dd/MM/yyyy"),
                    SoHoaDon = g.Count(),
                    DoanhThu = g.Sum(x => x.TongTien)
                })
                .OrderByDescending(x => DateTime.ParseExact(x.Ngay, "dd/MM/yyyy", null))
                .ToList();

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Xuất báo cáo doanh thu";
                saveDialog.Filter = "Text file (*.txt)|*.txt";
                saveDialog.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    decimal tongDoanhThu = invoices.Sum(x => x.TongTien);
                    int tongHoaDon = invoices.Count;
                    decimal trungBinhDon = tongHoaDon > 0 ? tongDoanhThu / tongHoaDon : 0;

                    System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    builder.AppendLine("BÁO CÁO DOANH THU");
                    builder.AppendLine("Từ ngày: " + fromDate.ToString("dd/MM/yyyy"));
                    builder.AppendLine("Đến ngày: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
                    builder.AppendLine(new string('=', 50));
                    builder.AppendLine("Tổng doanh thu : " + tongDoanhThu.ToString("N0") + " đ");
                    builder.AppendLine("Số hóa đơn     : " + tongHoaDon);
                    builder.AppendLine("Trung bình/đơn : " + trungBinhDon.ToString("N0") + " đ");
                    builder.AppendLine(new string('-', 50));

                    foreach (var item in revenueByDate)
                    {
                        builder.AppendLine(
                            item.Ngay.PadRight(15) +
                            ("Số HĐ: " + item.SoHoaDon).PadRight(18) +
                            "Doanh thu: " + item.DoanhThu.ToString("N0") + " đ");
                    }

                    System.IO.File.WriteAllText(saveDialog.FileName, builder.ToString(), System.Text.Encoding.UTF8);

                    MessageBox.Show("Xuất file thành công.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất file thất bại. " + ex.Message, "Thông báo");
                }
            }
        }

        private void LoadSummary()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            var invoices = _invoiceService.GetAll()
                .Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate)
                .ToList();

            var stockIns = _stockInService.GetAll()
                .Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDate)
                .ToList();

            var lowStockProducts = _productService.GetAll()
                .Where(x =>
                    x.TrangThai &&
                    x.SoLuongTon <= 10 &&
                    (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today))
                .ToList();

            decimal tongDoanhThu = invoices
                .Where(x => string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                .Sum(x => x.TongTien);

            int tongHoaDon = invoices.Count;
            int tongPhieuNhap = stockIns.Count;
            int tongSapHetHang = lowStockProducts.Count;

            lblTongDoanhThu.Text = "Tổng doanh thu" + Environment.NewLine + tongDoanhThu.ToString("N0") + " đ";
            lblTongHoaDon.Text = "Số hóa đơn" + Environment.NewLine + tongHoaDon.ToString();
            lblTongNhapKho.Text = "Phiếu nhập" + Environment.NewLine + tongPhieuNhap.ToString();
            lblTonKhoThap.Text = "Sắp hết hàng" + Environment.NewLine + tongSapHetHang.ToString();
        }

        private void RenderSelectedReport()
        {
            string selected = cboReportType.SelectedItem != null
                ? cboReportType.SelectedItem.ToString()
                : string.Empty;

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            pnlReportViewer.Controls.Clear();

            if (selected == "Báo cáo doanh thu")
            {
                var frm = new frmRevenueReport(fromDate, toDate)
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };

                pnlReportViewer.Controls.Add(frm);
                frm.Show();
                frm.BringToFront();
                return;
            }

            if (selected == "Báo cáo hóa đơn")
            {
                pnlReportViewer.Controls.Add(BuildInvoiceReportView(fromDate, toDate));
                return;
            }

            if (selected == "Báo cáo nhập kho")
            {
                pnlReportViewer.Controls.Add(BuildStockInReportView(fromDate, toDate));
                return;
            }

            pnlReportViewer.Controls.Add(BuildInventoryReportView());
        }

        private Control BuildInvoiceReportView(DateTime fromDate, DateTime toDate)
        {
            DateTime toDateInclusive = toDate.Date.AddDays(1).AddTicks(-1);

            var invoices = _invoiceService.GetAll()
                .Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDateInclusive)
                .OrderByDescending(x => x.NgayLap)
                .Select(x => new
                {
                    x.MaHD,
                    NgayLap = x.NgayLap.ToString("dd/MM/yyyy HH:mm"),
                    x.MaNV,
                    TongTien = x.TongTien.ToString("N0") + " đ",
                    TrangThai = GetInvoiceStatusLabel(x.TrangThai),
                    GhiChu = string.IsNullOrWhiteSpace(x.GhiChu) ? "-" : x.GhiChu
                })
                .ToList();

            return BuildGridReportView(
                "Báo cáo hóa đơn",
                "Danh sách hóa đơn trong khoảng thời gian đã chọn",
                invoices);
        }

        private Control BuildStockInReportView(DateTime fromDate, DateTime toDate)
        {
            DateTime toDateInclusive = toDate.Date.AddDays(1).AddTicks(-1);

            var stockIns = _stockInService.GetAll()
                .Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDateInclusive)
                .OrderByDescending(x => x.NgayNhap)
                .Select(x => new
                {
                    x.MaPN,
                    NgayNhap = x.NgayNhap.ToString("dd/MM/yyyy HH:mm"),
                    x.MaNV,
                    TongTien = x.TongTien.ToString("N0") + " đ",
                    GhiChu = string.IsNullOrWhiteSpace(x.GhiChu) ? "-" : x.GhiChu
                })
                .ToList();

            return BuildGridReportView(
                "Báo cáo nhập kho",
                "Danh sách phiếu nhập trong khoảng thời gian đã chọn",
                stockIns);
        }

        private Control BuildInventoryReportView()
        {
            var products = _productService.GetAll()
                .Where(x => x.TrangThai)
                .OrderBy(x => x.SoLuongTon)
                .Select(x => new
                {
                    x.MaSP,
                    x.TenSP,
                    x.MaVach,
                    x.SoLuongTon,
                    GiaBan = x.GiaBan.ToString("N0") + " đ",
                    HanSuDung = x.HanSuDung.HasValue ? x.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                    TrangThai = x.TrangThai ? "Đang bán" : "Ngừng bán",
                    MucDo = x.SoLuongTon <= 5
                        ? "Cần nhập gấp"
                        : x.SoLuongTon <= 10
                            ? "Sắp hết"
                            : "Ổn định"
                })
                .ToList();

            return BuildGridReportView(
                "Báo cáo tồn kho",
                "Tồn kho hiện tại của các sản phẩm đang kinh doanh",
                products);
        }

        private Control BuildGridReportView(string title, string subtitle, object dataSource)
        {
            Panel wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Label lblViewTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(18, 16)
            };

            Label lblViewSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(18, 42)
            };

            DataGridView grid = new DataGridView
            {
                Location = new Point(18, 74),
                Size = new Size(Math.Max(200, pnlReportViewer.Width - 36), Math.Max(120, pnlReportViewer.Height - 92)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                DataSource = dataSource
            };

            wrapper.Controls.Add(lblViewTitle);
            wrapper.Controls.Add(lblViewSubtitle);
            wrapper.Controls.Add(grid);

            return wrapper;
        }

        private string GetInvoiceStatusLabel(string status)
        {
            if (string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã thanh toán";
            }

            if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                return "Đã hủy";
            }

            return string.IsNullOrWhiteSpace(status) ? "-" : status;
        }
    }
}
