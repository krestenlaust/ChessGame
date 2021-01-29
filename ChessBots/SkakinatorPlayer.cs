using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGame;

namespace ChessBots
{
    /// <summary>
    /// The player referenced by chess games implemented using <c>ChessGame</c> library.
    /// </summary>
    public class SkakinatorPlayer : Player
    {
        private readonly SkakinatorLogic logic;
        private readonly BotUI UI;

        public SkakinatorPlayer(string name) : base(name)
        {
            logic = new SkakinatorLogic();
            UI = new BotUI();
            UI.Show();
            UI.SetProgress(1, 10);
            logic.onSingleMoveCalculated += Logic_onSingleMoveCalculated;
        }

        private void Logic_onSingleMoveCalculated(int arg1, int arg2)
        {
            UI.Invoke((MethodInvoker)delegate
            {
                UI.SetProgress(arg1, arg2);
            });
        }

        public override void TurnStarted(Chessboard board)
        {
            board.PerformMove(logic.GenerateMove(board));
        }
    }
}
