using System.Drawing;
using System.Windows.Forms;

public class TransparentLabel : Label
{
    public int Opacity { get; set; } = 255; // da 0 (trasparente) a 255 (opaco)

    protected override void OnPaint(PaintEventArgs e)
    {
        using (SolidBrush brush = new SolidBrush(Color.FromArgb(Opacity, this.ForeColor)))
        {
            e.Graphics.DrawString(this.Text, this.Font, brush, 0, 0);
        }
    }
}