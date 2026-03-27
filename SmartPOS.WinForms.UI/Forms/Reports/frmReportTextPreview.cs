using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Forms.Reports
{
    public class frmReportTextPreview : Form
    {
        private readonly string _content;
        private readonly string _defaultFileName;

        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlPreview;
        private TextBox txtPreview;
        private Button btnExportFile;
        private Button btnPrint;
        private Button btnClose;

        private PrintDocument _printDocument;
        private string[] _printLines;
        private int _printLineIndex;

        public frmReportTextPreview(string title, string subtitle, string content, string defaultFileName)
        {
            _content = content ?? string.Empty;
            _defaultFileName = string.IsNullOrWhiteSpace(defaultFileName)
                ? "BaoCao_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
                : defaultFileName;

            InitializeComponent(title, subtitle);
        }

        private void InitializeComponent(string title, string subtitle)
        {
            this.Text = title;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(980, 680);
            this.MinimumSize = new Size(980, 680);
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);

            lblTitle = new Label
            {
                Text = string.IsNullOrWhiteSpace(title) ? "Xem trước báo cáo" : title,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            lblSubtitle = new Label
            {
                Text = string.IsNullOrWhiteSpace(subtitle) ? "Kiểm tra nội dung báo cáo trước khi in hoặc xuất file" : subtitle,
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
                ForeColor = Color.FromArgb(40, 40, 40),
                Text = _content
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
                Text = "In báo cáo",
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
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(pnlPreview);
            this.Controls.Add(btnExportFile);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnClose);

            this.Load += FrmReportTextPreview_Load;
        }

        private void FrmReportTextPreview_Load(object sender, EventArgs e)
        {
            txtPreview.SelectionStart = 0;
            txtPreview.SelectionLength = 0;
            InitializePrintDocument();
        }

        private void InitializePrintDocument()
        {
            _printDocument = new PrintDocument();
            _printDocument.DocumentName = _defaultFileName.Replace(".txt", string.Empty);
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
                saveDialog.Title = "Xuất báo cáo";
                saveDialog.Filter = "Text file (*.txt)|*.txt";
                saveDialog.FileName = _defaultFileName;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && _printDocument != null)
            {
                _printDocument.PrintPage -= PrintDocument_PrintPage;
                _printDocument.Dispose();
                _printDocument = null;
            }

            base.Dispose(disposing);
        }
    }
}
