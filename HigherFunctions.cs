using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

public partial class HigherFunctions : Form
{
    public HigherFunctions()
    {
        InitializeComponent();
        Cursor = new Cursor("assets\\polybius_cursor.cur");
        int y = 132;

        foreach (KeyValuePair<string, bool> higherFunction in Globals.HIGHER_FUNCTIONS)
        {
            Label higherFunctionLabel = new Label();

            higherFunctionLabel.Text = higherFunction.Key.ToUpper();
            higherFunctionLabel.Font = new Font("Polybius1981", 36, FontStyle.Bold);
            higherFunctionLabel.Location = new Point(185, y);
            higherFunctionLabel.ForeColor = Color.FromArgb(90, 231, 103);
            higherFunctionLabel.AutoSize = true;
            higherFunctionLabel.TextAlign = ContentAlignment.TopCenter;
            higherFunctionLabel.Size = new Size(488, 43);
            higherFunctionLabel.Anchor = AnchorStyles.None;

            Label enableFunctionLabel = new Label();

            enableFunctionLabel.Text = higherFunction.Value ? "ENABLED" : "DISABLED";
            enableFunctionLabel.Font = new Font("Polybius1981", 36, FontStyle.Bold);
            enableFunctionLabel.Location = new Point(679, y);
            enableFunctionLabel.ForeColor = Color.FromArgb(90, 231, 103);
            enableFunctionLabel.AutoSize = true;
            enableFunctionLabel.TextAlign = ContentAlignment.TopCenter;
            enableFunctionLabel.Size = new Size(167, 43);
            enableFunctionLabel.Anchor = AnchorStyles.None;

            enableFunctionLabel.MouseEnter += (s, e) =>
            {
                enableFunctionLabel.ForeColor = Color.FromArgb(69, 138, 182);
            };

            enableFunctionLabel.MouseLeave += (s, e) =>
            {
                enableFunctionLabel.ForeColor = Color.FromArgb(90, 231, 103);
            };

            enableFunctionLabel.MouseClick += (s, e) =>
            {
                bool isLeftButton = ((MouseEventArgs)e).Button == MouseButtons.Left;

                if (isLeftButton)
                {
                    Globals.HIGHER_FUNCTIONS[higherFunction.Key] = !Globals.HIGHER_FUNCTIONS[higherFunction.Key];

                    if (Globals.HIGHER_FUNCTIONS[higherFunction.Key])
                    {
                        enableFunctionLabel.Text = "ENABLED";
                    }
                    else
                    {
                        enableFunctionLabel.Text = "DISABLED";
                    }
                }
            };

            Controls.Add(higherFunctionLabel);
            Controls.Add(enableFunctionLabel);

            y += 43;
        }
    }

    private void HigherFunctions_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void HigherFunctions_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyValue.Equals(Keys.F2) || e.KeyCode.Equals(Keys.F2))
        {
            this.HideForm();
            PolybiusIntro polybiusIntro = new PolybiusIntro();
            polybiusIntro.Show();
        }
    }
}