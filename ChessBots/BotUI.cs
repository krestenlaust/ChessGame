using System;
using System.Windows.Forms;

namespace ChessBots
{
    public partial class BotUI : Form
    {
        private int currentX = 0;

        // TODO: Auto-scroll
        public int DepthSetting
        {
            get
            {
                return (int)numericUpDownDepth.Value;
            }
        }

        public BotUI()
        {
            InitializeComponent();
        }

        private void BotUI_Load(object sender, EventArgs e)
        {
            numericUpDownDepth.Value = SkakinatorPlayer.DEFAULT_DEPTH;   
        }

        public void AddPoint(float evaluation)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<float>(AddPoint), new object[] { evaluation });
                return;
            }

            currentX++;
            chartBoardEvaluation.Series[0].Points.AddXY(currentX, evaluation);
        }

        public void ClearLog()
        {
            richTextBoxLog.Text = "";
        }

        public void PrintLog(string line)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(PrintLog), new object[] { line });
                return;
            }

            richTextBoxLog.Text += $"{line}\n";
        }

        public void SetProgress(int current, int max)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<int, int>(SetProgress), new object[] { current, max });
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
            //Update();
        }

        private void richTextBoxLog_TextChanged(object sender, EventArgs e)
        {
            richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
            richTextBoxLog.ScrollToCaret();
        }

        private void checkBoxShowGraph_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as CheckBox).Checked;

            if (isChecked)
            {
                this.Width += chartBoardEvaluation.Width + 20;
            }
            else
            {
                this.Width -= chartBoardEvaluation.Width + 20;
            }
        }
    }
}
