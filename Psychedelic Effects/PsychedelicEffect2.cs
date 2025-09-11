using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class PsychedelicEffect2
{
    private static Timer timer;
    private static float angle = 0f;
    private static float introProgress = 0f;
    private static bool introDone = false;

    private static Form _form;

    private static readonly Color[] bandColors = new Color[]
    {
        Color.Red, Color.Orange, Color.Yellow,
        Color.Green, Color.Cyan, Color.Blue, Color.Magenta
    };

    public static void Start(Form form)
    {
        _form = form;

        timer = new Timer();
        timer.Interval = 16;

        timer.Tick += (s, e) =>
        {
            angle += 10f;

            if (angle > 360) angle -= 360;

            if (!introDone)
            {
                introProgress += 0.004f;

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
        g.TranslateTransform(w / 2, h / 2);

        int numBands = bandColors.Length;
        float maxRadius = (float)Math.Sqrt(w * w + h * h);
        float lengthFactor = introProgress;

        for (int i = 0; i < numBands; i++)
        {
            float bandAngle = i * 360f / numBands + angle;
            float nextAngle = (i + 1) * 360f / numBands + angle;

            float distortion = (float)Math.Sin(DateTime.Now.Ticks / 10000000.0 + i) * 0F;

            float rad1 = (bandAngle + distortion) * (float)Math.PI / 180f;
            float rad2 = (nextAngle + distortion) * (float)Math.PI / 180f;

            PointF p1 = new PointF((float)Math.Cos(rad1) * maxRadius * lengthFactor,
                                   (float)Math.Sin(rad1) * maxRadius * lengthFactor);
            PointF p2 = new PointF((float)Math.Cos(rad2) * maxRadius * lengthFactor,
                                   (float)Math.Sin(rad2) * maxRadius * lengthFactor);

            GraphicsPath path = new GraphicsPath();
            path.AddLine(0, 0, p1.X, p1.Y);
            path.AddLine(p1.X, p1.Y, p2.X, p2.Y);
            path.CloseFigure();

            using (Brush b = new SolidBrush(bandColors[i % bandColors.Length]))
            {
                g.FillPath(b, path);
            }
        }
    }
}
