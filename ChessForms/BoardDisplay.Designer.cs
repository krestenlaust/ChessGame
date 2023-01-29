
namespace ChessForms;

partial class BoardDisplay
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
        this.tableLayoutPanelBoard = new System.Windows.Forms.TableLayoutPanel();
        this.backgroundWorkerMove = new System.ComponentModel.BackgroundWorker();
        this.SuspendLayout();
        // 
        // tableLayoutPanelBoard
        // 
        this.tableLayoutPanelBoard.ColumnCount = 1;
        this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanelBoard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        this.tableLayoutPanelBoard.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanelBoard.Location = new System.Drawing.Point(0, 0);
        this.tableLayoutPanelBoard.Name = "tableLayoutPanelBoard";
        this.tableLayoutPanelBoard.RowCount = 1;
        this.tableLayoutPanelBoard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
        this.tableLayoutPanelBoard.Size = new System.Drawing.Size(782, 753);
        this.tableLayoutPanelBoard.TabIndex = 0;
        // 
        // backgroundWorkerMove
        // 
        this.backgroundWorkerMove.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerMove_DoWork);
        this.backgroundWorkerMove.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerMove_RunWorkerCompleted);
        // 
        // BoardDisplay
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(782, 753);
        this.Controls.Add(this.tableLayoutPanelBoard);
        this.KeyPreview = true;
        this.Name = "BoardDisplay";
        this.Text = "Super Skak!";
        this.Load += new System.EventHandler(this.BoardDisplay_Load);
        this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BoardDisplay_KeyUp);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoard;
    private System.ComponentModel.BackgroundWorker backgroundWorkerMove;
}

