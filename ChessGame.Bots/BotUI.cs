namespace ChessGame.Bots
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// A windows forms window allowing configuration of bot.
    /// </summary>
    public partial class BotUI : Form
    {
        int currentX = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotUI"/> class.
        /// </summary>
        public BotUI()
        {
            InitializeComponent();
        }

        // TODO: Auto-scroll
        public int DepthSetting
        {
            get
            {
                return (int)numericUpDownDepth.Value;
            }
        }

        public void AddPoint(float evaluation)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<float>(AddPoint), new object[] { evaluation });
                return;
            }

            currentX++;
            chartBoardEvaluation.Series[0].Points.AddXY(currentX, evaluation);
        }

        public void ClearLog()
        {
            richTextBoxLog.Text = string.Empty;
        }

        public void PrintLog(string line)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(PrintLog), new object[] { line });
                return;
            }

            richTextBoxLog.Text += $"{line}\n";
        }

        public void SetProgress(int current, int max)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int, int>(SetProgress), new object[] { current, max });
                return;
            }

            if (current == max)
            {
                labelCalculation.Text = $"Finished calculating";
            }
            else
            {
                labelCalculation.Text = $"Calculated moves: {current}/{max}";
            }

            progressBarCalculation.Maximum = max;
            progressBarCalculation.Value = current;
        }

        void BotUI_Load(object sender, EventArgs e)
        {
            numericUpDownDepth.Value = SkakinatorPlayer.DEFAULT_DEPTH;
        }

        void RichTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
            richTextBoxLog.ScrollToCaret();
        }

        void CheckBoxShowGraph_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as CheckBox).Checked;

            if (isChecked)
            {
                Width += chartBoardEvaluation.Width + 20;
            }
            else
            {
                Width -= chartBoardEvaluation.Width + 20;
            }
        }
    }
}
