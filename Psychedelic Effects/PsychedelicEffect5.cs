using System;
using System.Drawing;
using System.Windows.Forms;

public class PsychedelicEffect5
{
    private static Form _form;
    private static Timer timer;
    private static float ring_offset = 0F;

    private static float introProgress = 0f;
    private static bool introDone = false;

    public static void Start(Form form)
    {
        _form = form;
        _form.WindowState = FormWindowState.Maximized;
        _form.BackColor = Color.Black;

        timer = new Timer();
        timer.Interval = 16;

        timer.Tick += (sender, e) =>
        {
            ring_offset += 7F;

            if (!introDone)
            {
                introProgress += 0.006f;

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
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        int w = _form.ClientSize.Width;
        int h = _form.ClientSize.Height;
        float centerX = w / 2f;
        float centerY = h / 2f;
        float maxRadius = (float)Math.Sqrt(centerX * centerX + centerY * centerY) * introProgress;

        const float ringWidth = 18F;
        const float bandSpacing = 20F;

        float currentOffset = ring_offset % (bandSpacing * 2);

        for (float r = currentOffset; r < maxRadius + bandSpacing; r += bandSpacing * 2)
        {
            using (Pen pen = new Pen(Color.FromArgb(209, 229, 230), ringWidth))
            {
                g.DrawEllipse(pen, centerX - r, centerY - r, r * 2, r * 2);
            }

            float nextRingRadius = r + bandSpacing;

            if (nextRingRadius < maxRadius + bandSpacing)
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 0, 0), ringWidth))
                {
                    g.DrawEllipse(pen, centerX - nextRingRadius, centerY - nextRingRadius, nextRingRadius * 2, nextRingRadius * 2);
                }
            }
        }
    }
}