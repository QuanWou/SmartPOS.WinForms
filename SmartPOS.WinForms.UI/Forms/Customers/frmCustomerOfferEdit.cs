using System;
using System.Drawing;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.Forms.Customers
{
    public class frmCustomerOfferEdit : Form
    {
        private readonly CustomerDTO _customer;
        private readonly ICustomerOfferService _customerOfferService;

        private Label lblCustomer;
        private TextBox txtTenUuDai;
        private NumericUpDown nudPhanTramGiam;
        private CheckBox chkHasExpiry;
        private DateTimePicker dtpNgayHetHan;
        private TextBox txtGhiChu;
        private Button btnSave;
        private Button btnCancel;

        public bool IsSavedSuccessfully { get; private set; }

        public frmCustomerOfferEdit(CustomerDTO customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _customerOfferService = new CustomerOfferService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Tao uu dai khach hang";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, 330);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            var lblTitle = new Label
            {
                Text = "Tao uu dai khach hang",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblCustomer = new Label
            {
                Text = "Khach hang: " + _customer.HoTen,
                ForeColor = Color.FromArgb(90, 100, 125),
                AutoSize = false,
                Size = new Size(370, 22),
                Location = new Point(22, 54)
            };

            var lblName = MakeLabel("Ten uu dai", 22, 92);
            txtTenUuDai = new TextBox
            {
                Location = new Point(128, 88),
                Size = new Size(250, 26)
            };

            var lblPercent = MakeLabel("% giam", 22, 130);
            nudPhanTramGiam = new NumericUpDown
            {
                Location = new Point(128, 126),
                Size = new Size(90, 26),
                Minimum = 1,
                Maximum = 100,
                DecimalPlaces = 0,
                Value = 10
            };

            chkHasExpiry = new CheckBox
            {
                Text = "Co han dung",
                AutoSize = true,
                Location = new Point(128, 164)
            };
            chkHasExpiry.CheckedChanged += (s, e) => dtpNgayHetHan.Enabled = chkHasExpiry.Checked;

            dtpNgayHetHan = new DateTimePicker
            {
                Location = new Point(238, 160),
                Size = new Size(140, 26),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(30),
                Enabled = false
            };

            var lblNote = MakeLabel("Ghi chu", 22, 204);
            txtGhiChu = new TextBox
            {
                Location = new Point(128, 200),
                Size = new Size(250, 58),
                Multiline = true
            };

            btnSave = new Button
            {
                Text = "Luu",
                Size = new Size(96, 34),
                Location = new Point(178, 280),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Dong",
                Size = new Size(96, 34),
                Location = new Point(284, 280),
                DialogResult = DialogResult.Cancel,
                BackColor = Color.FromArgb(235, 238, 245),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[]
            {
                lblTitle, lblCustomer, lblName, txtTenUuDai, lblPercent, nudPhanTramGiam,
                chkHasExpiry, dtpNgayHetHan, lblNote, txtGhiChu, btnSave, btnCancel
            });
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = Color.FromArgb(90, 100, 125),
                Location = new Point(x, y)
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var offer = new CustomerOfferDTO
            {
                MaKH = _customer.MaKH,
                TenUuDai = txtTenUuDai.Text,
                PhanTramGiam = nudPhanTramGiam.Value,
                NgayHetHan = chkHasExpiry.Checked ? (DateTime?)dtpNgayHetHan.Value.Date : null,
                GhiChu = txtGhiChu.Text
            };

            OperationResult result = _customerOfferService.Insert(offer);
            MessageBox.Show(
                result.Message,
                "Thong bao",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (!result.IsSuccess)
            {
                return;
            }

            IsSavedSuccessfully = true;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
