using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.UI.Helpers;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmReports : Form, IGlobalSearchHandler
    {
        private const string RevenueReportType = "Báo cáo doanh thu";
        private const string InvoiceReportType = "Báo cáo hóa đơn";
        private const string StockInReportType = "Báo cáo nhập kho";
        private const string InventoryReportType = "Báo cáo tồn kho";

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
            Text = "Báo cáo";
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(248, 249, 251);
            Font = new Font("Segoe UI", 9F);
            Dock = DockStyle.Fill;

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
            cboReportType.Items.AddRange(new object[]
            {
                RevenueReportType,
                InvoiceReportType,
                StockInReportType,
                InventoryReportType
            });
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

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(lblFromDate);
            Controls.Add(dtpFromDate);
            Controls.Add(lblToDate);
            Controls.Add(dtpToDate);
            Controls.Add(lblReportType);
            Controls.Add(cboReportType);
            Controls.Add(btnGenerate);
            Controls.Add(btnPreview);
            Controls.Add(btnExport);
            Controls.Add(pnlSummary);
            Controls.Add(pnlReportViewer);

            cboReportType.SelectedIndexChanged += (s, e) => RenderSelectedReport();
            Load += FrmReports_Load;
            Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void FrmReports_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsStaff)
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo tổng.", "Thông báo");
                BeginInvoke(new Action(Close));
                return;
            }

            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            RefreshReport();
            UpdateResponsiveLayout();
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

            RefreshReport();
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            if (!ValidateDateRange())
            {
                return;
            }

            RefreshReport();

            ReportPreviewData previewData = BuildCurrentReportPreview();
            using (frmReportTextPreview frm = new frmReportTextPreview(
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

            RefreshReport();
            ExportReportToFile(BuildCurrentReportPreview());
        }

        private void RefreshReport()
        {
            LoadSummary();
            RenderSelectedReport();
        }

        private void LoadSummary()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            List<InvoiceDTO> invoices = _invoiceService.GetAll()
                .Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate)
                .ToList();

            List<StockInDTO> stockIns = _stockInService.GetAll()
                .Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDate)
                .ToList();

            List<ProductDTO> lowStockProducts = _productService.GetAll()
                .Where(x =>
                    x.TrangThai &&
                    x.SoLuongTon <= 10 &&
                    (!x.HanSuDung.HasValue || x.HanSuDung.Value.Date >= DateTime.Today))
                .ToList();

            decimal tongDoanhThu = invoices
                .Where(x => string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                .Sum(x => x.TongTien);

            lblTongDoanhThu.Text = "Tổng doanh thu" + Environment.NewLine + tongDoanhThu.ToString("N0") + " đ";
            lblTongHoaDon.Text = "Số hóa đơn" + Environment.NewLine + invoices.Count.ToString();
            lblTongNhapKho.Text = "Phiếu nhập" + Environment.NewLine + stockIns.Count.ToString();
            lblTonKhoThap.Text = "Sắp hết hàng" + Environment.NewLine + lowStockProducts.Count.ToString();
        }

        private bool ValidateDateRange()
        {
            if (dtpFromDate.Value.Date <= dtpToDate.Value.Date)
            {
                return true;
            }

            MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Thông báo");
            return false;
        }

        private void RenderSelectedReport()
        {
            string selected = cboReportType.SelectedItem as string ?? string.Empty;
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            pnlReportViewer.Controls.Clear();

            if (selected == RevenueReportType)
            {
                frmRevenueReport frm = new frmRevenueReport(fromDate, toDate)
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

            if (selected == InvoiceReportType)
            {
                pnlReportViewer.Controls.Add(BuildInvoiceReportView(fromDate, toDate));
                return;
            }

            if (selected == StockInReportType)
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
                    MucDo = GetInventoryLevelLabel(x.SoLuongTon)
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
                BackColor = Color.White,
                Padding = new Padding(18, 16, 18, 18)
            };

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.White
            };

            Label lblViewTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            Label lblViewSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 26)
            };

            header.Controls.Add(lblViewTitle);
            header.Controls.Add(lblViewSubtitle);

            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
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
            UiGridHelper.ApplyResponsiveStyle(grid);

            wrapper.Controls.Add(grid);
            wrapper.Controls.Add(header);

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
            string selected = cboReportType.SelectedItem as string ?? string.Empty;

            if (selected == InvoiceReportType)
            {
                return BuildInvoicePreview();
            }

            if (selected == StockInReportType)
            {
                return BuildStockInPreview();
            }

            if (selected == InventoryReportType)
            {
                return BuildInventoryPreview();
            }

            return BuildRevenuePreview();
        }

        private ReportPreviewData BuildRevenuePreview()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            List<InvoiceDTO> invoices = _invoiceService.GetAll()
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
            builder.AppendLine("BÁO CÁO DOANH THU");
            builder.AppendLine("Từ ngày: " + fromDate.ToString("dd/MM/yyyy"));
            builder.AppendLine("Đến ngày: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
            builder.AppendLine(new string('=', 60));
            builder.AppendLine("Tổng doanh thu : " + tongDoanhThu.ToString("N0") + " đ");
            builder.AppendLine("Số hóa đơn     : " + tongHoaDon);
            builder.AppendLine("Trung bình/đơn : " + trungBinhDon.ToString("N0") + " đ");
            builder.AppendLine(new string('-', 60));
            builder.AppendLine("Ngày".PadRight(16) + "Số HĐ".PadRight(12) + "Doanh thu");
            builder.AppendLine(new string('-', 60));

            foreach (var item in revenueByDate)
            {
                builder.AppendLine(
                    item.Ngay.ToString("dd/MM/yyyy").PadRight(16) +
                    item.SoHoaDon.ToString().PadRight(12) +
                    item.DoanhThu.ToString("N0") + " đ");
            }

            if (!revenueByDate.Any())
            {
                builder.AppendLine("Không có dữ liệu trong khoảng thời gian đã chọn.");
            }

            return new ReportPreviewData
            {
                Title = "Xem trước báo cáo doanh thu",
                Subtitle = "Kiểm tra báo cáo doanh thu trước khi in hoặc xuất file",
                Content = builder.ToString(),
                DefaultFileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            };
        }

        private ReportPreviewData BuildInvoicePreview()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            List<InvoiceDTO> invoices = _invoiceService.GetAll()
                .Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate)
                .OrderByDescending(x => x.NgayLap)
                .ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("BÁO CÁO HÓA ĐƠN");
            builder.AppendLine("Từ ngày: " + fromDate.ToString("dd/MM/yyyy"));
            builder.AppendLine("Đến ngày: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
            builder.AppendLine(new string('=', 90));
            builder.AppendLine("Tổng hóa đơn : " + invoices.Count);
            builder.AppendLine("Đã thanh toán: " + invoices.Count(x => string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase)));
            builder.AppendLine("Đã hủy       : " + invoices.Count(x => string.Equals(x.TrangThai, "Cancelled", StringComparison.OrdinalIgnoreCase)));
            builder.AppendLine(new string('-', 90));
            builder.AppendLine("Mã".PadRight(8) + "Ngày lập".PadRight(20) + "NV".PadRight(8) + "Trạng thái".PadRight(18) + "Tổng tiền");
            builder.AppendLine(new string('-', 90));

            foreach (InvoiceDTO item in invoices)
            {
                builder.AppendLine(
                    item.MaHD.ToString().PadRight(8) +
                    item.NgayLap.ToString("dd/MM/yyyy HH:mm").PadRight(20) +
                    item.MaNV.ToString().PadRight(8) +
                    GetInvoiceStatusLabel(item.TrangThai).PadRight(18) +
                    item.TongTien.ToString("N0") + " đ");

                if (!string.IsNullOrWhiteSpace(item.GhiChu))
                {
                    builder.AppendLine("  Ghi chú: " + item.GhiChu);
                }
            }

            if (!invoices.Any())
            {
                builder.AppendLine("Không có hóa đơn trong khoảng thời gian đã chọn.");
            }

            return new ReportPreviewData
            {
                Title = "Xem trước báo cáo hóa đơn",
                Subtitle = "Kiểm tra danh sách hóa đơn trước khi in hoặc xuất file",
                Content = builder.ToString(),
                DefaultFileName = "BaoCaoHoaDon_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            };
        }

        private ReportPreviewData BuildStockInPreview()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            List<StockInDTO> stockIns = _stockInService.GetAll()
                .Where(x => x.NgayNhap >= fromDate && x.NgayNhap <= toDate)
                .OrderByDescending(x => x.NgayNhap)
                .ToList();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("BÁO CÁO NHẬP KHO");
            builder.AppendLine("Từ ngày: " + fromDate.ToString("dd/MM/yyyy"));
            builder.AppendLine("Đến ngày: " + dtpToDate.Value.Date.ToString("dd/MM/yyyy"));
            builder.AppendLine(new string('=', 90));
            builder.AppendLine("Số phiếu nhập : " + stockIns.Count);
            builder.AppendLine("Tổng tiền nhập: " + stockIns.Sum(x => x.TongTien).ToString("N0") + " đ");
            builder.AppendLine(new string('-', 90));
            builder.AppendLine("Mã".PadRight(8) + "Ngày nhập".PadRight(20) + "NV".PadRight(8) + "Tổng tiền");
            builder.AppendLine(new string('-', 90));

            foreach (StockInDTO item in stockIns)
            {
                builder.AppendLine(
                    item.MaPN.ToString().PadRight(8) +
                    item.NgayNhap.ToString("dd/MM/yyyy HH:mm").PadRight(20) +
                    item.MaNV.ToString().PadRight(8) +
                    item.TongTien.ToString("N0") + " đ");

                if (!string.IsNullOrWhiteSpace(item.GhiChu))
                {
                    builder.AppendLine("  Ghi chú: " + item.GhiChu);
                }
            }

            if (!stockIns.Any())
            {
                builder.AppendLine("Không có phiếu nhập trong khoảng thời gian đã chọn.");
            }

            return new ReportPreviewData
            {
                Title = "Xem trước báo cáo nhập kho",
                Subtitle = "Kiểm tra danh sách phiếu nhập trước khi in hoặc xuất file",
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
            builder.AppendLine("BÁO CÁO TỒN KHO");
            builder.AppendLine("Ngày lập: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            builder.AppendLine(new string('=', 110));
            builder.AppendLine("Tổng sản phẩm đang bán : " + products.Count);
            builder.AppendLine("Sắp hết hàng (<=10)    : " + products.Count(x => x.SoLuongTon <= 10));
            builder.AppendLine("Cần nhập gấp (<=5)     : " + products.Count(x => x.SoLuongTon <= 5));
            builder.AppendLine(new string('-', 110));
            builder.AppendLine(
                "Mã".PadRight(8) +
                "Tên sản phẩm".PadRight(30) +
                "Tồn".PadRight(8) +
                "Giá bán".PadRight(16) +
                "HSD".PadRight(16) +
                "Mức độ");
            builder.AppendLine(new string('-', 110));

            foreach (ProductDTO item in products)
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
                    (item.GiaBan.ToString("N0") + " đ").PadRight(16) +
                    (item.HanSuDung.HasValue ? item.HanSuDung.Value.ToString("dd/MM/yyyy") : "-").PadRight(16) +
                    GetInventoryLevelLabel(item.SoLuongTon));
            }

            if (!products.Any())
            {
                builder.AppendLine("Không có sản phẩm đang kinh doanh.");
            }

            return new ReportPreviewData
            {
                Title = "Xem trước báo cáo tồn kho",
                Subtitle = "Kiểm tra tình trạng tồn kho trước khi in hoặc xuất file",
                Content = builder.ToString(),
                DefaultFileName = "BaoCaoTonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            };
        }

        private string GetInventoryLevelLabel(int soLuongTon)
        {
            if (soLuongTon <= 5)
            {
                return "Cần nhập gấp";
            }

            if (soLuongTon <= 10)
            {
                return "Sắp hết";
            }

            return "Ổn định";
        }

        private void ExportReportToFile(ReportPreviewData previewData)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Xuất báo cáo";
                saveDialog.Filter = "Text file (*.txt)|*.txt";
                saveDialog.FileName = previewData.DefaultFileName;

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    File.WriteAllText(saveDialog.FileName, previewData.Content, Encoding.UTF8);
                    MessageBox.Show("Xuất file thành công.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất file thất bại. " + ex.Message, "Thông báo");
                }
            }
        }

        public void ApplyGlobalSearch(string keyword)
        {
            string normalized = NormalizeSearchText(keyword);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return;
            }

            int? targetIndex = null;

            if (normalized.Contains("hoa don") || normalized.Contains("invoice"))
            {
                targetIndex = 1;
            }
            else if (normalized.Contains("nhap kho") || normalized.Contains("phieu nhap") || normalized.Contains("stock in"))
            {
                targetIndex = 2;
            }
            else if (normalized.Contains("ton kho") || normalized.Contains("inventory"))
            {
                targetIndex = 3;
            }
            else if (normalized.Contains("doanh thu") || normalized.Contains("revenue") || normalized.Contains("bao cao"))
            {
                targetIndex = 0;
            }

            if (!targetIndex.HasValue)
            {
                return;
            }

            if (cboReportType.SelectedIndex != targetIndex.Value)
            {
                cboReportType.SelectedIndex = targetIndex.Value;
            }

            RefreshReport();
        }

        public void ClearGlobalSearch()
        {
            if (cboReportType.SelectedIndex != 0)
            {
                cboReportType.SelectedIndex = 0;
            }

            RefreshReport();
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
                .Replace('đ', 'd')
                .Normalize(NormalizationForm.FormC);
        }

        private sealed class ReportPreviewData
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Content { get; set; }
            public string DefaultFileName { get; set; }
        }

        private void UpdateResponsiveLayout()
        {
            int left = 20;
            int right = 20;
            int width = Math.Max(320, ClientSize.Width - left - right);

            pnlSummary.SetBounds(left, 160, width, 90);
            pnlReportViewer.SetBounds(left, 270, width, Math.Max(220, ClientSize.Height - 290));
        }
    }
}
