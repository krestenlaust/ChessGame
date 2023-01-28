using System;
using ChessGame;

namespace ChessGame.Bots;

/// <summary>
/// The player referenced by chess games implemented using <c>ChessGame</c> library.
/// </summary>
public class SkakinatorPlayer : Player
{
    public const int DEFAULT_DEPTH = 3;
    readonly SkakinatorLogic logic;
    readonly BotUI UI;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkakinatorPlayer"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="enableUI"></param>
    public SkakinatorPlayer(string name, bool enableUI)
        : base(name)
    {
        logic = new SkakinatorLogic();
        if (enableUI)
        {
            UI = new BotUI
            {
                Text = $"Bot info: {name}",
            };
            UI.Show();
            logic.SingleMoveCalculated += Logic_onSingleMoveCalculated;
        }
    }

    int Depth
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

    public override void TurnStarted(Chessboard board)
    {
        board.PerformMove(logic.GenerateMoveParallel(board, Depth));
    }

    void Logic_onSingleMoveCalculated(int current, int max, Move move, float evaluation)
    {
        UI.SetProgress(current, max);

        if (current == 0)
        {
            UI.AddPoint(evaluation);
            UI.PrintLog("\n Board: " + evaluation);
        }
        else
        {
            UI.PrintLog($"[{current}] {move.ToString(MoveNotation.UCI)} = {Math.Round(evaluation, 2)}");
        }
    }
}
