using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.UI.Helpers;

namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmRevenueReport : Form
    {
        private readonly IInvoiceService _invoiceService;
        private readonly bool _usePresetRange;
        private readonly DateTime _presetFromDate;
        private readonly DateTime _presetToDate;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblFromDate;
        private Label lblToDate;

        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;

        private Button btnGenerate;
        private Panel pnlSummary;
        private DataGridView dgvRevenue;

        public frmRevenueReport()
        {
            _invoiceService = new InvoiceService();
            InitializeComponent();
        }

        public frmRevenueReport(DateTime fromDate, DateTime toDate)
        {
            _invoiceService = new InvoiceService();
            _usePresetRange = true;
            _presetFromDate = fromDate.Date;
            _presetToDate = toDate.Date;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo cáo doanh thu";
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Báo cáo doanh thu",
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 16)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi doanh thu theo ngày trong khoảng thời gian đã chọn",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 44)
            };

            lblFromDate = new Label
            {
                Text = "Từ ngày",
                AutoSize = true,
                Location = new Point(20, 82)
            };

            dtpFromDate = new DateTimePicker
            {
                Location = new Point(20, 104),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            lblToDate = new Label
            {
                Text = "Đến ngày",
                AutoSize = true,
                Location = new Point(185, 82)
            };

            dtpToDate = new DateTimePicker
            {
                Location = new Point(185, 104),
                Size = new Size(150, 27),
                Format = DateTimePickerFormat.Short
            };

            btnGenerate = new Button
            {
                Text = "Tạo báo cáo",
                Location = new Point(355, 101),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click += BtnGenerate_Click;

            BuildSummaryPanel();
            BuildGrid();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblFromDate);
            this.Controls.Add(dtpFromDate);
            this.Controls.Add(lblToDate);
            this.Controls.Add(dtpToDate);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(pnlSummary);
            this.Controls.Add(dgvRevenue);

            this.Load += FrmRevenueReport_Load;
            this.Resize += FrmRevenueReport_Resize;
        }

        private void FrmRevenueReport_Load(object sender, EventArgs e)
        {
            if (_usePresetRange)
            {
                dtpFromDate.Value = _presetFromDate;
                dtpToDate.Value = _presetToDate;
            }
            else
            {
                dtpFromDate.Value = DateTime.Today.AddMonths(-1);
                dtpToDate.Value = DateTime.Today;
            }

            RecalcLayout();
            LoadReport();
        }

        private void FrmRevenueReport_Resize(object sender, EventArgs e)
        {
            RecalcLayout();
        }

        private void BuildSummaryPanel()
        {
            pnlSummary = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlSummary.Controls.Add(BuildSummaryLabel("Tổng doanh thu", "0 đ", 20));
            pnlSummary.Controls.Add(BuildSummaryLabel("Số hóa đơn", "0", 240));
            pnlSummary.Controls.Add(BuildSummaryLabel("Trung bình / đơn", "0 đ", 430));
            pnlSummary.Controls.Add(BuildSummaryLabel("Ngày cao nhất", "0 đ", 650));
        }

        private Label BuildSummaryLabel(string title, string value, int x)
        {
            return new Label
            {
                AutoSize = false,
                Size = new Size(190, 52),
                Location = new Point(x, 16),
                Text = title + Environment.NewLine + value,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72)
            };
        }

        private void BuildGrid()
        {
            dgvRevenue = new DataGridView
            {
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

            dgvRevenue.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Ngay",
                HeaderText = "Ngày",
                DataPropertyName = "Ngay",
                Width = 160
            });

            dgvRevenue.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoHoaDon",
                HeaderText = "Số hóa đơn",
                DataPropertyName = "SoHoaDon",
                Width = 140
            });

            dgvRevenue.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoanhThu",
                HeaderText = "Doanh thu",
                DataPropertyName = "DoanhThu",
                Width = 180
            });

            dgvRevenue.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                HeaderText = "Ghi chú",
                DataPropertyName = "GhiChu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            UiGridHelper.ApplyResponsiveStyle(dgvRevenue);
        }

        private void RecalcLayout()
        {
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            pnlSummary.SetBounds(20, 150, Math.Max(300, width - 40), 86);
            dgvRevenue.SetBounds(20, 250, Math.Max(300, width - 40), Math.Max(120, height - 270));
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);

            var invoices = _invoiceService.GetAll()
                .Where(x =>
                    x.NgayLap >= fromDate &&
                    x.NgayLap <= toDate &&
                    string.Equals(x.TrangThai, "Paid", StringComparison.OrdinalIgnoreCase))
                .ToList();

            decimal tongDoanhThu = invoices.Sum(x => x.TongTien);
            int soHoaDon = invoices.Count;
            decimal trungBinhDon = soHoaDon > 0 ? tongDoanhThu / soHoaDon : 0;

            var revenueByDate = invoices
                .GroupBy(x => x.NgayLap.Date)
                .Select(g => new
                {
                    NgayDate = g.Key,
                    Ngay = g.Key.ToString("dd/MM/yyyy"),
                    SoHoaDon = g.Count(),
                    DoanhThuRaw = g.Sum(x => x.TongTien),
                    DoanhThu = g.Sum(x => x.TongTien).ToString("N0") + " đ",
                    GhiChu = "Doanh thu ngày " + g.Key.ToString("dd/MM/yyyy")
                })
                .OrderByDescending(x => x.NgayDate)
                .ToList();

            decimal ngayCaoNhat = revenueByDate.Any() ? revenueByDate.Max(x => x.DoanhThuRaw) : 0;

            SetSummaryText(0, "Tổng doanh thu", tongDoanhThu.ToString("N0") + " đ");
            SetSummaryText(1, "Số hóa đơn", soHoaDon.ToString());
            SetSummaryText(2, "Trung bình / đơn", trungBinhDon.ToString("N0") + " đ");
            SetSummaryText(3, "Ngày cao nhất", ngayCaoNhat.ToString("N0") + " đ");

            dgvRevenue.DataSource = null;
            dgvRevenue.DataSource = revenueByDate.Select(x => new
            {
                x.Ngay,
                x.SoHoaDon,
                x.DoanhThu,
                x.GhiChu
            }).ToList();
        }

        private void SetSummaryText(int index, string title, string value)
        {
            if (pnlSummary.Controls.Count > index && pnlSummary.Controls[index] is Label lbl)
            {
                lbl.Text = title + Environment.NewLine + value;
            }
        }
    }
}
