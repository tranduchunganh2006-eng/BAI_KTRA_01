namespace TechMartManager;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Form ──────────────────────────────────────────
        this.Text            = "TechMart Product Manager";
        this.Size            = new Size(1150, 700);
        this.MinimumSize     = new Size(950, 580);
        this.StartPosition   = FormStartPosition.CenterScreen;
        this.BackColor       = Color.FromArgb(245, 247, 250);
        this.Font            = new Font("Segoe UI", 9f);

        errorProvider = new ErrorProvider(components)
        {
            BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError
        };

        BuildMenuStrip();
        BuildStatusStrip();
        BuildMainLayout();
    }
    #endregion
}
