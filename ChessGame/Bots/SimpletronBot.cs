using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Bots
{
    public class SimpletronBot : Chessbot
    {
        private TeamColor GetOppositeColor(TeamColor color) => color == TeamColor.Black ? TeamColor.White : TeamColor.Black;

        private List<(int, Move)> CheckMovesRecursive(Chessboard board, TeamColor color, int depth, List<Move> moveStack = null)
        {
            List<(int, Move)> moves = new List<(int, Move)>();

            foreach (var move in board.GetMoves(color))
            {
                Chessboard boardSimulation = new Chessboard(board);
                boardSimulation.ExecuteMove(move);

                if (depth == 0)
                {
                    moves.Add((boardSimulation.MaterialSum, moveStack[0]));
                }
                else
                {
                    if (moveStack is null)
                    {
                        moveStack = new List<Move>();
                    }

                    moveStack.Add(move);

                    
                    return CheckMovesRecursive(boardSimulation, GetOppositeColor(color), depth - 1, moveStack);
                }
            }

            return moves;
        }

        protected override Move GenerateMove(Chessboard board)
        {

            List<(int, Move)> longerMoves = CheckMovesRecursive(board, board.CurrentTurn, 2);

            List<(int, Move)> sortedMoves = longerMoves.OrderByDescending(material => material.Item1).ToList();

            int newLuckyNumber;
            if (board.CurrentTurn == TeamColor.Black)
            {
                newLuckyNumber = sortedMoves[sortedMoves.Count - 1].Item1;
            }
            else
            {
                newLuckyNumber = sortedMoves[0].Item1;
            }

            List<Move> viableMoves = (from singleMove in sortedMoves 
                                     where singleMove.Item1 == newLuckyNumber 
                                     select singleMove.Item2).ToList();

            foreach (var item in viableMoves)
            {
                if (!item.Moves[0].Piece.hasMoved)
                {
                    return item;
                }
            }

            return viableMoves[0];
        }

        private List<Move> FindBestMoves(Chessboard board, TeamColor teamColor)
        {
            List<(int, Move)> moves = new List<(int, Move)>();

            foreach (var move in board.GetMoves(teamColor))
            {
                Chessboard boardCheck = new Chessboard(board);
                boardCheck.ExecuteMove(move);
                moves.Add((boardCheck.MaterialSum, move));
            }

            if (moves.Count == 0)
            {
                return null;
            }

            List<(int, Move)> sortedMoves = moves.OrderByDescending(material => material.Item1).ToList();

            int luckyNumber;

            if (teamColor == TeamColor.Black)
            {
                luckyNumber = sortedMoves[sortedMoves.Count - 1].Item1;
            }
            else
            {
                luckyNumber = sortedMoves[0].Item1;
            }

            return (from move in sortedMoves where move.Item1 == luckyNumber select move.Item2).ToList();
        }
    }
}
