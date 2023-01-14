namespace ChessBots
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
            this.InitializeComponent();
        }

        // TODO: Auto-scroll
        public int DepthSetting
        {
            get
            {
                return (int)this.numericUpDownDepth.Value;
            }
        }

        public void AddPoint(float evaluation)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<float>(this.AddPoint), new object[] { evaluation });
                return;
            }

            this.currentX++;
            this.chartBoardEvaluation.Series[0].Points.AddXY(this.currentX, evaluation);
        }

        public void ClearLog()
        {
            this.richTextBoxLog.Text = "";
        }

        public void PrintLog(string line)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(this.PrintLog), new object[] { line });
                return;
            }

            this.richTextBoxLog.Text += $"{line}\n";
        }

        public void SetProgress(int current, int max)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<int, int>(this.SetProgress), new object[] { current, max });
                return;
            }

            if (current == max)
            {
                this.labelCalculation.Text = $"Finished calculating";
            }
            else
            {
                this.labelCalculation.Text = $"Calculated moves: {current}/{max}";
            }

            this.progressBarCalculation.Maximum = max;
            this.progressBarCalculation.Value = current;
        }

        void BotUI_Load(object sender, EventArgs e)
        {
            this.numericUpDownDepth.Value = SkakinatorPlayer.DEFAULT_DEPTH;
        }

        void RichTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            this.richTextBoxLog.SelectionStart = this.richTextBoxLog.Text.Length;
            this.richTextBoxLog.ScrollToCaret();
        }

        void CheckBoxShowGraph_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as CheckBox).Checked;

            if (isChecked)
            {
                this.Width += this.chartBoardEvaluation.Width + 20;
            }
            else
            {
                this.Width -= this.chartBoardEvaluation.Width + 20;
            }
        }
    }
}
