using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Helpers;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Customers
{
    public class frmCustomers : Form, IGlobalSearchHandler
    {
        private readonly ICustomerService _customerService;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblBanner;
        private Panel pnlKpis;
        private CardPanel pnlTable;
        private CardPanel pnlRight;
        private TextBox txtKeyword;
        private ComboBox cboRank;
        private Button btnSearch;
        private Button btnReload;
        private Button btnExport;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnHistory;
        private Label lblShowing;
        private DataGridView dgvCustomers;

        private Label lblTotalCustomersValue;
        private Label lblMembersValue;
        private Label lblNewCustomersValue;
        private Label lblRedeemedPointsValue;

        private Label lblAvatar;
        private Label lblDetailName;
        private Label lblDetailRank;
        private Label lblDetailPhone;
        private Label lblDetailAddress;
        private Label lblDetailJoinDate;
        private Label lblDetailSpend;
        private Label lblDetailPoints;
        private Label lblDetailRedeemValue;
        private Label lblDetailPurchaseCount;
        private DonutChartPanel pnlCategoryChart;
        private FlowLayoutPanel flpCategoryLegend;
        private FlowLayoutPanel flpTopProducts;
        private FlowLayoutPanel flpInsights;
        private Button btnRedeem;
        private Button btnCreateOffer;
        private Button btnViewHistory;

        private List<CustomerDTO> _customers;
        private CustomerDTO _selectedCustomer;

        private static readonly Color Bg = Color.FromArgb(248, 249, 251);
        private static readonly Color Surface = Color.White;
        private static readonly Color Primary = Color.FromArgb(22, 32, 72);
        private static readonly Color Accent = Color.FromArgb(90, 110, 200);
        private static readonly Color Border = Color.FromArgb(228, 231, 238);
        private static readonly Color Muted = Color.FromArgb(120, 132, 160);
        private static readonly Color Soft = Color.FromArgb(245, 247, 252);

        public frmCustomers()
        {
            _customerService = new CustomerService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Quản lý khách hàng";
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Bg;
            Font = new Font("Segoe UI", 9F);
            Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Quản lý khách hàng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblSubtitle = new Label
            {
                Text = "Theo dõi hội viên, điểm thưởng và xu hướng mua sắm",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(22, 48)
            };

            lblBanner = new Label
            {
                Text = "  ⓘ  Khách mua nhiều lần hoặc chi tiêu cao có thể nâng cấp thành hội viên và tích điểm đổi giảm giá.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 95, 20),
                BackColor = Color.FromArgb(255, 248, 229),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlKpis = new Panel
            {
                BackColor = Color.Transparent
            };

            lblTotalCustomersValue = AddKpiCard("👥", "Tổng khách hàng", "0", "So với tháng trước", "+ 0.00%");
            lblMembersValue = AddKpiCard("♛", "Hội viên", "0", "Đang hoạt động", "+ 0.00%");
            lblNewCustomersValue = AddKpiCard("👤+", "Khách mới tháng này", "0", "Từ đầu tháng", "+ 0.00%");
            lblRedeemedPointsValue = AddKpiCard("★", "Điểm đã quy đổi", "0", "Tổng điểm", "+ 0.00%");

            BuildTablePanel();
            BuildRightPanel();

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(lblBanner);
            Controls.Add(pnlKpis);
            Controls.Add(pnlTable);
            Controls.Add(pnlRight);

            Load += FrmCustomers_Load;
            Resize += (s, e) => UpdateResponsiveLayout();
        }

        private Label AddKpiCard(string icon, string title, string value, string subtitle, string delta)
        {
            var card = new CardPanel
            {
                BackColor = Surface,
                Radius = 10,
                BorderColor = Border
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Primary,
                BackColor = Soft,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(36, 36),
                Location = new Point(14, 14)
            };
            ApplyRoundedRegion(lblIcon, 10);

            var lblTitleText = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = false,
                Size = new Size(150, 22),
                Location = new Point(62, 16)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = false,
                Size = new Size(160, 38),
                Location = new Point(14, 60)
            };

            var lblSubtitleText = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Muted,
                AutoSize = false,
                Size = new Size(140, 18),
                Location = new Point(14, 96)
            };

            var lblDelta = new Label
            {
                Text = delta,
                Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 160, 100),
                BackColor = Color.FromArgb(228, 248, 235),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(62, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            card.Controls.AddRange(new Control[] { lblIcon, lblTitleText, lblValue, lblSubtitleText, lblDelta });
            pnlKpis.Controls.Add(card);
            return lblValue;
        }

        private void BuildTablePanel()
        {
            pnlTable = new CardPanel
            {
                BackColor = Surface,
                Radius = 10,
                BorderColor = Border
            };

            var lblListTitle = new Label
            {
                Text = "Danh sách khách hàng",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(16, 16)
            };

            txtKeyword = new TextBox
            {
                Location = new Point(16, 58),
                Size = new Size(250, 28)
            };
            txtKeyword.KeyDown += TxtKeyword_KeyDown;

            cboRank = new ComboBox
            {
                Location = new Point(280, 58),
                Size = new Size(130, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSearch = MakeButton("Tìm kiếm", Primary, Color.White, 90);
            btnSearch.Location = new Point(426, 56);
            btnSearch.Click += (s, e) => SearchCustomers();

            btnReload = MakeButton("Tải lại", Color.FromArgb(230, 233, 240), Color.Black, 80);
            btnReload.Location = new Point(526, 56);
            btnReload.Click += BtnReload_Click;

            btnExport = MakeButton("⇩  Xuất Excel", Surface, Primary, 104);
            btnExport.FlatAppearance.BorderColor = Border;
            btnExport.FlatAppearance.BorderSize = 1;
            btnExport.Location = new Point(616, 56);
            btnExport.Click += BtnExport_Click;

            btnAdd = MakeButton("+  Thêm hội viên", Primary, Color.White, 128);
            btnAdd.Location = new Point(730, 56);
            btnAdd.Click += BtnAdd_Click;

            btnEdit = MakeButton("Sửa", Accent, Color.White, 70);
            btnEdit.Location = new Point(868, 56);
            btnEdit.Click += BtnEdit_Click;

            btnHistory = MakeButton("Lịch sử", Color.FromArgb(230, 233, 240), Color.Black, 78);
            btnHistory.Location = new Point(948, 56);
            btnHistory.Click += BtnHistory_Click;

            dgvCustomers = new DataGridView
            {
                Location = new Point(0, 104),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                BackgroundColor = Surface,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
            dgvCustomers.CellDoubleClick += (s, e) => BtnHistory_Click(s, EventArgs.Empty);
            BuildGridColumns();
            UiGridHelper.ApplyResponsiveStyle(dgvCustomers);

            lblShowing = new Label
            {
                Text = "Hiển thị 0 khách hàng",
                ForeColor = Muted,
                AutoSize = true
            };

            pnlTable.Controls.AddRange(new Control[]
            {
                lblListTitle, txtKeyword, cboRank, btnSearch, btnReload, btnExport,
                btnAdd, btnEdit, btnHistory, dgvCustomers, lblShowing
            });
        }

        private void BuildRightPanel()
        {
            pnlRight = new CardPanel
            {
                BackColor = Surface,
                Radius = 12,
                BorderColor = Border,
                AutoScroll = true
            };

            lblAvatar = new Label
            {
                Text = "KH",
                Size = new Size(54, 54),
                Location = new Point(22, 20),
                BackColor = Color.FromArgb(86, 120, 220),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
            };
            ApplyRoundedRegion(lblAvatar, 27);

            lblDetailName = new Label
            {
                Text = "-",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = false,
                Size = new Size(210, 24),
                Location = new Point(90, 22)
            };

            lblDetailRank = new Label
            {
                Text = "Member",
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(115, 75, 20),
                BackColor = Color.FromArgb(255, 241, 204),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(78, 22),
                Location = new Point(270, 22)
            };
            ApplyRoundedRegion(lblDetailRank, 11);

            lblDetailPhone = MakeDetailLabel("☎ -", 90, 52);
            lblDetailAddress = MakeDetailLabel("⌖ -", 90, 78);
            lblDetailJoinDate = MakeDetailLabel("▣ -", 90, 104);

            lblDetailSpend = MakeSmallMetric("Tổng chi tiêu", "0 đ", 18, 150);
            lblDetailPoints = MakeSmallMetric("Điểm hiện có", "0", 108, 150);
            lblDetailRedeemValue = MakeSmallMetric("Có thể quy đổi", "0 đ", 198, 150);
            lblDetailPurchaseCount = MakeSmallMetric("Số lần mua", "0", 288, 150);

            var lblCategoryTitle = MakeSectionTitle("Chi tiêu theo danh mục", 18, 235);
            pnlCategoryChart = new DonutChartPanel
            {
                Location = new Point(20, 270),
                Size = new Size(105, 105),
                BackColor = Surface
            };

            flpCategoryLegend = new FlowLayoutPanel
            {
                Location = new Point(135, 268),
                Size = new Size(220, 110),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Surface
            };

            var lblTopTitle = MakeSectionTitle("Mặt hàng mua nhiều nhất", 18, 394);
            flpTopProducts = new FlowLayoutPanel
            {
                Location = new Point(18, 426),
                Size = new Size(340, 102),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Surface
            };

            var lblInsightTitle = MakeSectionTitle("Xu hướng tiêu dùng", 18, 546);
            flpInsights = new FlowLayoutPanel
            {
                Location = new Point(18, 578),
                Size = new Size(340, 130),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Surface
            };

            btnRedeem = MakeButton("★  Đổi", Surface, Primary, 86);
            btnRedeem.FlatAppearance.BorderColor = Border;
            btnRedeem.FlatAppearance.BorderSize = 1;
            btnRedeem.Click += (s, e) => ShowSelectedCustomerHistory();

            btnCreateOffer = MakeButton("🎁  Tạo ưu đãi", Surface, Primary, 96);
            btnCreateOffer.FlatAppearance.BorderColor = Border;
            btnCreateOffer.FlatAppearance.BorderSize = 1;
            btnCreateOffer.Click += (s, e) => MessageBox.Show("Chức năng tạo ưu đãi sẽ dùng dữ liệu xu hướng của khách hàng đang chọn.", "Tạo ưu đãi");

            btnViewHistory = MakeButton("☷  Xem lịch sử mua", Surface, Primary, 96);
            btnViewHistory.FlatAppearance.BorderColor = Border;
            btnViewHistory.FlatAppearance.BorderSize = 1;
            btnViewHistory.Click += (s, e) => ShowSelectedCustomerHistory();

            pnlRight.Controls.AddRange(new Control[]
            {
                lblAvatar, lblDetailName, lblDetailRank, lblDetailPhone, lblDetailAddress, lblDetailJoinDate,
                lblCategoryTitle, pnlCategoryChart, flpCategoryLegend, lblTopTitle, flpTopProducts,
                lblInsightTitle, flpInsights, btnRedeem, btnCreateOffer, btnViewHistory
            });
        }

        private Label MakeDetailLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                ForeColor = Color.FromArgb(70, 82, 120),
                AutoSize = false,
                Size = new Size(260, 22),
                Location = new Point(x, y)
            };
        }

        private Label MakeSectionTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private Label MakeSmallMetric(string title, string value, int x, int y)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(82, 58),
                BackColor = Surface
            };
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 8))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(250, 251, 254)))
                using (Pen pen = new Pen(Border))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            var lblTitleText = new Label
            {
                Text = title,
                ForeColor = Muted,
                Font = new Font("Segoe UI", 7F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(82, 20),
                Location = new Point(0, 6)
            };

            var lblValue = new Label
            {
                Text = value,
                ForeColor = Primary,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(82, 24),
                Location = new Point(0, 27)
            };

            panel.Controls.Add(lblTitleText);
            panel.Controls.Add(lblValue);
            pnlRight?.Controls.Add(panel);
            return lblValue;
        }

        private Button MakeButton(string text, Color bg, Color fg, int width)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(width, 32),
                BackColor = bg,
                ForeColor = fg,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void BuildGridColumns()
        {
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "RawMaKH", HeaderText = "RawMaKH", DataPropertyName = "RawMaKH", Visible = false });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaKH", HeaderText = "Mã KH", DataPropertyName = "MaKH", Width = 110 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "HoTen", HeaderText = "Họ tên", DataPropertyName = "HoTen", Width = 210 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoDienThoai", HeaderText = "SĐT", DataPropertyName = "SoDienThoai", Width = 140 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "HangThanhVien", HeaderText = "Hạng thành viên", DataPropertyName = "HangThanhVien", Width = 145 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiemHienCo", HeaderText = "Điểm tích lũy", DataPropertyName = "DiemHienCo", Width = 120 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongChiTieuText", HeaderText = "Tổng chi tiêu", DataPropertyName = "TongChiTieuText", Width = 145 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "TopProduct", HeaderText = "Mặt hàng top", DataPropertyName = "TopProduct", Width = 300 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThaiText", HeaderText = "Trạng thái", DataPropertyName = "TrangThaiText", Width = 130 });
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void FrmCustomers_Load(object sender, EventArgs e)
        {
            LoadRankFilter();
            LoadCustomers();
            UpdateResponsiveLayout();
        }

        private void LoadRankFilter()
        {
            cboRank.Items.Clear();
            cboRank.Items.Add(new ComboBoxItem(string.Empty, "Tất cả hạng"));
            cboRank.Items.Add(new ComboBoxItem("Member", "Member"));
            cboRank.Items.Add(new ComboBoxItem("Silver", "Silver"));
            cboRank.Items.Add(new ComboBoxItem("Gold", "Gold"));
            cboRank.Items.Add(new ComboBoxItem("Platinum", "Platinum"));
            cboRank.SelectedIndex = 0;
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAll().ToList();
            BindGrid(_customers);
            BindStats();
        }

        private void SearchCustomers()
        {
            var request = new CustomerSearchRequest
            {
                Keyword = txtKeyword.Text.Trim()
            };

            if (cboRank.SelectedItem is ComboBoxItem rankItem && !string.IsNullOrWhiteSpace(rankItem.Value))
            {
                request.HangThanhVien = rankItem.Value;
            }

            _customers = _customerService.Search(request).ToList();
            BindGrid(_customers);
        }

        private void BindGrid(List<CustomerDTO> customers)
        {
            var rows = customers.Select(x => new
            {
                MaKH = "KH" + x.MaKH.ToString("D6"),
                RawMaKH = x.MaKH,
                x.HoTen,
                x.SoDienThoai,
                x.HangThanhVien,
                x.DiemHienCo,
                TongChiTieuText = x.TongChiTieu.ToString("N0") + " đ",
                TopProduct = string.IsNullOrWhiteSpace(x.TopProduct) ? "-" : x.TopProduct,
                TrangThaiText = x.TrangThai ? "Đang hoạt động" : "Ngừng"
            }).ToList();

            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = rows;
            if (dgvCustomers.Columns.Contains("RawMaKH"))
            {
                dgvCustomers.Columns["RawMaKH"].Visible = false;
            }

            lblShowing.Text = "Hiển thị " + rows.Count + " khách hàng";

            if (rows.Count > 0)
            {
                dgvCustomers.ClearSelection();
                dgvCustomers.Rows[0].Selected = true;
                dgvCustomers.CurrentCell = dgvCustomers.Rows[0].Cells["MaKH"];
                SelectCustomerById((int)rows[0].RawMaKH);
            }
            else
            {
                BindCustomerDetail(null);
            }
        }

        private void BindStats()
        {
            CustomerStatsResponse stats = _customerService.GetStats();
            lblTotalCustomersValue.Text = stats.TotalCustomers.ToString("N0");
            lblMembersValue.Text = stats.MemberCustomers.ToString("N0");
            lblNewCustomersValue.Text = stats.NewCustomersThisMonth.ToString("N0");
            lblRedeemedPointsValue.Text = stats.RedeemedPoints.ToString("N0");
        }

        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow == null || dgvCustomers.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            object rawValue = dgvCustomers.CurrentRow.Cells["RawMaKH"].Value;
            int maKH;
            if (rawValue != null && int.TryParse(rawValue.ToString(), out maKH))
            {
                SelectCustomerById(maKH);
            }
        }

        private void SelectCustomerById(int maKH)
        {
            _selectedCustomer = _customerService.GetById(maKH);
            BindCustomerDetail(_selectedCustomer);
        }

        private void BindCustomerDetail(CustomerDTO customer)
        {
            if (customer == null)
            {
                lblAvatar.Text = "KH";
                lblDetailName.Text = "Chưa chọn khách hàng";
                lblDetailRank.Text = "-";
                lblDetailPhone.Text = "☎ -";
                lblDetailAddress.Text = "⌖ -";
                lblDetailJoinDate.Text = "▣ -";
                lblDetailSpend.Text = "0 đ";
                lblDetailPoints.Text = "0";
                lblDetailRedeemValue.Text = "0 đ";
                lblDetailPurchaseCount.Text = "0";
                pnlCategoryChart.SetData(new List<CustomerCategoryTrendResponse>());
                flpCategoryLegend.Controls.Clear();
                flpTopProducts.Controls.Clear();
                flpInsights.Controls.Clear();
                return;
            }

            lblAvatar.Text = GetInitials(customer.HoTen);
            lblDetailName.Text = customer.HoTen;
            lblDetailRank.Text = customer.HangThanhVien;
            lblDetailRank.BackColor = GetRankBackColor(customer.HangThanhVien);
            lblDetailRank.ForeColor = GetRankForeColor(customer.HangThanhVien);
            lblDetailPhone.Text = "☎  " + customer.SoDienThoai;
            lblDetailAddress.Text = "⌖  " + (string.IsNullOrWhiteSpace(customer.DiaChi) ? "-" : customer.DiaChi);
            lblDetailJoinDate.Text = "▣  Ngày tham gia: " + customer.NgayThamGia.ToString("dd/MM/yyyy");
            lblDetailSpend.Text = customer.TongChiTieu.ToString("N0") + " đ";
            lblDetailPoints.Text = customer.DiemHienCo.ToString("N0");
            lblDetailRedeemValue.Text = _customerService.CalculateRedeemValue(customer.DiemHienCo).ToString("N0") + " đ";
            lblDetailPurchaseCount.Text = customer.SoLanMua.ToString("N0");

            var categories = _customerService.GetCategoryTrends(customer.MaKH).ToList();
            pnlCategoryChart.SetData(categories);
            BindCategoryLegend(categories);
            BindTopProducts(_customerService.GetTopProducts(customer.MaKH).ToList());
            BindInsights(customer, categories);
        }

        private void BindCategoryLegend(List<CustomerCategoryTrendResponse> categories)
        {
            flpCategoryLegend.Controls.Clear();
            Color[] colors = DonutChartPanel.ChartColors;

            if (categories.Count == 0)
            {
                flpCategoryLegend.Controls.Add(MakeInfoLabel("Chưa có dữ liệu mua hàng."));
                return;
            }

            for (int i = 0; i < categories.Count && i < 4; i++)
            {
                var item = categories[i];
                var row = new Label
                {
                    Text = "■  " + item.TenLoai + "    " + item.TyLe.ToString("N0") + "%",
                    ForeColor = i < colors.Length ? colors[i] : Muted,
                    Font = new Font("Segoe UI", 8F),
                    AutoSize = false,
                    Size = new Size(210, 22),
                    Margin = new Padding(0, 0, 0, 2)
                };
                flpCategoryLegend.Controls.Add(row);
            }
        }

        private void BindTopProducts(List<CustomerTopProductResponse> products)
        {
            flpTopProducts.Controls.Clear();

            if (products.Count == 0)
            {
                flpTopProducts.Controls.Add(MakeInfoLabel("Chưa có sản phẩm mua nhiều."));
                return;
            }

            for (int i = 0; i < products.Count && i < 3; i++)
            {
                var item = products[i];
                var row = new Label
                {
                    Text = (i + 1) + "   " + item.TenSP + "     " + item.SoLuong + " lần",
                    ForeColor = Primary,
                    Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                    AutoSize = false,
                    Size = new Size(320, 26),
                    Margin = new Padding(0, 0, 0, 4)
                };
                flpTopProducts.Controls.Add(row);
            }
        }

        private void BindInsights(CustomerDTO customer, List<CustomerCategoryTrendResponse> categories)
        {
            flpInsights.Controls.Clear();

            string topCategory = categories.FirstOrDefault()?.TenLoai ?? "nhóm hàng chính";
            flpInsights.Controls.Add(MakeInsightLabel("🛒", "Khách thường mua " + topCategory + " vào cuối tuần."));
            flpInsights.Controls.Add(MakeInsightLabel("↗", "Tần suất mua: " + customer.SoLanMua.ToString("N0") + " đơn đã ghi nhận."));
            flpInsights.Controls.Add(MakeInsightLabel("🎁", customer.DiemHienCo > 0
                ? "Có thể đổi tối đa " + _customerService.CalculateRedeemValue(customer.DiemHienCo).ToString("N0") + " đ cho đơn tới."
                : "Có thể tư vấn đăng ký ưu đãi để tăng quay lại."));
        }

        private Label MakeInfoLabel(string text)
        {
            return new Label
            {
                Text = text,
                ForeColor = Muted,
                Font = new Font("Segoe UI", 8F),
                AutoSize = false,
                Size = new Size(300, 24)
            };
        }

        private Label MakeInsightLabel(string icon, string text)
        {
            return new Label
            {
                Text = icon + "   " + text,
                ForeColor = Color.FromArgb(70, 82, 120),
                BackColor = Color.FromArgb(246, 248, 254),
                Font = new Font("Segoe UI", 8F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(320, 34),
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(8, 0, 6, 0)
            };
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new frmCustomerEdit())
            {
                frm.ShowDialog(this);
                if (frm.IsSavedSuccessfully)
                {
                    LoadCustomers();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            CustomerDTO customer = GetSelectedCustomer();
            if (customer == null)
            {
                return;
            }

            using (var frm = new frmCustomerEdit(customer))
            {
                frm.ShowDialog(this);
                if (frm.IsSavedSuccessfully)
                {
                    LoadCustomers();
                }
            }
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            ShowSelectedCustomerHistory();
        }

        private void ShowSelectedCustomerHistory()
        {
            CustomerDTO customer = GetSelectedCustomer();
            if (customer == null)
            {
                return;
            }

            using (var frm = new frmCustomerHistory(customer))
            {
                frm.ShowDialog(this);
            }
        }

        private CustomerDTO GetSelectedCustomer()
        {
            if (_selectedCustomer != null)
            {
                return _selectedCustomer;
            }

            MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo");
            return null;
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cboRank.SelectedIndex = 0;
            LoadCustomers();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (_customers == null || _customers.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo");
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv";
                dialog.FileName = "customers_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var builder = new StringBuilder();
                builder.AppendLine("MaKH,HoTen,SoDienThoai,HangThanhVien,DiemHienCo,TongChiTieu,SoLanMua,TrangThai");
                foreach (CustomerDTO customer in _customers)
                {
                    builder.AppendLine(string.Join(",",
                        Csv(customer.MaKH.ToString()),
                        Csv(customer.HoTen),
                        Csv(customer.SoDienThoai),
                        Csv(customer.HangThanhVien),
                        Csv(customer.DiemHienCo.ToString()),
                        Csv(customer.TongChiTieu.ToString("0")),
                        Csv(customer.SoLanMua.ToString()),
                        Csv(customer.TrangThai ? "Dang hoat dong" : "Ngung")));
                }

                File.WriteAllText(dialog.FileName, builder.ToString(), new UTF8Encoding(true));
                MessageBox.Show("Xuất dữ liệu thành công.", "Thông báo");
            }
        }

        private string Csv(string value)
        {
            value = value ?? string.Empty;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private void TxtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            SearchCustomers();
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtKeyword.Text = keyword ?? string.Empty;
            SearchCustomers();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                return;
            }

            txtKeyword.Clear();
            SearchCustomers();
        }

        private string GetInitials(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "KH";
            }

            string[] parts = value.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return parts[0].Substring(0, 1).ToUpperInvariant();
            }

            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private Color GetRankBackColor(string rank)
        {
            switch (rank)
            {
                case "Platinum":
                    return Color.FromArgb(236, 224, 255);
                case "Gold":
                    return Color.FromArgb(255, 241, 204);
                case "Silver":
                    return Color.FromArgb(236, 239, 245);
                default:
                    return Color.FromArgb(232, 238, 255);
            }
        }

        private Color GetRankForeColor(string rank)
        {
            switch (rank)
            {
                case "Platinum":
                    return Color.FromArgb(105, 52, 180);
                case "Gold":
                    return Color.FromArgb(145, 92, 10);
                case "Silver":
                    return Color.FromArgb(95, 105, 125);
                default:
                    return Color.FromArgb(55, 80, 160);
            }
        }

        private void UpdateResponsiveLayout()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return;
            }

            int pad = 20;
            int gap = 14;

            lblTitle.Location = new Point(pad, 18);
            lblSubtitle.Location = new Point(pad + 2, 48);

            lblBanner.SetBounds(
                pad,
                78,
                Math.Max(300, ClientSize.Width - pad * 2),
                42);

            int contentTop = 136;
            int contentHeight = Math.Max(420, ClientSize.Height - contentTop - 20);

            bool showRightPanel = ClientSize.Width >= 1100;

            int rightWidth = showRightPanel
                ? Math.Min(390, Math.Max(340, ClientSize.Width / 3))
                : 0;

            int leftWidth = showRightPanel
                ? ClientSize.Width - pad * 2 - gap - rightWidth
                : ClientSize.Width - pad * 2;

            leftWidth = Math.Max(620, leftWidth);

            pnlKpis.SetBounds(pad, contentTop, leftWidth, 112);

            int tableTop = contentTop + 130;
            pnlTable.SetBounds(
                pad,
                tableTop,
                leftWidth,
                Math.Max(300, ClientSize.Height - tableTop - 20));

            pnlRight.Visible = showRightPanel;

            if (showRightPanel)
            {
                pnlRight.SetBounds(
                    pnlTable.Right + gap,
                    contentTop,
                    rightWidth,
                    contentHeight);
            }

            LayoutKpiCards();
            LayoutTablePanel();
            LayoutRightPanel();
        }
        private void LayoutKpiCards()
        {
            if (pnlKpis == null)
            {
                return;
            }

            int gap = 12;
            int count = Math.Max(1, pnlKpis.Controls.Count);
            int cardWidth = Math.Max(145, (pnlKpis.Width - gap * (count - 1)) / count);

            for (int i = 0; i < pnlKpis.Controls.Count; i++)
            {
                Control card = pnlKpis.Controls[i];
                card.SetBounds(i * (cardWidth + gap), 0, cardWidth, 112);

                foreach (Control child in card.Controls)
                {
                    if (child.Anchor.HasFlag(AnchorStyles.Right))
                    {
                        child.Left = card.Width - child.Width - 12;
                    }
                }
            }
        }

        private void LayoutTablePanel()
        {
            if (pnlTable == null)
            {
                return;
            }

            int width = pnlTable.Width;
            int padding = 16;
            int toolbarY = 58;

            // Action buttons từ phải sang trái
            int x = width - padding;

            btnHistory.Location = new Point(x - btnHistory.Width, 56);
            x = btnHistory.Left - 10;

            btnEdit.Location = new Point(x - btnEdit.Width, 56);
            x = btnEdit.Left - 10;

            btnAdd.Location = new Point(x - btnAdd.Width, 56);
            x = btnAdd.Left - 10;

            btnExport.Location = new Point(x - btnExport.Width, 56);
            x = btnExport.Left - 10;

            btnReload.Location = new Point(x - btnReload.Width, 56);
            x = btnReload.Left - 10;

            btnSearch.Location = new Point(x - btnSearch.Width, 56);

            // Filter bên trái
            int filterRight = btnSearch.Left - 12;
            int rankWidth = 130;
            int keywordWidth = Math.Max(180, filterRight - padding - rankWidth - 12);

            // Nếu không đủ chỗ, ẩn bớt nút phụ
            bool compact = keywordWidth < 170 || width < 760;

            btnExport.Visible = !compact;
            btnEdit.Visible = true;
            btnHistory.Visible = true;

            if (compact)
            {
                x = width - padding;

                btnAdd.Location = new Point(x - btnAdd.Width, 56);
                x = btnAdd.Left - 10;

                btnReload.Location = new Point(x - btnReload.Width, 56);
                x = btnReload.Left - 10;

                btnSearch.Location = new Point(x - btnSearch.Width, 56);

                filterRight = btnSearch.Left - 12;
                keywordWidth = Math.Max(180, filterRight - padding - rankWidth - 12);
            }

            txtKeyword.SetBounds(padding, toolbarY, keywordWidth, 28);
            cboRank.SetBounds(txtKeyword.Right + 12, toolbarY, rankWidth, 28);

            dgvCustomers.SetBounds(
                0,
                104,
                width,
                Math.Max(160, pnlTable.Height - 142));

            lblShowing.Location = new Point(padding, pnlTable.Height - 30);
        }

        private void LayoutRightPanel()
        {
            if (pnlRight == null || !pnlRight.Visible)
            {
                return;
            }

            int w = pnlRight.ClientSize.Width;
            int pad = 18;

            lblAvatar.SetBounds(pad, 20, 54, 54);

            int detailX = 88;
            int detailW = Math.Max(150, w - detailX - pad);

            lblDetailName.SetBounds(detailX, 22, detailW - 84, 24);
            lblDetailRank.SetBounds(w - pad - 78, 22, 78, 22);

            lblDetailPhone.SetBounds(detailX, 52, detailW, 22);
            lblDetailAddress.SetBounds(detailX, 78, detailW, 22);
            lblDetailJoinDate.SetBounds(detailX, 104, detailW, 22);

            // 4 metric nhỏ chia đều theo chiều rộng panel
            int metricGap = 8;
            int metricY = 150;
            int metricW = Math.Max(68, (w - pad * 2 - metricGap * 3) / 4);

            LayoutMetricLabel(lblDetailSpend, pad + 0 * (metricW + metricGap), metricY, metricW);
            LayoutMetricLabel(lblDetailPoints, pad + 1 * (metricW + metricGap), metricY, metricW);
            LayoutMetricLabel(lblDetailRedeemValue, pad + 2 * (metricW + metricGap), metricY, metricW);
            LayoutMetricLabel(lblDetailPurchaseCount, pad + 3 * (metricW + metricGap), metricY, metricW);

            pnlCategoryChart.SetBounds(pad, 270, 105, 105);
            flpCategoryLegend.SetBounds(135, 268, Math.Max(160, w - 150), 110);

            flpTopProducts.SetBounds(pad, 426, w - pad * 2, 102);
            flpInsights.SetBounds(pad, 578, w - pad * 2, 130);

            foreach (Control c in flpTopProducts.Controls)
            {
                c.Width = flpTopProducts.Width - 4;
            }

            foreach (Control c in flpInsights.Controls)
            {
                c.Width = flpInsights.Width - 4;
            }

            // Nút dưới cùng
            int buttonY = Math.Max(730, pnlRight.ClientSize.Height - 46);
            int buttonGap = 8;
            int buttonW = Math.Max(82, (w - pad * 2 - buttonGap * 2) / 3);

            btnRedeem.SetBounds(pad, buttonY, buttonW, 32);
            btnCreateOffer.SetBounds(btnRedeem.Right + buttonGap, buttonY, buttonW, 32);
            btnViewHistory.SetBounds(btnCreateOffer.Right + buttonGap, buttonY, buttonW, 32);
        }
        private void LayoutMetricLabel(Label valueLabel, int x, int y, int width)
        {
            if (valueLabel == null || valueLabel.Parent == null)
            {
                return;
            }

            Panel panel = valueLabel.Parent as Panel;
            if (panel == null)
            {
                return;
            }

            panel.SetBounds(x, y, width, 58);

            foreach (Control child in panel.Controls)
            {
                child.Width = width;
            }
        }
        private static void ApplyRoundedRegion(Control ctrl, int radius)
        {
            using (var path = new GraphicsPath())
            {
                int d = radius * 2;
                Rectangle r = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                ctrl.Region = new Region(path);
            }
        }

        private class ComboBoxItem
        {
            public string Value { get; set; }

            public string Text { get; set; }

            public ComboBoxItem(string value, string text)
            {
                Value = value;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private class CardPanel : Panel
        {
            public int Radius { get; set; } = 10;

            public Color BorderColor { get; set; } = Border;

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (GraphicsPath path = RoundedPath(rect, Radius))
                using (SolidBrush brush = new SolidBrush(BackColor))
                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private class DonutChartPanel : Panel
        {
            public static readonly Color[] ChartColors =
            {
                Color.FromArgb(22, 32, 72),
                Color.FromArgb(90, 110, 220),
                Color.FromArgb(82, 185, 120),
                Color.FromArgb(170, 180, 205)
            };

            private List<CustomerCategoryTrendResponse> _items = new List<CustomerCategoryTrendResponse>();

            public void SetData(List<CustomerCategoryTrendResponse> items)
            {
                _items = items ?? new List<CustomerCategoryTrendResponse>();
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(8, 8, Width - 16, Height - 16);

                if (_items.Count == 0)
                {
                    using (var pen = new Pen(Color.FromArgb(228, 231, 238), 14))
                    {
                        e.Graphics.DrawEllipse(pen, rect);
                    }
                    return;
                }

                float start = -90f;
                decimal total = _items.Sum(x => x.SoLuong);
                for (int i = 0; i < _items.Count && i < ChartColors.Length; i++)
                {
                    float sweep = total <= 0 ? 0 : (float)(_items[i].SoLuong / total * 360m);
                    using (var pen = new Pen(ChartColors[i], 16))
                    {
                        e.Graphics.DrawArc(pen, rect, start, sweep);
                    }
                    start += sweep;
                }
            }
        }

        private static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
