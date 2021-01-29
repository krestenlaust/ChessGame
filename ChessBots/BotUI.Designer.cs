
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
            this.progressBarCalculation = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCalculation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarCalculation
            // 
            this.progressBarCalculation.Location = new System.Drawing.Point(12, 59);
            this.progressBarCalculation.Name = "progressBarCalculation";
            this.progressBarCalculation.Size = new System.Drawing.Size(251, 23);
            this.progressBarCalculation.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Move calculation:";
            // 
            // labelCalculation
            // 
            this.labelCalculation.AutoSize = true;
            this.labelCalculation.Location = new System.Drawing.Point(12, 36);
            this.labelCalculation.Name = "labelCalculation";
            this.labelCalculation.Size = new System.Drawing.Size(101, 17);
            this.labelCalculation.TabIndex = 2;
            this.labelCalculation.Text = "Not calculating";
            // 
            // BotUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 95);
            this.Controls.Add(this.labelCalculation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarCalculation);
            this.Name = "BotUI";
            this.Text = "BotUI";
            this.Load += new System.EventHandler(this.BotUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCalculation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCalculation;
    }
}