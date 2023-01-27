using ChessGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessForms;

public partial class PromotionPrompt : Form
{
    public Piece SelectedPiece { get; private set; }
    public Dictionary<Image, Piece> PieceButtons = new Dictionary<Image, Piece>();

    public PromotionPrompt(List<Piece> pieces)
    {
        InitializeComponent();

        InstantiatePieceButtons(pieces);
    }

    private void PromotionPrompt_Load(object sender, EventArgs e)
    {
    }

    private void InstantiatePieceButtons(List<Piece> pieces)
    {
        tableLayoutPanel.ColumnCount = pieces.Count;
        tableLayoutPanel.ColumnStyles.Clear();
        int i;
        for (i = 0; i < pieces.Count; i++)
        {
            // set size to any percent, doesnt matter
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
        }

        foreach (var piece in pieces)
        {
            PictureBox control = new PictureBox
            {
                Image = BoardDisplay.GetPieceImage(piece),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Width = 200,
                Height = 200,
            };
            control.Click += Control_Click;

            PieceButtons[control.Image] = piece;
            tableLayoutPanel.Controls.Add(control);
        }
    }

    private void Control_Click(object sender, EventArgs e)
    {
        SelectedPiece = PieceButtons[(sender as PictureBox).Image];
        DialogResult = DialogResult.OK;
        Close();
    }
}
