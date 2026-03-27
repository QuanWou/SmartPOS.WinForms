using System;
using System.Collections.Generic;
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
using SmartPOS.WinForms.UI.Interfaces;
namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmReports : Form, IGlobalSearchHandler
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
            cboReportType.SelectedIndexChanged += (s, e) => RenderSelectedReport();

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
    if (!ValidateDateRange())
    {
        return;
    }

    LoadSummary();
    RenderSelectedReport();
}

private void BtnPreview_Click(object sender, EventArgs e)
{
    if (!ValidateDateRange())
    {
        return;
    }

    LoadSummary();
    RenderSelectedReport();

    ReportPreviewData previewData = BuildCurrentReportPreview();
    using (var frm = new frmReportTextPreview(
        previewData.Title,
        previewData.Subtitle,
        previewData.Content,
        previewData.DefaultFileName))
    {
        frm.ShowDialog(this);
    }
}

private void BtnExport_Click(object sender, EventArgs e)
{
    if (!ValidateDateRange())
    {
        return;
    }

    ExportReportToFile(BuildCurrentReportPreview());
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

private bool ValidateDateRange()
{
    if (dtpFromDate.Value.Date <= dtpToDate.Value.Date)
    {
        return true;
    }

    MessageBox.Show("Ng?y b?t ??u kh?ng ???c l?n h?n ng?y k?t th?c.", "Th?ng b?o");
    return false;
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

private ReportPreviewData BuildCurrentReportPreview()
{
    string selected = cboReportType.SelectedItem != null
        ? cboReportType.SelectedItem.ToString()
        : string.Empty;

    if (selected == "B?o c?o h?a ??n")
    {
        return BuildInvoicePreview();
    }

    if (selected == "B?o c?o nh?p kho")
    {
        return BuildStockInPreview();
    }

    if (selected == "B?o c?o t?n kho")
    {
        return BuildInventoryPreview();
    }

    return BuildRevenuePreview();
}

private ReportPreviewData BuildRevenuePreview()
{
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
            Ngay = g.Key,
            SoHoaDon = g.Count(),
            DoanhThu = g.Sum(x => x.TongTien)
        })
        .OrderByDescending(x => x.Ngay)
        .ToList();

    decimal tongDoanhThu = invoices.Sum(x => x.TongTien);
    int tongHoaDon = invoices.Count;
    decimal trungBinhDon = tongHoaDon > 0 ? tongDoanhThu / tongHoaDon : 0;

    StringBuilder builder = new StringBuilder();
    builder.AppendLine("B?O C?O DOANH THU");
    builder.AppendLine("T? ng?y: " + fromDate.ToString("dd/MM/yyyy"));
    builder.AppendLine("??n ng?y: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
    builder.AppendLine(new string('=', 60));
    builder.AppendLine("T?ng doanh thu : " + tongDoanhThu.ToString("N0") + " ?");
    builder.AppendLine("S? h?a ??n     : " + tongHoaDon);
    builder.AppendLine("Trung b?nh/??n : " + trungBinhDon.ToString("N0") + " ?");
    builder.AppendLine(new string('-', 60));
    builder.AppendLine("Ng?y".PadRight(16) + "S? H?".PadRight(12) + "Doanh thu");
    builder.AppendLine(new string('-', 60));

    foreach (var item in revenueByDate)
    {
        builder.AppendLine(
            item.Ngay.ToString("dd/MM/yyyy").PadRight(16) +
            item.SoHoaDon.ToString().PadRight(12) +
            item.DoanhThu.ToString("N0") + " ?");
    }

    if (!revenueByDate.Any())
    {
        builder.AppendLine("Kh?ng c? d? li?u trong kho?ng th?i gian ?? ch?n.");
    }

    return new ReportPreviewData
    {
        Title = "Xem tr??c b?o c?o doanh thu",
        Subtitle = "Ki?m tra b?o c?o doanh thu tr??c khi in ho?c xu?t file",
        Content = builder.ToString(),
        DefaultFileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
    };
}

private ReportPreviewData BuildInvoicePreview()
{
    DateTime fromDate = dtpFromDate.Value.Date;
    DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

    var invoices = _invoiceService.GetAll()
        .Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate)
        .OrderByDescending(x => x.NgayLap)
        .ToList();

    StringBuilder builder = new StringBuilder();
    builder.AppendLine("B?O C?O H?A ??N");
    builder.AppendLine("T? ng?y: " + fromDate.ToString("dd/MM/yyyy"));
    builder.AppendLine("??n ng?y: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
    builder.AppendLine(new string('=', 90));
    builder.AppendLine("T?ng h?a ??n : " + invoices.Count);
    builder.AppendLine("?? thanh to?n: " + invoices.Count(x => string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase)));
    builder.AppendLine("?? h?y       : " + invoices.Count(x => string.Equals(x.TrangThai, "Cancelled", StringComparison.OrdinalIgnoreCase)));
    builder.AppendLine(new string('-', 90));
    builder.AppendLine("M?".PadRight(8) + "Ng?y l?p".PadRight(20) + "NV".PadRight(8) + "Tr?ng th?i".PadRight(18) + "T?ng ti?n");
    builder.AppendLine(new string('-', 90));

    foreach (var item in invoices)
    {
        builder.AppendLine(
            item.MaHD.ToString().PadRight(8) +
            item.NgayLap.ToString("dd/MM/yyyy HH:mm").PadRight(20) +
            item.MaNV.ToString().PadRight(8) +
            GetInvoiceStatusLabel(item.TrangThai).PadRight(18) +
            item.TongTien.ToString("N0") + " ?");

        if (!string.IsNullOrWhiteSpace(item.GhiChu))
        {
            builder.AppendLine("  Ghi ch?: " + item.GhiChu);
        }
    }

    if (!invoices.Any())
    {
        builder.AppendLine("Kh?ng c? h?a ??n trong kho?ng th?i gian ?? ch?n.");
    }

    return new ReportPreviewData
    {
        Title = "Xem tr??c b?o c?o h?a ??n",
        Subtitle = "Ki?m tra danh s?ch h?a ??n tr??c khi in ho?c xu?t file",
        Content = builder.ToString(),
        DefaultFileName = "BaoCaoHoaDon_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
    };
}

private ReportPreviewData BuildStockInPreview()
{
    DateTime fromDate = dtpFromDate.Value.Date;
    DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

    var stockIns = _stockInService.GetAll()
        .Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDate)
        .OrderByDescending(x => x.NgayNhap)
        .ToList();

    StringBuilder builder = new StringBuilder();
    builder.AppendLine("B?O C?O NH?P KHO");
    builder.AppendLine("T? ng?y: " + fromDate.ToString("dd/MM/yyyy"));
    builder.AppendLine("??n ng?y: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
    builder.AppendLine(new string('=', 90));
    builder.AppendLine("S? phi?u nh?p : " + stockIns.Count);
    builder.AppendLine("T?ng ti?n nh?p: " + stockIns.Sum(x => x.TongTien).ToString("N0") + " ?");
    builder.AppendLine(new string('-', 90));
    builder.AppendLine("M?".PadRight(8) + "Ng?y nh?p".PadRight(20) + "NV".PadRight(8) + "T?ng ti?n");
    builder.AppendLine(new string('-', 90));

    foreach (var item in stockIns)
    {
        builder.AppendLine(
            item.MaPN.ToString().PadRight(8) +
            item.NgayNhap.ToString("dd/MM/yyyy HH:mm").PadRight(20) +
            item.MaNV.ToString().PadRight(8) +
            item.TongTien.ToString("N0") + " ?");

        if (!string.IsNullOrWhiteSpace(item.GhiChu))
        {
            builder.AppendLine("  Ghi ch?: " + item.GhiChu);
        }
    }

    if (!stockIns.Any())
    {
        builder.AppendLine("Kh?ng c? phi?u nh?p trong kho?ng th?i gian ?? ch?n.");
    }

    return new ReportPreviewData
    {
        Title = "Xem tr??c b?o c?o nh?p kho",
        Subtitle = "Ki?m tra danh s?ch phi?u nh?p tr??c khi in ho?c xu?t file",
        Content = builder.ToString(),
        DefaultFileName = "BaoCaoNhapKho_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
    };
}

private ReportPreviewData BuildInventoryPreview()
{
    List<ProductDTO> products = _productService.GetAll()
        .Where(x => x.TrangThai)
        .OrderBy(x => x.SoLuongTon)
        .ToList();

    StringBuilder builder = new StringBuilder();
    builder.AppendLine("B?O C?O T?N KHO");
    builder.AppendLine("Ng?y l?p: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
    builder.AppendLine(new string('=', 110));
    builder.AppendLine("T?ng s?n ph?m ?ang b?n : " + products.Count);
    builder.AppendLine("S?p h?t h?ng (<=10)    : " + products.Count(x => x.SoLuongTon <= 10));
    builder.AppendLine("C?n nh?p g?p (<=5)     : " + products.Count(x => x.SoLuongTon <= 5));
    builder.AppendLine(new string('-', 110));
    builder.AppendLine(
        "M?".PadRight(8) +
        "T?n s?n ph?m".PadRight(30) +
        "T?n".PadRight(8) +
        "Gi? b?n".PadRight(16) +
        "HSD".PadRight(16) +
        "M?c ??");
    builder.AppendLine(new string('-', 110));

    foreach (var item in products)
    {
        string tenSP = item.TenSP ?? string.Empty;
        if (tenSP.Length > 26)
        {
            tenSP = tenSP.Substring(0, 26);
        }

        builder.AppendLine(
            item.MaSP.ToString().PadRight(8) +
            tenSP.PadRight(30) +
            item.SoLuongTon.ToString().PadRight(8) +
            (item.GiaBan.ToString("N0") + " ?").PadRight(16) +
            (item.HanSuDung.HasValue ? item.HanSuDung.Value.ToString("dd/MM/yyyy") : "-").PadRight(16) +
            GetInventoryLevelLabel(item.SoLuongTon));
    }

    if (!products.Any())
    {
        builder.AppendLine("Kh?ng c? s?n ph?m ?ang kinh doanh.");
    }

    return new ReportPreviewData
    {
        Title = "Xem tr??c b?o c?o t?n kho",
        Subtitle = "Ki?m tra t?nh tr?ng t?n kho tr??c khi in ho?c xu?t file",
        Content = builder.ToString(),
        DefaultFileName = "BaoCaoTonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
    };
}

private string GetInventoryLevelLabel(int soLuongTon)
{
    if (soLuongTon <= 5)
    {
        return "C?n nh?p g?p";
    }

    if (soLuongTon <= 10)
    {
        return "S?p h?t";
    }

    return "?n ??nh";
}

private void ExportReportToFile(ReportPreviewData previewData)
{
    using (SaveFileDialog saveDialog = new SaveFileDialog())
    {
        saveDialog.Title = "Xu?t b?o c?o";
        saveDialog.Filter = "Text file (*.txt)|*.txt";
        saveDialog.FileName = previewData.DefaultFileName;

        if (saveDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            File.WriteAllText(saveDialog.FileName, previewData.Content, Encoding.UTF8);
            MessageBox.Show("Xu?t file th?nh c?ng.", "Th?ng b?o");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Xu?t file th?t b?i. " + ex.Message, "Th?ng b?o");
        }
    }
}

public void ApplyGlobalSearch(string keyword)
{
    string normalized = NormalizeSearchText(keyword);

    if (normalized.Contains("hoa don"))
    {
        cboReportType.SelectedIndex = 1;
    }
    else if (normalized.Contains("nhap kho"))
    {
        cboReportType.SelectedIndex = 2;
    }
    else if (normalized.Contains("ton kho"))
    {
        cboReportType.SelectedIndex = 3;
    }
    else
    {
        cboReportType.SelectedIndex = 0;
    }

    LoadSummary();
    RenderSelectedReport();
}

public void ClearGlobalSearch()
{
    if (cboReportType.SelectedIndex != 0)
    {
        cboReportType.SelectedIndex = 0;
    }

    LoadSummary();
    RenderSelectedReport();
}

private string NormalizeSearchText(string value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return string.Empty;
    }

    string normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
    StringBuilder builder = new StringBuilder(normalized.Length);
    foreach (char c in normalized)
    {
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        {
            builder.Append(c);
        }
    }

    return builder
        .ToString()
        .Replace('?', 'd')
        .Normalize(NormalizationForm.FormC);
}

private class ReportPreviewData
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Content { get; set; }
    public string DefaultFileName { get; set; }
}
    }
}
