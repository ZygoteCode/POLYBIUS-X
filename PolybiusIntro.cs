using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

public partial class PolybiusIntro : Form
{
    private Image _originalImage, _differentImage1;
    private int _ticks = 0;
    private WaveOutEvent _outputDevice;
    private AudioFileReader _audioFile;
    private bool _canChangeScreen;

    public PolybiusIntro()
    {
        InitializeComponent();

        _originalImage = pictureBox1.BackgroundImage;
        _differentImage1 = InvertImage(_originalImage);

        timer1.Start();
        timer2.Start();

        Play(Path.GetFullPath("assets\\intro.mp3"));
    }

    private void Play(string filePath)
    {
        Stop();

        _audioFile = new AudioFileReader(filePath);

        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_audioFile);
        _outputDevice.Play();
    }

    private void Stop()
    {
        if (_outputDevice != null)
        {
            _outputDevice.Stop();
            _outputDevice.Dispose();
            _outputDevice = null;
        }

        if (_audioFile != null)
        {
            _audioFile.Dispose();
            _audioFile = null;
        }
    }

    private void PolybiusIntro_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        switch (_ticks)
        {
            case 0:
                pictureBox1.BackgroundImage = _differentImage1;
                _ticks = 1;
                break;
            case 1:
                pictureBox1.BackgroundImage = _originalImage;
                _ticks = 0;
                break;
        }
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
        timer2.Stop();
        timer1.Stop();
        timer3.Start();

        Stop();
        pictureBox1.BackgroundImage = _originalImage;
        _canChangeScreen = true;
    }

    private void timer3_Tick(object sender, EventArgs e)
    {
        timer3.Stop();
        this.HideForm();
        PolybiusGame polybiusGame = new PolybiusGame();
        polybiusGame.Show();
    }

    private void PolybiusIntro_KeyDown(object sender, KeyEventArgs e)
    {
        if (!_canChangeScreen)
        {
            return;
        }

        if (e.KeyValue.Equals(Keys.F2) || e.KeyCode.Equals(Keys.F2))
        {
            timer3.Stop();
            this.HideForm();
            HigherFunctions higherFunctions = new HigherFunctions();
            higherFunctions.Show();
        }
        else if ((e.KeyValue.Equals(Keys.Enter) || e.KeyCode.Equals(Keys.Enter)) 
            || (e.KeyValue.Equals(Keys.Space) || e.KeyCode.Equals(Keys.Space)))
        {
            timer3.Stop();
            this.HideForm();
            PolybiusGame polybiusGame = new PolybiusGame();
            polybiusGame.Show();
        }
    }

    public static Image InvertImage(Image original)
    {
        Bitmap inverted = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(inverted))
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {-1,  0,  0,  0, 0},
                new float[] { 0, -1,  0,  0, 0},
                new float[] { 0,  0, -1,  0, 0},
                new float[] { 0,  0,  0,  1, 0},
                new float[] { 1,  1,  1,  0, 1}
            });

            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(
                    original,
                    new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height,
                    GraphicsUnit.Pixel,
                    attributes
                );
            }
        }

        return inverted;
    }
}