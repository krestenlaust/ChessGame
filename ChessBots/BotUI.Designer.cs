
namespace ChessBots
{
    partial class BotUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.progressBarCalculation = new System.Windows.Forms.ProgressBar();
            this.labelCalculation = new System.Windows.Forms.Label();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.numericUpDownDepth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chartBoardEvaluation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBoxShowGraph = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBoardEvaluation)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarCalculation
            // 
            this.progressBarCalculation.Location = new System.Drawing.Point(12, 29);
            this.progressBarCalculation.Name = "progressBarCalculation";
            this.progressBarCalculation.Size = new System.Drawing.Size(293, 23);
            this.progressBarCalculation.TabIndex = 0;
            // 
            // labelCalculation
            // 
            this.labelCalculation.AutoSize = true;
            this.labelCalculation.Location = new System.Drawing.Point(12, 9);
            this.labelCalculation.Name = "labelCalculation";
            this.labelCalculation.Size = new System.Drawing.Size(101, 17);
            this.labelCalculation.TabIndex = 2;
            this.labelCalculation.Text = "Not calculating";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.Location = new System.Drawing.Point(12, 142);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(293, 165);
            this.richTextBoxLog.TabIndex = 3;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.TextChanged += new System.EventHandler(this.RichTextBoxLog_TextChanged);
            // 
            // numericUpDownDepth
            // 
            this.numericUpDownDepth.Location = new System.Drawing.Point(12, 84);
            this.numericUpDownDepth.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDepth.Name = "numericUpDownDepth";
            this.numericUpDownDepth.Size = new System.Drawing.Size(128, 22);
            this.numericUpDownDepth.TabIndex = 2;
            this.numericUpDownDepth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Depth:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Move evaluations:";
            // 
            // chartBoardEvaluation
            // 
            chartArea7.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea7.Name = "ChartArea1";
            this.chartBoardEvaluation.ChartAreas.Add(chartArea7);
            this.chartBoardEvaluation.Location = new System.Drawing.Point(342, 9);
            this.chartBoardEvaluation.Name = "chartBoardEvaluation";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Name = "Series1";
            series7.YValuesPerPoint = 2;
            this.chartBoardEvaluation.Series.Add(series7);
            this.chartBoardEvaluation.Size = new System.Drawing.Size(325, 300);
            this.chartBoardEvaluation.TabIndex = 7;
            // 
            // checkBoxShowGraph
            // 
            this.checkBoxShowGraph.AutoSize = true;
            this.checkBoxShowGraph.Location = new System.Drawing.Point(161, 85);
            this.checkBoxShowGraph.Name = "checkBoxShowGraph";
            this.checkBoxShowGraph.Size = new System.Drawing.Size(144, 21);
            this.checkBoxShowGraph.TabIndex = 8;
            this.checkBoxShowGraph.Text = "Show game graph";
            this.checkBoxShowGraph.UseVisualStyleBackColor = true;
            this.checkBoxShowGraph.CheckedChanged += new System.EventHandler(this.CheckBoxShowGraph_CheckedChanged);
            // 
            // BotUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 319);
            this.Controls.Add(this.checkBoxShowGraph);
            this.Controls.Add(this.chartBoardEvaluation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownDepth);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.labelCalculation);
            this.Controls.Add(this.progressBarCalculation);
            this.MaximizeBox = false;
            this.Name = "BotUI";
            this.ShowIcon = false;
            this.Text = "Bot info";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.BotUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBoardEvaluation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCalculation;
        private System.Windows.Forms.Label labelCalculation;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.NumericUpDown numericUpDownDepth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBoardEvaluation;
        private System.Windows.Forms.CheckBox checkBoxShowGraph;
    }
}