namespace ChessForms
{
    partial class BoardMoveHistoryEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelMoveNotation = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // labelMoveNotation
            // 
            labelMoveNotation.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMoveNotation.Location = new System.Drawing.Point(0, 0);
            labelMoveNotation.Name = "labelMoveNotation";
            labelMoveNotation.Size = new System.Drawing.Size(102, 38);
            labelMoveNotation.TabIndex = 0;
            labelMoveNotation.Text = "label1";
            labelMoveNotation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BoardMoveHistoryEntry
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelMoveNotation);
            Name = "BoardMoveHistoryEntry";
            Size = new System.Drawing.Size(102, 38);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label labelMoveNotation;
    }
}
