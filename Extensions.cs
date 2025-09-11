using System.Windows.Forms;
using System.Drawing;

public static class Extensions
{
    public static void HideForm(this Form form)
    {
        form.Visible = false;
        form.Opacity = 0;
        form.Size = new Size(0, 0);
        form.Location = new Point(0, 0);
        form.WindowState = FormWindowState.Minimized;
        form.Hide();
    }
}