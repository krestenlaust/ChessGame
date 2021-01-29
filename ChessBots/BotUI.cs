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
        public BotUI()
        {
            InitializeComponent();
        }

        private void BotUI_Load(object sender, EventArgs e)
        {

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
