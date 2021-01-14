using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Bots
{
    public class SimpletronBot : Chessbot
    {
        protected override Move GenerateMove(Chessboard board, TeamColor teamColor)
        {
            List<(int, Move)> longerMoves = new List<(int, Move)>();

            foreach (var botMove1 in board.GetMoves(teamColor))
            {
                Chessboard checkBoard = new Chessboard(board);

                checkBoard.DoMove(botMove1);

                TeamColor oppositeColor = teamColor == TeamColor.Black ? TeamColor.White : TeamColor.Black;

                foreach (var enemyMove1 in checkBoard.GetMoves(oppositeColor))
                {
                    Chessboard checkboardDepth1 = new Chessboard(checkBoard);
                    checkboardDepth1.DoMove(enemyMove1);

                    foreach (var botMove2 in FindBestMoves(checkboardDepth1, teamColor))
                    {
                        Chessboard checkboardDepth2 = new Chessboard(checkboardDepth1);
                        checkboardDepth2.DoMove(botMove2);
                    
                        longerMoves.Add((checkboardDepth2.MaterialSum, botMove1));
                    }
                }
            }

            List<(int, Move)> sortedMoves = longerMoves.OrderByDescending(material => material.Item1).ToList();

            int newLuckyNumber;
            if (teamColor == TeamColor.Black)
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
                boardCheck.DoMove(move);
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
