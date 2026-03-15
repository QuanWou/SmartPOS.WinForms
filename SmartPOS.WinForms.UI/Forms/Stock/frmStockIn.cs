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

namespace SmartPOS.WinForms.UI.Forms.Stock
{
    public class frmStockIn : Form
    {
        private readonly IProductService _productService;
        private readonly IStockInService _stockInService;

        private Panel pnlHeader;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblSearch;

        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnCameraScan;
        private Button btnPhoneScan;
        private Button btnAddItem;
        private Button btnSave;

        private DataGridView dgvProducts;
        private DataGridView dgvStockInDetails;

        private Label lblTongPhieu;
        private Label lblTongPhieuValue;
        private Label lblGhiChu;
        private TextBox txtGhiChu;

        private List<ProductDTO> _products;
        private List<StockInDetailDTO> _stockInItems;

        public frmStockIn()
        {
            _productService = new ProductService();
            _stockInService = new StockInService();
            _stockInItems = new List<StockInDetailDTO>();

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
        }

        private void BuildLayout()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
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

            btnAddItem = new Button
            {
                Text = "Thêm vào phiếu",
                Location = new Point(660, 62),
                Size = new Size(110, 30),
                BackColor = Color.FromArgb(90, 110, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.Click += BtnAddItem_Click;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblSearch);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(btnSearch);
            pnlHeader.Controls.Add(btnCameraScan);
            pnlHeader.Controls.Add(btnPhoneScan);
            pnlHeader.Controls.Add(btnAddItem);

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
                AutoSize = true,
                Location = new Point(14, 12)
            };

            dgvProducts = new DataGridView
            {
                Location = new Point(14, 42),
                Size = new Size(432, 420),
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

            card.Controls.Add(lbl);
            card.Controls.Add(dgvProducts);
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

            var lbl = new Label
            {
                Text = "Chi tiết phiếu nhập",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 32, 72),
                AutoSize = true,
                Location = new Point(14, 12)
            };

            dgvStockInDetails = new DataGridView
            {
                Location = new Point(14, 42),
                Size = new Size(470, 420),
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
            dgvStockInDetails.DoubleClick += DgvStockInDetails_DoubleClick;

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

            card.Controls.Add(lbl);
            card.Controls.Add(dgvStockInDetails);
            pnlRight.Controls.Add(card);
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

            decimal tongTien = _stockInItems.Sum(x => x.ThanhTien);
            lblTongPhieuValue.Text = tongTien.ToString("N0") + " đ";
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
            ProductDTO product = GetSelectedProductFromGrid();
            if (product == null)
            {
                return;
            }

            AddProductToStockIn(product);
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
            if (dgvStockInDetails.CurrentRow == null)
            {
                return;
            }

            object cellValue = dgvStockInDetails.CurrentRow.Cells["LineId"].Value;
            if (cellValue == null)
            {
                return;
            }

            Guid lineId;
            if (!Guid.TryParse(cellValue.ToString(), out lineId))
            {
                return;
            }

            StockInDetailDTO item = _stockInItems.FirstOrDefault(x => x.LineId == lineId);
            if (item == null)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Xóa sản phẩm này khỏi phiếu nhập?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _stockInItems.Remove(item);
                RefreshStockInDetailsView();
            }
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
                LoadProducts();
            }
        }

        private ProductDTO GetSelectedProductFromGrid()
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để thêm.", "Thông báo");
                return null;
            }

            object cellValue = dgvProducts.CurrentRow.Cells["MaSP"].Value;
            if (cellValue == null)
            {
                MessageBox.Show("Không xác định được sản phẩm.", "Thông báo");
                return null;
            }

            int maSP;
            if (!int.TryParse(cellValue.ToString(), out maSP))
            {
                MessageBox.Show("Dữ liệu sản phẩm không hợp lệ.", "Thông báo");
                return null;
            }

            ProductDTO product = _productService.GetById(maSP);
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm.", "Thông báo");
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

        private void AddProductToStockIn(ProductDTO product)
        {
            if (product == null)
            {
                return;
            }

            string soLuongText = Prompt.ShowDialog("Nhập số lượng:", "Thêm vào phiếu nhập", "1");
            if (string.IsNullOrWhiteSpace(soLuongText))
            {
                return;
            }

            int soLuong;
            if (!int.TryParse(soLuongText.Trim(), out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Thông báo");
                return;
            }

            string giaNhapText = Prompt.ShowDialog("Nhập giá nhập:", "Thêm vào phiếu nhập", product.GiaNhap.ToString("0.##"));
            if (string.IsNullOrWhiteSpace(giaNhapText))
            {
                return;
            }

            decimal giaNhap;
            if (!decimal.TryParse(giaNhapText.Trim(), out giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ.", "Thông báo");
                return;
            }

            string hanSuDungText = Prompt.ShowDialog(
                "Nhập hạn sử dụng (dd/MM/yyyy, bỏ trống nếu không có):",
                "Thêm vào phiếu nhập",
                product.HanSuDung.HasValue ? product.HanSuDung.Value.ToString("dd/MM/yyyy") : string.Empty);

            DateTime? hanSuDung = null;
            if (!string.IsNullOrWhiteSpace(hanSuDungText))
            {
                DateTime parsedDate;
                string[] supportedFormats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" };

                if (!DateTime.TryParseExact(
                    hanSuDungText.Trim(),
                    supportedFormats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out parsedDate))
                {
                    MessageBox.Show("Hạn sử dụng không hợp lệ.", "Thông báo");
                    return;
                }

                hanSuDung = parsedDate.Date;
            }

            StockInDetailDTO existing = _stockInItems.FirstOrDefault(x =>
                x.MaSP == product.MaSP &&
                x.GiaNhapLucNhap == giaNhap &&
                x.HanSuDung == hanSuDung);
            if (existing == null)
            {
                _stockInItems.Add(new StockInDetailDTO
                {
                    LineId = Guid.NewGuid(),
                    MaSP = product.MaSP,
                    SoLuong = soLuong,
                    GiaNhapLucNhap = giaNhap,
                    ThanhTien = soLuong * giaNhap,
                    HanSuDung = hanSuDung
                });
            }
            else
            {
                existing.SoLuong += soLuong;
                existing.GiaNhapLucNhap = giaNhap;
                existing.ThanhTien = existing.SoLuong * existing.GiaNhapLucNhap;
                if (hanSuDung.HasValue)
                {
                    existing.HanSuDung = hanSuDung;
                }
            }

            RefreshStockInDetailsView();
        }

        private static class Prompt
        {
            public static string ShowDialog(string text, string caption, string defaultValue)
            {
                Form prompt = new Form()
                {
                    Width = 360,
                    Height = 180,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterParent,
                    MinimizeBox = false,
                    MaximizeBox = false,
                    BackColor = Color.White
                };

                Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 300, Text = defaultValue ?? string.Empty };
                Button confirmation = new Button()
                {
                    Text = "OK",
                    Left = 160,
                    Width = 75,
                    Top = 90,
                    DialogResult = DialogResult.OK
                };
                Button cancel = new Button()
                {
                    Text = "Hủy",
                    Left = 245,
                    Width = 75,
                    Top = 90,
                    DialogResult = DialogResult.Cancel
                };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = confirmation;
                prompt.CancelButton = cancel;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }
    }
}
