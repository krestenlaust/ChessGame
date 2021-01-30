using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessBots
{
    public partial class BotUI : Form
    {
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

        public void ClearLog()
        {
            richTextBoxLog.Text = "";
        }

        public void PrintLog(string line)
        {
            richTextBoxLog.Text += $"{line}\n";
        }

        public void SetProgress(int current, int max)
        {
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
            Update();
        }
    }
}
