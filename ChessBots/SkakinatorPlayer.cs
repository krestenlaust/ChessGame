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
        public const int DEFAULT_DEPTH = 2;
        private readonly SkakinatorLogic logic;
        private readonly BotUI UI;
        private int Depth
        {
            get
            {
                if (UI is null)
                {
                    return DEFAULT_DEPTH;
                }

                return UI.DepthSetting;
            }
        }

        public SkakinatorPlayer(string name, bool enableUI) : base(name)
        {
            logic = new SkakinatorLogic();
            if (enableUI)
            {
                UI = new BotUI();
                UI.Text = $"Bot info: {name}";
                UI.Show();
                logic.onSingleMoveCalculated += Logic_onSingleMoveCalculated;
            }
        }

        private void Logic_onSingleMoveCalculated(int current, int max, Move move, float evaluation)
        {
            UI.Invoke((MethodInvoker)delegate
            {
                UI.SetProgress(current, max);

                if (current == 0)
                {
                    UI.ClearLog();
                }

                UI.PrintLog($"[{current}] {move} = {evaluation}");
            });
        }

        public override void TurnStarted(Chessboard board)
        {
            board.PerformMove(logic.GenerateMove(board, Depth));
        }
    }
}
