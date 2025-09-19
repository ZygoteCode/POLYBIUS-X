using System.Diagnostics;
using System.Windows.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        Cursor = new Cursor("assets\\polybius_cursor.cur");
        timer1.Start();    
    }

    private void timer1_Tick(object sender, System.EventArgs e)
    {
        timer1.Stop();
        this.HideForm();
        GameLoading gameLoading = new GameLoading();
        gameLoading.Show();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }
}