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
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            LoadSummary();
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

            lblTongDoanhThu = BuildSummaryItem("Tổng doanh thu", "0 đ", 20);
            lblTongHoaDon = BuildSummaryItem("Số hóa đơn", "0", 260);
            lblTongNhapKho = BuildSummaryItem("Phiếu nhập", "0", 470);
            lblTonKhoThap = BuildSummaryItem("Sắp hết hàng", "0", 680);

            pnlSummary.Controls.Add(lblTongDoanhThu);
            pnlSummary.Controls.Add(lblTongHoaDon);
            pnlSummary.Controls.Add(lblTongNhapKho);
            pnlSummary.Controls.Add(lblTonKhoThap);
        }

        private Label BuildSummaryItem(string title, string value, int x)
        {
            return new Label
            {
                AutoSize = false,
                Size = new Size(220, 60),
                Location = new Point(x, 15),
                Text = title + Environment.NewLine + value,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72)
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

            string selected = cboReportType.SelectedItem != null
                ? cboReportType.SelectedItem.ToString()
                : string.Empty;

            pnlReportViewer.Controls.Clear();

            if (selected == "Báo cáo doanh thu")
            {
                var frm = new frmRevenueReport
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

            Label lblPlaceholder = new Label
            {
                Text = "Loại báo cáo này sẽ được hoàn thiện ở bước tiếp theo.",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 185, 200),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlReportViewer.Controls.Add(lblPlaceholder);
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            string selected = cboReportType.SelectedItem != null
                ? cboReportType.SelectedItem.ToString()
                : string.Empty;

            if (selected == "Báo cáo doanh thu")
            {
                pnlReportViewer.Controls.Clear();

                var frm = new frmRevenueReport
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

            MessageBox.Show("Chưa hỗ trợ xem trước cho loại báo cáo này.", "Thông báo");
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
                .Where(x => x.TrangThai && x.SoLuongTon <= 10)
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
    }
}