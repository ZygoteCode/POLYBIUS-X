using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;

public class PsychedelicEffect7
{
    private static Form _form;
    private static Timer timer;
    private static float ring_offset = 0f;
    private static float ringSpeed = 14F; // puoi modificare la velocità

    private static float introProgress = 0f;
    private static bool introDone = false;

    public static void Start(Form form)
    {
        _form = form;
        _form.WindowState = FormWindowState.Maximized;
        _form.BackColor = Color.Black;

        // Abilita double buffering sul form (reflection perché la proprietà è protetta)
        var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        if (prop != null) prop.SetValue(_form, true, null);

        timer = new Timer();
        timer.Interval = 16; // ~60 FPS
        timer.Tick += (sender, e) =>
        {
            ring_offset += ringSpeed;

            if (!introDone)
            {
                introProgress += 0.002f;

                if (introProgress >= 1f)
                {
                    introProgress = 1f;
                    introDone = true;
                }
            }

            _form.Invalidate();
        };

        timer.Start();
    }

    public static void ProcessEffect(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int w = _form.ClientSize.Width;
        int h = _form.ClientSize.Height;
        float centerX = w / 2f;
        float centerY = h / 2f;
        float maxRadius = (Math.Min(w, h) * 5F) * introProgress; // solo nella parte centrale

        const float ringWidth = 50F;     // spessore della "fascia"
        const float bandSpacing = 50F;   // distanza tra i bordi delle bande

        // offset ciclico, come nel tuo esempio con i cerchi
        float currentOffset = ring_offset % (bandSpacing * 2f);

        // disegna coppie di bande: una chiara e la successiva nera
        for (float r = currentOffset; r < maxRadius + bandSpacing; r += bandSpacing * 2f)
        {
            DrawSquareRing(g, centerX, centerY, r, ringWidth, Color.FromArgb(209, 229, 230));
            float nextR = r + bandSpacing;
            if (nextR < maxRadius + bandSpacing)
            {
                DrawSquareRing(g, centerX, centerY, nextR, ringWidth, Color.Black);
            }
        }
    }

    private static void DrawSquareRing(Graphics g, float cx, float cy, float cornerRadius, float bandWidth, Color color)
    {
        // outer square (rombo più grande)
        PointF[] outer = GetDiamondPoints(cx, cy, cornerRadius + bandWidth / 2f);
        // inner square (rombo più piccolo)
        PointF[] inner = GetDiamondPoints(cx, cy, cornerRadius - bandWidth / 2f);

        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddPolygon(outer);
            path.AddPolygon(inner);
            using (Brush brush = new SolidBrush(color))
            {
                g.FillPath(brush, path);
            }
        }
    }

    private static PointF[] GetDiamondPoints(float cx, float cy, float radius)
    {
        float angleOffset = (float)(Math.PI / 2.0F);
        PointF[] pts = new PointF[4];
        for (int i = 0; i < 4; i++)
        {
            float angle = angleOffset + i * (float)(Math.PI / 2.0);
            pts[i] = new PointF(
                cx + radius * (float)Math.Cos(angle),
                cy + radius * (float)Math.Sin(angle)
            );
        }
        return pts;
    }
}
