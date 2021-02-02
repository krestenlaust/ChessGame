using ChessGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChessForms
{
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
            foreach (var piece in pieces)
            {
                PictureBox control = new PictureBox();
                control.Image = BoardDisplay.GetPieceImage(piece);
                control.SizeMode = PictureBoxSizeMode.StretchImage;
                control.Click += Control_Click;
                PieceButtons[control.Image] = piece;
                flowLayoutPanel.Controls.Add(control);
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            SelectedPiece = PieceButtons[(sender as PictureBox).Image];
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
