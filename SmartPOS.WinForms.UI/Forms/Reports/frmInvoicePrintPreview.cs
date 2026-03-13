using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmInvoicePrintPreview : Form
    {
        private readonly int _invoiceId;
        private readonly IReportService _reportService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlPreview;
        private TextBox txtPreview;

        private Button btnPrint;
        private Button btnExportFile;
        private Button btnClose;

        private PrintDocument _printDocument;
        private string[] _printLines;
        private int _printLineIndex;

        public frmInvoicePrintPreview(int invoiceId)
        {
            _invoiceId = invoiceId;
            _reportService = new ReportService();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Xem trước hóa đơn";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(980, 680);
            this.MinimumSize = new Size(980, 680);
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = "Xem trước hóa đơn",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = "Kiểm tra nội dung hóa đơn trước khi in",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            pnlPreview = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(920, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtPreview = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 10F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 40, 40)
            };

            pnlPreview.Controls.Add(txtPreview);

            btnExportFile = new Button
            {
                Text = "Xuất file",
                Location = new Point(620, 605),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExportFile.FlatAppearance.BorderSize = 0;
            btnExportFile.Click += BtnExportFile_Click;

            btnPrint = new Button
            {
                Text = "In hóa đơn",
                Location = new Point(730, 605),
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
                Location = new Point(840, 605),
                Size = new Size(100, 34),
                BackColor = Color.FromArgb(230, 233, 240),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += BtnClose_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(pnlPreview);
            this.Controls.Add(btnExportFile);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            this.Load += FrmInvoicePrintPreview_Load;
        }

        private void FrmInvoicePrintPreview_Load(object sender, EventArgs e)
        {
            LoadInvoicePreview();
            InitializePrintDocument();
        }

        private void LoadInvoicePreview()
        {
            var printData = _reportService.GetInvoicePrintData(_invoiceId);

            if (printData == null || printData.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn.", "Thông báo");
                this.Close();
                return;
            }

            InvoicePrintItemDTO first = printData.First();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("==================================================");
            builder.AppendLine("                  SMARTPOS STORE                  ");
            builder.AppendLine("==================================================");
            builder.AppendLine("HÓA ĐƠN BÁN HÀNG");
            builder.AppendLine("--------------------------------------------------");
            builder.AppendLine("Mã hóa đơn : " + first.MaHD);
            builder.AppendLine("Ngày lập   : " + first.NgayLap.ToString("dd/MM/yyyy HH:mm"));
            builder.AppendLine("Nhân viên  : " + first.TenNhanVien);
            builder.AppendLine("Trạng thái : " + first.TrangThai);
            builder.AppendLine("Ghi chú    : " + (string.IsNullOrWhiteSpace(first.GhiChu) ? "" : first.GhiChu));
            builder.AppendLine("--------------------------------------------------");
            builder.AppendLine(string.Format("{0,-6}{1,-24}{2,6}{3,12}", "Mã", "Sản phẩm", "SL", "T.tiền"));
            builder.AppendLine("--------------------------------------------------");

            foreach (InvoicePrintItemDTO item in printData)
            {
                if (item.MaSP <= 0)
                {
                    continue;
                }

                string tenSP = item.TenSP ?? string.Empty;
                if (tenSP.Length > 22)
                {
                    tenSP = tenSP.Substring(0, 22);
                }

                builder.AppendLine(string.Format(
                    "{0,-6}{1,-24}{2,6}{3,12}",
                    item.MaSP,
                    tenSP,
                    item.SoLuong,
                    item.ThanhTien.ToString("N0")));
            }

            builder.AppendLine("--------------------------------------------------");
            builder.AppendLine("TỔNG TIỀN : " + first.TongTien.ToString("N0") + " đ");
            builder.AppendLine("==================================================");
            builder.AppendLine("         Cảm ơn quý khách và hẹn gặp lại!         ");
            builder.AppendLine("==================================================");

            txtPreview.Text = builder.ToString();
            txtPreview.SelectionStart = 0;
            txtPreview.SelectionLength = 0;
        }

        private void InitializePrintDocument()
        {
            _printDocument = new PrintDocument();
            _printDocument.DocumentName = "HoaDon_" + _invoiceId;
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_printLines == null || _printLines.Length == 0)
            {
                e.HasMorePages = false;
                return;
            }

            using (Font printFont = new Font("Consolas", 10F))
            {
                float left = e.MarginBounds.Left;
                float top = e.MarginBounds.Top;
                float lineHeight = printFont.GetHeight(e.Graphics) + 2;
                float y = top;

                while (_printLineIndex < _printLines.Length)
                {
                    if (y + lineHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    e.Graphics.DrawString(_printLines[_printLineIndex], printFont, Brushes.Black, left, y);
                    _printLineIndex++;
                    y += lineHeight;
                }
            }

            e.HasMorePages = false;
            _printLineIndex = 0;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPreview.Text))
            {
                MessageBox.Show("Không có dữ liệu để in.", "Thông báo");
                return;
            }

            _printLines = txtPreview.Text.Replace("\r\n", "\n").Split('\n');
            _printLineIndex = 0;

            using (PrintPreviewDialog previewDialog = new PrintPreviewDialog())
            {
                previewDialog.Document = _printDocument;
                previewDialog.Width = 1000;
                previewDialog.Height = 700;
                previewDialog.ShowDialog(this);
            }
        }

        private void BtnExportFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Xuất hóa đơn";
                saveDialog.Filter = "Text file (*.txt)|*.txt";
                saveDialog.FileName = "HoaDon_" + _invoiceId + ".txt";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtPreview.Text, Encoding.UTF8);
                    MessageBox.Show("Xuất file thành công.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất file thất bại. " + ex.Message, "Thông báo");
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_printDocument != null)
                {
                    _printDocument.PrintPage -= PrintDocument_PrintPage;
                    _printDocument.Dispose();
                    _printDocument = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}