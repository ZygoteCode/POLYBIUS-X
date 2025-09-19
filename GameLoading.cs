using System.Diagnostics;
using System.Windows.Forms;

public partial class GameLoading : Form
{
    private int ticks = 0;

    public GameLoading()
    {
        InitializeComponent();
        Cursor = new Cursor("assets\\polybius_cursor.cur");
        timer1.Start();
    }

    private void GameLoading_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void timer1_Tick(object sender, System.EventArgs e)
    {
        switch (ticks)
        {
            case 0:
                label1.Text = "CHECKING POLYBIUS\r\n\r\n   ROM CHECK  OK\r\n\r\n   HIGHER FUNCTIONS";
                break;
            case 1:
                label1.Text = "CHECKING POLYBIUS\r\n\r\n   ROM CHECK  OK\r\n\r\n   HIGHER FUNCTIONS  OK";
                break;
            case 2:
                label1.Text = "CHECKING POLYBIUS\r\n\r\n   ROM CHECK  OK\r\n\r\n   HIGHER FUNCTIONS  OK\r\n\r\n   COGNITIVE INTERFACE\r\n       OPERATIONAL";
                break;
            case 3:
                timer1.Stop();
                this.HideForm();
                PolybiusIntro polybiusIntro = new PolybiusIntro();
                polybiusIntro.Show();
                break;
        }

        ticks++;
    }
}