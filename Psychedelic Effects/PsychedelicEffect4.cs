using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class PsychedelicEffect4
{
    private static Timer timer;
    private static float angle = 0f;
    private static float introProgress = 0f;
    private static bool introDone = false;

    private static Form _form;
    private static Bitmap backBuffer;

    private static readonly Color[] bandColors = new Color[]
    {
        Color.FromArgb(0, 0, 0),
        Color.FromArgb(0, 0, 255),
    };

    public static void Start(Form form)
    {
        _form = form;
        ResizeBuffer();
        _form.Resize += (s, e) => ResizeBuffer();

        timer = new Timer();
        timer.Interval = 16;

        timer.Tick += (s, e) =>
        {
            // ## 1. VELOCITÀ DI ROTAZIONE AUMENTATA DRASTICAMENTE ##
            angle += 1000.0f; // Sentiti libero di aumentare o diminuire questo valore!

            if (!introDone)
            {
                introProgress += 0.05f;

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
            backBuffer = new Bitmap(_form.ClientSize.Width, _form.ClientSize.Height, PixelFormat.Format32bppPArgb);
        }
    }

    public static void ProcessEffect(PaintEventArgs e)
    {
        if (backBuffer == null) return;

        int w = backBuffer.Width;
        int h = backBuffer.Height;
        float centerX = w / 2f;
        float centerY = h / 2f;

        BitmapData bmpData = backBuffer.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
        int stride = bmpData.Stride;
        byte[] pixels = new byte[Math.Abs(stride) * h];

        int numBands = bandColors.Length;
        float anglePerBand = 360f / numBands;
        float maxRadius = (float)Math.Sqrt(w * w + h * h) / 2f * introProgress;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float dx = x - centerX;
                float dy = y - centerY;

                float pixelAngleRad = (float)Math.Atan2(dy, dx);
                float pixelAngleDeg = pixelAngleRad * (180f / (float)Math.PI);
                float pixelDist = (float)Math.Sqrt(dx * dx + dy * dy);

                // ## 2. VELOCITÀ DELLA DISTORSIONE AUMENTATA DRASTICAMENTE ##
                float distortion = (float)Math.Sin(pixelDist * 0.025f + angle * 0.3f) * 500F;
                float finalAngle = pixelAngleDeg + distortion;

                float rawAngle = finalAngle - angle;
                float normalizedAngle = ((rawAngle % 360f) + 360f) % 360f;

                int bandIndex = (int)(normalizedAngle / anglePerBand);
                Color pixelColor = bandColors[bandIndex];

                if (pixelDist > maxRadius)
                {
                    pixelColor = Color.Black;
                }

                int index = y * stride + (x * 4);
                pixels[index] = pixelColor.B;
                pixels[index + 1] = pixelColor.G;
                pixels[index + 2] = pixelColor.R;
                pixels[index + 3] = 255;
            }
        }

        System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
        backBuffer.UnlockBits(bmpData);

        e.Graphics.DrawImage(backBuffer, 0, 0);
    }
}