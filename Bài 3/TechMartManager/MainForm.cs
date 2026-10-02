using System.ComponentModel;
using System.Text;
using TechMartManager.Models;

namespace TechMartManager;

public partial class MainForm : Form
{
    // ── Controls ──────────────────────────────────────────
    private TableLayoutPanel   tlpMain       = null!;
    private Panel              panelLeft     = null!;
    private DataGridView       dgvProducts   = null!;
    private TextBox            txtProductId  = null!;
    private TextBox            txtProductName= null!;
    private TextBox            txtUnitPrice  = null!;
    private TextBox            txtQuantity   = null!;
    private TextBox            txtSearch     = null!;
    private ComboBox           cboCategory   = null!;
    private PictureBox         picAvatar     = null!;
    private Button             btnChooseImage= null!;
    private Button             btnAdd        = null!;
    private Button             btnUpdate     = null!;
    private Button             btnDelete     = null!;
    private ErrorProvider      errorProvider = null!;
    private MenuStrip          menuStrip     = null!;
    private StatusStrip        statusStrip   = null!;
    private ToolStripStatusLabel lblStatus   = null!;

    // ── Data ──────────────────────────────────────────────
    private readonly BindingList<SanPham> _bindingList   = new();
    private          BindingSource        _bindingSource  = null!;
    private readonly List<SanPham>        _allItems       = new();
    private          Image?               _selectedImage;

    // ─────────────────────────────────────────────────────
    public MainForm()
    {
        InitializeComponent();
        SetupDataBinding();
        LoadSampleData();
    }

    // ══════════════════════════════════════════════════════
    //  BUILD UI
    // ══════════════════════════════════════════════════════

    private void BuildMenuStrip()
    {
        menuStrip = new MenuStrip
        {
            BackColor = Color.FromArgb(37, 99, 235),
            ForeColor = Color.White,
            Renderer  = new ToolStripProfessionalRenderer(new BlueMenuColorTable())
        };

        var mnuFile   = new ToolStripMenuItem("📁 File") { ForeColor = Color.White };
        var mnuExport = new ToolStripMenuItem("Export CSV\tCtrl+E")
        {
            ShortcutKeys = Keys.Control | Keys.E,
            Image        = null
        };
        var mnuSep  = new ToolStripSeparator();
        var mnuExit = new ToolStripMenuItem("Exit\tCtrl+X")
        {
            ShortcutKeys = Keys.Control | Keys.X
        };

        mnuExport.Click += BtnExportCsv_Click;
        mnuExit.Click   += (_, _) => Application.Exit();
        mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuExport, mnuSep, mnuExit });
        menuStrip.Items.Add(mnuFile);

        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);
    }

    private void BuildStatusStrip()
    {
        statusStrip = new StatusStrip
        {
            BackColor = Color.FromArgb(37, 99, 235),
            SizingGrip = false
        };
        lblStatus = new ToolStripStatusLabel("Tổng số sản phẩm: 0")
        {
            ForeColor = Color.White,
            Font      = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        statusStrip.Items.Add(lblStatus);
        this.Controls.Add(statusStrip);
    }

    private void BuildMainLayout()
    {
        tlpMain = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 2,
            RowCount    = 1,
            Padding     = new Padding(10),
            BackColor   = Color.Transparent
        };
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
        tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        this.Controls.Add(tlpMain);

        BuildLeftPanel();
        BuildRightPanel();
    }

    // ── Cột trái – nhập liệu ──────────────────────────────
    private void BuildLeftPanel()
    {
        panelLeft = new Panel
        {
            Dock      = DockStyle.Fill,
            BackColor = Color.White,
            Padding   = new Padding(16),
            Margin    = new Padding(0, 0, 6, 0)
        };
        panelLeft.Paint += PanelLeft_Paint;

        int y    = 10;
        int lw   = 120;
        int xCtrl = 138;
        int ctrlW = 165;

        // Tiêu đề
        var lblTitle = NewLabel("THÔNG TIN SẢN PHẨM", 0, y, 310, 30);
        lblTitle.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        panelLeft.Controls.Add(lblTitle);
        y += 40;

        // Mã SP
        panelLeft.Controls.Add(NewLabel("Mã SP:", 0, y + 3, lw, 22));
        txtProductId = NewTextBox(xCtrl, y, ctrlW);
        panelLeft.Controls.Add(txtProductId);
        y += 32;

        // Tên SP
        panelLeft.Controls.Add(NewLabel("Tên SP (*):", 0, y + 3, lw, 22));
        txtProductName = NewTextBox(xCtrl, y, ctrlW);
        panelLeft.Controls.Add(txtProductName);
        y += 32;

        // Danh mục
        panelLeft.Controls.Add(NewLabel("Danh mục:", 0, y + 3, lw, 22));
        cboCategory = new ComboBox
        {
            Location      = new Point(xCtrl, y),
            Width         = ctrlW,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Ten",
            ValueMember   = "MaDM"
        };
        cboCategory.Items.AddRange(new DanhMucItem[]
        {
            new() { MaDM = "DT",  Ten = "Điện thoại" },
            new() { MaDM = "LT",  Ten = "Laptop" },
            new() { MaDM = "PKI", Ten = "Phụ kiện" }
        });
        cboCategory.SelectedIndex = 0;
        panelLeft.Controls.Add(cboCategory);
        y += 32;

        // Đơn giá
        panelLeft.Controls.Add(NewLabel("Đơn giá (*) > 0:", 0, y + 3, lw, 22));
        txtUnitPrice = NewTextBox(xCtrl, y, ctrlW);
        panelLeft.Controls.Add(txtUnitPrice);
        y += 32;

        // Số lượng
        panelLeft.Controls.Add(NewLabel("Số lượng (≥ 0):", 0, y + 3, lw, 22));
        txtQuantity = NewTextBox(xCtrl, y, ctrlW);
        panelLeft.Controls.Add(txtQuantity);
        y += 32;

        // Hình ảnh
        panelLeft.Controls.Add(NewLabel("Hình ảnh:", 0, y + 3, lw, 22));
        btnChooseImage = new Button
        {
            Text      = "📁 Chọn ảnh",
            Location  = new Point(xCtrl, y),
            Width     = ctrlW, Height = 26,
            BackColor = Color.FromArgb(243, 244, 246),
            FlatStyle = FlatStyle.Flat
        };
        btnChooseImage.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
        btnChooseImage.Click += BtnChooseImage_Click;
        panelLeft.Controls.Add(btnChooseImage);
        y += 30;

        picAvatar = new PictureBox
        {
            Location    = new Point(xCtrl, y),
            Width       = ctrlW,
            Height      = 110,
            SizeMode    = PictureBoxSizeMode.Zoom,
            BackColor   = Color.FromArgb(243, 244, 246),
            BorderStyle = BorderStyle.FixedSingle
        };
        panelLeft.Controls.Add(picAvatar);
        y += 120;

        // Nút hành động
        y += 8;
        btnAdd    = NewActionBtn("➕ Thêm mới",  Color.FromArgb(37, 99, 235),  0,   y);
        btnUpdate = NewActionBtn("✏️ Cập nhật",   Color.FromArgb(5, 150, 105),  158, y);
        panelLeft.Controls.Add(btnAdd);
        panelLeft.Controls.Add(btnUpdate);
        y += 36;
        btnDelete = NewActionBtn("🗑️ Xóa",         Color.FromArgb(220, 38, 38),  0,   y);
        panelLeft.Controls.Add(btnDelete);

        btnAdd.Click    += BtnAdd_Click;
        btnUpdate.Click += BtnUpdate_Click;
        btnDelete.Click += BtnDelete_Click;

        tlpMain.Controls.Add(panelLeft, 0, 0);
    }

    private void PanelLeft_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(209, 213, 219), 1);
        e.Graphics.DrawRectangle(pen, 0, 0, panelLeft.Width - 1, panelLeft.Height - 1);
    }

    // ── Cột phải – DataGridView ───────────────────────────
    private void BuildRightPanel()
    {
        var panelRight = new Panel
        {
            Dock      = DockStyle.Fill,
            BackColor = Color.White,
            Padding   = new Padding(8)
        };

        // Search bar
        var lblSearch = new Label
        {
            Text      = "🔍 Tìm theo tên:",
            AutoSize  = true,
            Location  = new Point(0, 8),
            ForeColor = Color.FromArgb(75, 85, 99)
        };
        txtSearch = new TextBox
        {
            Location        = new Point(130, 5),
            Width           = 240,
            PlaceholderText = "Nhập tên sản phẩm...",
            Anchor          = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        txtSearch.TextChanged += TxtSearch_TextChanged;
        panelRight.Controls.Add(lblSearch);
        panelRight.Controls.Add(txtSearch);

        // DataGridView
        dgvProducts = new DataGridView
        {
            Location              = new Point(0, 38),
            AutoGenerateColumns   = false,
            SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect           = false,
            ReadOnly              = true,
            AllowUserToAddRows    = false,
            AllowUserToDeleteRows = false,
            BorderStyle           = BorderStyle.None,
            BackgroundColor       = Color.White,
            GridColor             = Color.FromArgb(229, 231, 235),
            RowHeadersVisible     = false,
            Font                  = new Font("Segoe UI", 9f),
            ColumnHeadersHeight   = 36,
            Anchor                = AnchorStyles.Top | AnchorStyles.Bottom
                                  | AnchorStyles.Left | AnchorStyles.Right
        };
        dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
        dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvProducts.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
        dgvProducts.EnableHeadersVisualStyles               = false;
        dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

        // Định nghĩa cột
        dgvProducts.Columns.Add(MakeCol("MaSP",    "Mã SP",    75));
        dgvProducts.Columns.Add(MakeCol("TenSP",   "Tên SP",   200));
        dgvProducts.Columns.Add(MakeCol("DanhMuc", "Danh Mục", 110));
        dgvProducts.Columns.Add(MakePriceCol());
        dgvProducts.Columns.Add(MakeCol("SoLuong", "Số Lượng", 80));

        dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
        panelRight.Controls.Add(dgvProducts);

        panelRight.Resize += (_, _) =>
        {
            dgvProducts.Width  = panelRight.Width  - 16;
            dgvProducts.Height = panelRight.Height - 52;
        };

        tlpMain.Controls.Add(panelRight, 1, 0);
    }

    // ══════════════════════════════════════════════════════
    //  DATA BINDING
    // ══════════════════════════════════════════════════════

    private void SetupDataBinding()
    {
        _bindingSource = new BindingSource { DataSource = _bindingList };
        dgvProducts.DataSource = _bindingSource;
    }

    private void LoadSampleData()
    {
        AddToAll(new SanPham { MaSP = "SP001", TenSP = "iPhone 15 Pro Max",      DanhMuc = "Điện thoại", DonGia = 28_990_000, SoLuong = 15 });
        AddToAll(new SanPham { MaSP = "SP002", TenSP = "MacBook M1Max",     DanhMuc = "Laptop",     DonGia = 32_990_000, SoLuong = 8  });
        AddToAll(new SanPham { MaSP = "SP003", TenSP = "Samsung Galaxy A09", DanhMuc = "Điện thoại", DonGia = 22_490_000, SoLuong = 20 });
        AddToAll(new SanPham { MaSP = "SP004", TenSP = "HP ProBook",        DanhMuc = "Laptop",     DonGia = 45_000_000, SoLuong = 5  });
        AddToAll(new SanPham { MaSP = "SP005", TenSP = "Tai nghe ",   DanhMuc = "Phụ kiện",   DonGia = 4_990_000,  SoLuong = 30 });
        UpdateStatus();
    }

    private void AddToAll(SanPham sp)
    {
        _allItems.Add(sp);
        _bindingList.Add(sp);
    }

    private void UpdateStatus()
        => lblStatus.Text = $"Tổng số sản phẩm: {_allItems.Count}";

    // ══════════════════════════════════════════════════════
    //  VALIDATION
    // ══════════════════════════════════════════════════════

    private bool ValidateForm()
    {
        errorProvider.Clear();
        bool ok = true;

        if (string.IsNullOrWhiteSpace(txtProductName.Text))
        {
            errorProvider.SetError(txtProductName, "Tên SP không được để trống!");
            ok = false;
        }
        if (!decimal.TryParse(txtUnitPrice.Text, out decimal gia) || gia <= 0)
        {
            errorProvider.SetError(txtUnitPrice, "Đơn giá phải là số và > 0!");
            ok = false;
        }
        if (!int.TryParse(txtQuantity.Text, out int sl) || sl < 0)
        {
            errorProvider.SetError(txtQuantity, "Số lượng phải là số nguyên ≥ 0!");
            ok = false;
        }
        return ok;
    }

    private string GenMaSP()
    {
        int max = _allItems.Count == 0 ? 0
            : _allItems.Select(s =>
            {
                int.TryParse(s.MaSP.Replace("SP", ""), out int n);
                return n;
            }).Max();
        return $"SP{(max + 1):D3}";
    }

    // ══════════════════════════════════════════════════════
    //  SỰ KIỆN
    // ══════════════════════════════════════════════════════

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (!ValidateForm()) return;

        var sp = new SanPham
        {
            MaSP    = string.IsNullOrWhiteSpace(txtProductId.Text) ? GenMaSP() : txtProductId.Text.Trim(),
            TenSP   = txtProductName.Text.Trim(),
            DanhMuc = (cboCategory.SelectedItem as DanhMucItem)?.Ten ?? "",
            DonGia  = decimal.Parse(txtUnitPrice.Text),
            SoLuong = int.Parse(txtQuantity.Text)
        };

        if (_selectedImage != null)
            sp.DuongDanAnh = "image_loaded";

        AddToAll(sp);
        UpdateStatus();
        ClearForm();
        MessageBox.Show($"Đã thêm sản phẩm \"{sp.TenSP}\" thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (dgvProducts.CurrentRow == null)
        {
            MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!ValidateForm()) return;

        var sp = (SanPham)dgvProducts.CurrentRow.DataBoundItem!;
        sp.TenSP   = txtProductName.Text.Trim();
        sp.DanhMuc = (cboCategory.SelectedItem as DanhMucItem)?.Ten ?? "";
        sp.DonGia  = decimal.Parse(txtUnitPrice.Text);
        sp.SoLuong = int.Parse(txtQuantity.Text);
        if (_selectedImage != null) picAvatar.Image = _selectedImage;

        _bindingSource.ResetCurrentItem();
        UpdateStatus();
        MessageBox.Show("Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvProducts.CurrentRow == null)
        {
            MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var sp  = (SanPham)dgvProducts.CurrentRow.DataBoundItem!;
        var dlg = MessageBox.Show(
            $"Bạn có chắc muốn xóa sản phẩm \"{sp.TenSP}\"?",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (dlg == DialogResult.Yes)
        {
            _allItems.Remove(sp);
            _bindingList.Remove(sp);
            UpdateStatus();
            ClearForm();
        }
    }

    private void BtnChooseImage_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Title  = "Chọn hình ảnh sản phẩm",
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*"
        };
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            _selectedImage  = Image.FromFile(ofd.FileName);
            picAvatar.Image = _selectedImage;
        }
    }

    private void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        string kw = txtSearch.Text.Trim();
        _bindingList.Clear();
        var filtered = string.IsNullOrEmpty(kw)
            ? _allItems
            : _allItems.Where(s => s.TenSP.Contains(kw, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var sp in filtered)
            _bindingList.Add(sp);
    }

    private void DgvProducts_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is not SanPham sp) return;

        txtProductId.Text   = sp.MaSP;
        txtProductName.Text = sp.TenSP;
        txtUnitPrice.Text   = sp.DonGia.ToString();
        txtQuantity.Text    = sp.SoLuong.ToString();

        foreach (DanhMucItem item in cboCategory.Items)
            if (item.Ten == sp.DanhMuc) { cboCategory.SelectedItem = item; break; }

        picAvatar.Image = null;
        _selectedImage  = null;
    }

    private void BtnExportCsv_Click(object? sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Title      = "Lưu file CSV",
            Filter     = "CSV Files|*.csv",
            FileName   = $"SanPham_{DateTime.Now:yyyyMMdd_HHmm}.csv",
            DefaultExt = "csv"
        };
        if (sfd.ShowDialog() != DialogResult.OK) return;

        var sb = new StringBuilder();
        sb.AppendLine("Mã SP,Tên SP,Danh Mục,Đơn Giá,Số Lượng");
        foreach (var sp in _allItems)
            sb.AppendLine($"{sp.MaSP},\"{sp.TenSP}\",{sp.DanhMuc},{sp.DonGia},{sp.SoLuong}");

        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
        MessageBox.Show($"Xuất CSV thành công!\n📁 {sfd.FileName}",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ══════════════════════════════════════════════════════
    //  HELPERS
    // ══════════════════════════════════════════════════════

    private void ClearForm()
    {
        txtProductId.Text    = "";
        txtProductName.Text  = "";
        txtUnitPrice.Text    = "";
        txtQuantity.Text     = "";
        cboCategory.SelectedIndex = 0;
        picAvatar.Image      = null;
        _selectedImage       = null;
        errorProvider.Clear();
    }

    private static Label NewLabel(string text, int x, int y, int w, int h)
        => new()
        {
            Text      = text, Location  = new Point(x, y),
            Width     = w,    Height    = h,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(55, 65, 81)
        };

    private static TextBox NewTextBox(int x, int y, int w)
        => new() { Location = new Point(x, y), Width = w };

    private static Button NewActionBtn(string text, Color color, int x, int y)
    {
        var btn = new Button
        {
            Text      = text,  Location  = new Point(x, y),
            Width     = 150,   Height    = 30,
            BackColor = color, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private static DataGridViewTextBoxColumn MakeCol(string prop, string header, int w)
        => new()
        {
            DataPropertyName = prop,
            HeaderText       = header,
            Width            = w,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
        };

    private static DataGridViewTextBoxColumn MakePriceCol()
    {
        var col = MakeCol("DonGia", "Đơn Giá (VNĐ)", 140);
        col.DefaultCellStyle.Format    = "N0";
        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        return col;
    }
}

// ── Custom menu color (giữ màu xanh dương) ──────────────────
internal class BlueMenuColorTable : ProfessionalColorTable
{
    public override Color MenuStripGradientBegin => Color.FromArgb(37, 99, 235);
    public override Color MenuStripGradientEnd   => Color.FromArgb(37, 99, 235);
}
