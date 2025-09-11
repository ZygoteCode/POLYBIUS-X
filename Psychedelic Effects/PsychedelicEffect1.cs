using System;
using System.Drawing;
using System.Windows.Forms;

public class PsychedelicEffect1
{
    private static Timer timer;
    private static float angle = 0f;
    private static float ringScale = 1f;
    private static bool expanding = true;

    private static float introProgress = 0f;
    private static bool introDone = false;

    private static Form _form;

    public static void Start(Form form)
    {
        _form = form;

        timer = new Timer();
        timer.Interval = 16;

        timer.Tick += (s, e) =>
        {
            angle += 0.03F;

            if (!introDone)
            {
                introProgress += 0.008f;

                if (introProgress >= 1f)
                {
                    introProgress = 1f;
                    introDone = true;
                }
            }
            else
            {
                float maxScale = 5.0f;
                float minScale = 0.5f;

                if (expanding)
                {
                    ringScale += 0.06f;

                    if (ringScale > maxScale)
                    {
                        expanding = false;
                    }
                }
                else
                {
                    ringScale -= 0.12f;

                    if (ringScale < minScale)
                    {
                        expanding = true;
                    }
                }
            }

            _form.Invalidate();
        };

        timer.Start();
    }

    public static void ProcessEffect(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.TranslateTransform(_form.ClientSize.Width / 2, _form.ClientSize.Height / 2);

        int numLines = 50;
        float radius = Math.Min(_form.ClientSize.Width, _form.ClientSize.Height);

        int visibleLines = (int)(numLines * introProgress);
        float lineLengthFactor = introProgress;

        using (Pen thickPen = new Pen(Color.White, 15F))
        {
            for (int i = 0; i < visibleLines; i++)
            {
                float a = (float)(i * 2 * Math.PI / numLines + angle);
                float x = (float)Math.Cos(a) * radius * lineLengthFactor;
                float y = (float)Math.Sin(a) * radius * lineLengthFactor;
                g.DrawLine(thickPen, 0, 0, x, y);
            }
        }

        float ringRadius = 250 * ringScale;
        float thickness = 20F;

        using (Pen p = new Pen(Color.Magenta, thickness))
        {
            g.DrawEllipse(p, -ringRadius, -ringRadius, ringRadius * 2, ringRadius * 2);
        }
    }
}