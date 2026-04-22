using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Forms.Products;
using SmartPOS.WinForms.UI.Forms.Shared;
using SmartPOS.WinForms.UI.Helpers;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.Stock
{
    public class frmStockIn : Form, IGlobalSearchHandler
    {
        private readonly IProductService _productService;
        private readonly IStockInService _stockInService;
        private readonly int? _initialProductId;
        private bool _promptInitialProductOnLoad;

        private Panel pnlHeader;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlFooter;
        private Panel pnlLineEditor;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblSearch;
        private Label lblSelectedProduct;
        private Label lblQuantity;
        private Label lblPrice;

        private TextBox txtSearch;
        private NumericUpDown nudQuantity;
        private NumericUpDown nudPrice;
        private CheckBox chkUseExpiry;
        private DateTimePicker dtpExpiry;
        private Button btnSearch;
        private Button btnCameraScan;
        private Button btnPhoneScan;
        private Button btnAddItem;
        private Button btnUpdateItem;
        private Button btnDeleteItem;
        private Button btnClearItem;
        private Button btnSave;

        private DataGridView dgvProducts;
        private DataGridView dgvStockInDetails;

        private Label lblTongPhieu;
        private Label lblTongPhieuValue;
        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private List<ProductDTO> _products;
        private List<StockInDetailDTO> _stockInItems;
        private int? _selectedProductId;
        private Guid? _editingLineId;
        private bool _suppressProductSelectionChanged;

        public frmStockIn()
            : this(null, false)
        {
        }

        public frmStockIn(int? initialProductId, bool promptInitialProductOnLoad = false)
        {
            _productService = new ProductService();
            _stockInService = new StockInService();
            _stockInItems = new List<StockInDetailDTO>();
            _initialProductId = initialProductId;
            _promptInitialProductOnLoad = promptInitialProductOnLoad;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Nhập kho";
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(248, 249, 251);
            this.Font = new Font("Segoe UI", 9F);
            this.Dock = DockStyle.Fill;

            BuildLayout();
            this.Load += FrmStockIn_Load;
            this.Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void FrmStockIn_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsStaff)
            {
                MessageBox.Show("Bạn không có quyền nhập kho.", "Thông báo");
                this.BeginInvoke(new Action(Close));
                return;
            }

            LoadProducts();
            RefreshStockInDetailsView();
            ClearLineEditor();
            ApplyInitialProductSelection();
            UpdateResponsiveLayout();
        }

        private void BuildLayout()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 184,
                BackColor = Color.White,
                Padding = new Padding(20, 16, 20, 12)
            };

            lblTitle = new Label
            {
                Text = "Nhập kho",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(20, 12)
            };

            lblSubtitle = new Label
            {
                Text = "Chọn sản phẩm, quét mã bằng camera nếu cần, rồi nhập tay số lượng, giá nhập và hạn sử dụng",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 42)
            };

            lblSearch = new Label
            {
                Text = "Tìm sản phẩm",
                AutoSize = true,
                Location = new Point(20, 68)
            };

            txtSearch = new TextBox
            {
                Location = new Point(110, 64),
                Size = new Size(240, 27)
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnSearch = new Button
            {
                Text = "Tìm",
                Location = new Point(360, 62),
                Size = new Size(70, 30),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            btnCameraScan = new Button
            {
                Text = "Quét cam",
                Location = new Point(440, 62),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCameraScan.FlatAppearance.BorderSize = 0;
            btnCameraScan.Click += BtnCameraScan_Click;

            btnPhoneScan = new Button
            {
                Text = "Qu\u00e9t \u0111t",
                Location = new Point(550, 62),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(49, 82, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPhoneScan.FlatAppearance.BorderSize = 0;
            btnPhoneScan.Click += BtnPhoneScan_Click;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblSearch);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(btnSearch);
            pnlHeader.Controls.Add(btnCameraScan);
            pnlHeader.Controls.Add(btnPhoneScan);
            BuildLineEditorPanel();
            pnlHeader.Controls.Add(pnlLineEditor);

            pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 480,
                BackColor = Color.FromArgb(248, 249, 251),
                Padding = new Padding(20, 20, 10, 20)
            };

            pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 251),
                Padding = new Padding(10, 20, 20, 20)
            };

            pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.White,
                Padding = new Padding(18, 12, 18, 12)
            };

            BuildLeftPanel();
            BuildRightPanel();
            BuildFooter();

            this.Controls.Add(pnlRight);
            this.Controls.Add(pnlLeft);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private void BuildLeftPanel()
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(14)
            };

            var lbl = new Label
            {
                Text = "Danh sách sản phẩm",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 34,
                TextAlign = ContentAlignment.MiddleLeft
            };

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            dgvProducts.CellClick += (s, e) => LoadSelectedProductIntoEditor();
            dgvProducts.DoubleClick += (s, e) => LoadSelectedProductIntoEditor();
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã",
                DataPropertyName = "MaSP",
                Width = 60
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 180
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuongTon",
                HeaderText = "Tồn",
                DataPropertyName = "SoLuongTon",
                Width = 60
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhap",
                HeaderText = "Giá nhập",
                DataPropertyName = "GiaNhap",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            UiGridHelper.ApplyResponsiveStyle(dgvProducts);

            card.Controls.Add(dgvProducts);
            card.Controls.Add(lbl);
            pnlLeft.Controls.Add(card);
        }

        private void BuildRightPanel()
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(14)
            };

            var titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = Color.White
            };

            var lbl = new Label
            {
                Text = "Chi tiết phiếu nhập",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            titlePanel.Controls.Add(lbl);

            dgvStockInDetails = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            dgvStockInDetails.CellClick += (s, e) => LoadSelectedStockInLineIntoEditor();
            dgvStockInDetails.DoubleClick += (s, e) => LoadSelectedStockInLineIntoEditor();

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LineId",
                HeaderText = "LineId",
                DataPropertyName = "LineId",
                Visible = false
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaSP",
                HeaderText = "Mã SP",
                DataPropertyName = "MaSP",
                Width = 70
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenSP",
                HeaderText = "Tên sản phẩm",
                DataPropertyName = "TenSP",
                Width = 170
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong",
                HeaderText = "SL",
                DataPropertyName = "SoLuong",
                Width = 50
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaNhapLucNhap",
                HeaderText = "Giá nhập",
                DataPropertyName = "GiaNhapLucNhap",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HanSuDung",
                HeaderText = "HSD",
                DataPropertyName = "HanSuDung",
                Width = 90
            });

            dgvStockInDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien",
                HeaderText = "Thành tiền",
                DataPropertyName = "ThanhTien",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            UiGridHelper.ApplyResponsiveStyle(dgvStockInDetails);

            card.Controls.Add(dgvStockInDetails);
            card.Controls.Add(titlePanel);
            pnlRight.Controls.Add(card);
        }

        private void BuildLineEditorPanel()
        {
            pnlLineEditor = new Panel
            {
                Dock = DockStyle.None,
                Height = 72,
                BackColor = Color.FromArgb(250, 251, 253),
                Padding = new Padding(12)
            };

            lblSelectedProduct = new Label
            {
                Text = "Chưa chọn sản phẩm",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = false,
                Location = new Point(12, 8),
                Size = new Size(420, 24)
            };

            lblQuantity = new Label
            {
                Text = "Số lượng",
                AutoSize = true,
                Location = new Point(12, 42)
            };

            nudQuantity = new NumericUpDown
            {
                Location = new Point(78, 38),
                Size = new Size(82, 27),
                Minimum = 1,
                Maximum = 1000000,
                Value = 1,
                ThousandsSeparator = true
            };

            lblPrice = new Label
            {
                Text = "Giá nhập",
                AutoSize = true,
                Location = new Point(174, 42)
            };

            nudPrice = new NumericUpDown
            {
                Location = new Point(236, 38),
                Size = new Size(122, 27),
                Minimum = 0,
                Maximum = 1000000000,
                Increment = 1000,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };

            chkUseExpiry = new CheckBox
            {
                Text = "Có HSD",
                AutoSize = true,
                Location = new Point(372, 41)
            };
            chkUseExpiry.CheckedChanged += (s, e) =>
            {
                dtpExpiry.Enabled = chkUseExpiry.Checked;
            };

            dtpExpiry = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new Point(445, 38),
                Size = new Size(112, 27),
                MinDate = DateTime.Today,
                Enabled = false
            };

            btnAddItem = MakeEditorButton("Thêm dòng", Color.FromArgb(90, 110, 200), Color.White);
            btnAddItem.Click += BtnAddItem_Click;

            btnUpdateItem = MakeEditorButton("Cập nhật", Color.FromArgb(22, 32, 72), Color.White);
            btnUpdateItem.Click += BtnUpdateItem_Click;

            btnDeleteItem = MakeEditorButton("Xóa dòng", Color.FromArgb(220, 80, 80), Color.White);
            btnDeleteItem.Click += BtnDeleteItem_Click;

            btnClearItem = MakeEditorButton("Làm mới", Color.FromArgb(230, 233, 240), Color.Black);
            btnClearItem.Click += (s, e) => ClearLineEditor();

            pnlLineEditor.Controls.Add(lblSelectedProduct);
            pnlLineEditor.Controls.Add(lblQuantity);
            pnlLineEditor.Controls.Add(nudQuantity);
            pnlLineEditor.Controls.Add(lblPrice);
            pnlLineEditor.Controls.Add(nudPrice);
            pnlLineEditor.Controls.Add(chkUseExpiry);
            pnlLineEditor.Controls.Add(dtpExpiry);
            pnlLineEditor.Controls.Add(btnAddItem);
            pnlLineEditor.Controls.Add(btnUpdateItem);
            pnlLineEditor.Controls.Add(btnDeleteItem);
            pnlLineEditor.Controls.Add(btnClearItem);
        }

        private Button MakeEditorButton(string text, Color backColor, Color foreColor)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(94, 32),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void BuildFooter()
        {
            lblTongPhieu = new Label
            {
                Text = "Tổng phiếu nhập",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                Location = new Point(18, 16)
            };

            lblTongPhieuValue = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(18, 40)
            };

            lblGhiChu = new Label
            {
                Text = "Ghi chú",
                AutoSize = true,
                Location = new Point(260, 18)
            };

            txtGhiChu = new TextBox
            {
                Location = new Point(260, 40),
                Size = new Size(380, 27)
            };

            btnSave = new Button
            {
                Text = "Lưu phiếu nhập",
                Size = new Size(150, 38),
                Location = new Point(760, 38),
                BackColor = Color.FromArgb(22, 32, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnlFooter.Controls.Add(lblTongPhieu);
            pnlFooter.Controls.Add(lblTongPhieuValue);
            pnlFooter.Controls.Add(lblGhiChu);
            pnlFooter.Controls.Add(txtGhiChu);
            pnlFooter.Controls.Add(btnSave);
        }

        private void LoadProducts()
        {
            _products = _productService.GetAll()
                .Where(x => x.TrangThai)
                .ToList();

            BindProductsGrid(_products);
        }

        private void ApplyInitialProductSelection()
        {
            if (!_initialProductId.HasValue)
            {
                return;
            }

            ProductDTO product = _products != null
                ? _products.FirstOrDefault(x => x.MaSP == _initialProductId.Value)
                : null;

            if (product == null)
            {
                return;
            }

            txtSearch.Text = !string.IsNullOrWhiteSpace(product.MaVach)
                ? product.MaVach
                : product.TenSP;
            SearchProducts();
            SelectProductRow(product.MaSP);

            if (!_promptInitialProductOnLoad)
            {
                return;
            }

            _promptInitialProductOnLoad = false;
            BeginInvoke((Action)(() => AddProductToStockIn(product)));
        }

        private void BindProductsGrid(List<ProductDTO> products)
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = products.Select(x => new
            {
                x.MaSP,
                x.TenSP,
                x.SoLuongTon,
                x.GiaNhap
            }).ToList();
            ClearGridSelection(dgvProducts);
        }

        private void RefreshStockInDetailsView()
        {
            var viewData = _stockInItems.Select(x =>
            {
                ProductDTO product = _products != null
                    ? _products.FirstOrDefault(p => p.MaSP == x.MaSP)
                    : null;

                return new
                {
                    LineId = x.LineId,
                    x.MaSP,
                    TenSP = product != null ? product.TenSP : string.Empty,
                    x.SoLuong,
                    x.GiaNhapLucNhap,
                    HanSuDung = x.HanSuDung.HasValue ? x.HanSuDung.Value.ToString("dd/MM/yyyy") : "-",
                    x.ThanhTien
                };
            }).ToList();

            dgvStockInDetails.DataSource = null;
            dgvStockInDetails.DataSource = viewData;
            ClearGridSelection(dgvStockInDetails);

            decimal tongTien = _stockInItems.Sum(x => x.ThanhTien);
            lblTongPhieuValue.Text = tongTien.ToString("N0") + " đ";
            btnSave.Enabled = _stockInItems.Count > 0;
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressProductSelectionChanged || !dgvProducts.ContainsFocus)
            {
                return;
            }

            LoadSelectedProductIntoEditor();
        }

        private void SearchProducts()
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindProductsGrid(_products);
                return;
            }

            List<ProductDTO> filtered = _products.Where(x =>
                (!string.IsNullOrWhiteSpace(x.TenSP) && x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrWhiteSpace(x.MaVach) && x.MaVach.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();

            BindProductsGrid(filtered);

            ProductDTO exactBarcode = filtered.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.MaVach) &&
                string.Equals(x.MaVach.Trim(), keyword, StringComparison.OrdinalIgnoreCase));

            if (exactBarcode != null)
            {
                SelectProductRow(exactBarcode.MaSP);
                SetSelectedProductInEditor(exactBarcode, true);
                return;
            }

            if (filtered.Count == 1)
            {
                SelectProductRow(filtered[0].MaSP);
                SetSelectedProductInEditor(filtered[0], true);
            }
        }

        public void ApplyGlobalSearch(string keyword)
        {
            txtSearch.Text = keyword ?? string.Empty;
            SearchProducts();
        }

        public void ClearGlobalSearch()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                return;
            }

            txtSearch.Clear();
            SearchProducts();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchProducts();
            }
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            AddLineFromEditor();
        }

        private void BtnUpdateItem_Click(object sender, EventArgs e)
        {
            UpdateLineFromEditor();
        }

        private void BtnDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedStockInLine();
        }

        private void BtnCameraScan_Click(object sender, EventArgs e)
        {
            using (frmCameraScanner frm = new frmCameraScanner(
                "Quét mã nhập kho",
                "Quét mã vạch sản phẩm để nhận diện sản phẩm trước. Sau đó bạn sẽ nhập tay số lượng, giá nhập và hạn sử dụng của lô mới."))
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(frm.ScannedCode))
                {
                    return;
                }

                txtSearch.Text = frm.ScannedCode;
                SearchProducts();

                ProductDTO product = FindProductByBarcode(frm.ScannedCode);
                if (product == null)
                {
                    product = PromptCreateProductFromBarcode(frm.ScannedCode);
                    if (product == null)
                    {
                        return;
                    }
                }

                SelectProductRow(product.MaSP);
                AddProductToStockIn(product);
            }
        }

        private void BtnPhoneScan_Click(object sender, EventArgs e)
        {
            using (frmPhoneScannerBridge frm = new frmPhoneScannerBridge(
                "Qu\u00e9t nh\u1eadp kho b\u1eb1ng \u0111i\u1ec7n tho\u1ea1i",
                "D\u00f9ng camera \u0111i\u1ec7n tho\u1ea1i \u0111\u1ec3 ch\u1ee5p m\u00e3 v\u1ea1ch, g\u1eedi v\u1ec1 m\u00e1y t\u00ednh, r\u1ed3i app s\u1ebd t\u00ecm s\u1ea3n ph\u1ea9m v\u00e0 m\u1edf lu\u1ed3ng nh\u1eadp l\u00f4."))
            {
                if (frm.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(frm.ScannedCode))
                {
                    return;
                }

                txtSearch.Text = frm.ScannedCode;
                SearchProducts();

                ProductDTO product = FindProductByBarcode(frm.ScannedCode);
                if (product == null)
                {
                    product = PromptCreateProductFromBarcode(frm.ScannedCode);
                    if (product == null)
                    {
                        return;
                    }
                }

                SelectProductRow(product.MaSP);
                AddProductToStockIn(product);
            }
        }

        private void DgvStockInDetails_DoubleClick(object sender, EventArgs e)
        {
            LoadSelectedStockInLineIntoEditor();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_stockInItems.Count == 0)
            {
                MessageBox.Show("Phiếu nhập chưa có sản phẩm.", "Thông báo");
                return;
            }

            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Không xác định được nhân viên đăng nhập.", "Thông báo");
                return;
            }

            StockInRequest request = new StockInRequest
            {
                MaNV = SessionManager.CurrentUser.MaNV,
                GhiChu = txtGhiChu.Text.Trim(),
                ChiTietNhapKho = _stockInItems.Select(x => new StockInDetailDTO
                {
                    MaSP = x.MaSP,
                    SoLuong = x.SoLuong,
                    GiaNhapLucNhap = x.GiaNhapLucNhap,
                    ThanhTien = x.ThanhTien,
                    HanSuDung = x.HanSuDung
                }).ToList()
            };

            OperationResult result = _stockInService.Insert(request);

            MessageBox.Show(
                result.Message,
                "Thông báo",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                _stockInItems.Clear();
                txtGhiChu.Clear();
                RefreshStockInDetailsView();
                ClearLineEditor();
                LoadProducts();
            }
        }

        private ProductDTO GetSelectedProductFromGrid(bool showMessage = true)
        {
            if (dgvProducts.CurrentRow == null)
            {
                if (showMessage)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để thêm.", "Thông báo");
                }
                return null;
            }

            object cellValue = dgvProducts.CurrentRow.Cells["MaSP"].Value;
            if (cellValue == null)
            {
                if (showMessage)
                {
                    MessageBox.Show("Không xác định được sản phẩm.", "Thông báo");
                }
                return null;
            }

            int maSP;
            if (!int.TryParse(cellValue.ToString(), out maSP))
            {
                if (showMessage)
                {
                    MessageBox.Show("Dữ liệu sản phẩm không hợp lệ.", "Thông báo");
                }
                return null;
            }

            ProductDTO product = _productService.GetById(maSP);
            if (product == null)
            {
                if (showMessage)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.", "Thông báo");
                }
                return null;
            }

            return product;
        }

        private ProductDTO FindProductByBarcode(string barcode)
        {
            string normalizedBarcode = string.IsNullOrWhiteSpace(barcode)
                ? string.Empty
                : barcode.Trim();

            if (string.IsNullOrWhiteSpace(normalizedBarcode))
            {
                return null;
            }

            ProductDTO product = _products != null
                ? _products.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.MaVach) &&
                    string.Equals(x.MaVach.Trim(), normalizedBarcode, StringComparison.OrdinalIgnoreCase))
                : null;

            return product ?? _productService.GetByBarcode(normalizedBarcode);
        }

        private ProductDTO PromptCreateProductFromBarcode(string barcode)
        {
            string normalizedBarcode = string.IsNullOrWhiteSpace(barcode)
                ? string.Empty
                : barcode.Trim();

            if (!BarcodeHelper.IsValidBarcode(normalizedBarcode))
            {
                MessageBox.Show("Không tìm thấy sản phẩm theo mã vừa quét.", "Thông báo");
                return null;
            }

            DialogResult result = MessageBox.Show(
                "Mã vạch này chưa có trong hệ thống. Bạn có muốn chuyển sang thêm sản phẩm mới không?",
                "Sản phẩm mới",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return null;
            }

            using (var frm = new frmProductEdit(normalizedBarcode))
            {
                frm.ShowDialog(this);
                if (!frm.IsSavedSuccessfully)
                {
                    return null;
                }
            }

            LoadProducts();
            txtSearch.Text = normalizedBarcode;
            SearchProducts();

            ProductDTO product = FindProductByBarcode(normalizedBarcode);
            if (product == null)
            {
                MessageBox.Show("Đã lưu sản phẩm mới nhưng chưa đọc lại được dữ liệu sản phẩm.", "Thông báo");
                return null;
            }

            return product;
        }

        private void SelectProductRow(int maSP)
        {
            bool suppressPrevious = _suppressProductSelectionChanged;
            _suppressProductSelectionChanged = true;

            try
            {
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    object cellValue = row.Cells["MaSP"].Value;
                    if (cellValue == null)
                    {
                        continue;
                    }

                    int currentMaSP;
                    if (!int.TryParse(cellValue.ToString(), out currentMaSP))
                    {
                        continue;
                    }

                    if (currentMaSP != maSP)
                    {
                        continue;
                    }

                    row.Selected = true;
                    dgvProducts.CurrentCell = row.Cells["TenSP"];
                    dgvProducts.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
            finally
            {
                _suppressProductSelectionChanged = suppressPrevious;
            }
        }

        private void AddProductToStockIn(ProductDTO product)
        {
            if (product == null)
            {
                return;
            }

            SetSelectedProductInEditor(product, true);
            nudQuantity.Focus();
        }

        private void LoadSelectedProductIntoEditor()
        {
            ProductDTO product = GetSelectedProductFromGrid(false);
            if (product == null)
            {
                return;
            }

            SetSelectedProductInEditor(product, true);
        }

        private void SetSelectedProductInEditor(ProductDTO product, bool resetValues)
        {
            if (product == null)
            {
                return;
            }

            _selectedProductId = product.MaSP;
            lblSelectedProduct.Text = product.MaSP + " - " + product.TenSP + "  |  Tồn hiện tại: " + product.SoLuongTon;

            if (resetValues)
            {
                _editingLineId = null;
                SetNumericValue(nudQuantity, 1);
                SetNumericValue(nudPrice, product.GiaNhap);

                DateTime suggestedExpiry = product.HanSuDung.HasValue && product.HanSuDung.Value.Date >= DateTime.Today
                    ? product.HanSuDung.Value.Date
                    : DateTime.Today;

                dtpExpiry.Value = suggestedExpiry;
                chkUseExpiry.Checked = product.HanSuDung.HasValue && product.HanSuDung.Value.Date >= DateTime.Today;
                dtpExpiry.Enabled = chkUseExpiry.Checked;
            }

            UpdateLineEditorButtons();
        }

        private void ClearLineEditor()
        {
            _selectedProductId = null;
            _editingLineId = null;
            lblSelectedProduct.Text = "Chưa chọn sản phẩm";
            SetNumericValue(nudQuantity, 1);
            SetNumericValue(nudPrice, 0);
            dtpExpiry.Value = DateTime.Today;
            chkUseExpiry.Checked = false;
            dtpExpiry.Enabled = false;
            ClearGridSelection(dgvProducts);
            ClearGridSelection(dgvStockInDetails);
            UpdateLineEditorButtons();
        }

        private void AddLineFromEditor()
        {
            StockInDetailDTO draft;
            if (!TryBuildLineFromEditor(out draft))
            {
                return;
            }

            StockInDetailDTO existing = _stockInItems.FirstOrDefault(x =>
                IsSameLot(x, draft.MaSP, draft.GiaNhapLucNhap, draft.HanSuDung));

            if (existing == null)
            {
                draft.LineId = Guid.NewGuid();
                draft.ThanhTien = draft.SoLuong * draft.GiaNhapLucNhap;
                _stockInItems.Add(draft);
                RefreshStockInDetailsView();
                SelectStockInLine(draft.LineId);
                LoadLineIntoEditor(draft);
                return;
            }

            existing.SoLuong += draft.SoLuong;
            existing.ThanhTien = existing.SoLuong * existing.GiaNhapLucNhap;
            RefreshStockInDetailsView();
            SelectStockInLine(existing.LineId);
            LoadLineIntoEditor(existing);
        }

        private void UpdateLineFromEditor()
        {
            if (!_editingLineId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn dòng trong phiếu nhập để cập nhật.", "Thông báo");
                return;
            }

            StockInDetailDTO current = _stockInItems.FirstOrDefault(x => x.LineId == _editingLineId.Value);
            if (current == null)
            {
                MessageBox.Show("Không tìm thấy dòng phiếu nhập cần cập nhật.", "Thông báo");
                ClearLineEditor();
                return;
            }

            StockInDetailDTO draft;
            if (!TryBuildLineFromEditor(out draft))
            {
                return;
            }

            StockInDetailDTO duplicate = _stockInItems.FirstOrDefault(x =>
                x.LineId != current.LineId &&
                IsSameLot(x, draft.MaSP, draft.GiaNhapLucNhap, draft.HanSuDung));

            if (duplicate != null)
            {
                duplicate.SoLuong += draft.SoLuong;
                duplicate.ThanhTien = duplicate.SoLuong * duplicate.GiaNhapLucNhap;
                _stockInItems.Remove(current);
                RefreshStockInDetailsView();
                SelectStockInLine(duplicate.LineId);
                LoadLineIntoEditor(duplicate);
                return;
            }

            current.MaSP = draft.MaSP;
            current.SoLuong = draft.SoLuong;
            current.GiaNhapLucNhap = draft.GiaNhapLucNhap;
            current.HanSuDung = draft.HanSuDung;
            current.ThanhTien = draft.SoLuong * draft.GiaNhapLucNhap;

            RefreshStockInDetailsView();
            SelectStockInLine(current.LineId);
            LoadLineIntoEditor(current);
        }

        private void DeleteSelectedStockInLine()
        {
            StockInDetailDTO item = GetSelectedStockInLine();
            if (item == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa trong phiếu nhập.", "Thông báo");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Xóa sản phẩm này khỏi phiếu nhập?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            _stockInItems.Remove(item);
            RefreshStockInDetailsView();
            ClearLineEditor();
        }

        private bool TryBuildLineFromEditor(out StockInDetailDTO line)
        {
            line = null;
            ProductDTO product = GetSelectedProductFromEditor();
            if (product == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm ở danh sách bên trái.", "Thông báo");
                return false;
            }

            DateTime? expiry = null;
            if (chkUseExpiry.Checked)
            {
                DateTime selectedDate = dtpExpiry.Value.Date;
                if (selectedDate < DateTime.Today)
                {
                    MessageBox.Show("Hạn sử dụng của lô nhập phải từ hôm nay trở đi.", "Thông báo");
                    dtpExpiry.Focus();
                    return false;
                }

                expiry = selectedDate;
            }

            line = new StockInDetailDTO
            {
                MaSP = product.MaSP,
                SoLuong = Convert.ToInt32(nudQuantity.Value),
                GiaNhapLucNhap = nudPrice.Value,
                HanSuDung = expiry,
                ThanhTien = Convert.ToInt32(nudQuantity.Value) * nudPrice.Value
            };

            return true;
        }

        private ProductDTO GetSelectedProductFromEditor()
        {
            if (!_selectedProductId.HasValue)
            {
                ProductDTO productFromGrid = GetSelectedProductFromGrid(false);
                if (productFromGrid != null)
                {
                    SetSelectedProductInEditor(productFromGrid, true);
                }
            }

            if (!_selectedProductId.HasValue)
            {
                return null;
            }

            ProductDTO product = _products != null
                ? _products.FirstOrDefault(x => x.MaSP == _selectedProductId.Value)
                : null;

            return product ?? _productService.GetById(_selectedProductId.Value);
        }

        private void LoadSelectedStockInLineIntoEditor()
        {
            StockInDetailDTO item = GetSelectedStockInLine();
            if (item == null)
            {
                return;
            }

            LoadLineIntoEditor(item);
        }

        private StockInDetailDTO GetSelectedStockInLine()
        {
            if (_editingLineId.HasValue)
            {
                StockInDetailDTO editing = _stockInItems.FirstOrDefault(x => x.LineId == _editingLineId.Value);
                if (editing != null)
                {
                    return editing;
                }
            }

            if (dgvStockInDetails.CurrentRow == null)
            {
                return null;
            }

            object cellValue = dgvStockInDetails.CurrentRow.Cells["LineId"].Value;
            if (cellValue == null)
            {
                return null;
            }

            Guid lineId;
            if (!Guid.TryParse(cellValue.ToString(), out lineId))
            {
                return null;
            }

            return _stockInItems.FirstOrDefault(x => x.LineId == lineId);
        }

        private void LoadLineIntoEditor(StockInDetailDTO item)
        {
            if (item == null)
            {
                return;
            }

            ProductDTO product = _products != null
                ? _products.FirstOrDefault(x => x.MaSP == item.MaSP)
                : null;
            product = product ?? _productService.GetById(item.MaSP);

            if (product == null)
            {
                return;
            }

            _selectedProductId = item.MaSP;
            _editingLineId = item.LineId;
            lblSelectedProduct.Text = product.MaSP + " - " + product.TenSP + "  |  Đang sửa dòng phiếu";
            SetNumericValue(nudQuantity, item.SoLuong);
            SetNumericValue(nudPrice, item.GiaNhapLucNhap);

            if (item.HanSuDung.HasValue)
            {
                dtpExpiry.Value = item.HanSuDung.Value.Date < DateTime.Today
                    ? DateTime.Today
                    : item.HanSuDung.Value.Date;
                chkUseExpiry.Checked = true;
            }
            else
            {
                dtpExpiry.Value = DateTime.Today;
                chkUseExpiry.Checked = false;
            }

            dtpExpiry.Enabled = chkUseExpiry.Checked;
            SelectProductRow(item.MaSP);
            UpdateLineEditorButtons();
        }

        private bool IsSameLot(StockInDetailDTO item, int maSP, decimal giaNhap, DateTime? hanSuDung)
        {
            return item != null &&
                item.MaSP == maSP &&
                item.GiaNhapLucNhap == giaNhap &&
                NullableDateEquals(item.HanSuDung, hanSuDung);
        }

        private bool NullableDateEquals(DateTime? left, DateTime? right)
        {
            if (!left.HasValue && !right.HasValue)
            {
                return true;
            }

            return left.HasValue &&
                right.HasValue &&
                left.Value.Date == right.Value.Date;
        }

        private void SelectStockInLine(Guid lineId)
        {
            foreach (DataGridViewRow row in dgvStockInDetails.Rows)
            {
                object cellValue = row.Cells["LineId"].Value;
                if (cellValue == null)
                {
                    continue;
                }

                Guid currentLineId;
                if (!Guid.TryParse(cellValue.ToString(), out currentLineId) || currentLineId != lineId)
                {
                    continue;
                }

                row.Selected = true;
                dgvStockInDetails.CurrentCell = row.Cells["TenSP"];
                break;
            }
        }

        private void SetNumericValue(NumericUpDown control, decimal value)
        {
            if (control == null)
            {
                return;
            }

            decimal safeValue = Math.Max(control.Minimum, Math.Min(control.Maximum, value));
            control.Value = safeValue;
        }

        private void UpdateLineEditorButtons()
        {
            bool hasProduct = _selectedProductId.HasValue;
            bool isEditing = _editingLineId.HasValue;

            if (btnAddItem != null)
            {
                btnAddItem.Enabled = hasProduct;
            }

            if (btnUpdateItem != null)
            {
                btnUpdateItem.Enabled = hasProduct && isEditing;
            }

            if (btnDeleteItem != null)
            {
                btnDeleteItem.Enabled = isEditing;
            }
        }

        private void ClearGridSelection(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            bool suppressPrevious = _suppressProductSelectionChanged;
            _suppressProductSelectionChanged = true;

            try
            {
                grid.ClearSelection();
                if (grid.RowCount > 0)
                {
                    grid.CurrentCell = null;
                }
            }
            catch (InvalidOperationException)
            {
                grid.ClearSelection();
            }
            finally
            {
                _suppressProductSelectionChanged = suppressPrevious;
            }
        }

        private void UpdateResponsiveLayout()
        {
            if (pnlLeft != null && ClientSize.Width > 0)
            {
                pnlLeft.Width = Math.Max(420, Math.Min(560, ClientSize.Width * 44 / 100));
            }

            if (pnlHeader != null)
            {
                int x = 110;
                int y = 64;
                int gap = 10;
                int buttonsWidth = btnSearch.Width + btnCameraScan.Width + btnPhoneScan.Width + gap * 3;
                int searchWidth = Math.Max(180, pnlHeader.ClientSize.Width - x - 20 - buttonsWidth);

                txtSearch.SetBounds(x, y, searchWidth, txtSearch.Height);
                btnSearch.Location = new Point(txtSearch.Right + gap, 62);
                btnCameraScan.Location = new Point(btnSearch.Right + gap, 62);
                btnPhoneScan.Location = new Point(btnCameraScan.Right + gap, 62);
            }

            if (pnlFooter != null && btnSave != null)
            {
                btnSave.Location = new Point(
                    Math.Max(520, pnlFooter.ClientSize.Width - 18 - btnSave.Width),
                    38);
                txtGhiChu.Width = Math.Max(240, btnSave.Left - 20 - txtGhiChu.Left);
            }

            UpdateLineEditorLayout();
        }

        private void UpdateLineEditorLayout()
        {
            if (pnlLineEditor == null || lblSelectedProduct == null)
            {
                return;
            }

            int editorWidth = Math.Max(760, pnlHeader.ClientSize.Width - 40);
            pnlLineEditor.SetBounds(20, 104, editorWidth, 66);

            int width = pnlLineEditor.ClientSize.Width;
            lblSelectedProduct.Width = Math.Max(280, width - 24);

            int top = 34;
            int gap = 10;
            lblQuantity.Location = new Point(12, top + 4);
            nudQuantity.Location = new Point(78, top);
            lblPrice.Location = new Point(nudQuantity.Right + gap + 4, top + 4);
            nudPrice.Location = new Point(lblPrice.Right + 8, top);
            chkUseExpiry.Location = new Point(nudPrice.Right + gap + 4, top + 3);
            dtpExpiry.Location = new Point(chkUseExpiry.Right + 8, top);
            dtpExpiry.Width = 112;

            int buttonTop = top - 2;
            int firstButtonLeft = dtpExpiry.Right + 18;
            btnAddItem.Location = new Point(firstButtonLeft, buttonTop);
            btnUpdateItem.Location = new Point(btnAddItem.Right + gap, buttonTop);
            btnDeleteItem.Location = new Point(btnUpdateItem.Right + gap, buttonTop);
            btnClearItem.Location = new Point(btnDeleteItem.Right + gap, buttonTop);
        }
    }
}
