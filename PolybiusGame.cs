using NAudio.Wave;
using System.Diagnostics;
using System.Windows.Forms;

public partial class PolybiusGame : Form
{
    private WaveOutEvent _outputDevice1;
    private AudioFileReader _audioFile1;

    private WaveOutEvent _outputDevice2;
    private AudioFileReader _audioFile2;

    private bool _isStopped;

    private string[] _subliminalMessages = new string[]
    {
        "KILL YOURSELF",
        "NO THOUGHT",
        "NO IMAGINATION",
        "NO IDEAS",
        "NO THINKING",
        "CONSUME",
        "CONFORM",
        "HONOR APATHY",
        "OBEY",
        "OBEY AUTHORITY",
        "HONOR AUTHORITY",
        "DO NOT QUESTION AUTHORITY",
        "SUBMIT",
        "SURRENDER",
        "MARRY AND REPRODUCE",
        "STAY ASLEEP",
        "SLEEP"
    };

    private int _subliminalMessageIndex = 0;

    public PolybiusGame()
    {
        InitializeComponent();
        PlayGameIntro();
        PsychedelicEffect7.Start(this);
        timer4.Start();
    }

    public void ShowSubliminalMessage(string subliminalMessage)
    {
        label1.Text = subliminalMessage;
        label1.Visible = true;
        timer2.Start();
    }

    private void timer2_Tick(object sender, System.EventArgs e)
    {
        timer2.Stop();
        label1.Visible = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        PsychedelicEffect7.ProcessEffect(e);
    }

    private void PlayGameIntro()
    {
        StopGameIntro();

        _audioFile1 = new AudioFileReader("assets\\intro_to_game.mp3");

        _outputDevice1 = new WaveOutEvent();
        _outputDevice1.PlaybackStopped += _outputDevice1_PlaybackStopped;
        _outputDevice1.Init(_audioFile1);
        _outputDevice1.Play();
    }

    private void _outputDevice1_PlaybackStopped(object sender, StoppedEventArgs e)
    {
        _isStopped = true;
    }

    private void StopGameIntro()
    {
        if (_outputDevice1 != null)
        {
            _outputDevice1.Stop();
            _outputDevice1.Dispose();
            _outputDevice1 = null;
        }

        if (_audioFile1 != null)
        {
            _audioFile1.Dispose();
            _audioFile1 = null;
        }
    }

    private void PlayGameMusic()
    {
        StopGameMusic();

        _audioFile2 = new AudioFileReader("assets\\game_music.mp3");

        _outputDevice2 = new WaveOutEvent();
        _outputDevice2.PlaybackStopped += _outputDevice2_PlaybackStopped;
        _outputDevice2.Init(_audioFile2);
        _outputDevice2.Play();
    }

    private void _outputDevice2_PlaybackStopped(object sender, StoppedEventArgs e)
    {
        PlayGameMusic();
    }

    private void StopGameMusic()
    {
        if (_outputDevice2 != null)
        {
            _outputDevice2.Stop();
            _outputDevice2.Dispose();
            _outputDevice2 = null;
        }

        if (_audioFile2 != null)
        {
            _audioFile2.Dispose();
            _audioFile2 = null;
        }
    }

    private void PolybiusGame_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void timer3_Tick(object sender, System.EventArgs e)
    {
        ShowSubliminalMessage(_subliminalMessages[_subliminalMessageIndex]);
        _subliminalMessageIndex++;

        if (_subliminalMessageIndex >= _subliminalMessages.Length)
        {
            _subliminalMessageIndex = 0;
        }
    }

    private void timer4_Tick(object sender, System.EventArgs e)
    {
        if (_isStopped)
        {
            _isStopped = false;
            timer4.Stop();
            PlayGameMusic();

            if (Globals.HIGHER_FUNCTIONS["Subliminal Messages"])
            {
                timer3.Start();
            }
        }
    }
}