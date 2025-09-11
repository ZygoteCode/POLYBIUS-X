using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class PsychedelicEffect6
{
    private static Form _form;
    private static Timer timer;
    private static float angle_offset = 0f;
    private static Bitmap backBuffer;

    private static float introProgress = 0f;
    private static bool introDone = false;

    // Palette di colori ispirata all'immagine
    private static readonly Color[] colors = new Color[]
    {
        Color.FromArgb(50, 255, 100),   // Verde chiaro
        Color.FromArgb(180, 0, 180),    // Magenta
        Color.FromArgb(255, 150, 0),    // Arancione
        Color.FromArgb(120, 80, 50),    // Marrone
        Color.FromArgb(20, 255, 255)    // Ciano
    };

    public static void Start(Form form)
    {
        _form = form;
        _form.WindowState = FormWindowState.Maximized;
        _form.BackColor = Color.Black;

        ResizeBuffer();
        _form.Resize += (s, e) => ResizeBuffer();

        timer = new Timer();
        timer.Interval = 16; // Circa 60 FPS
        timer.Tick += (sender, e) =>
        {
            angle_offset += 0.5f; // Controlla la velocità di rotazione

            if (!introDone)
            {
                introProgress += 0.01f;

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

    private static void ResizeBuffer()
    {
        if (_form.ClientSize.Width > 0 && _form.ClientSize.Height > 0)
        {
            backBuffer?.Dispose();
            backBuffer = new Bitmap(_form.ClientSize.Width, _form.ClientSize.Height, PixelFormat.Format32bppArgb);
        }
    }

    public static void ProcessEffect(PaintEventArgs e)
    {
        if (backBuffer == null) return;

        using (Graphics g = Graphics.FromImage(backBuffer))
        {
            g.Clear(Color.Black);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = backBuffer.Width;
            int h = backBuffer.Height;
            float centerX = w / 2f;
            float centerY = h / 2f;
            float maxRadius = Math.Max(w, h) * introProgress;

            const int numSpokes = 80;
            const int segmentsPerSpoke = 55;

            float angleIncrement = (float)(2 * Math.PI / numSpokes);

            for (int i = 0; i < numSpokes; i++)
            {
                float currentAngle = i * angleIncrement + angle_offset;

                for (int j = 0; j < segmentsPerSpoke; j++)
                {
                    // Calcola la distanza dal centro
                    float distanceFactor = (float)j / (segmentsPerSpoke - 1);
                    float currentRadius = distanceFactor * maxRadius;

                    // Calcola la dimensione del segmento
                    float segmentWidth = 55f;
                    float segmentHeight = 10F + (distanceFactor * 15f);

                    float x = centerX + currentRadius * (float)Math.Cos(currentAngle);
                    float y = centerY + currentRadius * (float)Math.Sin(currentAngle);

                    using (SolidBrush brush = new SolidBrush(colors[(i + j) % colors.Length]))
                    {
                        var state = g.Save();
                        g.TranslateTransform(x, y);
                        g.RotateTransform((float)(currentAngle * 180 / Math.PI));
                        g.FillRectangle(brush, -segmentWidth / 2, -segmentHeight / 2, segmentWidth, segmentHeight);
                        g.Restore(state);
                    }
                }
            }
        }
        e.Graphics.DrawImage(backBuffer, 0, 0);
    }
}