using System;
using System.Windows.Forms;
using ChessGame;

namespace ChessBots
{
    /// <summary>
    /// The player referenced by chess games implemented using <c>ChessGame</c> library.
    /// </summary>
    public class SkakinatorPlayer : Player
    {
        public const int DEFAULT_DEPTH = 3;
        private readonly SkakinatorLogic logic;
        private readonly BotUI UI;
        private int Depth
        {
            get
            {
                if (this.UI is null)
                {
                    return DEFAULT_DEPTH;
                }

                return this.UI.DepthSetting;
            }
        }

        public SkakinatorPlayer(string name, bool enableUI) : base(name)
        {
            this.logic = new SkakinatorLogic();
            if (enableUI)
            {
                this.UI = new BotUI();
                this.UI.Text = $"Bot info: {name}";
                this.UI.Show();
                this.logic.SingleMoveCalculated += this.Logic_onSingleMoveCalculated;
            }
        }

        private void Logic_onSingleMoveCalculated(int current, int max, Move move, float evaluation)
        {
            this.UI.SetProgress(current, max);

            if (current == 0)
            {
                this.UI.AddPoint(evaluation);
                this.UI.PrintLog("\n Board: " + evaluation);
            }
            else
            {
                this.UI.PrintLog($"[{current}] {move.ToString(MoveNotation.UCI)} = {Math.Round(evaluation, 2)}");
            }
        }

        public override void TurnStarted(Chessboard board)
        {
            board.PerformMove(this.logic.GenerateMoveParrallel(board, this.Depth));
        }
    }
}
